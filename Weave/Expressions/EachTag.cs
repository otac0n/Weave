// -----------------------------------------------------------------------
// <copyright file="EachTag.cs" company="(none)">
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
    /// Represents a repeated section of the <see cref="Template"/>.
    /// </summary>
    public class EachTag : Element
    {
        private readonly IList<Element> body;
        private readonly string expression;
        private readonly IList<Element> noneBody;

        /// <summary>
        /// Initializes a new instance of the <see cref="EachTag"/> class.
        /// </summary>
        /// <param name="expression">The code expression that describes the iteration subject.</param>
        /// <param name="body">The body of the loop.</param>
        /// <param name="noneBody">An optional body that is rendered when the iteration subject is empty.</param>
        public EachTag(string expression, IList<Element> body, IList<Element> noneBody)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            this.body = body.ToList().AsReadOnly();
            this.expression = expression;
            this.noneBody = noneBody == null ? null : noneBody.ToList().AsReadOnly();
        }
    }
}
