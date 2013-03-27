// -----------------------------------------------------------------------
// <copyright file="GraphOperations.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains operations for working with <see cref="ControlFlowGraph&lt;T&gt;"/> objects.
    /// </summary>
    public static class GraphOperations
    {
        /// <summary>
        /// Does a breadth-first search for nodes preceding the given node that match the predicate, stopping before it passes a matched node.
        /// </summary>
        /// <typeparam name="T">The type of graph.</typeparam>
        /// <param name="node">The node to use as a starting point.</param>
        /// <param name="predicate">The criteria to match.</param>
        /// <returns>An enumerable collection of nodes that match the predicate.</returns>
        public static IEnumerable<ControlFlowGraph<T>.Node> FindFirstPredecessors<T>(this ControlFlowGraph<T>.Node node, Func<ControlFlowGraph<T>.Node, bool> predicate)
        {
            var visited = new HashSet<ControlFlowGraph<T>.Node>(node.Previous);
            var toVisit = new Queue<ControlFlowGraph<T>.Node>(visited);
            visited.Add(node);

            while (toVisit.Count > 0)
            {
                var subject = toVisit.Dequeue();
                if (predicate(subject))
                {
                    yield return subject;
                }
                else
                {
                    foreach (var next in subject.Previous)
                    {
                        if (visited.Add(next))
                        {
                            toVisit.Enqueue(next);
                        }
                    }
                }
            }
        }
    }
}
