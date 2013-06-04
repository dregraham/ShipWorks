using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// Writes the contents of a string to a zip file entry
    /// </summary>
    public class ZipWriterStringItem : ZipWriterItem
    {
        string content;
        Encoding encoding;

        /// <summary>
        /// Constructor
        /// </summary>
        public ZipWriterStringItem(string content, string zipEntryName) :
            this(content, Encoding.UTF8, zipEntryName)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ZipWriterStringItem(string content, Encoding encoding, string zipEntryName)
            : base(zipEntryName)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            this.content = content;
            this.encoding = encoding;
        }


        /// <summary>
        /// The stream that allows reading of the string to be zipped
        /// </summary>
        public override Stream OpenStream()
        {
            return new MemoryStream(encoding.GetBytes(content));
        }

        /// <summary>
        /// The lengh of the data to be written
        /// </summary>
        public override long Length
        {
            get
            {
                return encoding.GetByteCount(content);
            }
        }

        /// <summary>
        /// File attributes to be applied to the zip entry when unzipped
        /// </summary>
        public override FileAttributes FileAttributes
        {
            get
            {
                return FileAttributes.Normal;
            }
        }
    }
}
