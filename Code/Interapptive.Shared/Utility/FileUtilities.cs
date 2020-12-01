using System;
using System.IO;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// File utilities
    /// </summary>
    public static class FileUtilities
    {
        /// <summary>
        /// Determines if a file exists at any location on the environment PATH variable
        /// </summary>
        public static bool ExistsOnPath(string fileName)
        {
            return GetFullPath(fileName) != null;
        }

        /// <summary>
        /// Get full path of a file based on PATH environment variable
        /// </summary>
        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
            {
                return Path.GetFullPath(fileName);
            }

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(';'))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }

            return null;
        }
    }
}
