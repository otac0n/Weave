// -----------------------------------------------------------------------
// <copyright file="TemplateWalker.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Weave.Expressions;

    internal abstract class TemplateWalker
    {
        public virtual void WalkBranch(Branch branch)
        {
            this.WalkElements(branch.Body);
        }

        public virtual void WalkEachTag(EachTag eachTag)
        {
            this.WalkElements(eachTag.Body);

            if (eachTag.NoneBody != null)
            {
                this.WalkElements(eachTag.NoneBody);
            }
        }

        public virtual void WalkEchoTag(EchoTag echoTag)
        {
        }

        public virtual void WalkElement(Element element)
        {
            CodeElement codeElement;
            EachTag eachTag;
            EchoTag echoTag;
            IfTag ifTag;
            TextElement textElement;

            if ((codeElement = element as CodeElement) != null)
            {
                this.WalkCodeElement(codeElement);
            }
            else if ((eachTag = element as EachTag) != null)
            {
                this.WalkEachTag(eachTag);
            }
            else if ((echoTag = element as EchoTag) != null)
            {
                this.WalkEchoTag(echoTag);
            }
            else if ((ifTag = element as IfTag) != null)
            {
                this.WalkIfTag(ifTag);
            }
            else if ((textElement = element as TextElement) != null)
            {
                this.WalkTextElement(textElement);
            }
            else
            {
                throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, "Unimplemented element '{0}'.", element.GetType().Name));
            }
        }

        public virtual void WalkCodeElement(CodeElement codeElement)
        {
        }

        public virtual void WalkElements(IEnumerable<Element> elements)
        {
            foreach (var element in elements)
            {
                this.WalkElement(element);
            }
        }

        public virtual void WalkIfTag(IfTag ifTag)
        {
            foreach (var branch in ifTag.Branches)
            {
                this.WalkBranch(branch);
            }
        }

        public virtual void WalkTemplate(Template template)
        {
            this.WalkElements(template.Elements);
        }

        public virtual void WalkTextElement(TextElement textElement)
        {
        }
    }
}
