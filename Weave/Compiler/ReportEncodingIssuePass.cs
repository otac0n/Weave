// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
    using Weave.Expressions;
    using Weave.Properties;

    internal class ReportEncodingIssuePass : CompilePass
    {
        /// <inheritdoc/>
        public override IList<string> BlockedByErrors => System.Array.Empty<string>();

        /// <inheritdoc/>
        public override IList<string> ErrorsProduced => new[] { "WEAVE0005" };

        /// <inheritdoc/>
        public override void Run(Template template, CompileResult result)
        {
            if (template.AllSettings.Any(s => s.Key.Value == "encode"))
            {
                return;
            }

            var finder = new FindEncodedWalker();
            finder.WalkTemplate(template);

            if (finder.FoundEncoded)
            {
                result.AddError(template.SettingsEnd, () => Resources.WEAVE0005_ENCODE_NOT_SPECIFIED);
            }
        }

        private class FindEncodedWalker : TemplateWalker
        {
            public bool FoundEncoded { get; set; }

            public override void WalkEchoTag(EchoTag echoTag)
            {
                if (echoTag.Encoded == true)
                {
                    this.FoundEncoded = true;
                }
            }
        }
    }
}
