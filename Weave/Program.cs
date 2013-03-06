﻿// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave
{
    using System;
    using System.IO;
    using Weave.Parser;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllText(args[0]);
            var parser = new WeaveParser();
            var output = parser.Parse(input);
            Console.WriteLine(output);
        }
    }
}
