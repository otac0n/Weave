// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;
    using Weave.Compiler;
    using Weave.Expressions;
    using static System.Net.Mime.MediaTypeNames;

    [Generator]
    internal class GenerateWeaveSources : IIncrementalGenerator
    {
        private const string UseSourceGeneration = "build_metadata.WeaveTemplate.UseSourceGeneration";
        private const string ConfigFileExists = "build_metadata.WeaveTemplate.ConfigFileExists";
        private const string ConfigUseSourceGeneration = "build_metadata.WeaveTemplateConfig.UseSourceGeneration";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var weaveFiles = context.AdditionalTextsProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Select((p, ct) =>
                {
                    var options = p.Right.GetOptions(p.Left);
                    return new
                    {
                        AdditionalText = p.Left,
                        ConfigOptions = options,
                        UseSourceGeneration = GetBooleanOption(options, UseSourceGeneration) ?? GetBooleanOption(options, ConfigUseSourceGeneration),
                    };
                })
                .Where(p => p.UseSourceGeneration != null)
                .Select((p, ct) => new
                {
                    p.AdditionalText.Path,
                    Text = p.AdditionalText.GetText(ct),
                    p.UseSourceGeneration,
                    ConfigFileExists = GetBooleanOption(p.ConfigOptions, ConfigFileExists),
                });

            // TODO: Go ahead and compile _config files, but make sure that we emit a compile warning (using a pragma directive and a [Deprecated] attribute) about future removal.
            var sourceGeneratedFiles = weaveFiles
                .Where(p => p.UseSourceGeneration ?? false);

            context.RegisterSourceOutput(
                sourceGeneratedFiles,
                (context, file) => CompileWeaveFile(context, file.Path, file.Text));
        }

        private static bool? GetBooleanOption(AnalyzerConfigOptions options, string key) =>
            options.TryGetValue(key, out var value) && bool.TryParse(value, out var result)
                ? result
                : null;

        private static void CompileWeaveFile(SourceProductionContext context, string path, SourceText text)
        {
            if (text != null)
            {
                var parseResult = CompileManager.ParseTemplate(text.ToString(), path);
                var hadFatal = ReportErrors(context, parseResult);
                if (hadFatal)
                {
                    return;
                }

                var compileResult = WeaveCompiler.Compile(parseResult.Result);
                hadFatal = ReportErrors(context, compileResult);
                if (hadFatal)
                {
                    return;
                }

                context.AddSource(Path.GetFileName(path) + ".g.cs", compileResult.Code);
            }
        }

        private static bool ReportErrors<T>(SourceProductionContext context, CompileResult<T> result)
        {
            var hadFatal = false;
            foreach (var error in result.Errors)
            {
                hadFatal |= !error.IsWarning;
                context.ReportDiagnostic(ConvertToDiagnostic(error));
            }

            return hadFatal;
        }

        private static Diagnostic ConvertToDiagnostic(CompilerError error)
        {
            var (severity, level) = error.IsWarning
                ? (DiagnosticSeverity.Warning, 1)
                : (DiagnosticSeverity.Error, 0);

            return Diagnostic.Create(
                error.ErrorNumber,
                nameof(Weave),
                error.ErrorText,
                severity,
                severity,
                isEnabledByDefault: true,
                warningLevel: level);
        }
    }
}
