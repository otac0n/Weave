namespace Weave.Compiler
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    internal partial class Templates : TemplateWalker
    {
        private static Dictionary<char, string> simpleEscapeChars = new Dictionary<char, string>()
        {
            { '\'', "\\'" }, { '\"', "\\\"" }, { '\\', "\\\\" }, { '\0', "\\0" },
            { '\a', "\\a" }, { '\b', "\\b" }, { '\f', "\\f" }, { '\n', "\\n" },
            { '\r', "\\r" }, { '\t', "\\t" }, { '\v', "\\v" },
        };

        private readonly Dictionary<string, int> variables = new Dictionary<string, int>();

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
