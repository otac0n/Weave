// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Expressions
{
    using System;

    /// <summary>
    /// Represents a repeated section of the <see cref="Template"/>.
    /// </summary>
    public class EachElement : Element
    {
        private readonly DelimitBody delimitBody;
        private readonly EachBody eachBody;
        private readonly string endIndentation;
        private readonly NoneBody noneBody;

        /// <summary>
        /// Initializes a new instance of the <see cref="EachElement"/> class.
        /// </summary>
        /// <param name="eachBody">The body of the each element.</param>
        /// <param name="delimitBody">An optional body that is rendered between each pair of adjacent elements.</param>
        /// <param name="noneBody">An optional body that is rendered when the iteration subject is empty.</param>
        /// <param name="endIndentation">The ending indentation of this <see cref="EachElement"/>.</param>
        public EachElement(EachBody eachBody, DelimitBody delimitBody, NoneBody noneBody, string endIndentation = null)
        {
            this.eachBody = eachBody ?? throw new ArgumentNullException(nameof(eachBody));
            this.delimitBody = delimitBody;
            this.noneBody = noneBody;
            this.endIndentation = endIndentation;
        }

        /// <summary>
        /// Gets the optional delimit-body of this <see cref="EachElement"/>.
        /// </summary>
        public DelimitBody DelimitBody => this.delimitBody;

        /// <summary>
        /// Gets the optional none-body of this <see cref="EachElement"/>.
        /// </summary>
        public EachBody EachBody => this.eachBody;

        /// <summary>
        /// Gets the ending indentation of this <see cref="EachElement"/>.
        /// </summary>
        public string EndIndentation => this.endIndentation;

        /// <summary>
        /// Gets the optional none-body of this <see cref="EachElement"/>.
        /// </summary>
        public NoneBody NoneBody => this.noneBody;
    }
}
