// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the delimit section of an <see cref="EachElement"/>.
    /// </summary>
    public class DelimitBody : Element
    {
        private readonly IList<Element> body;
        private readonly string indentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitBody"/> class.
        /// </summary>
        /// <param name="body">The body of delimit section.</param>
        /// <param name="indentation">The indentation of this <see cref="DelimitBody"/>.</param>
        public DelimitBody(IEnumerable<Element> body, string indentation = null)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            this.body = body.ToList().AsReadOnly();
            this.indentation = indentation;
        }

        /// <summary>
        /// Gets the elements in this <see cref="DelimitBody"/>.
        /// </summary>
        public IList<Element> Body => this.body;

        /// <summary>
        /// Gets the indentation text of this <see cref="DelimitBody"/>.
        /// </summary>
        public string Indentation => this.indentation;
    }
}
