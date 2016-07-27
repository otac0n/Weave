// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

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
