// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Expressions
{
    using System;

    /// <summary>
    /// Represents the indentation at the start of a line.
    /// </summary>
    public class IndentationElement : Element
    {
        private readonly string indentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndentationElement"/> class.
        /// </summary>
        /// <param name="indentation">The indentation for the subsequent text on the same line.</param>
        public IndentationElement(string indentation)
        {
            this.indentation = indentation ?? throw new ArgumentNullException(nameof(indentation));
        }

        /// <summary>
        /// Gets the indentation text for for the subsequent text on the same line.
        /// </summary>
        public string Indentation => this.indentation;
    }
}
