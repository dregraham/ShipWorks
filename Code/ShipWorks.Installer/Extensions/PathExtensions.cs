using System;
using System.IO;

namespace ShipWorks.Installer.Extensions
{
    /// <summary>
    /// Path extension methods
    /// </summary>
    public static class PathExtensions
    {
        private static string tempFolder = "ShipWorks.Installer";

        /// <summary>
        /// Return a Temp folder 
        /// </summary>
        public static string GetTempPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), tempFolder);
        }

        /// <summary>
        /// Return a Temp folder path to given filename
        /// </summary>
        public static string Combine(string fileName)
        {
            return Path.Combine(GetTempPath(), fileName);
        }
    }
}
