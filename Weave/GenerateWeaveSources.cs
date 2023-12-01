// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    [Generator]
    internal class GenerateWeaveSources : IIncrementalGenerator
    {
        private const string UseSourceGeneration = "build_metadata.WeaveTemplate.UseSourceGeneration";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var weaveFiles = context.AdditionalTextsProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Where(p => p.Right.GetOptions(p.Left).Keys.Contains(UseSourceGeneration, StringComparer.InvariantCultureIgnoreCase))
                .Select((p, ct) => (p.Left.Path, Text: p.Left.GetText(ct)));

            var yesConfigFiles = weaveFiles.Where(f => Path.GetFileName(f.Path).ToUpperInvariant() == CompileManager.RecursiveConfigFileName.ToUpperInvariant());
            var nonConfigFiles = weaveFiles.Where(f => Path.GetFileName(f.Path).ToUpperInvariant() != CompileManager.RecursiveConfigFileName.ToUpperInvariant());

            // TODO: Go ahead and compile _config files, but make sure that we emit a compile warning (using a pragma directive and a [Deprecated] attribute) about future removal.
            // TODO: Somehow provide a positve ACK that a _config file is missing from the filesystem via MSBUILD. e.g. `build_metadata.WeaveTemplate.ParentConfigFileExists`

            var compileManager = new CompileManager(new SourceGeneratorVirtualFileSystem(weaveFiles));

            context.RegisterSourceOutput(
                weaveFiles,
                (context, file) => CompileWeaveFile(context, compileManager, file.Path, file.Text));
        }

        private static void CompileWeaveFile(SourceProductionContext context, CompileManager compileManager, string path, SourceText text)
        {
            if (text != null)
            {
                var result = compileManager.CompileFile(path);

                var hadFatal = false;
                foreach (var error in result.Errors)
                {
                    hadFatal |= !error.IsWarning;
                    context.ReportDiagnostic(ConvertToDiagnostic(error));
                }

                context.AddSource(Path.GetFileName(path) + ".g.cs", result.Code);
            }
        }

        private static Diagnostic ConvertToDiagnostic(CompilerError error)
        {
            var (severity, level) = error.IsWarning
                ? (DiagnosticSeverity.Warning, 1)
                : (DiagnosticSeverity.Error, 0);

            return Diagnostic.Create(
                error.ErrorNumber,
                nameof(Weave),
                error.ErrorText,
                severity,
                severity,
                isEnabledByDefault: true,
                warningLevel: level);
        }

        private class SourceGeneratorVirtualFileSystem : IFileSystem, IDirectory, IFile
        {
            private IncrementalValuesProvider<(string Path, SourceText Text)> weaveFiles;

            public SourceGeneratorVirtualFileSystem(IncrementalValuesProvider<(string Path, SourceText Text)> weaveFiles)
            {
                this.weaveFiles = weaveFiles;
            }

            public IDirectory Directory => this;

            public IDirectoryInfoFactory DirectoryInfo => throw new NotImplementedException();

            public IDriveInfoFactory DriveInfo => throw new NotImplementedException();

            public IFile File => this;

            public IFileInfoFactory FileInfo => throw new NotImplementedException();

            public IFileStreamFactory FileStream => throw new NotImplementedException();

            public IFileSystemWatcherFactory FileSystemWatcher => throw new NotImplementedException();

            public IPath Path => throw new NotImplementedException();

            IFileSystem IFileSystemEntity.FileSystem => throw new NotImplementedException();

            void IFile.AppendAllLines(string path, IEnumerable<string> contents)
            {
                throw new NotImplementedException();
            }

            void IFile.AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
            {
                throw new NotImplementedException();
            }

            void IFile.AppendAllText(string path, string contents)
            {
                throw new NotImplementedException();
            }

            void IFile.AppendAllText(string path, string contents, Encoding encoding)
            {
                throw new NotImplementedException();
            }

            StreamWriter IFile.AppendText(string path)
            {
                throw new NotImplementedException();
            }

            void IFile.Copy(string sourceFileName, string destFileName)
            {
                throw new NotImplementedException();
            }

            void IFile.Copy(string sourceFileName, string destFileName, bool overwrite)
            {
                throw new NotImplementedException();
            }

            FileSystemStream IFile.Create(string path)
            {
                throw new NotImplementedException();
            }

            FileSystemStream IFile.Create(string path, int bufferSize)
            {
                throw new NotImplementedException();
            }

            FileSystemStream IFile.Create(string path, int bufferSize, FileOptions options)
            {
                throw new NotImplementedException();
            }

            IDirectoryInfo IDirectory.CreateDirectory(string path)
            {
                throw new NotImplementedException();
            }

            StreamWriter IFile.CreateText(string path)
            {
                throw new NotImplementedException();
            }

            void IFile.Decrypt(string path)
            {
                throw new NotImplementedException();
            }

            void IDirectory.Delete(string path)
            {
                throw new NotImplementedException();
            }

            void IDirectory.Delete(string path, bool recursive)
            {
                throw new NotImplementedException();
            }

            void IFile.Delete(string path)
            {
                throw new NotImplementedException();
            }

            void IFile.Encrypt(string path)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IDirectory.EnumerateDirectories(string path)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IDirectory.EnumerateDirectories(string path, string searchPattern)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IDirectory.EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IDirectory.EnumerateFiles(string path)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IDirectory.EnumerateFiles(string path, string searchPattern)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IDirectory.EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IDirectory.EnumerateFileSystemEntries(string path)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IDirectory.EnumerateFileSystemEntries(string path, string searchPattern)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IDirectory.EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
            {
                throw new NotImplementedException();
            }

            bool IDirectory.Exists(string path)
            {
                throw new NotImplementedException();
            }

            bool IFile.Exists(string path)
            {
                throw new NotImplementedException();
            }

            FileAttributes IFile.GetAttributes(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IDirectory.GetCreationTime(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IFile.GetCreationTime(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IDirectory.GetCreationTimeUtc(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IFile.GetCreationTimeUtc(string path)
            {
                throw new NotImplementedException();
            }

            string IDirectory.GetCurrentDirectory()
            {
                throw new NotImplementedException();
            }

            string[] IDirectory.GetDirectories(string path)
            {
                throw new NotImplementedException();
            }

            string[] IDirectory.GetDirectories(string path, string searchPattern)
            {
                throw new NotImplementedException();
            }

            string[] IDirectory.GetDirectories(string path, string searchPattern, SearchOption searchOption)
            {
                throw new NotImplementedException();
            }

            string IDirectory.GetDirectoryRoot(string path)
            {
                throw new NotImplementedException();
            }

            string[] IDirectory.GetFiles(string path)
            {
                throw new NotImplementedException();
            }

            string[] IDirectory.GetFiles(string path, string searchPattern)
            {
                throw new NotImplementedException();
            }

            string[] IDirectory.GetFiles(string path, string searchPattern, SearchOption searchOption)
            {
                throw new NotImplementedException();
            }

            string[] IDirectory.GetFileSystemEntries(string path)
            {
                throw new NotImplementedException();
            }

            string[] IDirectory.GetFileSystemEntries(string path, string searchPattern)
            {
                throw new NotImplementedException();
            }

            string[] IDirectory.GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
            {
                throw new NotImplementedException();
            }

            DateTime IDirectory.GetLastAccessTime(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IFile.GetLastAccessTime(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IDirectory.GetLastAccessTimeUtc(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IFile.GetLastAccessTimeUtc(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IDirectory.GetLastWriteTime(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IFile.GetLastWriteTime(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IDirectory.GetLastWriteTimeUtc(string path)
            {
                throw new NotImplementedException();
            }

            DateTime IFile.GetLastWriteTimeUtc(string path)
            {
                throw new NotImplementedException();
            }

            string[] IDirectory.GetLogicalDrives()
            {
                throw new NotImplementedException();
            }

            IDirectoryInfo IDirectory.GetParent(string path)
            {
                throw new NotImplementedException();
            }

            void IDirectory.Move(string sourceDirName, string destDirName)
            {
                throw new NotImplementedException();
            }

            void IFile.Move(string sourceFileName, string destFileName)
            {
                throw new NotImplementedException();
            }

            FileSystemStream IFile.Open(string path, FileMode mode)
            {
                throw new NotImplementedException();
            }

            FileSystemStream IFile.Open(string path, FileMode mode, FileAccess access)
            {
                throw new NotImplementedException();
            }

            FileSystemStream IFile.Open(string path, FileMode mode, FileAccess access, FileShare share)
            {
                throw new NotImplementedException();
            }

            FileSystemStream IFile.OpenRead(string path)
            {
                throw new NotImplementedException();
            }

            StreamReader IFile.OpenText(string path)
            {
                throw new NotImplementedException();
            }

            FileSystemStream IFile.OpenWrite(string path)
            {
                throw new NotImplementedException();
            }

            byte[] IFile.ReadAllBytes(string path)
            {
                throw new NotImplementedException();
            }

            string[] IFile.ReadAllLines(string path)
            {
                throw new NotImplementedException();
            }

            string[] IFile.ReadAllLines(string path, Encoding encoding)
            {
                throw new NotImplementedException();
            }

            string IFile.ReadAllText(string path)
            {
                throw new NotImplementedException();
            }

            string IFile.ReadAllText(string path, Encoding encoding)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IFile.ReadLines(string path)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> IFile.ReadLines(string path, Encoding encoding)
            {
                throw new NotImplementedException();
            }

            void IFile.Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
            {
                throw new NotImplementedException();
            }

            void IFile.Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
            {
                throw new NotImplementedException();
            }

            void IFile.SetAttributes(string path, FileAttributes fileAttributes)
            {
                throw new NotImplementedException();
            }

            void IDirectory.SetCreationTime(string path, DateTime creationTime)
            {
                throw new NotImplementedException();
            }

            void IFile.SetCreationTime(string path, DateTime creationTime)
            {
                throw new NotImplementedException();
            }

            void IDirectory.SetCreationTimeUtc(string path, DateTime creationTimeUtc)
            {
                throw new NotImplementedException();
            }

            void IFile.SetCreationTimeUtc(string path, DateTime creationTimeUtc)
            {
                throw new NotImplementedException();
            }

            void IDirectory.SetCurrentDirectory(string path)
            {
                throw new NotImplementedException();
            }

            void IDirectory.SetLastAccessTime(string path, DateTime lastAccessTime)
            {
                throw new NotImplementedException();
            }

            void IFile.SetLastAccessTime(string path, DateTime lastAccessTime)
            {
                throw new NotImplementedException();
            }

            void IDirectory.SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
            {
                throw new NotImplementedException();
            }

            void IFile.SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
            {
                throw new NotImplementedException();
            }

            void IDirectory.SetLastWriteTime(string path, DateTime lastWriteTime)
            {
                throw new NotImplementedException();
            }

            void IFile.SetLastWriteTime(string path, DateTime lastWriteTime)
            {
                throw new NotImplementedException();
            }

            void IDirectory.SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
            {
                throw new NotImplementedException();
            }

            void IFile.SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
            {
                throw new NotImplementedException();
            }

            void IFile.WriteAllBytes(string path, byte[] bytes)
            {
                throw new NotImplementedException();
            }

            void IFile.WriteAllLines(string path, string[] contents)
            {
                throw new NotImplementedException();
            }

            void IFile.WriteAllLines(string path, IEnumerable<string> contents)
            {
                throw new NotImplementedException();
            }

            void IFile.WriteAllLines(string path, string[] contents, Encoding encoding)
            {
                throw new NotImplementedException();
            }

            void IFile.WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
            {
                throw new NotImplementedException();
            }

            void IFile.WriteAllText(string path, string contents)
            {
                throw new NotImplementedException();
            }

            void IFile.WriteAllText(string path, string contents, Encoding encoding)
            {
                throw new NotImplementedException();
            }
        }
    }
}
