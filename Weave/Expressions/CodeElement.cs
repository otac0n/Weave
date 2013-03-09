// -----------------------------------------------------------------------
// <copyright file="CodeElement.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Expressions
{
    using System;

    /// <summary>
    /// Represents an executed section of a <see cref="Template"/>.
    /// </summary>
    public class CodeElement : Element
    {
        private readonly SourceSpan expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeElement"/> class.
        /// </summary>
        /// <param name="expression">The code expression that will be executed.</param>
        public CodeElement(SourceSpan expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this.expression = expression;
        }

        /// <summary>
        /// Gets the code expression that will be executed.
        /// </summary>
        public SourceSpan Expression
        {
            get { return this.expression; }
        }
    }
}
