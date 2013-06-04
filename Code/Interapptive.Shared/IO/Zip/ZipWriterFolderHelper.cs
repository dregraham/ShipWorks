using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// Utility class for easily zipping entire folders
    /// </summary>
    public static class ZipWriterFolderHelper
    {
        /// <summary>
        /// Creates a ZipWriterFileItem representing each file in the given folder, recursively.
        /// 
        /// If includeFolderInZipName is true, and the folder was "C:\Something\FolderA\", then every zip
        ///   entry would start with "FolderA\".
        /// If it is false, then they will start with their child path (or just the filename if in the folder directly).
        /// </summary>
        public static List<ZipWriterItem> CreateFileItems(string folder, bool includeFolderInZipName)
        {
            // This is so removing the folder information from the file path works
            if (!folder.EndsWith(@"\"))
            {
                folder += @"\";
            }

            string pathToStrip = folder;

            if (includeFolderInZipName)
            {
                pathToStrip = folder.Remove(pathToStrip.LastIndexOf('\\', pathToStrip.Length - 2) + 1);
            }

            List<ZipWriterItem> zipItems = new List<ZipWriterItem>();

            foreach (string file in Directory.GetFiles(folder, "*", SearchOption.AllDirectories))
            {
                zipItems.Add(new ZipWriterFileItem(file, file.Remove(0, pathToStrip.Length)));
            }

            return zipItems;
        }
    }
}
