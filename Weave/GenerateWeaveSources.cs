// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;
    using Weave.Compiler;
    using WeaveTemplateItem = (string path, Microsoft.CodeAnalysis.Text.SourceText text, bool useSourceGeneration, bool? configFileExists);

    [Generator]
    internal class GenerateWeaveSources : IIncrementalGenerator
    {
        private const string UseSourceGeneration = "build_metadata.WeaveTemplateGenerate.UseSourceGeneration";
        private const string ConfigFileExists = "build_metadata.WeaveTemplateGenerate.ConfigFileExists";
        private const string ProjectDir = "build_property.projectdir";
        private const string GeneratedOutputPath = "build_property.compilergeneratedfilesoutputpath";

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

            var outputPath = context.AnalyzerConfigOptionsProvider
                .Select((p, ct) =>
                {
                    var gOptions = p.GlobalOptions.Keys.ToDictionary(k => k, k => p.GlobalOptions.TryGetValue(k, out var value) ? value : null);
                    p.GlobalOptions.TryGetValue(ProjectDir, out var root);
                    p.GlobalOptions.TryGetValue(GeneratedOutputPath, out var generated);
                    var assembly = Assembly.GetExecutingAssembly().FullName;
                    var generator = typeof(GenerateWeaveSources).FullName;
                    return Path.GetFullPath(Path.Combine(root, generated, assembly, generator));
                });

            var filesWithConfigs = sourceGeneratedFiles
                .Combine(allConfigFiles)
                .Combine(outputPath)
                .Select((p, ct) =>
                {
                    var source = p.Left.Left;
                    var allConfigs = p.Left.Right;
                    var outputPath = p.Right;
                    var configPath = Path.Combine(Path.GetDirectoryName(source.path), CompileManager.ConfigFileName);
                    return new
                    {
                        Source = source,
                        Config = source.configFileExists ?? true
                            ? allConfigs.Where(f => f.path == configPath).SingleOrDefault() // TODO: Case sensitivity can vary by filesystem.
                            : default(WeaveTemplateItem?),
                        OutputPath = outputPath,
                    };
                });

            context.RegisterSourceOutput(
                filesWithConfigs,
                (context, pair) => CompileWeaveFile(context, pair.Source, pair.Config, pair.OutputPath));
        }

        private static bool? GetBooleanOption(AnalyzerConfigOptions options, string key) =>
            options.TryGetValue(key, out var value) && bool.TryParse(value, out var result)
                ? result
                : null;

        private static void CompileWeaveFile(SourceProductionContext context, WeaveTemplateItem template, WeaveTemplateItem? config, string outputPath)
        {
            if (template.text != null)
            {
                var relativePath = PathUtils.MakeRelative(outputPath, template.path);
                var templateResult = CompileManager.ParseTemplate(template.text.ToString(), relativePath);
                if (template.configFileExists ?? (config != null))
                {
                    if (config is WeaveTemplateItem configItem)
                    {
                        var configResult = CompileManager.ParseTemplate(configItem.text.ToString(), PathUtils.MakeRelative(outputPath, configItem.path));
                        templateResult = CompileManager.CombineTemplateConfig(templateResult, configResult);
                    }
                    else
                    {
                        // TODO: Report a fatal error about failing to locate the config in additional items.
                        ReportErrors(context, templateResult);
                        return;
                    }
                }

                var hadFatal = ReportErrors(context, templateResult);
                if (hadFatal)
                {
                    return;
                }

                var compileResult = WeaveCompiler.Compile(templateResult.Result);
                hadFatal = ReportErrors(context, compileResult);
                if (hadFatal)
                {
                    return;
                }

                context.AddSource(PathUtils.EncodePathAsFilename(relativePath) + ".g.cs", compileResult.Code);
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

        private static class PathUtils
        {
            public static string MakeRelative(string root, string path)
            {
                root = Path.GetFullPath(root);
                var fullFilePath = Path.GetFullPath(Path.Combine(root, path));
                var relativeUri = new Uri(root).MakeRelativeUri(new Uri(fullFilePath));
                var relativePath = Uri.UnescapeDataString(relativeUri.ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                return relativePath;
            }

            public static string EncodePathAsFilename(string path) =>
                Uri.EscapeDataString(path).Replace("_", "__").Replace("%", "_p");
        }
    }
}
