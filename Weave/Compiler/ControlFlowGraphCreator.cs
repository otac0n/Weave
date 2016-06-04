// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Compiler
{
    using System;
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
                var noneBodyTail = new List<Element>();
                if (eachElement.NoneBody != null)
                {
                    this.Interpose(eachElement.NoneBody, () =>
                    {
                        this.WalkElements(eachElement.NoneBody.Body);
                        noneBodyTail.AddRange(this.previous);
                    });
                }
                else
                {
                    noneBodyTail.AddRange(this.previous);
                }

                var eachBodyTail = new List<Element>();
                this.Interpose(eachElement.EachBody, () =>
                {
                    this.WalkElements(eachElement.EachBody.Body);

                    eachBodyTail.AddRange(this.previous);

                    if (eachElement.DelimitBody != null)
                    {
                        this.Interpose(eachElement.DelimitBody, () =>
                        {
                            this.WalkElements(eachElement.DelimitBody.Body);

                            foreach (var prev in this.previous)
                            {
                                this.graph.AddEdge(prev, eachElement.EachBody);
                            }
                        });
                    }
                    else
                    {
                        foreach (var prev in this.previous)
                        {
                            this.graph.AddEdge(prev, eachElement.EachBody);
                        }
                    }
                });

                this.previous.Clear();
                this.previous.AddRange(eachBodyTail);
                this.previous.AddRange(noneBodyTail);
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

            public override void WalkWrapIfElement(WrapIfElement wrapIfElement)
            {
                base.WalkWrapIfElement(wrapIfElement);
                this.graph.AddEdge(wrapIfElement, wrapIfElement.Body);
            }

            private void Interpose(Element next, Action action)
            {
                // Wire up the list of previous to the next.
                var originalPrevious = this.previous.ToList();
                foreach (var prev in originalPrevious)
                {
                    this.graph.AddEdge(prev, next);
                }

                // Set up for the action.
                this.previous.Clear();
                this.previous.Add(next);

                action();

                // Reset to the original state.
                this.previous.Clear();
                this.previous.AddRange(originalPrevious);
            }
        }
    }
}
