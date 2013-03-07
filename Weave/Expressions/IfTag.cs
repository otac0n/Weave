// -----------------------------------------------------------------------
// <copyright file="IfTag.cs" company="(none)">
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
    /// Represents a conditional section in the <see cref="Template"/>.
    /// </summary>
    public class IfTag : Element
    {
        private readonly IList<Branch> branches;

        /// <summary>
        /// Initializes a new instance of the <see cref="IfTag"/> class.
        /// </summary>
        /// <param name="branches">The branches that make up this tag.</param>
        public IfTag(IEnumerable<Branch> branches)
        {
            if (branches == null)
            {
                throw new ArgumentNullException("branches");
            }

            this.branches = branches.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the branches in this <see cref="IfTag"/>.
        /// </summary>
        public IList<Branch> Branches
        {
            get { return this.branches; }
        }
    }
}
