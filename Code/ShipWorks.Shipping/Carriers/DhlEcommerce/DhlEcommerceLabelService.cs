using System;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.ComponentRegistration;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.DhlEcommerce.API;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Dhl Ecommerce Implementation
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.DhlEcommerce)]
    public class DhlEcommerceLabelService : ILabelService
    {
        private readonly IDhlEcommerceLabelClient labelClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceLabelService(IDhlEcommerceLabelClient labelClient)
        {
            this.labelClient = labelClient;
        }

        /// <summary>
        /// Create a label
        /// </summary>
        public async Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            try
            {
                var result = await labelClient.Create(shipment).ConfigureAwait(false);

                if (result.Value != null)
                {
                    shipment.TrackingStatus = TrackingStatus.Pending;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ShippingException(ex);
            }
        }

        /// <summary>
        /// Void the given shipment
        /// </summary>
        public void Void(ShipmentEntity shipment) => labelClient.Void(shipment);
    }
}
