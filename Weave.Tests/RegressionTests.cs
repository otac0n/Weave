// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Tests
{
    using Xunit;

    public class RegressionTests
    {
        [Fact]
        [Trait("GitHub bug", "10")]
        public void LeftMarginIndentation()
        {
            var template = StringUtilities.JoinLines(
                "a",
                " b",
                "",
                "  c");
            var model = 0;

            var result = TemplateHelper.Render(template, model, " ");

            var expected = StringUtilities.JoinLines(
                " a",
                "  b",
                "",
                "   c");
            Assert.Equal(expected, result);
        }
    }
}
