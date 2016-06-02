// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
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

            var toRemove = new List<Element>();

            foreach (var node in graph)
            {
                string indent;
                indentation.TryGetValue(node.Value, out indent);

                if (indent != null)
                {
                    var predecessorsWithIndentation = node.FindFirstPredecessors(p => indentation.ContainsKey(p.Value)).ToList();

                    var allPredecessorsHaveSameIndentation = predecessorsWithIndentation.Any()
                        ? predecessorsWithIndentation.All(p => indentation[p.Value] == indent)
                        : indent.Length == 0;

                    if (allPredecessorsHaveSameIndentation)
                    {
                        toRemove.Add(node.Value);
                    }
                }
            }

            foreach (var e in toRemove)
            {
                indentation.Remove(e);
            }
        }
    }
}
