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
            new GenerateCodeWalker(result).WalkTemplate(template);
        }

        private class GenerateCodeWalker : TemplateWalker
        {
            private readonly CompileResult result;

            public GenerateCodeWalker(CompileResult result)
            {
                this.result = result;
            }

            public override void WalkTemplate(Template template)
            {
                this.result.Code = template.ToString();
            }
        }
    }
}
