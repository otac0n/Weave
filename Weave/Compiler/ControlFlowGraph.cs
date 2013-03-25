// -----------------------------------------------------------------------
// <copyright file="ControlFlowGraph.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Represents a double-linked directed graph of nodes.
    /// </summary>
    /// <typeparam name="T">The type of the values of the nodes in the graph.</typeparam>
    [DebuggerDisplay("Graph(Count = {nodes.Count})")]
    public class ControlFlowGraph<T>
    {
        private Dictionary<T, Node> nodes = new Dictionary<T, Node>();

        /// <summary>
        /// Gets the corresponding node for a given value.
        /// </summary>
        /// <param name="value">The value to look up.</param>
        /// <returns>The corresponding <see cref="Node"/>.</returns>
        public Node this[T value]
        {
            get
            {
                return this.nodes[value];
            }
        }

        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="from">The preceding value.</param>
        /// <param name="to">The successive value.</param>
        public void AddEdge(T from, T to)
        {
            var fromNode = this.EnsureNode(from);
            var toNode = this.EnsureNode(to);

            toNode.AddPrevious(fromNode);
            fromNode.AddNext(toNode);
        }

        private Node EnsureNode(T value)
        {
            Node node;
            if (!this.nodes.TryGetValue(value, out node))
            {
                this.nodes.Add(value, node = new Node(this, value));
            }

            return node;
        }

        /// <summary>
        /// Represents a node in a <see cref="ControlFlowGraph&lt;T&gt;"/>.
        /// </summary>
        [DebuggerDisplay("Node({Value}, Previous = {previous.Count}, Next = {next.Count})")]
        public sealed class Node
        {
            private readonly ControlFlowGraph<T> graph;
            private readonly List<Node> next;
            private readonly ICollection<Node> nextPublic;
            private readonly List<Node> previous;
            private readonly ICollection<Node> previousPublic;
            private readonly T value;

            internal Node(ControlFlowGraph<T> graph, T value)
            {
                this.graph = graph;
                this.next = new List<Node>();
                this.nextPublic = this.next.AsReadOnly();
                this.previous = new List<Node>();
                this.previousPublic = this.previous.AsReadOnly();
                this.value = value;
            }

            /// <summary>
            /// Gets the <see cref="ControlFlowGraph&lt;T&gt;"/> to which this node belongs.
            /// </summary>
            public ControlFlowGraph<T> Graph
            {
                get { return this.graph; }
            }

            /// <summary>
            /// Gets a collection of successive nodes.
            /// </summary>
            public ICollection<Node> Next
            {
                get { return this.nextPublic; }
            }

            /// <summary>
            /// Gets a collection of preceding nodes.
            /// </summary>
            public ICollection<Node> Previous
            {
                get { return this.previousPublic; }
            }

            /// <summary>
            /// Gets the value that this node corresponds to.
            /// </summary>
            public T Value
            {
                get { return this.value; }
            }

            internal void AddPrevious(Node node)
            {
                this.previous.Add(node);
            }

            internal void AddNext(Node node)
            {
                this.next.Add(node);
            }
        }
    }
}
