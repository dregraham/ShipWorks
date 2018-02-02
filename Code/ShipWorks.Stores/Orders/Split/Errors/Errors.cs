using System;

namespace ShipWorks.Stores.Orders.Split.Errors
{
    /// <summary>
    /// Common errors
    /// </summary>
    public static class Error
    {
        private static Exception canceled = new CanceledException();
        private static Exception saveFailed = new OrderSplitException("Saving split order failed");
        private static Exception loadSurvivingOrderFailed = new OrderSplitException("Could not find surviving order");

        /// <summary>
        /// A dialog or operation was canceled
        /// </summary>
        public static Exception Canceled => canceled;

        /// <summary>
        /// A save failed
        /// </summary>
        public static Exception SaveFailed => saveFailed;

        /// <summary>
        /// A load failed
        /// </summary>
        public static Exception LoadSurvivingOrderFailed => loadSurvivingOrderFailed;
    }
}