// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Tests.IntegrationTests
{
    using System;
    using System.IO;
    using Xunit;

    public class ConfigTests
    {
        private static readonly string ExpectedConfigOutput = "";
        private static readonly string ExpectedTemplateOutput = $"Hello, world!{Environment.NewLine}";

        [Fact]
        public void CompiledConfig()
        {
            TestHelper(
                Generated.ConfigTest.CompiledConfig.Templates.Render_config, ExpectedConfigOutput);
            ////Generated.ConfigTest.CompiledConfig.Templates.RenderTestGeneratedConfig, ExpectedTemplateOutput
        }

        [Fact]
        public void LegacyCompiledConfig()
        {
            TestHelper(
                (Generated.ConfigTest.LegacyCompiledConfig.Templates.Render_config, ExpectedConfigOutput),
                (Generated.ConfigTest.LegacyCompiledConfig.Templates.RenderLegacyCompiledConfig, ExpectedTemplateOutput));
        }

        [Fact]
        public void LegacyGeneratedConfig()
        {
            ////(Generated.ConfigTest.LegacyGeneratedConfig.Templates.Render_config, ExpectedConfigOutput),
            TestHelper(
                (Generated.ConfigTest.LegacyGeneratedConfig.Templates.RenderLegacyGeneratedConfig, ExpectedTemplateOutput));
        }

        private static void TestHelper(params (Action<dynamic, TextWriter, string> render, string expected)[] tests)
        {
            foreach (var (render, expected) in tests)
            {
                TestHelper(render, expected);
            }
        }

        private static void TestHelper(Action<dynamic, TextWriter, string> render, string expected)
        {
            var output = TestHelper(render);
            Assert.Equal(expected, output);
        }

        private static string TestHelper(Action<dynamic, TextWriter, string> render)
        {
            var writer = new StringWriter();
            render(null, writer, null);
            return writer.ToString();
        }
    }
}
