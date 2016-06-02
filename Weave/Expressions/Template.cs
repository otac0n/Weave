// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Pegasus.Common;

    /// <summary>
    /// Represents a Weave <see cref="Template"/>.
    /// </summary>
    public class Template
    {
        private readonly Template config;
        private readonly Cursor start;
        private readonly IList<Element> elements;
        private readonly Cursor settingsEnd;
        private readonly IList<KeyValuePair<SourceSpan, SourceSpan>> settings;
        private readonly Cursor end;

        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class.
        /// </summary>
        /// <param name="start">The starting cursor of the <see cref="Template"/>.</param>
        /// <param name="settings">The settings in this <see cref="Template"/>.</param>
        /// <param name="settingsEnd">The ending cursor of the settings section.</param>
        /// <param name="elements">The elements in this <see cref="Template"/>.</param>
        /// <param name="end">The ending cursor of the <see cref="Template"/>.</param>
        public Template(Cursor start, IEnumerable<KeyValuePair<SourceSpan, SourceSpan>> settings, Cursor settingsEnd, IEnumerable<Element> elements, Cursor end)
        {
            if (start == null)
            {
                throw new ArgumentNullException("start");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (settingsEnd == null)
            {
                throw new ArgumentNullException("settingsEnd");
            }

            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }

            if (end == null)
            {
                throw new ArgumentNullException("end");
            }

            this.start = start;
            this.settings = settings.ToList().AsReadOnly();
            this.settingsEnd = settingsEnd;
            this.elements = elements.ToList().AsReadOnly();
            this.end = end;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class.
        /// </summary>
        /// <param name="template">The template to copy.</param>
        /// <param name="config">The configuration to apply.</param>
        public Template(Template template, Template config)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            this.config = config;
            this.start = template.start;
            this.settings = template.settings;
            this.settingsEnd = template.settingsEnd;
            this.elements = template.elements;
            this.end = template.end;
        }

        /// <summary>
        /// Gets all settings that affect this <see cref="Template"/>, including those from config files.
        /// </summary>
        public IEnumerable<KeyValuePair<SourceSpan, SourceSpan>> AllSettings
        {
            get
            {
                var template = this;
                while (template != null)
                {
                    foreach (var setting in template.settings)
                    {
                        yield return setting;
                    }

                    template = template.config;
                }
            }
        }

        /// <summary>
        /// Gets the config template for this <see cref="Template"/>.
        /// </summary>
        public Template Config
        {
            get { return this.config; }
        }

        /// <summary>
        /// Gets the elements in this <see cref="Template"/>.
        /// </summary>
        public IList<Element> Elements
        {
            get { return this.elements; }
        }

        /// <summary>
        /// Gets the ending cursor of this <see cref="Template"/>.
        /// </summary>
        public Cursor End
        {
            get { return this.end; }
        }

        /// <summary>
        /// Gets the cursor after the settings section.
        /// </summary>
        public Cursor SettingsEnd
        {
            get { return this.settingsEnd; }
        }

        /// <summary>
        /// Gets the starting cursor of this <see cref="Template"/>.
        /// </summary>
        public Cursor Start
        {
            get { return this.start; }
        }

        /// <summary>
        /// Gets the settings in this <see cref="Template"/>.
        /// </summary>
        public IList<KeyValuePair<SourceSpan, SourceSpan>> Settings
        {
            get { return this.settings; }
        }
    }
}
