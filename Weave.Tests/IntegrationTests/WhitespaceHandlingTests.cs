// -----------------------------------------------------------------------
// <copyright file="WhitespaceHandlingTests.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Tests.IntegrationTests
{
    using System.Linq;
    using NUnit.Framework;

    public class WhitespaceHandlingTests
    {
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
        public void IfBlockWithAnInlineStartTag_EmitsAllWhitespaceWhenTheConditionIsMet()
        {
            var template = "foo\n    bar {{if model}}\n    baz\n{{/if}}\nbaz";
            var model = true;

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("foo\n    bar \n    baz\n\nbaz"));
        }

        [Test]
        public void IfBlockWithAnInlineStartTag_EmitsSurroundingWhitespaceWhenTheConditionIsNotMet()
        {
            var template = "foo\n    bar {{if model}}\n    baz\n{{/if}}\nbaz";
            var model = false;

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("foo\n    bar \nbaz"));
        }

        [Test]
        public void IfBlockWithAnInlineEndTag_EmitsAllWhitespaceWhenTheConditionIsMet()
        {
            var template = "foo\n{{if model}}\n    baz {{/if}}\nbaz";
            var model = true;

            var result = TemplateHelper.Render(template, model);

            Assert.That(result, Is.EqualTo("foo\n\n    baz \nbaz"));
        }

        [Test]
        public void IfBlockWithAnInlineEndTag_EmitsSurroundingWhitespaceWhenTheConditionIsNotMet()
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
        public void StatementBlockLineWithExcessInternalWhitespace_DoesNotEmitAnyExternalWhitespace()
        {
            var template = "    {{\n\n   /* no-op */\n\n               }}    ";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void StatementBlockAtTheTop_DoesNotCauseAPrecedingNewlineToBeEmitted()
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
        public void IfStatementWithInconsistentIndenting_ChoosesSmallestNonZeroIndentation()
        {
            var template = "    {{if true}}\n        foo\n      foo\n       foo\n    foo\n    {{/if}}";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("      foo\n    foo\n     foo\n  foo\n"));
        }
    }
}
