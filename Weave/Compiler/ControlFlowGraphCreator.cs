// -----------------------------------------------------------------------
// <copyright file="ControlFlowGraphCreator.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
    using Weave.Expressions;

    /// <summary>
    /// Encapsulates creation of control flow graphs for the <see cref="Template"/> class.
    /// </summary>
    public static class ControlFlowGraphCreator
    {
        /// <summary>
        /// Creates a control flow graph for the given <see cref="Template"/>.
        /// </summary>
        /// <param name="template">The <see cref="Template"/> to analyze.</param>
        /// <returns>The newly created graph.</returns>
        public static ControlFlowGraph<Element> Create(Template template)
        {
            var walker = new Walker();
            walker.WalkTemplate(template);
            return walker.Graph;
        }

        private class Walker : TemplateWalker
        {
            private ControlFlowGraph<Element> graph = new ControlFlowGraph<Element>();

            private List<Element> previous = new List<Element>();

            public ControlFlowGraph<Element> Graph
            {
                get { return this.graph; }
            }

            public override void WalkEachElement(EachElement eachElement)
            {
                this.previous.Clear();

                this.graph.AddEdge(eachElement, eachElement.EachBody);
                this.previous.Add(eachElement.EachBody);

                this.WalkElements(eachElement.EachBody.Body);

                foreach (var prev in this.previous)
                {
                    this.graph.AddEdge(prev, eachElement.EachBody);
                }

                this.previous.Clear();

                if (eachElement.NoneBody != null)
                {
                    this.graph.AddEdge(eachElement, eachElement.NoneBody);
                    this.previous.Add(eachElement.NoneBody);

                    this.WalkElements(eachElement.NoneBody.Body);
                }

                this.previous.Insert(0, eachElement.EachBody);
            }

            public override void WalkElement(Element element)
            {
                foreach (var prev in this.previous)
                {
                    this.graph.AddEdge(prev, element);
                }

                this.previous.Clear();
                this.previous.Add(element);

                base.WalkElement(element);
            }

            public override void WalkIfElement(IfElement ifElement)
            {
                this.previous.Clear();

                var previous = new List<Element>();

                foreach (var branch in ifElement.Branches)
                {
                    this.graph.AddEdge(ifElement, branch);
                    this.previous.Add(branch);

                    this.WalkBranch(branch);

                    previous.AddRange(this.previous);
                    this.previous.Clear();
                }

                if (ifElement.Branches.Last().Expression != null)
                {
                    this.previous.Add(ifElement);
                }

                this.previous.AddRange(previous);
            }
        }
    }
}
