// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO.Abstractions;
    using System.Text.RegularExpressions;
    using Pegasus.Common;
    using Weave.Compiler;
    using Weave.Expressions;
    using Weave.Parser;

    internal class CompileManager
    {
        private readonly IFileSystem fileSystem;

        public CompileManager(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public static void CompileFile(string inputFileName, string outputFileName, Action<CompilerError> logError)
        {
            var fileSystem = new FileSystem();
            var compileManager = new CompileManager(fileSystem);
            outputFileName = outputFileName ?? inputFileName + ".cs";
            var result = compileManager.CompileFile(inputFileName);

            var hadFatal = false;
            foreach (var error in result.Errors)
            {
                hadFatal |= !error.IsWarning;
                logError(error);
            }

            if (!hadFatal)
            {
                fileSystem.Directory.CreateDirectory(fileSystem.Path.GetDirectoryName(outputFileName));
                fileSystem.File.WriteAllText(outputFileName, result.Result);
            }
        }

        public CompileResult CompileFile(string inputFileName)
        {
            var inputResult = this.ParseTemplate(inputFileName);
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

        private CompileResult<Template> ParseTemplate(string inputFileName)
        {
            var content = this.fileSystem.File.ReadAllText(inputFileName);
            return this.ParseTemplate(content, inputFileName);

            /* TODO: _config
            var configFile = Path.Combine(Path.GetDirectoryName(inputFile), "_config.weave");

            if (this.fileSystem.File.Exists(configFile))
            {
                var config = ParseTemplate(configFile);
                if (config == null)
                {
                    return;
                }

                template = new Template(template, config);
            }
            */
        }

        private CompileResult<Template> ParseTemplate(string inputFileContents, string inputFileName)
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
    }
}
