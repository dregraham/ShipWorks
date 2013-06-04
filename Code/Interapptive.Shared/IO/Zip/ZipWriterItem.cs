using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// An item for the ZipWriter to write to file
    /// </summary>
    public abstract class ZipWriterItem
    {
        string zipEntryName;

        DateTime lastModifiedTime = DateTime.Now;

        /// <summary>
        /// Constructor
        /// </summary>
        public ZipWriterItem(string zipEntryName)
        {
            if (string.IsNullOrEmpty(zipEntryName))
            {
                throw new ArgumentException("Parameter 'zipEntryName' cannot be null or empty.");
            }

            this.zipEntryName = zipEntryName;
        }

        /// <summary>
        /// The name of the file as it will be in the zip archive
        /// </summary>
        public string ZipEntryName
        {
            get { return zipEntryName; }
        }

        /// <summary>
        /// When the entry was last modified.
        /// </summary>
        public DateTime LastModifiedTime
        {
            get { return lastModifiedTime; }
            set { lastModifiedTime = value; }
        }

        /// <summary>
        /// Opens a stream for reading the zip item contents.
        /// </summary>
        public abstract Stream OpenStream();

        /// <summary>
        /// The length in bytes of the data
        /// </summary>
        public abstract long Length { get; }

        /// <summary>
        /// Any file attributes that should be applied when unzipping the file.
        /// </summary>
        public abstract FileAttributes FileAttributes { get; }
    }
}
