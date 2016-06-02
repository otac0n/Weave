// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the main body of an <see cref="EachElement"/>.
    /// </summary>
    public class EachBody : Element
    {
        private readonly IList<Element> body;
        private readonly SourceSpan expression;
        private readonly string indentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="EachBody"/> class.
        /// </summary>
        /// <param name="expression">The code expression that describes the iteration subject.</param>
        /// <param name="body">The body of the loop.</param>
        /// <param name="indentation">The indentation of this <see cref="EachBody"/>.</param>
        public EachBody(SourceSpan expression, IEnumerable<Element> body, string indentation = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            this.body = body.ToList().AsReadOnly();
            this.expression = expression;
            this.indentation = indentation;
        }

        /// <summary>
        /// Gets the elements in this <see cref="EachBody"/>.
        /// </summary>
        public IList<Element> Body
        {
            get { return this.body; }
        }

        /// <summary>
        /// Gets code expression that describes the iteration subject.
        /// </summary>
        public SourceSpan Expression
        {
            get { return this.expression; }
        }

        /// <summary>
        /// Gets the indentation text of this <see cref="EachBody"/>.
        /// </summary>
        public string Indentation
        {
            get { return this.indentation; }
        }
    }
}
