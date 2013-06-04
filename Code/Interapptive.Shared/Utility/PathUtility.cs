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
    }
}
