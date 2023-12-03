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
        public static readonly string ConfigFileName = "_config.weave";

        public static CompileResult<Template> ParseTemplate(string inputFileContents, string inputFileName)
        {
            var parser = new WeaveParser();
            var compileResult = new CompileResult<Template>();
            try
            {
                compileResult.Result = parser.Parse(inputFileContents, fileName: inputFileName);
            }
            catch (FormatException ex)
            {
                var cursor = ex.Data["cursor"] as Cursor;
                if (cursor != null && Regex.IsMatch(ex.Message, @"^WEAVE\d+:"))
                {
                    var parts = ex.Message.Split(new[] { ':' }, 2);
                    compileResult.Errors.Add(new CompilerError(cursor.FileName, cursor.Line, cursor.Column, parts[0], parts[1]));
                }
                else
                {
                    throw;
                }
            }

            return compileResult;
        }

        public static CompileResult<Template> CombineTemplateConfig(CompileResult<Template> template, CompileResult<Template> config)
        {
            if (config == null || (config.Result == null && config.Errors.Count == 0))
            {
                return template;
            }

            var result = new CompileResult<Template>
            {
                Result = new Template(template.Result, config.Result),
            };

            foreach (var error in config.Errors)
            {
                result.Errors.Add(error);
            }

            foreach (var error in result.Errors)
            {
                result.Errors.Add(error);
            }

            return result;
        }

        public static void CompileFile(string inputFileName, string outputFileName, Action<CompilerError> logError)
        {
            inputFileName = Path.GetFullPath(inputFileName);
            outputFileName = Path.GetFullPath(outputFileName ?? inputFileName + ".cs");
            var result = CompileFile(inputFileName, outputFileName);

            var hadFatal = false;
            foreach (var error in result.Errors)
            {
                hadFatal |= !error.IsWarning;
                logError(error);
            }

            if (!hadFatal)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFileName));
                File.WriteAllText(outputFileName, result.Result);
            }
        }

        private static CompileResult CompileFile(string inputFileName, string outputFileName)
        {
            var inputResult = ParseTemplateInternal(inputFileName, outputFileName);
            var inputTemplate = inputResult.Result;
            if (inputTemplate == null)
            {
                var errorResult = new CompileResult();
                foreach (var error in inputResult.Errors)
                {
                    errorResult.Errors.Add(error);
                }

                return errorResult;
            }

            return WeaveCompiler.Compile(inputTemplate);
        }

        private static CompileResult<Template> ParseTemplateInternal(string inputFileName, string outputFileName)
        {
            var relativePath = PathUtils.MakeRelative(outputFileName, inputFileName);
            var templateResult = ParseTemplate(File.ReadAllText(inputFileName), relativePath);

            var configFileName = Path.Combine(Path.GetDirectoryName(inputFileName), ConfigFileName);
            if (File.Exists(configFileName))
            {
                var configResult = ParseTemplate(File.ReadAllText(configFileName), PathUtils.MakeRelative(outputFileName, configFileName));
                templateResult = CombineTemplateConfig(templateResult, configResult);
            }

            return templateResult;
        }
    }
}
