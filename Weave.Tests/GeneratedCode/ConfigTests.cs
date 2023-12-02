// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Tests.IntegrationTests
{
    using Xunit;

    public class ConfigTests
    {
        [Fact]
        public void AbsentConfig()
        {
            Weave.Tests.Generated.Templates.RenderTestAbsentConfig();
        }

        [Fact]
        public void CompiledConfig()
        {
            Weave.Tests.Generated.Templates.RenderTestCompiledConfig();
        }
    }
}
