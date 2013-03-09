// -----------------------------------------------------------------------
// <copyright file="Template.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

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
        private readonly IList<Element> elements;
        private readonly Cursor settingsEnd;
        private readonly IList<KeyValuePair<SourceSpan, SourceSpan>> settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class.
        /// </summary>
        /// <param name="settings">The settings in this <see cref="Template"/>.</param>
        /// <param name="settingsEnd">The ending cursor of the settings section.</param>
        /// <param name="elements">The elements in this <see cref="Template"/>.</param>
        public Template(IEnumerable<KeyValuePair<SourceSpan, SourceSpan>> settings, Cursor settingsEnd, IEnumerable<Element> elements)
        {
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

            this.settings = settings.ToList().AsReadOnly();
            this.settingsEnd = settingsEnd;
            this.elements = elements.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the elements in this <see cref="Template"/>.
        /// </summary>
        public IList<Element> Elements
        {
            get { return this.elements; }
        }

        /// <summary>
        /// Gets the cursor after the settings section.
        /// </summary>
        public Cursor SettingsEnd
        {
            get { return this.settingsEnd; }
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
