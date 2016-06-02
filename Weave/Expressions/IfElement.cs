// Copyright © 2016 John Gietzen.  All Rights Reserved.
// This source is subject to the MIT license.
// Please see license.md for more information.

namespace Weave.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a conditional section in the <see cref="Template"/>.
    /// </summary>
    public class IfElement : Element
    {
        private readonly IList<Branch> branches;
        private readonly string endIndentation;

        /// <summary>
        /// Initializes a new instance of the <see cref="IfElement"/> class.
        /// </summary>
        /// <param name="branches">The branches that make up this tag.</param>
        /// <param name="endIndentation">The ending indentation of this <see cref="IfElement"/>.</param>
        public IfElement(IEnumerable<Branch> branches, string endIndentation = null)
        {
            if (branches == null)
            {
                throw new ArgumentNullException("branches");
            }

            this.branches = branches.ToList().AsReadOnly();
            this.endIndentation = endIndentation;
        }

        /// <summary>
        /// Gets the branches in this <see cref="IfElement"/>.
        /// </summary>
        public IList<Branch> Branches
        {
            get { return this.branches; }
        }

        /// <summary>
        /// Gets the ending indentation of this <see cref="IfElement"/>.
        /// </summary>
        public string EndIndentation
        {
            get { return this.endIndentation; }
        }
    }
}
