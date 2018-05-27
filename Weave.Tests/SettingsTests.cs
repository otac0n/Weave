// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Tests
{
    using System.Linq;
    using Weave.Compiler;
    using Weave.Parser;
    using Xunit;

    public class SettingsTests
    {
        [Fact]
        public void Compile_WhenTemplateContainsEncodedEchoExpressionAndEncodeSetting_Succeeds()
        {
            var template = "@namespace Tests\n@encode EncodeFoo\n{{: foo }}";

            var results = WeaveCompiler.Compile(new WeaveParser().Parse(template));

            Assert.Empty(results.Errors);
        }

        [Fact]
        public void Compile_WhenTemplateContainsEncodedEchoExpressionButNoEncodeSetting_YieldsError()
        {
            var template = "@namespace Tests\n{{: foo }}";

            var results = WeaveCompiler.Compile(new WeaveParser().Parse(template));

            Assert.Equal("WEAVE0005", results.Errors.Single().ErrorNumber);
        }

        [Fact]
        public void Compile_WhenTemplateDoesNotContainNamespaceSetting_YieldsError()
        {
            var template = string.Empty;

            var results = WeaveCompiler.Compile(new WeaveParser().Parse(template));

            Assert.Equal("WEAVE0004", results.Errors.Single().ErrorNumber);
        }
    }
}
