// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class RegressionTests
    {
        [Test(Description = "GitHub bug #10")]
        public void LeftMarginIndentation()
        {
            var template = StringUtilities.JoinLines(
                "a",
                " b",
                "",
                "  c");
            var model = 0;

            var result = TemplateHelper.Render(template, model, " ");

            Assert.That(result, Is.EqualTo(StringUtilities.JoinLines(
                " a",
                "  b",
                "",
                "   c")));
        }
    }
}
