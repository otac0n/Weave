// -----------------------------------------------------------------------
// <copyright file="TemplateHelper.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Tests
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.IO;
    using Microsoft.CSharp;
    using Weave.Compiler;
    using Weave.Parser;

    internal static class TemplateHelper
    {
        private static readonly CSharpCodeProvider Compiler;
        private static readonly CompilerParameters Options;

        static TemplateHelper()
        {
            Compiler = new CSharpCodeProvider();
            Options = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
            };
            Options.ReferencedAssemblies.Add("System.dll");
            Options.ReferencedAssemblies.Add("System.Core.dll");
            Options.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
        }

        public static string Render(string template, object model)
        {
            template = "@namespace Tests\n" + template;

            var result = WeaveCompiler.Compile(new WeaveParser().Parse(template));
            var compilationUnit = new CodeSnippetCompileUnit(result.Code);
            var results = Compiler.CompileAssemblyFromDom(Options, compilationUnit);
            var assembly = results.CompiledAssembly;
            var type = assembly.GetType("Tests.Templates");
            var method = type.GetMethod("Render");

            using (var writer = new StringWriter())
            {
                var instance = Activator.CreateInstance(type);
                method.Invoke(instance, new[] { model, writer });
                return writer.ToString();
            }
        }
    }
}
