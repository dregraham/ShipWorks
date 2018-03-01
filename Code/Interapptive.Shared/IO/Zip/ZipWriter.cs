using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using log4net;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// Used to create and write zip files
    /// </summary>
    public class ZipWriter
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ZipWriter));

        // Compression level used
        int compression = 1;

        // Size of buffer used when writing data
        int bufferSize = 8192;

        // The items to be zipped
        List<ZipWriterItem> zipItems = new List<ZipWriterItem>();

        // Raised periodically as the zip file is written
        public event EventHandler<ZipWriterProgressEventArgs> Progress;

        // Only valid during a Save, used to provide progress
        long totalBytesProcessed;
        long totalBytesTotal;
        bool currentCanceled;

        /// <summary>
        /// Constructor
        /// </summary>
        public ZipWriter()
        {

        }

        /// <summary>
        /// The items to be zipped
        /// </summary>
        public List<ZipWriterItem> Items
        {
            get
            {
                return zipItems;
            }
        }

        /// <summary>
        /// The level of compression to use when writing the zip file. 1 - 9
        /// </summary>
        public int CompressionLevel
        {
            get { return compression; }
            set { compression = value; }
        }

        /// <summary>
        /// The size of the buffer to use when writing data
        /// </summary>
        public int BufferSize
        {
            get
            {
                return bufferSize;
            }
            set
            {
                if (value < 16 || value > 32728)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                bufferSize = value;
            }
        }

        /// <summary>
        /// Save the zip file
        /// </summary>
        public void Save(string zipFile)
        {
            totalBytesProcessed = 0;
            totalBytesTotal = CalculateTotalBytes();

            currentCanceled = false;

            using (FileStream fileStream = File.Create(zipFile))
            {
                try
                {
                    // This can throw an Exception in the Dispose if an Exception occurrs during zipping.  The Exception 
                    // thrown during dispose then hides the reall exception - so we don't use the "using" clause here so
                    // we can get around that.
                    ZipOutputStream zipStream = new ZipOutputStream(fileStream);

                    try
                    {
                        // 0 - 9 possible compression levels
                        zipStream.SetLevel(compression);

                        foreach (ZipWriterItem item in Items)
                        {
                            AddFileToZip(zipStream, item);
                        }

                        zipStream.Dispose();
                    }
                    catch
                    {
                        try
                        {
                            zipStream.Dispose();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Supressing exception from ZipOutputStream because it would mask actual exception.", ex);
                        }
                         
                        throw;
                    }
                }
                catch (ZipException)
                {
                    // If we cancelled, then the zip wouldnt have been finished and would be invalid.  Otherwise
                    // something went wrong and we rethrow.
                    if (!currentCanceled)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Add the given file to the zip stream
        /// </summary>
        private void AddFileToZip(ZipOutputStream zipStream, ZipWriterItem item)
        {
            ZipEntry entry = CreateZipEntry(item);
            zipStream.PutNextEntry(entry);

            // Write the entry to the zip stream
            using (Stream inputStream = item.OpenStream())
            {
                byte[] buffer = new byte[bufferSize];
                long read = 0;
                long total = inputStream.Length;

                while (!currentCanceled)
                {
                    int count = inputStream.Read(buffer, 0, buffer.Length);
                    zipStream.Write(buffer, 0, count);

                    read += count;
                    totalBytesProcessed += count;

                    if (count < buffer.Length)
                    {
                        break;
                    }

                    RaiseProgressEvent(item, read, total);
                }
            }
        }

        /// <summary>
        /// Raise the progress event
        /// </summary>
        private void RaiseProgressEvent(ZipWriterItem item, long processed, long total)
        {
            EventHandler<ZipWriterProgressEventArgs> handler = Progress;
            if (handler != null)
            {
                ZipWriterProgressEventArgs args = new ZipWriterProgressEventArgs(item, processed, total, totalBytesProcessed, totalBytesTotal);
                handler(this, args);

                if (args.Cancel)
                {
                    currentCanceled = true;
                }
            }
        }

        /// <summary>
        /// Create a new ZipEntry for the given file and entyr name
        /// </summary>
        private static ZipEntry CreateZipEntry(ZipWriterItem item)
        {
            ZipEntry entry = new ZipEntry(item.ZipEntryName);

            entry.Size = item.Length;
            entry.ExternalFileAttributes = (int) item.FileAttributes;
            entry.DateTime = item.LastModifiedTime;

            return entry;
        }

        /// <summary>
        /// Calculate the total number of bytes to be written
        /// </summary>
        private long CalculateTotalBytes()
        {
            long total = 0;

            foreach (ZipWriterItem item in Items)
            {
                total += item.Length;
            }

            return total;
        }
    }
}
