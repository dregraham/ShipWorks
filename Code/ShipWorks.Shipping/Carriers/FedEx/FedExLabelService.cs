using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Label Service for the FedEx carrier
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.FedEx)]
    public class FedExLabelService : ILabelService
    {
        private readonly IFedExShippingClerkFactory shippingClerkFactory;
        private readonly Func<IEnumerable<IFedExShipResponse>, FedExDownloadedLabelData> createDownloadedLabelData;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLabelService(IFedExShippingClerkFactory shippingClerkFactory,
            Func<IEnumerable<IFedExShipResponse>, FedExDownloadedLabelData> createDownloadedLabelData)
        {
            this.shippingClerkFactory = shippingClerkFactory;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Creates the label
        /// </summary>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
        {
            try
            {
                return shippingClerkFactory.Create(shipment)
                    .Ship(shipment)
                    .Map(createDownloadedLabelData)
                    .Match(x => x, ex => { throw ex; });
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
            IShippingClerk shippingClerk = shippingClerkFactory.Create(shipment);

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