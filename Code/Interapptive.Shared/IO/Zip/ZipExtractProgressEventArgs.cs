using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// EventArgs for the extraction of a single ZipReaderItem
    /// </summary>
    public class ZipExtractProgressEventArgs : CancelEventArgs
    {
        long bytesProcessed;
        long bytesTotal;

        /// <summary>
        /// Constructor
        /// </summary>
        public ZipExtractProgressEventArgs(long bytesProcessed, long bytesTotal)
        {
            this.bytesProcessed = bytesProcessed;
            this.bytesTotal = bytesTotal;
        }

        /// <summary>
        /// Bytes processed so far
        /// </summary>
        public long BytesProcessed
        {
            get { return bytesProcessed; }
        }

        /// <summary>
        /// Total bytes to be processed
        /// </summary>
        public long BytesTotal
        {
            get { return bytesTotal; }
        }
    }
}
