using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Api.Orders.Shipments
{
    /// <summary>
    /// Generates a label
    /// </summary>
    [Component]
    public class ApiLabelFactory : IApiLabelFactory
    {
        private readonly IDataResourceManager dataResourceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiLabelFactory(IDataResourceManager dataResourceManager)
        {
            this.dataResourceManager = dataResourceManager;
        }

        /// <summary>
        /// Gets labels for ID
        /// </summary>
        /// <param name="consumerID">Either a shipment or package ID</param>
        public IEnumerable<LabelData> GetLabels(long consumerID)
        {
            return dataResourceManager.GetConsumerResourceReferences(consumerID)
                .Select(r => new LabelData(r.Label, Convert.ToBase64String(r.ReadAllBytes())));
        }

        /// <summary>
        /// Get labels for the given carrierShipmentAdapter
        /// </summary>
        public IEnumerable<LabelData> GetLabels(ICarrierShipmentAdapter carrierShipmentAdapter)
        {
            IEnumerable<long> consumerIDs = GetLabelConsumerIds(carrierShipmentAdapter);

            List<LabelData> result = new List<LabelData>();

            foreach (long consumerID in consumerIDs)
            {
                result.AddRange(GetLabels(consumerID));
            }

            return result;
        }
            

        /// <summary>
        /// Get the label ConsumerIds for the given adapter
        /// </summary>
        private IEnumerable<long> GetLabelConsumerIds(ICarrierShipmentAdapter shipmentAdapter)
        {
            // TODO: DHLECommerce Do we need to add DhlEcommerce here too?

            // Single package carriers and DHL Express store label data under the shipmentId
            // DHL Express is a ShipEngine carrier and therefor uses the common ShipEngine logic
            // for storing labels which is why it uses the shipmentId and not the packageId
            if (!shipmentAdapter.SupportsMultiplePackages ||
                shipmentAdapter.ShipmentTypeCode == ShipmentTypeCode.DhlExpress)
            {
                return new[] { shipmentAdapter.Shipment.ShipmentID };
            }
            else
            {
                return shipmentAdapter.GetPackageAdapters().Select(p => p.PackageId);
            }
        }
    }
}
