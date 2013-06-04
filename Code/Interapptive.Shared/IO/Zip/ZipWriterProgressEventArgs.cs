using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Interapptive.Shared.IO.Zip
{
    /// <summary>
    /// EventArgs for when a zip file is being saved by the ZipWriter
    /// </summary>
    public class ZipWriterProgressEventArgs : CancelEventArgs
    {
        // Item currently being written
        ZipWriterItem currentItem;

        // The number of bites this item is originally, and how many of them have been written
        long itemBytesProcessed;
        long itemBytesTotal;

        // The number of bites for all items being written, and how many of them have been written
        long totalBytesProcessed;
        long totalBytesTotal;

        /// <summary>
        /// Constructor
        /// </summary>
        public ZipWriterProgressEventArgs(
            ZipWriterItem currentItem,
            long itemBytesProcessed,
            long itemBytesTotal,
            long totalBytesProcessed,
            long totalBytesTotal)
        {
            this.currentItem = currentItem;
            this.itemBytesProcessed = itemBytesProcessed;
            this.itemBytesTotal = itemBytesTotal;
            this.totalBytesProcessed = totalBytesProcessed;
            this.totalBytesTotal = totalBytesTotal;
        }

        /// <summary>
        /// The item currently being written
        /// </summary>
        public ZipWriterItem CurrentItem
        {
            get { return currentItem; }
        }

        /// <summary>
        /// The number of bytes processed for the current item
        /// </summary>
        public long ItemBytesProcessed
        {
            get { return itemBytesProcessed; }
        }

        /// <summary>
        /// Total number of bytes to process for the current item
        /// </summary>
        public long ItemBytesTotal
        {
            get { return itemBytesTotal; }
        }

        /// <summary>
        /// Total number of bytes processed out of all items
        /// </summary>
        public long TotalBytesProcessed
        {
            get { return totalBytesProcessed; }
        }

        /// <summary>
        /// Total number of bytes to process for all items
        /// </summary>
        public long TotalBytesTotal
        {
            get { return totalBytesTotal; }
        }
    }
}
