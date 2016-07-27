// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Text.RegularExpressions;
    using Pegasus.Common;
    using Weave.Compiler;
    using Weave.Expressions;
    using Weave.Parser;

    internal static class CompileManager
    {
        public static void CompileFile(string inputFile, string outputFile, Action<CompilerError> logError)
        {
            outputFile = outputFile ?? inputFile + ".cs";
            var configFile = Path.Combine(Path.GetDirectoryName(inputFile), "_config.weave");

            var template = ParseTemplate(inputFile, logError);
            if (template == null)
            {
                return;
            }

            if (File.Exists(configFile))
            {
                var config = ParseTemplate(configFile, logError);
                if (config == null)
                {
                    return;
                }

                template = new Template(template, config);
            }

            var result = WeaveCompiler.Compile(template);

            bool hadFatal = false;
            foreach (var error in result.Errors)
            {
                hadFatal |= !error.IsWarning;
                logError(error);
            }

            if (!hadFatal)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                File.WriteAllText(outputFile, result.Code);
            }
        }

        private static Template ParseTemplate(string inputFile, Action<CompilerError> logError)
        {
            var subject = File.ReadAllText(inputFile);
            var parser = new WeaveParser();
            try
            {
                return parser.Parse(subject, fileName: inputFile);
            }
            catch (FormatException ex)
            {
                var cursor = ex.Data["cursor"] as Cursor;
                if (cursor != null && Regex.IsMatch(ex.Message, @"^WEAVE\d+:"))
                {
                    var parts = ex.Message.Split(new[] { ':' }, 2);
                    logError(new CompilerError(cursor.FileName, cursor.Line, cursor.Column, parts[0], parts[1]));
                    return null;
                }

                throw;
            }
        }
    }
}
