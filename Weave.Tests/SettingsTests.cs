// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using Weave.Compiler;

    [TestFixture]
    public class SettingsTests
    {
        [Test]
        public void Compile_WhenTemplateContainsEncodedEchoExpressionAndEncodeSetting_Succeeds()
        {
            var template = "@namespace Tests\n@encode EncodeFoo\n{{: foo }}";

            var results = WeaveCompiler.Compile(new WeaveParser().Parse(template));

            Assert.That(results.Errors, Is.Empty);
        }

        [Test]
        public void Compile_WhenTemplateContainsEncodedEchoExpressionButNoEncodeSetting_YieldsError()
        {
            var template = "@namespace Tests\n{{: foo }}";

            var results = WeaveCompiler.Compile(new WeaveParser().Parse(template));

            Assert.That(results.Errors.Single().ErrorNumber, Is.EqualTo("WEAVE0005"));
        }

        [Test]
        public void Compile_WhenTemplateDoesNotContainNamespaceSetting_YieldsError()
        {
            var template = string.Empty;

            var results = WeaveCompiler.Compile(new WeaveParser().Parse(template));

            Assert.That(results.Errors.Single().ErrorNumber, Is.EqualTo("WEAVE0004"));
        }
    }
}
