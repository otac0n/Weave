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

        public Templates(TextWriter writer)
        {
            this.writer = writer;
        }

        public override void WalkTemplate(Template template)
        {
            this.RenderTemplate(template, this.writer);
        }

        public override void WalkCodeElement(CodeElement codeElement)
        {
            this.RenderCodeElement(codeElement, this.writer);
        }

        public override void WalkIfTag(IfTag ifTag)
        {
            this.RenderIfTag(ifTag, this.writer);
        }

        public override void WalkBranch(Branch branch)
        {
            this.RenderBranch(branch, this.writer);
        }

        public override void WalkEachTag(EachTag eachTag)
        {
            this.RenderEachTag(eachTag, this.writer);
        }

        public override void WalkEchoTag(EchoTag echoTag)
        {
            this.RenderEchoTag(echoTag, this.writer);
        }

        public override void WalkTextElement(TextElement textElement)
        {
            this.RenderTextElement(textElement, this.writer);
        }

        private static string ToLiteral(string input)
        {
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
    }
}
