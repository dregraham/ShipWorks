using System;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.ComponentRegistration;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express Implementation
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressLabelService : ILabelService
    {
        private readonly IDhlExpressLabelClientFactory labelClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressLabelService(IDhlExpressLabelClientFactory labelClientFactory)
        {
            this.labelClientFactory = labelClientFactory;
        }

        /// <summary>
        /// Create a label
        /// </summary>
        public async Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            try
            {
                return await labelClientFactory.Create(shipment).CreateLabel(shipment);
            }
            catch (Exception ex)
            {
                throw new ShippingException(ex.Message);
            }
        }
        /// <summary>
        /// Void the given shipment
        /// </summary>
        public void Void(ShipmentEntity shipment) => labelClientFactory.Create(shipment).Void(shipment);
    }
}
