using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// A facade to all the single scan confirmation services
    /// </summary>
    public class SingleScanConfirmationService : ISingleScanConfirmationService
    {
        private readonly ISingleScanOrderConfirmationService orderConfirmationService;
        private readonly ISingleScanShipmentConfirmationService shipmentConfirmationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleScanConfirmationService"/> class.
        /// </summary>
        /// <param name="orderConfirmationService">The order confirmation service.</param>
        /// <param name="shipmentConfirmationService">The shipment confirmation service.</param>
        public SingleScanConfirmationService(ISingleScanOrderConfirmationService orderConfirmationService,
            ISingleScanShipmentConfirmationService shipmentConfirmationService)
        {
            this.orderConfirmationService = orderConfirmationService;
            this.shipmentConfirmationService = shipmentConfirmationService;
        }

        /// <summary>
        /// Confirms that the order with the given orderId should be printed.
        /// </summary>
        public bool ConfirmOrder(long orderId, int numberOfMatchedOrders, string scanText)
            => orderConfirmationService.Confirm(orderId, numberOfMatchedOrders, scanText);

        /// <summary>
        /// Gets the Shipments that SingleScan should auto print/process
        /// </summary>
        public Task<IEnumerable<ShipmentEntity>> GetShipments(long orderId, string scannedBarcode)
            => shipmentConfirmationService.GetShipments(orderId, scannedBarcode);
    }
}
