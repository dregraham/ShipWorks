using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Label Service for the FedEx carrier
    /// </summary>
    public class FedExLabelService : ILabelService
    {
        private readonly Func<ShipmentEntity, IShippingClerk> shippingClerkFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLabelService(Func<ShipmentEntity,IShippingClerk> shippingClerkFactory)
        {
            this.shippingClerkFactory = shippingClerkFactory;
        }

        /// <summary>
        /// Creates the label
        /// </summary>
        public void Create(ShipmentEntity shipment)
        {
            try
            {
                IShippingClerk shippingClerk = shippingClerkFactory(shipment);
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
            try
            {
                IShippingClerk shippingClerk = shippingClerkFactory(shipment);
                shippingClerk.Void(shipment);
            }
            catch (FedExException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
    }
}