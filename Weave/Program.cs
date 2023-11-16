// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
#if NETSTANDARD
            Console.WriteLine("The dotnet standard version of this application is intended to be used as a source generator. You may use the dotnet 4 or dotnet core version of this assembly to generate source code in a stand-alone way.");
#else
            foreach (var arg in args)
            {
                CompileManager.CompileFile(arg, null, Console.WriteLine);
            }
#endif
        }
    }
}
