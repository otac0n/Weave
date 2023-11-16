// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave
{
    using System;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    [Generator]
    internal class GenerateWeaveSources : IIncrementalGenerator
    {
        private const string OutputPath = "build_metadata.WeaveTemplate.OutputPath";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var weaveFiles = context.AdditionalTextsProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Where(p => p.Right.GetOptions(p.Left).Keys.Contains(OutputPath, StringComparer.InvariantCultureIgnoreCase))
                .Select((p, ct) => (p.Left.Path, Text: p.Left.GetText(ct)));

            context.RegisterSourceOutput(
                weaveFiles,
                static (context, file) => CompileWeaveFile(context, file.Path, file.Text));
        }

        private static void CompileWeaveFile(SourceProductionContext context, string path, SourceText? text)
        {
            if (text != null)
            {
                context.AddSource(Path.GetFileName(path) + ".g.cs", CompileManager.Compile(text.ToString(), path));
            }
        }
    }
}
