// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

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
            this.expression = expression ?? throw new ArgumentNullException(nameof(expression));
            this.indentation = indentation;
            this.method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Gets the code expression that evaluates to a model.
        /// </summary>
        public SourceSpan Expression => this.expression;

        /// <summary>
        /// Gets the indentation of this <see cref="RenderElement"/>.
        /// </summary>
        public string Indentation => this.indentation;

        /// <summary>
        /// Gets the method that will be called.
        /// </summary>
        public SourceSpan Method => this.method;
    }
}
