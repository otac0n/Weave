// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave
{
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;
    using Weave.Compiler;
    using Weave.Expressions;
    using WeaveTemplateItem = (string path, Microsoft.CodeAnalysis.Text.SourceText text, bool useSourceGeneration, bool? configFileExists);

    [Generator]
    internal class GenerateWeaveSources : IIncrementalGenerator
    {
        private const string UseSourceGeneration = "build_metadata.WeaveTemplateGenerate.UseSourceGeneration";
        private const string ConfigFileExists = "build_metadata.WeaveTemplateGenerate.ConfigFileExists";

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
                        UseSourceGeneration = GetBooleanOption(options, UseSourceGeneration),
                    };
                })
                .Where(p => p.UseSourceGeneration != null)
                .Select((p, ct) => (
                    path: p.AdditionalText.Path,
                    text: p.AdditionalText.GetText(ct),
                    useSourceGeneration: p.UseSourceGeneration.Value,
                    configFileExists: GetBooleanOption(p.ConfigOptions, ConfigFileExists)));

            // TODO: Go ahead and compile _config files, but make sure that we emit a compile warning (using a pragma directive and a [Deprecated] attribute) about future removal.
            var sourceGeneratedFiles = weaveFiles
                .Where(p => p.useSourceGeneration);
            var allConfigFiles = weaveFiles
                .Where(p => Path.GetFileName(p.path) == CompileManager.ConfigFileName) // TODO: Case sensitivity can vary by filesystem.
                .Collect();

            var filesWithConfigs = sourceGeneratedFiles
                .Combine(allConfigFiles)
                .Select((p, ct) =>
                {
                    var configPath = Path.Combine(Path.GetDirectoryName(p.Left.path), CompileManager.ConfigFileName);
                    return new
                    {
                        Source = p.Left,
                        Config = p.Left.configFileExists ?? true
                            ? p.Right.Where(f => f.path == configPath).SingleOrDefault() // TODO: Case sensitivity can vary by filesystem.
                            : default(WeaveTemplateItem?),
                    };
                });

            context.RegisterSourceOutput(
                filesWithConfigs,
                (context, pair) => CompileWeaveFile(context, pair.Source, pair.Config));
        }

        private static bool? GetBooleanOption(AnalyzerConfigOptions options, string key) =>
            options.TryGetValue(key, out var value) && bool.TryParse(value, out var result)
                ? result
                : null;

        private static void CompileWeaveFile(SourceProductionContext context, WeaveTemplateItem template, WeaveTemplateItem? config)
        {
            if (template.text != null)
            {
                var parseResult = CompileManager.ParseTemplate(template.text.ToString(), template.path);
                var hadFatal = ReportErrors(context, parseResult);
                if (hadFatal)
                {
                    return;
                }

                var parsedTemplate = parseResult.Result;
                if (template.configFileExists ?? (config != null))
                {
                    if (config is WeaveTemplateItem configItem)
                    {
                        var configResult = CompileManager.ParseTemplate(configItem.text.ToString(), configItem.path);
                        hadFatal = ReportErrors(context, configResult);
                        if (hadFatal)
                        {
                            return;
                        }

                        parsedTemplate = new Template(parsedTemplate, configResult.Result);
                    }
                    else
                    {
                        // TODO: Report a fatal error about failing to locate the config in additional items.
                    }
                }

                var compileResult = WeaveCompiler.Compile(parsedTemplate);
                hadFatal = ReportErrors(context, compileResult);
                if (hadFatal)
                {
                    return;
                }

                context.AddSource(Path.GetFileName(template.path) + ".g.cs", compileResult.Code);
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

            var linePosition = new LinePosition(error.Line - 1, error.Column - 1);

            return Diagnostic.Create(
                error.ErrorNumber,
                nameof(Weave),
                error.ErrorText,
                severity,
                severity,
                isEnabledByDefault: true,
                warningLevel: level,
                location: Location.Create(error.FileName, new TextSpan(0, 0), new LinePositionSpan(linePosition, linePosition)));
        }
    }
}
