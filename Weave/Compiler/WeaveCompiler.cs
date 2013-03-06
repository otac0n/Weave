// -----------------------------------------------------------------------
// <copyright file="WeaveCompiler.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Weave.Expressions;

    /// <summary>
    /// Provides error checking and compilation services for Weave templates.
    /// </summary>
    public class WeaveCompiler
    {
        private static readonly IList<Type> PassTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(CompilePass)))
            .ToList()
            .AsReadOnly();

        /// <summary>
        /// Compiles a Weave template into a program.
        /// </summary>
        /// <param name="template">The template to compile.</param>
        /// <returns>A <see cref="CompileResult"/> containing the errors, warnings, and results of compilation.</returns>
        public CompileResult Compile(Template template)
        {
            var result = new CompileResult();

            var passes = PassTypes.Select(t => (CompilePass)Activator.CreateInstance(t)).ToList();
            foreach (var pass in passes)
            {
                pass.Run(template, result);
            }

            return result;
        }
    }
}
