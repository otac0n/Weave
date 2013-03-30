// -----------------------------------------------------------------------
// <copyright file="SettingsTests.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using Weave.Compiler;
    using Weave.Parser;

    [TestFixture]
    public class SettingsTests
    {
        [Test]
        public void Compile_WhenTemplateContainsEncodedEchoExpressionButNoEncodeSetting_YieldsError()
        {
            var template = "@namespace Tests\n{{: foo }}";

            var results = WeaveCompiler.Compile(new WeaveParser().Parse(template));

            Assert.That(results.Errors.Single().ErrorNumber, Is.EqualTo("WEAVE0005"));
        }

        [Test]
        public void Compile_WhenTemplateContainsEncodedEchoExpressionAndEncodeSetting_Succeeds()
        {
            var template = "@namespace Tests\n@encode EncodeFoo\n{{: foo }}";

            var results = WeaveCompiler.Compile(new WeaveParser().Parse(template));

            Assert.That(results.Errors, Is.Empty);
        }
    }
}
