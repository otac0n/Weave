// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Tests.IntegrationTests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    public class WhiteSpaceHandlingTests
    {
        public static readonly IList<string> OriginalIndentation = new List<string> { string.Empty, "    ", "\t" };

        [Test]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "{",
                "    0",
                "",
                "    1",
                "",
                "    2",
                "}")));
        }

        [Test]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "var foo = x[0] ??",
                "          x[1] ??",
                "          x[2];",
                "")));
        }

        [Test]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "foo",
                "bar",
                "bar",
                "bar",
                "baz")));
        }

        [Test]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "foo",
                "bar",
                "baz")));
        }

        [Test]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "foo",
                "    bar ",
                "    baz",
                "",
                "baz")));
        }

        [Test]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "foo",
                "    bar ",
                "baz")));
        }

        [Test]
        public void IfBlockWithAnInlineEndTag_EmitsAllWhiteSpaceWhenTheConditionIsMet()
        {
            var template = StringUtilities.JoinLines(
                "foo",
                "{{if model}}",
                "    baz {{/if}}",
                "baz");
            var model = true;

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "foo",
                "",
                "    baz ",
                "baz")));
        }

        [Test]
        public void IfBlockWithAnInlineEndTag_EmitsSurroundingWhiteSpaceWhenTheConditionIsNotMet()
        {
            var template = StringUtilities.JoinLines(
                "foo",
                "{{if model}}",
                "    baz {{/if}}",
                "baz");
            var model = false;

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "foo",
                "",
                "baz")));
        }

        [Test]
        public void StatementBlockOnALineOfItsOwn_DoesNotEmitItselfOrTheLine()
        {
            var template = StringUtilities.JoinLines(
                "foo",
                "    {{ /* no-op */ }}    ",
                "bar");

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "foo",
                "bar")));
        }

        [Test]
        public void StatementBlockLineWithExcessInternalWhiteSpace_DoesNotEmitAnyExternalWhiteSpace()
        {
            var template = StringUtilities.JoinLines(
                "    {{",
                "",
                "   /* no-op */",
                "",
                "               }}    ");

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void StatementBlockAtTheTop_DoesNotCauseAPrecedingNewLineToBeEmitted()
        {
            var template = StringUtilities.JoinLines(
                "{{",
                "    /* no-op */",
                "}}",
                "<!DOCTYPE html>");

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("<!DOCTYPE html>"));
        }

        [Test]
        public void NestedIfStatements_StripAllIndentation()
        {
            var template = StringUtilities.JoinLines(
                "{{if true}}",
                "    {{if true}}",
                "        foo",
                "    {{/if}}",
                "{{/if}}");

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "foo",
                "")));
        }

        [Test]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "      foo",
                "    foo",
                "     foo",
                "  foo",
                "")));
        }

        [Test]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                "\ta",
                " \tb",
                "  \tc",
                "   \td",
                "    e",
                " \tf",
                " \tg",
                " \th",
                " \ti",
                "")));
        }

        [Test]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
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
                "")));
        }

        [Test]
        public void WrapIfBlock_EmitsLeadingIndentationAndBodyIndentationAppropriately(
            [ValueSource("OriginalIndentation")] string indentation,
            [Values(true, false)] bool condition,
            [Values("", " ", "    ")] string leading,
            [Values(" ", "    ")] string wrapIfNested,
            [Values("", " ", "    ")] string body,
            [Values("", " ", "    ")] string bodyNested)
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

            Assert.That(result, Is.EqualTo(
                condition
                    ? StringUtilities.JoinLines(
                        $"{indentation}{leading}outer",
                        $"{indentation}{leading}{body}inner",
                        $"{indentation}{leading}/outer",
                        $"")
                    : StringUtilities.JoinLines(
                        $"{indentation}{leading}inner",
                        $"")));
        }

        [TestCaseSource("OriginalIndentation")]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                $"{indentation}    inner",
                $"")));
        }

        [TestCaseSource("OriginalIndentation")]
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

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                $"{indentation}inner",
                $"")));
        }

        [TestCaseSource("OriginalIndentation")]
        public void InlineWrapIfBlock_EmitsExpectedIndentation(string indentation)
        {
            var template = "    {{wrapif false}}outer{{body}}inner{{/body}}/outer{{/wrapif}}";

            var result = TemplateHelper.Render(template, null, indentation);

            Assert.That(result, Is.EqualTo($"{indentation}    inner"));
        }
    }
}
