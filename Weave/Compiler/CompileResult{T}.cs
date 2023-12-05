// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave.Compiler
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq.Expressions;
    using Pegasus.Common;

    /// <summary>
    /// Encapsulates the results and errors from the compilation of a Weave resource.
    /// </summary>
    /// <typeparam name="T">The type resulting from compilation.</typeparam>
    public class CompileResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompileResult{T}"/> class.
        /// </summary>
        public CompileResult()
        {
            this.Errors = new List<CompilerError>();
        }

        /// <summary>
        /// Gets or sets the value resulting from compilation.
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Gets the collection of errors that occurred during compilation.
        /// </summary>
        public IList<CompilerError> Errors { get; private set; }

        internal void AddError(Cursor cursor, Expression<Func<string>> error, params object[] args)
        {
            this.AddCompilerError(cursor, error, args, isWarning: false);
        }

        internal void AddWarning(Cursor cursor, Expression<Func<string>> error, params object[] args)
        {
            this.AddCompilerError(cursor, error, args, isWarning: true);
        }

        [SuppressMessage("Usage", "RS1035:Do not use APIs banned for analyzers", Justification = "https://github.com/dotnet/roslyn/issues/71094")]
        private void AddCompilerError(Cursor cursor, Expression<Func<string>> error, object[] args, bool isWarning)
        {
            var errorId = ((MemberExpression)error.Body).Member.Name.Split('_')[0];
            var errorFormat = error.Compile()();
            var errorText = string.Format(CultureInfo.CurrentCulture, errorFormat, args);
            this.Errors.Add(new CompilerError(cursor.FileName, cursor.Line, cursor.Column, errorId, errorText) { IsWarning = isWarning });
        }
    }
}
