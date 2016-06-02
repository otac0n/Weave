// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Compiler
{
    using System.Collections.Generic;
    using Weave.Expressions;
    using Weave.Properties;

    internal class ReportNonEmptyConfigPass : CompilePass
    {
        public override IList<string> ErrorsProduced
        {
            get { return new[] { "WEAVE0006" }; }
        }

        public override IList<string> BlockedByErrors
        {
            get { return new string[0]; }
        }

        public override void Run(Template template, CompileResult result)
        {
            var config = template.Config;
            while (config != null)
            {
                if (config.SettingsEnd != config.End)
                {
                    result.AddError(config.SettingsEnd, () => Resources.WEAVE0006_CONFIG_NOT_EMPTY);
                }

                config = config.Config;
            }
        }
    }
}
