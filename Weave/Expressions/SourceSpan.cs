// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Expressions
{
    using System;
    using System.Diagnostics;
    using Pegasus.Common;

    /// <summary>
    /// Represents a span of text in the subject.
    /// </summary>
    [DebuggerDisplay("{Value}@{Start}")]
    public class SourceSpan
    {
        private readonly Cursor end;
        private readonly string value;
        private readonly Cursor start;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceSpan"/> class.
        /// </summary>
        /// <param name="value">The value of the <see cref="SourceSpan"/>.</param>
        /// <param name="start">The cursor just before the <see cref="SourceSpan"/>.</param>
        /// <param name="end">The cursor just after the <see cref="SourceSpan"/>.</param>
        public SourceSpan(string value, Cursor start, Cursor end)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            this.value = value;
            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// Gets the cursor just after the <see cref="SourceSpan"/>.
        /// </summary>
        public Cursor End
        {
            get { return this.end; }
        }

        /// <summary>
        /// Gets the value of the <see cref="SourceSpan"/>.
        /// </summary>
        public string Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Gets the cursor just before the <see cref="SourceSpan"/>.
        /// </summary>
        public Cursor Start
        {
            get { return this.start; }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.value;
        }
    }
}
