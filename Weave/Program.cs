// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                CompileManager.CompileFile(arg, null, Console.WriteLine);
            }
        }
    }
}
