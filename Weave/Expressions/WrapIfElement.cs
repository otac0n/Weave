// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Expressions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a body with conditional surrounding content.
    /// </summary>
    public class WrapIfElement : Element
    {
        private readonly IList<Element> after;
        private readonly IList<Element> before;
        private readonly BodyElement body;
        private readonly string endIndentation;
        private readonly SourceSpan expression;
        private readonly string indentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapIfElement"/> class.
        /// </summary>
        /// <param name="indentation">The indentation of this <see cref="WrapIfElement"/>.</param>
        /// <param name="expression">The code expression that determines whether the <paramref name="body"/> will be surrounded by content.</param>
        /// <param name="before">The content that conditionally appears before the <paramref name="body"/>.</param>
        /// <param name="body">The body content that will always be rendered.</param>
        /// <param name="after">The content that conditionally appears after the <paramref name="body"/>.</param>
        /// <param name="endIndentation">The ending indentation of this <see cref="WrapIfElement"/>.</param>
        public WrapIfElement(string indentation, SourceSpan expression, IList<Element> before, BodyElement body, IList<Element> after, string endIndentation)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (before == null)
            {
                throw new ArgumentNullException(nameof(before));
            }

            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (after == null)
            {
                throw new ArgumentNullException(nameof(after));
            }

            this.indentation = indentation;
            this.expression = expression;
            this.before = before;
            this.body = body;
            this.after = after;
            this.endIndentation = endIndentation;
        }

        /// <summary>
        /// Gets the content that conditionally appears after the <see cref="Body"/>.
        /// </summary>
        public IList<Element> After
        {
            get { return this.after; }
        }

        /// <summary>
        /// Gets the content that conditionally appears before the <see cref="Body"/>.
        /// </summary>
        public IList<Element> Before
        {
            get { return this.before; }
        }

        /// <summary>
        /// Gets the body content that will always be rendered.
        /// </summary>
        public BodyElement Body
        {
            get { return this.body; }
        }

        /// <summary>
        /// Gets the ending indentation of this <see cref="WrapIfElement"/>.
        /// </summary>
        public string EndIndentation
        {
            get { return this.endIndentation; }
        }

        /// <summary>
        /// Gets the code expression that determines whether the <see cref="Body"/> will be surrounded by content.
        /// </summary>
        public SourceSpan Expression
        {
            get { return this.expression; }
        }

        /// <summary>
        /// Gets the indentation of this <see cref="WrapIfElement"/>.
        /// </summary>
        public string Indentation
        {
            get { return this.indentation; }
        }
    }
}
