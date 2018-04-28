// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Expressions
{
    using System;

    /// <summary>
    /// Represents a computed section in the <see cref="Template"/>.
    /// </summary>
    public class EchoTag : Element
    {
        private readonly bool encoded;
        private readonly SourceSpan expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="EchoTag"/> class.
        /// </summary>
        /// <param name="expression">The code expression that will be used to compute the text to render.</param>
        /// <param name="encoded">True, if the output is to be encoded; false, otherwise.</param>
        public EchoTag(SourceSpan expression, bool encoded)
        {
            this.expression = expression ?? throw new ArgumentNullException(nameof(expression));
            this.encoded = encoded;
        }

        /// <summary>
        /// Gets a value indicating whether or the output is to be encoded.
        /// </summary>
        public bool Encoded => this.encoded;

        /// <summary>
        /// Gets the code expression that will be used to compute the text to render.
        /// </summary>
        public SourceSpan Expression => this.expression;
    }
}
