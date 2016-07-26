// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal static class StringUtilities
    {
        public static string JoinLines(params string[] lines)
        {
            return JoinLines(lines.AsEnumerable());
        }

        public static string JoinLines(IEnumerable<string> lines)
        {
            var sb = new StringBuilder();
            var first = true;
            foreach (var line in lines)
            {
                if (!first)
                {
                    sb.AppendLine();
                }
                else
                {
                    first = false;
                }

                sb.Append(line);
            }

            return sb.ToString();
        }
    }
}
