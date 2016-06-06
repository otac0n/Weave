// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Tests.IntegrationTests
{
    using System.Linq;
    using NUnit.Framework;

    public class WhiteSpaceHandlingTests
    {
        [Test]
        public void EachBlockWithBlankLineDelimiter_EmitsTightlySpacedBlock()
        {
            var template = "{\n    {{each i in model}}\n        {{= i }}\n    {{delimit}}\n\n    {{/each}}\n}";
            var model = Enumerable.Range(0, 3);

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("{\n    0\n\n    1\n\n    2\n}"));
        }

        [Test]
        public void InlineEachBlockWithDelimiterContainingNewLine_EmitsExpectedIndentation()
        {
            var template = "{{if true}}\n    var foo = {{each i in model}}x[{{= i }}]{{delimit}} ??\n              {{/each}};\n{{/if}}\n";
            var model = Enumerable.Range(0, 3);

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("var foo = x[0] ??\n          x[1] ??\n          x[2];\n"));
        }

        [Test]
        public void SimpleEachBlockWithASingleLineBody_EmitsASingleLinePerIterationOfTheBody()
        {
            var template = "foo\n{{each i in model}}\n    bar\n{{/each}}\nbaz";
            var model = Enumerable.Range(0, 3);

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("foo\nbar\nbar\nbar\nbaz"));
        }

        [Test]
        public void SimpleIfBlockWithASingleLineBody_EmitsASingleLineWhenTheConditionIsMet()
        {
            var template = "foo\n{{if model}}\n    bar\n{{/if}}\nbaz";
            var model = true;

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("foo\nbar\nbaz"));
        }

        [Test]
        public void IfBlockWithAnInlineStartTag_EmitsAllWhiteSpaceWhenTheConditionIsMet()
        {
            var template = "foo\n    bar {{if model}}\n    baz\n{{/if}}\nbaz";
            var model = true;

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("foo\n    bar \n    baz\n\nbaz"));
        }

        [Test]
        public void IfBlockWithAnInlineStartTag_EmitsSurroundingWhiteSpaceWhenTheConditionIsNotMet()
        {
            var template = "foo\n    bar {{if model}}\n    baz\n{{/if}}\nbaz";
            var model = false;

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("foo\n    bar \nbaz"));
        }

        [Test]
        public void IfBlockWithAnInlineEndTag_EmitsAllWhiteSpaceWhenTheConditionIsMet()
        {
            var template = "foo\n{{if model}}\n    baz {{/if}}\nbaz";
            var model = true;

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("foo\n\n    baz \nbaz"));
        }

        [Test]
        public void IfBlockWithAnInlineEndTag_EmitsSurroundingWhiteSpaceWhenTheConditionIsNotMet()
        {
            var template = "foo\n{{if model}}\n    baz {{/if}}\nbaz";
            var model = false;

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("foo\n\nbaz"));
        }

        [Test]
        public void StatementBlockOnALineOfItsOwn_DoesNotEmitItselfOrTheLine()
        {
            var template = "foo\n    {{ /* no-op */ }}    \r\nbar";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("foo\nbar"));
        }

        [Test]
        public void StatementBlockLineWithExcessInternalWhiteSpace_DoesNotEmitAnyExternalWhiteSpace()
        {
            var template = "    {{\n\n   /* no-op */\n\n               }}    ";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void StatementBlockAtTheTop_DoesNotCauseAPrecedingNewLineToBeEmitted()
        {
            var template = "{{\n    /* no-op */\n}}\n<!DOCTYPE html>";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("<!DOCTYPE html>"));
        }

        [Test]
        public void NestedIfStatements_StripAllIndentation()
        {
            var template = "{{if true}}\n    {{if true}}\n        foo\n    {{/if}}\n{{/if}}";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("foo\n"));
        }

        [Test]
        public void IfStatementWithInconsistentIndenting_ChoosesSmallestNonzeroIndentation()
        {
            var template = "    {{if true}}\n        foo\n      foo\n       foo\n    foo\n    {{/if}}";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("      foo\n    foo\n     foo\n  foo\n"));
        }

        [Test]
        public void IfStatementWithMixedTabsAndSpaces_TrimsFromTheRightWhenRemovingWhiteSpace()
        {
            var template = "@tabsize 4\n    {{if true}}\n\t\ta\n \t \tb\n  \t \tc\n   \t \td\n     \te\n \t \tf\n \t  \tg\n \t   \th\n \t    i\n    {{/if}}";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("\ta\n \tb\n  \tc\n   \td\n    e\n \tf\n \tg\n \th\n \ti\n"));
        }

        [Test]
        public void IfStatementWithMixedTabsAndSpacesAndIndentationsThatIsNotAnEvenMultipleOfTheTabSize_TrimsFromTheRightWhenRemovingWhiteSpace()
        {
            var template = "@tabsize 4\n     {{if true}}\n\t\ta\n\t \tb\n\t  \tc\n\t   \td\n\t    e\n\t\t f\n\t \t g\n\t  \t h\n\t   \t i\n\t     j\n     {{/if}}";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("\t a\n\t b\n\t c\n\t d\n\t e\n\t  f\n\t  g\n\t  h\n\t  i\n\t  j\n"));
        }

        [Test]
        public void WrapIfBlock_EmitsExtraIndentationWhenTheConditionIsTrue()
        {
            var template = "{{wrapif true}}\n    outer\n        {{body}}\n            inner\n        {{/body}}\n    /outer\n{{/wrapif}}";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("outer\n    inner\n/outer\n"));
        }

        [Test]
        public void WrapIfBlock_EmitsNoExtraIndentationWhenTheConditionIsFalse()
        {
            var template = "{{wrapif false}}\n    outer\n        {{body}}\n            inner\n        {{/body}}\n    /outer\n{{/wrapif}}";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("inner\n"));
        }
    }
}
