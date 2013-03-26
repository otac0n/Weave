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

        private Dictionary<Element, string> indentation;

        public Templates(TextWriter writer)
        {
            this.writer = writer;
        }

        public override void WalkTemplate(Template template)
        {
            this.indentation = IndentationAnalyzer.Analyze(template);

            var graph = ControlFlowGraphCreator.Create(template);

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

        private string CreateVariable(string prefix)
        {
            int instance;
            this.variables.TryGetValue(prefix, out instance);
            this.variables[prefix] = instance + 1;
            return prefix + instance;
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
