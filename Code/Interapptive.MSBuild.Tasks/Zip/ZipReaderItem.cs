using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// An item that has been read out of a zip file
    /// </summary>
    public class ZipReaderItem
    {
        // The position of the entry in the stream
        long entryPosition;

        // The entry being read
        ZipEntry zipEntry;

        // The stream the zip file is being read from
        ZipInputStream zipStream;

        // Buffer size to use when extracting
        readonly int bufferSize = 4096;

        /// <summary>
        /// Raised as the item is being extracted
        /// </summary>
        public event EventHandler<ZipExtractProgressEventArgs> Progress;

        /// <summary>
        /// Constructor.  The stream should be positioned at the start of the ZipEntry.
        /// </summary>
        public ZipReaderItem(ZipEntry zipEntry, ZipInputStream zipStream)
        {
            if (zipEntry == null)
            {
                throw new ArgumentNullException("zipEntry");
            }

            if (zipStream == null)
            {
                throw new ArgumentNullException("zipStream");
            }

            entryPosition = zipStream.Position;

            this.zipStream = zipStream;
            this.zipEntry = zipEntry;
        }

        /// <summary>
        /// The name of the entry in the zip file
        /// </summary>
        public string Name
        {
            get
            {
                return zipEntry.Name.Replace(@"/", @"\");
            }
        }

        /// <summary>
        /// Indicates if this is a directory and not a file.
        /// </summary>
        public bool IsDirectory
        {
            get
            {
                return zipEntry.IsDirectory;
            }
        }

        /// <summary>
        /// Extract the entry to the given file location
        /// </summary>
        public void Extract(string targetFile)
        {
            if (entryPosition != zipStream.Position)
            {
                throw new InvalidOperationException("ZipReaderItem no longer valid; the stream position has changed.");
            }

            // If this is actually a file, and not just a folder
            if (zipEntry.IsFile)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

                // Create the file stream to write to
                using (FileStream streamWriter = new FileStream(targetFile, FileMode.Create))
                {
                    byte[] data = new byte[bufferSize];

                    while (true)
                    {
                        int size = zipStream.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);

                            if (!RaiseProgressEvent())
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(targetFile);
            }
        }

        /// <summary>
        /// Raise the progress event.  Returns false if the user cancels
        /// </summary>
        private bool RaiseProgressEvent()
        {
            EventHandler<ZipExtractProgressEventArgs> handler = Progress;
            if (handler != null)
            {
                ZipExtractProgressEventArgs args = new ZipExtractProgressEventArgs(zipStream.Position - entryPosition, zipEntry.CompressedSize);
                handler(this, args);

                return !args.Cancel;
            }
            else
            {
                return true;
            }
        }
    }
}
