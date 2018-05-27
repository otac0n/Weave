// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Tests.IntegrationTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class WhiteSpaceHandlingTests
    {
        public static readonly IList<object[]> OriginalIndentation = new List<object[]>
        {
            new[] { string.Empty },
            new[] { "    " },
            new[] { "\t" },
        };

        public static readonly IList<object[]> WrapIfTestCases =
            (from indentation in OriginalIndentation
             from condition in new[] { true, false }
             from leading in new[] { "", " ", "    " }
             from wrapIfNested in new[] { " ", "    " }
             from body in new[] { "", " ", "    " }
             from bodyNested in new[] { "", " ", "    " }
             select indentation.Concat(new object[] { condition, leading, wrapIfNested, body, bodyNested }).ToArray()).ToList();

        [Fact]
        public void EachBlockWithBlankLineDelimiter_EmitsTightlySpacedBlock()
        {
            var template = StringUtilities.JoinLines(
                "{",
                "    {{each i in model}}",
                "        {{= i }}",
                "    {{delimit}}",
                "",
                "    {{/each}}",
                "}");
            var model = Enumerable.Range(0, 3);

            var result = TemplateHelper.Render(template, model);

            var expected = StringUtilities.JoinLines(
                "{",
                "    0",
                "",
                "    1",
                "",
                "    2",
                "}");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfBlockWithAnInlineEndTag_EmitsAllWhiteSpaceWhenTheConditionIsMet()
        {
            var template = StringUtilities.JoinLines(
                "foo",
                "{{if model}}",
                "    baz {{/if}}",
                "baz");
            var model = true;

            var result = TemplateHelper.Render(template, model);

            var expected = StringUtilities.JoinLines(
                "foo",
                "",
                "    baz ",
                "baz");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfBlockWithAnInlineEndTag_EmitsSurroundingWhiteSpaceWhenTheConditionIsNotMet()
        {
            var template = StringUtilities.JoinLines(
                "foo",
                "{{if model}}",
                "    baz {{/if}}",
                "baz");
            var model = false;

            var result = TemplateHelper.Render(template, model);

            var expected = StringUtilities.JoinLines(
                "foo",
                "",
                "baz");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfBlockWithAnInlineStartTag_EmitsAllWhiteSpaceWhenTheConditionIsMet()
        {
            var template = StringUtilities.JoinLines(
                "foo",
                "    bar {{if model}}",
                "    baz",
                "{{/if}}",
                "baz");
            var model = true;

            var result = TemplateHelper.Render(template, model);

            var expected = StringUtilities.JoinLines(
                "foo",
                "    bar ",
                "    baz",
                "",
                "baz");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfBlockWithAnInlineStartTag_EmitsSurroundingWhiteSpaceWhenTheConditionIsNotMet()
        {
            var template = StringUtilities.JoinLines(
                "foo",
                "    bar {{if model}}",
                "    baz",
                "{{/if}}",
                "baz");
            var model = false;

            var result = TemplateHelper.Render(template, model);

            var expected = StringUtilities.JoinLines(
                "foo",
                "    bar ",
                "baz");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfStatementWithInconsistentIndenting_ChoosesSmallestNonzeroIndentation()
        {
            var template = StringUtilities.JoinLines(
                "    {{if true}}",
                "        foo",
                "      foo",
                "       foo",
                "    foo",
                "    {{/if}}");

            var result = TemplateHelper.Render(template, null);

            var expected = StringUtilities.JoinLines(
                "      foo",
                "    foo",
                "     foo",
                "  foo",
                "");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfStatementWithMixedTabsAndSpaces_TrimsFromTheRightWhenRemovingWhiteSpace()
        {
            var template = StringUtilities.JoinLines(
                "@tabsize 4",
                "    {{if true}}",
                "\t\ta",
                " \t \tb",
                "  \t \tc",
                "   \t \td",
                "     \te",
                " \t \tf",
                " \t  \tg",
                " \t   \th",
                " \t    i",
                "    {{/if}}");

            var result = TemplateHelper.Render(template, null);

            var expected = StringUtilities.JoinLines(
                "\ta",
                " \tb",
                "  \tc",
                "   \td",
                "    e",
                " \tf",
                " \tg",
                " \th",
                " \ti",
                "");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IfStatementWithMixedTabsAndSpacesAndIndentationsThatIsNotAnEvenMultipleOfTheTabSize_TrimsFromTheRightWhenRemovingWhiteSpace()
        {
            var template = StringUtilities.JoinLines(
                "@tabsize 4",
                "     {{if true}}",
                "\t\ta",
                "\t \tb",
                "\t  \tc",
                "\t   \td",
                "\t    e",
                "\t\t f",
                "\t \t g",
                "\t  \t h",
                "\t   \t i",
                "\t     j",
                "     {{/if}}");

            var result = TemplateHelper.Render(template, null);

            var expected = StringUtilities.JoinLines(
                "\t a",
                "\t b",
                "\t c",
                "\t d",
                "\t e",
                "\t  f",
                "\t  g",
                "\t  h",
                "\t  i",
                "\t  j",
                "");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InlineEachBlockWithDelimiterContainingNewLine_EmitsExpectedIndentation()
        {
            var template = StringUtilities.JoinLines(
                "{{if true}}",
                "    var foo = {{each i in model}}x[{{= i }}]{{delimit}} ??",
                "              {{/each}};",
                "{{/if}}",
                "");
            var model = Enumerable.Range(0, 3);

            var result = TemplateHelper.Render(template, model);

            var expected = StringUtilities.JoinLines(
                "var foo = x[0] ??",
                "          x[1] ??",
                "          x[2];",
                "");
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(OriginalIndentation))]
        public void InlineWrapIfBlock_EmitsExpectedIndentation(string indentation)
        {
            var template = "    {{wrapif false}}outer{{body}}inner{{/body}}/outer{{/wrapif}}";

            var result = TemplateHelper.Render(template, null, indentation);

            Assert.Equal($"{indentation}    inner", result);
        }

        [Fact]
        public void NestedIfStatements_StripAllIndentation()
        {
            var template = StringUtilities.JoinLines(
                "{{if true}}",
                "    {{if true}}",
                "        foo",
                "    {{/if}}",
                "{{/if}}");

            var result = TemplateHelper.Render(template, null);

            var expected = StringUtilities.JoinLines(
                "foo",
                "");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleEachBlockWithASingleLineBody_EmitsASingleLinePerIterationOfTheBody()
        {
            var template = StringUtilities.JoinLines(
                "foo",
                "{{each i in model}}",
                "    bar",
                "{{/each}}",
                "baz");
            var model = Enumerable.Range(0, 3);

            var result = TemplateHelper.Render(template, model);

            var expected = StringUtilities.JoinLines(
                "foo",
                "bar",
                "bar",
                "bar",
                "baz");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleIfBlockWithASingleLineBody_EmitsASingleLineWhenTheConditionIsMet()
        {
            var template = StringUtilities.JoinLines(
                "foo",
                "{{if model}}",
                "    bar",
                "{{/if}}",
                "baz");
            var model = true;

            var result = TemplateHelper.Render(template, model);

            var expected = StringUtilities.JoinLines(
                "foo",
                "bar",
                "baz");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void StatementBlockAtTheTop_DoesNotCauseAPrecedingNewLineToBeEmitted()
        {
            var template = StringUtilities.JoinLines(
                "{{",
                "    /* no-op */",
                "}}",
                "<!DOCTYPE html>");

            var result = TemplateHelper.Render(template, null);

            Assert.Equal("<!DOCTYPE html>", result);
        }

        [Fact]
        public void StatementBlockLineWithExcessInternalWhiteSpace_DoesNotEmitAnyExternalWhiteSpace()
        {
            var template = StringUtilities.JoinLines(
                "    {{",
                "",
                "   /* no-op */",
                "",
                "               }}    ");

            var result = TemplateHelper.Render(template, null);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void StatementBlockOnALineOfItsOwn_DoesNotEmitItselfOrTheLine()
        {
            var template = StringUtilities.JoinLines(
                "foo",
                "    {{ /* no-op */ }}    ",
                "bar");

            var result = TemplateHelper.Render(template, null);

            var expected = StringUtilities.JoinLines(
                "foo",
                "bar");
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(OriginalIndentation))]
        public void WrapIfBlock_DoesNotBreakOptimizationsWhenTheBodyMatchesTheIndentationOfTheBeforeAndWrapIfElements(string indentation)
        {
            var template = StringUtilities.JoinLines(
                "{{wrapif false}}",
                "outer",
                "{{body}}",
                "    inner",
                "{{/body}}",
                "/outer",
                "{{/wrapif}}");

            var result = TemplateHelper.Render(template, null, indentation);

            var expected = StringUtilities.JoinLines(
                $"{indentation}inner",
                $"");
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(WrapIfTestCases))]
        public void WrapIfBlock_EmitsLeadingIndentationAndBodyIndentationAppropriately(string indentation, bool condition, string leading, string wrapIfNested, string body, string bodyNested)
        {
            var template = StringUtilities.JoinLines(
                leading + "{{wrapif model}}",
                leading + wrapIfNested + "outer",
                leading + wrapIfNested + body + "{{body}}",
                leading + wrapIfNested + body + bodyNested + "inner",
                leading + wrapIfNested + body + "{{/body}}",
                leading + wrapIfNested + "/outer",
                leading + "{{/wrapif}}");
            var model = condition;

            var result = TemplateHelper.Render(template, model, indentation);

            var expected = condition
                ? StringUtilities.JoinLines(
                    $"{indentation}{leading}outer",
                    $"{indentation}{leading}{body}inner",
                    $"{indentation}{leading}/outer",
                    $"")
                : StringUtilities.JoinLines(
                    $"{indentation}{leading}inner",
                    $"");
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(OriginalIndentation))]
        public void WrapIfBlock_EmitsLeadingIndentationWhenTheConditionIsFalseAndTheWrapIfElementIsAlsoWrapped(string indentation)
        {
            var template = StringUtilities.JoinLines(
                "    {{if true}}",
                "            {{wrapif false}}",
                "                outer",
                "                    {{body}}",
                "                        inner",
                "                    {{/body}}",
                "                /outer",
                "            {{/wrapif}}",
                "    {{/if}}");

            var result = TemplateHelper.Render(template, null, indentation);

            var expected = StringUtilities.JoinLines(
                $"{indentation}    inner",
                $"");
            Assert.Equal(expected, result);
        }
    }
}
