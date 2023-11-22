// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Compiler
{
    /// <summary>
    /// Encapsulates the results and errors from the compilation of a Weave template.
    /// </summary>
    public class CompileResult : CompileResult<string>
    {
        /// <summary>
        /// Gets or sets the code resulting from compilation.
        /// </summary>
        public string Code
        {
            get => this.Result;
            set => this.Result = value;
        }
    }
}
