// -----------------------------------------------------------------------
// <copyright file="CompilePass.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using Weave.Expressions;

    internal abstract class CompilePass
    {
        public abstract void Run(Template template, CompileResult result);
    }
}
