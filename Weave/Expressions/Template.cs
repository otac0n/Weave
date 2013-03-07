// -----------------------------------------------------------------------
// <copyright file="Template.cs" company="(none)">
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
    /// Represents a Weave <see cref="Template"/>.
    /// </summary>
    public class Template
    {
        private readonly IList<Element> elements;

        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class.
        /// </summary>
        /// <param name="elements">The elements in this <see cref="Template"/>.</param>
        public Template(IEnumerable<Element> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }

            this.elements = elements.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the elements in this <see cref="Template"/>.
        /// </summary>
        public IList<Element> Elements
        {
            get { return this.elements; }
        }
    }
}
