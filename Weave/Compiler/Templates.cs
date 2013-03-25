// -----------------------------------------------------------------------
// <copyright file="Templates.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Weave.Expressions;

    internal partial class Templates : TemplateWalker
    {
        private static Dictionary<char, string> simpleEscapeChars = new Dictionary<char, string>()
        {
            { '\'', "\\'" }, { '\"', "\\\"" }, { '\\', "\\\\" }, { '\0', "\\0" },
            { '\a', "\\a" }, { '\b', "\\b" }, { '\f', "\\f" }, { '\n', "\\n" },
            { '\r', "\\r" }, { '\t', "\\t" }, { '\v', "\\v" },
        };

        private readonly Dictionary<string, int> variables = new Dictionary<string, int>();
        private readonly TextWriter writer;

        private string currentIndentation = string.Empty;

        private string lastIndentation = string.Empty;
        private int amountToSubtract = 0;

        public Templates(TextWriter writer)
        {
            this.writer = writer;
        }

        public override void WalkTemplate(Template template)
        {
            this.RenderTemplate(template, this.writer, this.currentIndentation);
        }

        public override void WalkCodeElement(CodeElement codeElement)
        {
            this.RenderCodeElement(codeElement, this.writer, this.currentIndentation);
        }

        public override void WalkIfElement(IfElement ifElement)
        {
            this.RenderIfElement(ifElement, this.writer, this.currentIndentation);
        }

        public override void WalkBranch(Branch branch)
        {
            this.RenderBranch(branch, this.writer, this.currentIndentation);
        }

        public override void WalkEachElement(EachElement eachElement)
        {
            this.RenderEachElement(eachElement, this.writer, this.currentIndentation);
        }

        public override void WalkEchoTag(EchoTag echoTag)
        {
            this.RenderEchoTag(echoTag, this.writer, this.currentIndentation);
        }

        public override void WalkNewLineElement(NewLineElement newLineElement)
        {
            this.RenderNewLineElement(newLineElement, this.writer, this.currentIndentation);
        }

        public override void WalkRenderElement(RenderElement renderElement)
        {
            this.RenderRenderElement(renderElement, this.writer, this.currentIndentation);
        }

        public override void WalkTextElement(TextElement textElement)
        {
            this.RenderTextElement(textElement, this.writer, this.currentIndentation);
        }

        public override void WalkIndentationElement(IndentationElement indentationElement)
        {
            this.RenderIndentationElement(indentationElement, this.writer, this.currentIndentation);
        }

        private static string FindIndentation(Element element)
        {
            CodeElement codeElement;
            EachElement eachElement;
            IfElement ifElement;
            IndentationElement indentationElement;
            RenderElement renderElement;

            if ((codeElement = element as CodeElement) != null)
            {
                return codeElement.Indentation;
            }
            else if ((eachElement = element as EachElement) != null)
            {
                return eachElement.EachBody.Indentation;
            }
            else if ((ifElement = element as IfElement) != null)
            {
                return ifElement.Branches[0].Indentation;
            }
            else if ((indentationElement = element as IndentationElement) != null)
            {
                return indentationElement.Indentation;
            }
            else if ((renderElement = element as RenderElement) != null)
            {
                return renderElement.Indentation;
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
                    select (int?)offset).FirstOrDefault() ?? 0;
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

        private static string ToLiteral(string input)
        {
            if (input == null)
            {
                return "default(string)";
            }

            var sb = new StringBuilder(input.Length * 2);
            sb.Append("\"");
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];

                string literal;
                if (simpleEscapeChars.TryGetValue(c, out literal))
                {
                    sb.Append(literal);
                }
                else if (c >= 32 && c <= 126)
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append("\\u").Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
                }
            }

            sb.Append("\"");
            return sb.ToString();
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

        private string CreateVariable(string prefix)
        {
            int instance;
            this.variables.TryGetValue(prefix, out instance);
            this.variables[prefix] = instance + 1;
            return prefix + instance;
        }

        private void UpdateIndentation(string model, TextWriter writer, string indentation)
        {
            if (model == null)
            {
                return;
            }

            if (this.amountToSubtract > 0)
            {
                model = TrimIndentation(model, this.amountToSubtract);
            }

            if (model != this.lastIndentation)
            {
                writer.Write(indentation);
                writer.Write("indentation = originalIndentation");

                if (model.Length > 0)
                {
                    writer.Write(" + ");
                    writer.Write(ToLiteral(model));
                }

                writer.Write(";");
                writer.WriteLine();

                this.lastIndentation = model;
            }
        }

        private void BaseWalkTemplate(Template template, TextWriter writer, string indentation)
        {
            var temp = this.currentIndentation;
            this.currentIndentation = indentation;
            base.WalkTemplate(template);
            this.currentIndentation = temp;
        }

        private void WalkElements(IEnumerable<Element> elements, TextWriter writer, string indentation)
        {
            var temp = this.currentIndentation;
            this.currentIndentation = indentation;
            this.WalkElements(elements);
            this.currentIndentation = temp;
        }

        private void WalkBranch(Branch branch, TextWriter writer, string indentation)
        {
            var temp = this.currentIndentation;
            this.currentIndentation = indentation;
            this.WalkBranch(branch);
            this.currentIndentation = temp;
        }
    }
}
