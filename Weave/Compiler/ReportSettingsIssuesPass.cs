// -----------------------------------------------------------------------
// <copyright file="ReportSettingsIssuesPass.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Compiler
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Weave.Expressions;
    using Weave.Properties;

    internal class ReportSettingsIssuesPass : CompilePass
    {
        private static readonly Dictionary<string, bool> KnownSettings = new Dictionary<string, bool>
        {
            { "namespace", true },
            { "accessibility", true },
            { "classname", true },
            { "methodname", true },
            { "model", true },
            { "using", false },
        };

        private static readonly Dictionary<string, string> ValuePatterns = new Dictionary<string, string>
        {
            { "accessibility", @"^\s*(public|internal)\s*$" },
        };

        public override IList<string> ErrorsProduced
        {
            get { return new[] { "WEAVE0001", "WEAVE0002", "WEAVE0003", "WEAVE0004" }; }
        }

        public override IList<string> BlockedByErrors
        {
            get { return new string[0]; }
        }

        public override void Run(Template template, CompileResult result)
        {
            var seenSettings = new HashSet<string>();

            foreach (var setting in template.Settings)
            {
                var settingName = setting.Key.Value;
                var cursor = setting.Key.Start;

                bool singleAllowed;
                if (KnownSettings.TryGetValue(settingName, out singleAllowed))
                {
                    if (singleAllowed && !seenSettings.Add(settingName))
                    {
                        result.AddError(cursor, () => Resources.WEAVE0001_SETTING_ALREADY_SPECIFIED, settingName);
                    }
                }
                else
                {
                    result.AddWarning(cursor, () => Resources.WEAVE0002_SETTING_UNKNOWN, settingName);
                }

                string pattern;
                if (ValuePatterns.TryGetValue(settingName, out pattern))
                {
                    if (!Regex.IsMatch(setting.Value.ToString(), pattern))
                    {
                        result.AddError(cursor, () => Resources.WEAVE0003_SETTING_VALUE_INVALID, setting.Value.Value, settingName);
                    }
                }
            }

            if (!seenSettings.Contains("namespace"))
            {
                result.AddError(template.SettingsEnd, () => Resources.WEAVE0004_NAMESPACE_NOT_SPECIFIED);
            }
        }
    }
}
