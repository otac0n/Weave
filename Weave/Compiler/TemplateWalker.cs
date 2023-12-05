// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Weave.Expressions;

    internal abstract class TemplateWalker
    {
        public virtual void WalkBodyElement(BodyElement bodyElement)
        {
            this.WalkElements(bodyElement.Body);
        }

        public virtual void WalkBranch(Branch branch)
        {
            this.WalkElements(branch.Body);
        }

        public virtual void WalkCodeElement(CodeElement codeElement)
        {
        }

        public virtual void WalkEachElement(EachElement eachElement)
        {
            this.WalkElements(eachElement.EachBody.Body);

            if (eachElement.DelimitBody != null)
            {
                this.WalkElements(eachElement.DelimitBody.Body);
            }

            if (eachElement.NoneBody != null)
            {
                this.WalkElements(eachElement.NoneBody.Body);
            }
        }

        public virtual void WalkEchoTag(EchoTag echoTag)
        {
        }

        [SuppressMessage("Usage", "RS1035:Do not use APIs banned for analyzers", Justification = "This is a fall-back mainly intended for tests. It is not a code path that is expected to be hit.")]
        public virtual void WalkElement(Element element)
        {
            if (element is BodyElement bodyElement)
            {
                this.WalkBodyElement(bodyElement);
            }
            else if (element is CodeElement codeElement)
            {
                this.WalkCodeElement(codeElement);
            }
            else if (element is EachElement eachElement)
            {
                this.WalkEachElement(eachElement);
            }
            else if (element is EchoTag echoTag)
            {
                this.WalkEchoTag(echoTag);
            }
            else if (element is IfElement ifElement)
            {
                this.WalkIfElement(ifElement);
            }
            else if (element is IndentationElement indentationElement)
            {
                this.WalkIndentationElement(indentationElement);
            }
            else if (element is NewLineElement newLineElement)
            {
                this.WalkNewLineElement(newLineElement);
            }
            else if (element is RenderElement renderElement)
            {
                this.WalkRenderElement(renderElement);
            }
            else if (element is TextElement textElement)
            {
                this.WalkTextElement(textElement);
            }
            else if (element is WrapIfElement wrapIfElement)
            {
                this.WalkWrapIfElement(wrapIfElement);
            }
            else
            {
                throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, "Unimplemented element '{0}'.", element.GetType().Name));
            }
        }

        public virtual void WalkElements(IEnumerable<Element> elements)
        {
            foreach (var element in elements)
            {
                this.WalkElement(element);
            }
        }

        public virtual void WalkIfElement(IfElement ifElement)
        {
            foreach (var branch in ifElement.Branches)
            {
                this.WalkBranch(branch);
            }
        }

        public virtual void WalkIndentationElement(IndentationElement indentationElement)
        {
        }

        public virtual void WalkNewLineElement(NewLineElement newLineElement)
        {
        }

        public virtual void WalkRenderElement(RenderElement renderElement)
        {
        }

        public virtual void WalkTemplate(Template template)
        {
            this.WalkElements(template.Elements);
        }

        public virtual void WalkTextElement(TextElement textElement)
        {
        }

        public virtual void WalkWrapIfElement(WrapIfElement wrapIfElement)
        {
            this.WalkElements(wrapIfElement.Before);
            this.WalkElement(wrapIfElement.Body);
            this.WalkElements(wrapIfElement.After);
        }
    }
}
