// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the none section of an <see cref="EachElement"/>.
    /// </summary>
    public class NoneBody : Element
    {
        private readonly IList<Element> body;
        private readonly string indentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoneBody"/> class.
        /// </summary>
        /// <param name="body">The body of none section.</param>
        /// <param name="indentation">The indentation of this <see cref="NoneBody"/>.</param>
        public NoneBody(IEnumerable<Element> body, string indentation = null)
        {
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            this.body = body.ToList().AsReadOnly();
            this.indentation = indentation;
        }

        /// <summary>
        /// Gets the elements in this <see cref="NoneBody"/>.
        /// </summary>
        public IList<Element> Body
        {
            get { return this.body; }
        }

        /// <summary>
        /// Gets the indentation text of this <see cref="NoneBody"/>.
        /// </summary>
        public string Indentation
        {
            get { return this.indentation; }
        }
    }
}
