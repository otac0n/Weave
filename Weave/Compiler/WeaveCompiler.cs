// -----------------------------------------------------------------------
// <copyright file="WeaveCompiler.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using Weave.Expressions;

    /// <summary>
    /// Provides error checking and compilation services for Weave templates.
    /// </summary>
    public class WeaveCompiler
    {
        /// <summary>
        /// Compiles a Weave template into a program.
        /// </summary>
        /// <param name="template">The template to compile.</param>
        /// <returns>A <see cref="CompileResult"/> containing the errors, warnings, and results of compilation.</returns>
        public CompileResult Compile(Template template)
        {
            var result = new CompileResult();

            result.Code = template.ToString();

            return result;
        }
    }
}
