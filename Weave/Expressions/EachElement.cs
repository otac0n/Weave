// -----------------------------------------------------------------------
// <copyright file="EachElement.cs" company="(none)">
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

    /// <summary>
    /// Represents a repeated section of the <see cref="Template"/>.
    /// </summary>
    public class EachElement : Element
    {
        private readonly IList<Element> body;
        private readonly SourceSpan expression;
        private readonly IList<Element> noneBody;

        /// <summary>
        /// Initializes a new instance of the <see cref="EachElement"/> class.
        /// </summary>
        /// <param name="expression">The code expression that describes the iteration subject.</param>
        /// <param name="body">The body of the loop.</param>
        /// <param name="noneBody">An optional body that is rendered when the iteration subject is empty.</param>
        public EachElement(SourceSpan expression, IEnumerable<Element> body, IEnumerable<Element> noneBody)
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
            this.noneBody = noneBody == null ? null : noneBody.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the elements in this <see cref="EachElement"/>
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
        /// Gets the optional none-body of this <see cref="EachElement"/>.
        /// </summary>
        public IList<Element> NoneBody
        {
            get { return this.noneBody; }
        }
    }
}
