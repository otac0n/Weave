// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave
{
    using System.IO;
    using Weave.Compiler;
    using Weave.Parser;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllText(args[0]);

            var parser = new WeaveParser();
            var parsed = parser.Parse(input);

            var compiler = new WeaveCompiler();
            var output = compiler.Compile(parsed);

            File.WriteAllText(args[0] + ".cs", output.Code);
        }
    }
}
