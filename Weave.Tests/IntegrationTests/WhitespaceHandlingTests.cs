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

            Assert.That(result, Is.EqualTo("foo\n    bar\n    bar\n    bar\nbaz"));
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
        public void StatementBlockAtTheBottomWithNoPrecedingOrFollowingBlankLine_DoesNotEmitBlankLineAtTheBottom()
        {
            var template = "foo\n{{ /* no-op */ }}";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("foo"));
        }

        [Test]
        public void StatementBlockAtTheBottomWithPrecedingBlankLine_EmitsBlankLineAtTheBottom()
        {
            var template = "foo\n\n{{ /* no-op */ }}";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("foo\n"));
        }

        [Test]
        public void StatementBlockAtTheBottomWithFollowingBlankLine_EmitsBlankLineAtTheBottom()
        {
            var template = "foo\n{{ /* no-op */ }}\n";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("foo\n"));
        }

        [Test]
        public void StatementBlockAtTheBottomWithBothPrecedingAndFollowingBlankLine_EmitsASingleBlankLineAtTheBottom()
        {
            var template = "foo\n\n{{ /* no-op */ }}\n";

            var result = TemplateHelper.Render(template, null);

            Assert.That(result, Is.EqualTo("foo\n"));
        }
    }
}
