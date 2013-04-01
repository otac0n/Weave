// -----------------------------------------------------------------------
// <copyright file="CompileWeaveTemplate.cs" company="(none)">
//   Copyright © 2013 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.txt for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Weave
{
    using System.CodeDom.Compiler;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Provides compilation services for Weave templates as an MSBuild task.
    /// </summary>
    public class CompileWeaveTemplate : Task
    {
        /// <summary>
        /// Gets or sets the filename of a file containing a Weave template.
        /// </summary>
        [Required]
        public string InputFile { get; set; }

        /// <summary>
        /// Gets or sets the output filename that will contain the resulting code.
        /// </summary>
        /// <remarks>
        /// Set to null to use the default, which is the input filename with ".cs" appended.
        /// </remarks>
        public string OutputFile { get; set; }

        /// <summary>
        /// Reads and compiles the specified template.
        /// </summary>
        /// <returns>true, if the compilation was successful; false, otherwise.</returns>
        public override bool Execute()
        {
            CompileManager.CompileFile(this.InputFile, this.OutputFile, this.LogError);
            return !this.Log.HasLoggedErrors;
        }

        private void LogError(CompilerError error)
        {
            if (error.IsWarning)
            {
                this.Log.LogWarning(null, error.ErrorNumber, null, error.FileName, error.Line, error.Column, 0, 0, error.ErrorText);
            }
            else
            {
                this.Log.LogError(null, error.ErrorNumber, null, error.FileName, error.Line, error.Column, 0, 0, error.ErrorText);
            }
        }
    }
}
