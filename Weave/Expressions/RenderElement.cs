// -----------------------------------------------------------------------
// <copyright file="RenderElement.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Expressions
{
    using System;

    /// <summary>
    /// Represents a call to render a sub-template.
    /// </summary>
    public class RenderElement : Element
    {
        private readonly SourceSpan expression;
        private readonly string indentation;
        private readonly SourceSpan method;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderElement"/> class.
        /// </summary>
        /// <param name="method">The method that will be called.</param>
        /// <param name="expression">The code expression that evaluates to a model.</param>
        /// <param name="indentation">The indentation of this <see cref="RenderElement"/>.</param>
        public RenderElement(SourceSpan method, SourceSpan expression, string indentation = null)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this.expression = expression;
            this.indentation = indentation;
            this.method = method;
        }

        /// <summary>
        /// Gets the code expression that evaluates to a model.
        /// </summary>
        public SourceSpan Expression
        {
            get { return this.expression; }
        }

        /// <summary>
        /// Gets the indentation of this <see cref="RenderElement"/>.
        /// </summary>
        public string Indentation
        {
            get { return this.indentation; }
        }

        /// <summary>
        /// Gets the method that will be called.
        /// </summary>
        public SourceSpan Method
        {
            get { return this.method; }
        }
    }
}
