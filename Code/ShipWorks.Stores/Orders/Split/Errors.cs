using System;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Common errors
    /// </summary>
    public class Errors
    {
        private static Exception canceled = new Exception();

        /// <summary>
        /// A dialog or operation was canceled
        /// </summary>
        public static Exception Canceled => canceled;
    }
}