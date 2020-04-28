using System.Collections.Generic;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Api.Orders.Shipments
{
    /// <summary>
    /// Generates a label 
    /// </summary>
    public interface IApiLabelFactory
    {
        /// <summary>
        /// Gets labels for ID
        /// </summary>
        /// <param name="consumerID">Either a shipment or package ID</param>
        IEnumerable<LabelData> GetLabels(long consumerID);

        /// <summary>
        /// Get labels for the given adapter
        /// </summary>
        IEnumerable<LabelData> GetLabels(ICarrierShipmentAdapter carrierShipmentAdapter);
    }
}
