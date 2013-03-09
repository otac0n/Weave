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
            foreach (var arg in args)
            {
                var input = File.ReadAllText(arg);

                var parser = new WeaveParser();
                var parsed = parser.Parse(input, arg);

                var output = WeaveCompiler.Compile(parsed);

                File.WriteAllText(arg + ".cs", output.Code);
            }
        }
    }
}
