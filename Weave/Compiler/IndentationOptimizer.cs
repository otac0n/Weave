// -----------------------------------------------------------------------
// <copyright file="IndentationOptimizer.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using System.Collections.Generic;
    using Weave.Expressions;

    /// <summary>
    /// Optimizes the placement of indentation in a <see cref="Template"/>.
    /// </summary>
    public static class IndentationOptimizer
    {
        /// <summary>
        /// Optimizes the placement of indentation.
        /// </summary>
        /// <param name="indentation">The collection of indentation to modify.</param>
        /// <param name="template">The associated <see cref="Template"/> for the given indentation collection.</param>
        public static void Optimize(Dictionary<Element, string> indentation, Template template)
        {
            var graph = ControlFlowGraphCreator.Create(template);
        }
    }
}
