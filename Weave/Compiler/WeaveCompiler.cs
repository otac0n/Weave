// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

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
    public static class WeaveCompiler
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
        public static CompileResult Compile(Template template)
        {
            var result = new CompileResult();

            var passes = PassTypes.Select(t => (CompilePass)Activator.CreateInstance(t)).ToList();
            while (true)
            {
                var existingErrors = new HashSet<string>(result.Errors.Select(e => e.ErrorNumber));
                var pendingErrors = new HashSet<string>(passes.SelectMany(p => p.ErrorsProduced));

                var nextPasses = passes
                    .Where(p => !p.BlockedByErrors.Any(e => existingErrors.Contains(e)))
                    .Where(p => !p.BlockedByErrors.Any(e => pendingErrors.Contains(e)))
                    .ToList();

                if (nextPasses.Count == 0)
                {
                    break;
                }

                foreach (var pass in nextPasses)
                {
                    pass.Run(template, result);
                    passes.Remove(pass);
                }
            }

            return result;
        }
    }
}
