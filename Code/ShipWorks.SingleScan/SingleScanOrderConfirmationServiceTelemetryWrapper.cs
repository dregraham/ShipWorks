using Interapptive.Shared.Metrics;
using ShipWorks.Shipping.Services;

namespace ShipWorks.SingleScan
{
    public class SingleScanOrderConfirmationServiceTelemetryDecorator : ISingleScanOrderConfirmationService
    {
        private readonly ISingleScanOrderConfirmationService singleScanOrderConfirmationService;

        public SingleScanOrderConfirmationServiceTelemetryDecorator(ISingleScanOrderConfirmationService singleScanOrderConfirmationService)
        {
            this.singleScanOrderConfirmationService = singleScanOrderConfirmationService;
        }

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