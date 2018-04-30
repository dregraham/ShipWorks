using System;

namespace ShipWorks.Stores.Orders.Archive.Errors
{
    /// <summary>
    /// Common errors
    /// </summary>
    public static class Error
    {
        private static Exception canceled = new CanceledException();
        private static Exception archiveFailed = new OrderArchiveException("Archiving orders failed");

        /// <summary>
        /// A dialog or operation was canceled
        /// </summary>
        public static Exception Canceled => canceled;

        /// <summary>
        /// The archive failed
        /// </summary>
        public static Exception ArchiveFailed => archiveFailed;
    }
}