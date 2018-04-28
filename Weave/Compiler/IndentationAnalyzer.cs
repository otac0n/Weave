// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Weave.Expressions;

    /// <summary>
    /// Computes indentation for all nodes in a <see cref="Template"/>.
    /// </summary>
    public static class IndentationAnalyzer
    {
        /// <summary>
        /// Analyzes a <see cref="Template"/>'s indentation.
        /// </summary>
        /// <param name="template">The <see cref="Template"/> to analyze.</param>
        /// <returns>The indentation for the entire <see cref="Template"/>.</returns>
        public static Dictionary<Element, Tuple<Element, string>> Analyze(Template template)
        {
            var walker = new MeasureIndentationWalker();
            walker.WalkTemplate(template);
            return walker.Results;
        }

        private class MeasureIndentationWalker : TemplateWalker
        {
            private int amountToSubtract = 0;
            private Element baseElement;
            private Dictionary<Element, Tuple<Element, string>> results = new Dictionary<Element, Tuple<Element, string>>();

            public Dictionary<Element, Tuple<Element, string>> Results => this.results;

            public override void WalkBodyElement(BodyElement bodyElement)
            {
                var amount = GetIndentationOffset(bodyElement.Indentation, bodyElement.Body);
                this.amountToSubtract += amount;
                base.WalkBodyElement(bodyElement);
                this.amountToSubtract -= amount;
            }

            public override void WalkBranch(Branch branch)
            {
                var amount = GetIndentationOffset(branch.Indentation, branch.Body);
                this.amountToSubtract += amount;
                base.WalkBranch(branch);
                this.amountToSubtract -= amount;
            }

            public override void WalkCodeElement(CodeElement codeElement)
            {
                var indent = this.ComputeIndentation(codeElement.Indentation);
                if (indent != null)
                {
                    this.results[codeElement] = Tuple.Create(this.baseElement, indent);
                }
            }

            public override void WalkEachElement(EachElement eachElement)
            {
                var amount = GetIndentationOffset(eachElement.EachBody.Indentation, eachElement.EachBody.Body);
                this.amountToSubtract += amount;
                this.WalkElements(eachElement.EachBody.Body);
                this.amountToSubtract -= amount;

                if (eachElement.DelimitBody != null)
                {
                    amount = GetIndentationOffset(eachElement.DelimitBody.Indentation, eachElement.DelimitBody.Body);
                    this.amountToSubtract += amount;
                    this.WalkElements(eachElement.DelimitBody.Body);
                    this.amountToSubtract -= amount;
                }

                if (eachElement.NoneBody != null)
                {
                    amount = GetIndentationOffset(eachElement.NoneBody.Indentation, eachElement.NoneBody.Body);
                    this.amountToSubtract += amount;
                    this.WalkElements(eachElement.NoneBody.Body);
                    this.amountToSubtract -= amount;
                }
            }

            public override void WalkIndentationElement(IndentationElement indentationElement)
            {
                var indent = this.ComputeIndentation(indentationElement.Indentation);
                if (indent != null)
                {
                    this.results[indentationElement] = Tuple.Create(this.baseElement, indent);
                }
            }

            public override void WalkRenderElement(RenderElement renderElement)
            {
                var indent = this.ComputeIndentation(renderElement.Indentation);
                if (indent != null)
                {
                    this.results[renderElement] = Tuple.Create(this.baseElement, indent);
                }
            }

            public override void WalkWrapIfElement(WrapIfElement wrapIfElement)
            {
                var amount = GetIndentationOffset(wrapIfElement.Indentation, wrapIfElement.Before.Concat(new[] { wrapIfElement.Body }).Concat(wrapIfElement.After));
                this.amountToSubtract += amount;
                this.WalkElements(wrapIfElement.Before);

                if (wrapIfElement.Body.Indentation == null)
                {
                    this.WalkElement(wrapIfElement.Body);
                }
                else
                {
                    var originalBaseElement = this.baseElement;
                    var originalAmountToSubtract = this.amountToSubtract;

                    var trueIndentation = this.ComputeIndentation(wrapIfElement.Body.Indentation);
                    this.amountToSubtract -= amount;
                    this.amountToSubtract += Math.Max(0, MeasureString(wrapIfElement.Body.Indentation) - MeasureString(wrapIfElement.Indentation));
                    var falseIndentation = this.ComputeIndentation(wrapIfElement.Body.Indentation);
                    this.results[wrapIfElement] = Tuple.Create(this.baseElement, falseIndentation);
                    this.results[wrapIfElement.Body] = Tuple.Create(this.baseElement, trueIndentation);

                    this.baseElement = wrapIfElement.Body;
                    this.amountToSubtract = MeasureString(wrapIfElement.Body.Indentation);
                    this.WalkElement(wrapIfElement.Body);

                    this.baseElement = originalBaseElement;
                    this.amountToSubtract = originalAmountToSubtract;
                }

                this.WalkElements(wrapIfElement.After);
                this.amountToSubtract -= amount;
            }

            private static string FindIndentation(Element element)
            {
                if (element is BodyElement bodyElement)
                {
                    return bodyElement.Indentation;
                }
                else if (element is CodeElement codeElement)
                {
                    return codeElement.Indentation;
                }
                else if (element is EachElement eachElement)
                {
                    return eachElement.EachBody.Indentation;
                }
                else if (element is IfElement ifElement)
                {
                    return ifElement.Branches[0].Indentation;
                }
                else if (element is IndentationElement indentationElement)
                {
                    return indentationElement.Indentation;
                }
                else if (element is RenderElement renderElement)
                {
                    return renderElement.Indentation;
                }
                else if (element is WrapIfElement wrapIfElement)
                {
                    return wrapIfElement.Indentation;
                }

                return null;
            }

            private static int GetIndentationOffset(string indentation, IEnumerable<Element> body)
            {
                if (indentation == null)
                {
                    return 0;
                }

                var ourIndentation = MeasureString(indentation);

                return (from element in body
                        let offset = MeasureString(FindIndentation(element)) - ourIndentation
                        where offset > 0
                        orderby offset
                        select offset).FirstOrDefault();
            }

            private static int MeasureString(string indentation, int tabWidth = 4)
            {
                var count = 0;

                if (indentation == null)
                {
                    return count;
                }

                for (var i = 0; i < indentation.Length; i++)
                {
                    if (indentation[i] == '\t')
                    {
                        count += tabWidth - (count % tabWidth);
                    }
                    else
                    {
                        count++;
                    }
                }

                return count;
            }

            private static string TrimIndentation(string indentation, int amoutToTrim, int tabWidth = 4)
            {
                var count = MeasureString(indentation, tabWidth);
                var desiredCount = count - amoutToTrim;

                if (desiredCount <= 0)
                {
                    return string.Empty;
                }

                var newCount = 0;
                var i = 0;

                for (i = 0; i < indentation.Length; i++)
                {
                    var previousCount = newCount;

                    if (indentation[i] == '\t')
                    {
                        newCount += tabWidth - (newCount % tabWidth);
                    }
                    else
                    {
                        newCount++;
                    }

                    if (newCount > desiredCount)
                    {
                        newCount = previousCount;
                        break;
                    }
                }

                return indentation.Substring(0, i) + new string(' ', desiredCount - newCount);
            }

            private string ComputeIndentation(string indentation)
            {
                if (indentation == null)
                {
                    return null;
                }

                if (this.amountToSubtract > 0)
                {
                    indentation = TrimIndentation(indentation, this.amountToSubtract);
                }

                return indentation;
            }
        }
    }
}
