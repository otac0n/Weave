// -----------------------------------------------------------------------
// <copyright file="GenerateCodePass.cs" company="(none)">
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

    internal class GenerateCodePass : CompilePass
    {
        public override void Run(Template template, CompileResult result)
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new GenerateCodeWalker(stringWriter).WalkTemplate(template);
                result.Code = stringWriter.ToString();
            }
        }

        private class GenerateCodeWalker : TemplateWalker
        {
            private static Dictionary<char, string> simpleEscapeChars = new Dictionary<char, string>()
            {
                { '\'', "\\'" }, { '\"', "\\\"" }, { '\\', "\\\\" }, { '\0', "\\0" },
                { '\a', "\\a" }, { '\b', "\\b" }, { '\f', "\\f" }, { '\n', "\\n" },
                { '\r', "\\r" }, { '\t', "\\t" }, { '\v', "\\v" },
            };

            private readonly TextWriter writer;
            private readonly Dictionary<string, int> variables = new Dictionary<string, int>();

            public GenerateCodeWalker(TextWriter writer)
            {
                this.writer = writer;
            }

            public override void WalkIfTag(IfTag ifTag)
            {
                var first = true;
                foreach (var branch in ifTag.Branches)
                {
                    if (!first)
                    {
                        this.writer.Write("else ");
                    }

                    this.WalkBranch(branch);

                    first = false;
                }
            }

            public override void WalkBranch(Branch branch)
            {
                if (branch.Expression != null)
                {
                    this.writer.Write("if (" + branch.Expression + ")\r\n");
                }

                this.writer.Write("{\r\n");
                this.WalkElements(branch.Body);
                this.writer.Write("}\r\n");
            }

            public override void WalkEachTag(EachTag eachTag)
            {
                var flag = this.CreateVariable("_flag");

                if (eachTag.NoneBody != null)
                {
                    this.writer.Write("bool " + flag + ";\r\n");
                }

                this.writer.Write("foreach (" + eachTag.Expression + ")\r\n{\r\n");

                if (eachTag.NoneBody != null)
                {
                    this.writer.Write(flag + " = true;\r\n");
                }

                this.WalkElements(eachTag.Body);
                this.writer.Write("}\r\n");

                if (eachTag.NoneBody != null)
                {
                    this.writer.Write("if (!" + flag + ")\r\n{\r\n");
                    this.WalkElements(eachTag.NoneBody);
                    this.writer.Write("}\r\n");
                }
            }

            public override void WalkEchoTag(EchoTag echoTag)
            {
                this.writer.Write("writer.Write(" + echoTag.Expression + ");\r\n");
            }

            public override void WalkTextElement(TextElement textElement)
            {
                this.writer.Write("writer.Write(" + ToLiteral(textElement.Value) + ");\r\n");
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
}
