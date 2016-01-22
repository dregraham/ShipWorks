using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Label Service for the FedEx carrier
    /// </summary>
    public class FedExLabelService : ILabelService
    {
        private readonly FedExShippingClerkFactory shippingClerkFactory;
        private readonly IIndex<ShipmentTypeCode, ICarrierSettingsRepository> settingsRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLabelService(FedExShippingClerkFactory shippingClerkFactory, IIndex<ShipmentTypeCode, ICarrierSettingsRepository> settingsRepository)
        {
            this.shippingClerkFactory = shippingClerkFactory;
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Creates the label
        /// </summary>
        public void Create(ShipmentEntity shipment)
        {
            IShippingClerk shippingClerk = shippingClerkFactory.CreateShippingClerk(shipment, settingsRepository[ShipmentTypeCode.FedEx]);

            try
            {
                shippingClerk.Ship(shipment);
            }
            catch (FedExException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Voids the label
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            IShippingClerk shippingClerk = shippingClerkFactory.CreateShippingClerk(shipment, settingsRepository[ShipmentTypeCode.FedEx]);
            try
            {
                shippingClerk.Void(shipment);
            }
            catch (FedExException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
    }
}