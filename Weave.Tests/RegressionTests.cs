// -----------------------------------------------------------------------
// <copyright file="WhitespaceHandlingTests.cs" company="(none)">
//   Copyright © 2015 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class RegressionTests
    {
        [Test(Description = "GitHub bug #10")]
        public void LeftMarginIndentation()
        {
            var template = "a\n b\n\n  c";
            var model = 0;

            var result = TemplateHelper.Render(template, model, " ");

            Assert.That(result, Is.EqualTo(" a\n  b\n\n   c"));
        }
    }
}
