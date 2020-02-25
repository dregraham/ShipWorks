using System.Collections.Generic;

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
    }
}
