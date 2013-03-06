// -----------------------------------------------------------------------
// <copyright file="CompileResult.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;

    /// <summary>
    /// Encapsulates the results and errors from the compilation of a Weave template.
    /// </summary>
    public class CompileResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompileResult"/> class.
        /// </summary>
        public CompileResult()
        {
            this.Errors = new List<CompilerError>();
        }

        /// <summary>
        /// Gets the collection of errors that occurred during compilation.
        /// </summary>
        public IList<CompilerError> Errors { get; private set; }

        /// <summary>
        /// Gets or sets the code resulting from compilation.
        /// </summary>
        public string Code { get; set; }
    }
}
