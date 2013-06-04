using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// Writes the contents of a byte array to a zip file entry
    /// </summary>
    public class ZipWriterBinaryItem : ZipWriterItem
    {
        byte[] content;

        /// <summary>
        /// Constructor
        /// </summary>
        public ZipWriterBinaryItem(byte[] content, string zipEntryName)
            : base(zipEntryName)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            this.content = content;
        }

        /// <summary>
        /// The stream that allows reading of the string to be zipped
        /// </summary>
        public override Stream OpenStream()
        {
            return new MemoryStream(content);
        }

        /// <summary>
        /// The lengh of the data to be written
        /// </summary>
        public override long Length
        {
            get
            {
                return content.Length;
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
