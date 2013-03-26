// -----------------------------------------------------------------------
// <copyright file="Branch.cs" company="(none)">
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
    /// Represents one branch of an <see cref="IfElement"/>.
    /// </summary>
    public class Branch : Element
    {
        private readonly IList<Element> body;
        private readonly SourceSpan expression;
        private readonly string indentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Branch"/> class.
        /// </summary>
        /// <param name="expression">The code expression that determines whether this branch will execute. Null, if this is the default branch.</param>
        /// <param name="body">The body of the branch.</param>
        /// <param name="indentation">The indentation of this <see cref="Branch"/>.</param>
        public Branch(SourceSpan expression, IEnumerable<Element> body, string indentation = null)
        {
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            this.body = body.ToList().AsReadOnly();
            this.expression = expression;
            this.indentation = indentation;
        }

        /// <summary>
        /// Gets the body of this <see cref="Branch"/>.
        /// </summary>
        public IList<Element> Body
        {
            get { return this.body; }
        }

        /// <summary>
        /// Gets the code expression that determines whether this branch will execute.
        /// </summary>
        public SourceSpan Expression
        {
            get { return this.expression; }
        }

        /// <summary>
        /// Gets the indentation of this <see cref="Branch"/>.
        /// </summary>
        public string Indentation
        {
            get { return this.indentation; }
        }
    }
}
