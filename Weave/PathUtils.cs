// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Weave
{
    using System;
    using System.IO;

    internal static class PathUtils
    {
        public static string MakeRelative(string root, string path)
        {
            root = Path.GetFullPath(root);
            var fullFilePath = Path.GetFullPath(Path.Combine(root, path));
            var relativeUri = new Uri(root).MakeRelativeUri(new Uri(fullFilePath));
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            return relativePath;
        }

        public static string EncodePathAsFilename(string path) =>
            Uri.EscapeDataString(path)
                .Replace("_", "__")
                .Replace("%", "_p");
    }
}
