using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// Represents a file to be written to a zip file.
    /// </summary>
    public class ZipWriterFileItem : ZipWriterItem
    {
        string filename;

        /// <summary>
        /// Constructor that allows directly specifing the zip entry name
        /// </summary>
        public ZipWriterFileItem(string filename, string zipEntryName) : base(zipEntryName)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException("Parameter 'filename' cannot be null or empty.");
            }

            this.filename = filename;
        }

        /// <summary>
        /// Constructor that determins the zip entry name by removing the basePath portion of the logFile path name
        /// </summary>
        public ZipWriterFileItem(string filename, DirectoryInfo basePath) : base(DetermineZipEntryName(filename, basePath))
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException("Parameter 'filename' cannot be null or empty.");
            }

            this.filename = filename;
        }

        /// <summary>
        /// The file that will be zipped
        /// </summary>
        public string FileName
        {
            get { return filename; }
        }

        /// <summary>
        /// Open the file stream for reading
        /// </summary>
        public override Stream OpenStream()
        {
            return new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        /// <summary>
        /// The length in bytes of the file to be zipped
        /// </summary>
        public override long Length
        {
            get
            {
                FileInfo fi = new FileInfo(FileName);
                return fi.Length;
            }
        }

        /// <summary>
        /// The file attributes that exist on the file being zipped
        /// </summary>
        public override FileAttributes FileAttributes
        {
            get
            {
                FileInfo fi = new FileInfo(FileName);
                return fi.Attributes;
            }
        }

        /// <summary>
        /// Determine the ZipEntryName to use by subtracting the basePath from the given filename
        /// </summary>
        private static string DetermineZipEntryName(string filename, DirectoryInfo basePath)
        {
            string basePathWithSlash = basePath.FullName;

            // This is so removing the folder information from the file path works
            if (!basePathWithSlash.EndsWith(@"\"))
            {
                basePathWithSlash += @"\";
            }

            return filename.Remove(0, basePathWithSlash.Length);
        }
    }
}
