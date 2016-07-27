// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Expressions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the unconditional portion of a <see cref="WrapIfElement"/>.
    /// </summary>
    public class BodyElement : Element
    {
        private readonly IList<Element> body;
        private readonly string endIndentation;
        private readonly string indentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyElement"/> class.
        /// </summary>
        /// <param name="indentation">The indentation of this <see cref="BodyElement"/>.</param>
        /// <param name="body">The contents of the body.</param>
        /// <param name="endIndentation">The ending indentation of this <see cref="BodyElement"/>.</param>
        public BodyElement(string indentation, IList<Element> body, string endIndentation)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            this.indentation = indentation;
            this.body = body;
            this.endIndentation = endIndentation;
        }

        /// <summary>
        /// Gets the contents of the body.
        /// </summary>
        public IList<Element> Body
        {
            get { return this.body; }
        }

        /// <summary>
        /// Gets the ending indentation of this <see cref="BodyElement"/>.
        /// </summary>
        public string EndIndentation
        {
            get { return this.endIndentation; }
        }

        /// <summary>
        /// Gets the indentation of this <see cref="BodyElement"/>.
        /// </summary>
        public string Indentation
        {
            get { return this.indentation; }
        }
    }
}
