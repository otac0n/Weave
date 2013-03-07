// -----------------------------------------------------------------------
// <copyright file="EchoTag.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Expressions
{
    using System;

    /// <summary>
    /// Represents a computed section in the <see cref="Template"/>.
    /// </summary>
    public class EchoTag : Element
    {
        private readonly bool encoded;
        private readonly string expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="EchoTag"/> class.
        /// </summary>
        /// <param name="expression">The code expression that will be used to compute the text to render.</param>
        /// <param name="encoded">True, if the output is to be encoded; false, otherwise.</param>
        public EchoTag(string expression, bool encoded)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this.expression = expression;
            this.encoded = encoded;
        }

        /// <summary>
        /// Gets the code expression that will be used to compute the text to render.
        /// </summary>
        public string Expression
        {
            get { return this.expression; }
        }
    }
}
