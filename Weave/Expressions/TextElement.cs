// -----------------------------------------------------------------------
// <copyright file="TextElement.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave.Expressions
{
    using System;

    /// <summary>
    /// Represents literal text in the <see cref="Template"/>.
    /// </summary>
    public class TextElement : Element
    {
        private readonly string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextElement"/> class.
        /// </summary>
        /// <param name="value">The literal value of this element.</param>
        public TextElement(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.value = value;
        }

        /// <summary>
        /// Gets the value of this <see cref="TextElement"/>.
        /// </summary>
        public string Value
        {
            get { return this.value; }
        }
    }
}
