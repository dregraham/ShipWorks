using Interapptive.Shared.Metrics;
using ShipWorks.Shipping.Services;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Uses decorator pattern to collect telemetry data from SingleScanOrderConfirmationService
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Services.ISingleScanOrderConfirmationService" />
    public class TelemetricSingleScanOrderConfirmationService : ISingleScanOrderConfirmationService
    {
        private readonly ISingleScanOrderConfirmationService singleScanOrderConfirmationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetricSingleScanOrderConfirmationService"/> class.
        /// </summary>
        /// <param name="singleScanOrderConfirmationService">The single scan order confirmation service.</param>
        public TelemetricSingleScanOrderConfirmationService(ISingleScanOrderConfirmationService singleScanOrderConfirmationService)
        {
            this.singleScanOrderConfirmationService = singleScanOrderConfirmationService;
        }

        /// <summary>
        /// Confirms that the order with the given orderId should be printed, collecting desired telemetry data
        /// </summary>
        public bool Confirm(long orderId, int numberOfMatchedOrders, string scanText)
        {
            using (TrackedDurationEvent orderCollisionTrackedDurationEvent =
                new TrackedDurationEvent("SingleScan.AutoPrint.Confirmation.OrderCollision"))
            {
                bool shouldPrint = singleScanOrderConfirmationService.Confirm(orderId, numberOfMatchedOrders, scanText);

                orderCollisionTrackedDurationEvent.AddProperty(
                    "SingleScan.AutoPrint.Confirmation.OrderCollision.Barcode", scanText);
                orderCollisionTrackedDurationEvent.AddMetric(
                    "SingleScan.AutoPrint.Confirmation.OrderCollision.TotalOrders",
                    numberOfMatchedOrders);
                orderCollisionTrackedDurationEvent.AddProperty(
                    "SingleScan.AutoPrint.Confirmation.OrderCollision.Action", shouldPrint ? "Continue" : "Cancel");

                return shouldPrint;
            }
        }
    }
}