using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Collections;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// For reading and extracting items from zip files
    /// </summary>
    public class ZipReader : IDisposable
    {
        FileStream fileStream;
        ZipInputStream zipStream;

        long totalBytes;

        bool itemCanceled;

        /// <summary>
        /// Raised as the zip file items are exracted
        /// </summary>
        public event EventHandler<ZipReaderProgressEventArgs> Progress;

        /// <summary>
        /// Constructor
        /// </summary>
        public ZipReader(string zipFilePath)
        {
            // Open the zip file
            fileStream = File.OpenRead(zipFilePath);
            zipStream = new ZipInputStream(fileStream);

            totalBytes = fileStream.Length;
            itemCanceled = false;
        }

        /// <summary>
        /// Read all items in the zip file as ZipReaderItem's
        /// </summary>
        public IEnumerable ReadItems()
        {
            if (zipStream == null)
            {
                throw new ObjectDisposedException("ZipReader");
            }

            ZipEntry entry;
            
            while (!itemCanceled && (entry = zipStream.GetNextEntry()) != null)
            {
                ZipReaderItem item = new ZipReaderItem(entry, zipStream);

                if (RaiseProgressEvent(item, 0, entry.CompressedSize))
                {
                    // Listen for and propogate events from the item
                    item.Progress += new EventHandler<ZipExtractProgressEventArgs>(OnItemProgress);

                    yield return item;
                }
            }

            Dispose();
        }

        /// <summary>
        /// Progress of an individual item is happening
        /// </summary>
        void OnItemProgress(object sender, ZipExtractProgressEventArgs e)
        {
            if (!RaiseProgressEvent((ZipReaderItem) sender, e.BytesProcessed, e.BytesTotal))
            {
                e.Cancel = true;
                itemCanceled = true;
            }
        }

        /// <summary>
        /// Raise the progress event
        /// </summary>
        private bool RaiseProgressEvent(ZipReaderItem item, long processed, long compressedSize)
        {
            EventHandler<ZipReaderProgressEventArgs> handler = Progress;
            if (handler != null)
            {
                ZipReaderProgressEventArgs args = new ZipReaderProgressEventArgs(item, processed, compressedSize, zipStream.Position, totalBytes);
                handler(this, args);

                return !args.Cancel;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Implement IDisposable
        /// </summary>
        public void Dispose()
        {
            if (zipStream != null)
            {
                zipStream.Dispose();
                zipStream = null;
            }

            if (fileStream != null)
            {
                fileStream.Dispose();
                fileStream = null;
            }
        }
    }
}
