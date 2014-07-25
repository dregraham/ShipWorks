using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Interapptive.Shared.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility class for working with paths
    /// </summary>
    public static class PathUtility
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(PathUtility));

        static readonly HashSet<char> illegalFileNameChars = new HashSet<char>(Path.GetInvalidFileNameChars());
        static readonly HashSet<char> illegalPathChars = new HashSet<char>(Path.GetInvalidPathChars().Concat(new[] { '*', '?', ':' }));
        static readonly Regex pathRootPattern = new Regex(@"^[a-zA-Z]\:\\.*", RegexOptions.Compiled);

        /// <summary>
        /// Trim the given path to fit within the specified number of pixels.
        /// </summary>
        public static string CompactPath(string path, int maxWidth)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (maxWidth <= 0)
            {
                throw new ArgumentException("maxWidth must be greater than zero.");
            }

            StringBuilder interopString = new StringBuilder(path);

            // The API seems to make it too short...
            maxWidth += 75;

            if (!NativeMethods.PathCompactPath(IntPtr.Zero, interopString, maxWidth))
            {
                log.InfoFormat("PathCompactPath failed: '{0}' => '{1}'", path, interopString);
            }

            return interopString.ToString();
        }

        /// <summary>
        /// Compare the two paths for equality
        /// </summary>
        public static bool IsSamePath(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path1) && string.IsNullOrEmpty(path2))
            {
                return true;
            }

            if (string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2))
            {
                return false;
            }

            return string.Compare(
                Path.GetFullPath(path1).TrimEnd('\\'),
                Path.GetFullPath(path2).TrimEnd('\\'),
                StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        /// <summary>
        /// Determines if the given filename matches the specified DOS wildcard pattern, where * and ? are valid.
        /// </summary>
        public static bool IsDosWildcardMatch(string name, string pattern)
        {
            string dot = "_____SWDOT";
            string star = "______SWSTAR";
            string question = "______SWQUESTION";

            pattern = pattern.Replace(".", dot);
            pattern = pattern.Replace("*", star);
            pattern = pattern.Replace("?", question);

            pattern = Regex.Escape(pattern);

            pattern = pattern.Replace(dot, "[.]");
            pattern = pattern.Replace(star, ".*");
            pattern = pattern.Replace(question, ".?");

            return Regex.Match(name, pattern).Success;
        }

        /// <summary>
        /// Cleans the specified path by replacing any invalid characters with underscores
        /// </summary>
        public static string CleanPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            // We can't use Path.IsPathRooted here because it throws if the path contains certain illegal chars
            bool isRooted = pathRootPattern.IsMatch(path);

            string root = isRooted ? path.Substring(0, 3) : string.Empty;
            string directory = isRooted ? path.Substring(3) : path;

            StringBuilder output = new StringBuilder(root, path.Length);

            foreach (char c in directory)
            {
                output.Append(illegalPathChars.Contains(c) ? '_' : c);
            }

            return output.ToString();
        }

        /// <summary>
        /// Cleans the specified file name by replacing any invalid characters with underscores
        /// </summary>
        public static string CleanFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            StringBuilder output = new StringBuilder(name.Length);

            foreach (char c in name)
            {
                output.Append(illegalFileNameChars.Contains(c) ? '_' : c);
            }

            return output.ToString();
        }
    }
}
