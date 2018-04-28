// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Compiler
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a double-linked directed graph of nodes.
    /// </summary>
    /// <typeparam name="T">The type of the values of the nodes in the graph.</typeparam>
    [DebuggerDisplay("Graph(Count = {nodes.Count})")]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "'Graph' in the name of this class takes the place of 'Collection'.")]
    public class ControlFlowGraph<T> : IEnumerable<ControlFlowGraph<T>.Node>
    {
        private Dictionary<T, Node> nodes = new Dictionary<T, Node>();

        /// <summary>
        /// Gets the corresponding node for a given value.
        /// </summary>
        /// <param name="value">The value to look up.</param>
        /// <returns>The corresponding <see cref="Node"/>.</returns>
        public Node this[T value] => this.nodes[value];

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

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ControlFlowGraph&lt;T&gt;"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator&lt;Node&gt;"/> for the <see cref="ControlFlowGraph&lt;T&gt;"/>.</returns>
        public IEnumerator<Node> GetEnumerator()
        {
            return this.nodes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private Node EnsureNode(T value)
        {
            if (!this.nodes.TryGetValue(value, out var node))
            {
                this.nodes.Add(value, node = new Node(this, value));
            }

            return node;
        }

        /// <summary>
        /// Represents a node in a <see cref="ControlFlowGraph&lt;T&gt;"/>.
        /// </summary>
        [DebuggerDisplay("Node({Value}, Previous = {previous.Count}, Next = {next.Count})")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "This is a clean way of dealing with graph nodes.")]
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
            public ControlFlowGraph<T> Graph => this.graph;

            /// <summary>
            /// Gets a collection of successive nodes.
            /// </summary>
            public ICollection<Node> Next => this.nextPublic;

            /// <summary>
            /// Gets a collection of preceding nodes.
            /// </summary>
            public ICollection<Node> Previous => this.previousPublic;

            /// <summary>
            /// Gets the value that this node corresponds to.
            /// </summary>
            public T Value => this.value;

            internal void AddNext(Node node)
            {
                this.next.Add(node);
            }

            internal void AddPrevious(Node node)
            {
                this.previous.Add(node);
            }
        }
    }
}
