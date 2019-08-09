using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Search;
using ShipWorks.Stores.Communication;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Utility for retrieving an orderid from an order
    /// </summary>
    /// <remarks>
    /// Handles on demand downloads and confirming the correct order
    /// when there are multiple matching results
    /// </remarks>
    [Component]
    public class OrderLookupOrderIDRetriever : IOrderLookupOrderIDRetriever
    {
        private readonly ISingleScanOrderShortcut singleScanOrderShortcut;
        private readonly IOnDemandDownloaderFactory onDemandDownloaderFactory;
        private readonly IOrderLookupOrderRepository orderRepository;
        private readonly IOrderLookupConfirmationService orderLookupConfirmationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupOrderIDRetriever(ISingleScanOrderShortcut singleScanOrderShortcut,
            IOnDemandDownloaderFactory onDemandDownloaderFactory,
            IOrderLookupOrderRepository orderRepository,
            IOrderLookupConfirmationService orderLookupConfirmationService)
        {
            this.singleScanOrderShortcut = singleScanOrderShortcut;
            this.onDemandDownloaderFactory = onDemandDownloaderFactory;
            this.orderRepository = orderRepository;
            this.orderLookupConfirmationService = orderLookupConfirmationService;
        }

        /// <summary>
        /// Get OrderID based on scanned text
        /// </summary>
        public async Task<TelemetricResult<long?>> GetOrderID(string scannedText,
            string userInputTelemetryTimeSliceName,
            string dataLoadingTelemetryTimeSliceName,
            string orderCountTelemetryPropertyName)
        {
            TelemetricResult<long?> telemetricResult = new TelemetricResult<long?>("Order");

            // If it was a Single Scan Order Shortcut we know the order has already been downloaded and
            // the scanned barcode is not one that we should try to download again.
            if (singleScanOrderShortcut.AppliesTo(scannedText))
            {
                telemetricResult.RunTimedEvent(dataLoadingTelemetryTimeSliceName, () =>
                {
                    // We're looking up by order ID, there will be at most 1 record
                    telemetricResult.SetValue(singleScanOrderShortcut.GetOrderID(scannedText));
                    telemetricResult.AddEntry(orderCountTelemetryPropertyName, telemetricResult.Value.HasValue ? 1 : 0);
                });
            }
            else
            {
                // There's the potential to get multiple orders back in this code path
                List<long> orderIds = new List<long>();

                // Track the time it took to load the order
                await telemetricResult.RunTimedEventAsync(dataLoadingTelemetryTimeSliceName, async () =>
                {
                    await Task.Run(() => onDemandDownloaderFactory.CreateOnDemandDownloader().Download(scannedText)).ConfigureAwait(true);
                    orderIds = orderRepository.GetOrderIDs(scannedText);

                    // Make a note of how many orders were found, so we can marry this up with the confirmation telemetry
                    telemetricResult.AddEntry(orderCountTelemetryPropertyName, orderIds.Count);
                }).ConfigureAwait(true);

                // Track the time it takes to confirm user input from the confirmation service
                long? selectedOrderId = await telemetricResult.RunTimedEventAsync(
                        userInputTelemetryTimeSliceName,
                        () => orderLookupConfirmationService.ConfirmOrder(scannedText, orderIds))
                    .ConfigureAwait(false);
                telemetricResult.SetValue(selectedOrderId);
            }

            return telemetricResult;
        }
    }
}
