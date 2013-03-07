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
    /// Represents one branch of an <see cref="IfTag"/>.
    /// </summary>
    public class Branch
    {
        private readonly IList<Element> body;
        private readonly string expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="Branch"/> class.
        /// </summary>
        /// <param name="expression">The code expression that determines whether this branch will execute. Null, if this is the default branch.</param>
        /// <param name="body">The body of the branch.</param>
        public Branch(string expression, IList<Element> body)
        {
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            this.body = body.ToList().AsReadOnly();
            this.expression = expression;
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
        public string Expression
        {
            get { return this.expression; }
        }
    }
}
