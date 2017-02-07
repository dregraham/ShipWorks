using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Confirm what the user wants to do when auto printing and multiple matching orders are found
    /// </summary>
    [Component]
    public class SingleScanOrderConfirmationService : ISingleScanOrderConfirmationService
    {
        private readonly IAutoPrintConfirmationDlgFactory dlgFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IStoreManager storeManager;
        private readonly Func<string, ITrackedDurationEvent> trackedDurationEventFactory;


        /// <summary>
        /// Initializes a new instance of the <see cref="SingleScanOrderConfirmationService"/> class.
        /// </summary>
        public SingleScanOrderConfirmationService(IAutoPrintConfirmationDlgFactory dlgFactory, IMessageHelper messageHelper, IStoreManager storeManager,
            Func<string, ITrackedDurationEvent> trackedDurationEventFactory)
        {
            this.dlgFactory = dlgFactory;
            this.messageHelper = messageHelper;
            this.storeManager = storeManager;
            this.trackedDurationEventFactory = trackedDurationEventFactory;
        }

        /// <summary>
        /// Confirms that the order with the given orderId should be printed.
        /// </summary>
        public bool Confirm(IEnumerable<long?> orderIds, int numberOfMatchedOrders, string scanText)
        {
            long mostRecentOrderID = 0;
            if (orderIds.FirstOrDefault().HasValue)
            {
                mostRecentOrderID = orderIds.FirstOrDefault().Value;
            }

            // We should never get here with 0 matched orders
            if (numberOfMatchedOrders == 0)
            {
                throw new ShippingException("Unable to locate order for processing.");
            }

            if (numberOfMatchedOrders > 1)
            {
                MessagingText messaging = GetMessageingText(mostRecentOrderID, numberOfMatchedOrders);

                using (ITrackedDurationEvent orderCollisionTrackedDurationEvent =
                        trackedDurationEventFactory("SingleScan.AutoPrint.Confirmation.OrderCollision"))
                {
                    DialogResult result = messageHelper.ShowDialog(() => dlgFactory.Create(scanText, messaging));

                    bool shouldPrint = result == DialogResult.OK;

                    AddTelemetryProperties(orderCollisionTrackedDurationEvent, scanText, numberOfMatchedOrders, orderIds, shouldPrint);

                    return shouldPrint;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds the telemetry properties.
        /// </summary>
        /// <param name="orderCollisionTrackedDurationEvent">The order collision tracked duration event.</param>
        /// <param name="scanText">The scan text.</param>
        /// <param name="numberOfMatchedOrders">The number of matched orders.</param>
        /// <param name="orderIds">The order ids.</param>
        /// <param name="shouldPrint">if set to <c>true</c> [should print].</param>
        private void AddTelemetryProperties(ITrackedDurationEvent orderCollisionTrackedDurationEvent, string scanText,
            int numberOfMatchedOrders, IEnumerable<long?> orderIds, bool shouldPrint)
        {
            List<string> storeTypes = new List<string>();

            foreach (long? orderId in orderIds)
            {
                if (orderId.HasValue)
                {
                    storeTypes.Add(EnumHelper.GetDescription(storeManager.GetRelatedStore(orderId.Value).StoreTypeCode));
                }
            }

            orderCollisionTrackedDurationEvent.AddProperty("SingleScan.AutoPrint.Confirmation.OrderCollision.Barcode", scanText);
            orderCollisionTrackedDurationEvent.AddMetric("SingleScan.AutoPrint.Confirmation.OrderCollision.TotalOrders",
                numberOfMatchedOrders);
            orderCollisionTrackedDurationEvent.AddProperty("SingleScan.AutoPrint.Confirmation.OrderCollision.StoreTypes",
                string.Join(",", storeTypes.Distinct()));
            orderCollisionTrackedDurationEvent.AddProperty("SingleScan.AutoPrint.Confirmation.OrderCollision.Action",
                shouldPrint ? "Continue" : "Cancel");
        }

        /// <summary>
        /// Generate the message displayed to the user
        /// </summary>
        private MessagingText GetMessageingText(long orderId, int numberOfMatchedOrders)
        {
            string store = storeManager.GetRelatedStore(orderId)?.StoreName;

            return new MessagingText
            {
                Title = "Multiple Matches Found",
                Body = $"ShipWorks found {numberOfMatchedOrders} orders matching this order number. The most recent order is from your '{store}' store. Scan the bar code again or click 'Use Most Recent Order' to print the label(s) for this order.",
                Continue = "Use Most Recent Order"
            };
        }
    }
}