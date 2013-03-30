// -----------------------------------------------------------------------
// <copyright file="GenerateCodePass.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Weave.Expressions;

    internal class GenerateCodePass : CompilePass
    {
        public override IList<string> ErrorsProduced
        {
            get { return new string[0]; }
        }

        public override IList<string> BlockedByErrors
        {
            get { return new[] { "WEAVE0001", "WEAVE0003", "WEAVE0004", "WEAVE0005" }; }
        }

        public override void Run(Template template, CompileResult result)
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new Templates(stringWriter).WalkTemplate(template);
                result.Code = stringWriter.ToString();
            }
        }
    }
}
