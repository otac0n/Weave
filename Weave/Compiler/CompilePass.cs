// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Compiler
{
    using System.Collections.Generic;
    using Weave.Expressions;

    internal abstract class CompilePass
    {
        public abstract IList<string> ErrorsProduced { get; }

        public abstract IList<string> BlockedByErrors { get; }

        public abstract void Run(Template template, CompileResult result);
    }
}
