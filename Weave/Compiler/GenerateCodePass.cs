// -----------------------------------------------------------------------
// <copyright file="GenerateCodePass.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using Weave.Expressions;

    internal class GenerateCodePass : CompilePass
    {
        public override void Run(Template template, CompileResult result)
        {
            result.Code = template.ToString();
        }
    }
}
