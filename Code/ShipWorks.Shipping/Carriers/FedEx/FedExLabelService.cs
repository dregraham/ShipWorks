using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Label Service for the FedEx carrier
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.FedEx)]
    public class FedExLabelService : ILabelService
    {
        private readonly IFedExShippingClerkFactory shippingClerkFactory;
        private readonly Func<IEnumerable<ICarrierResponse>, FedExDownloadedLabelData> createDownloadedLabelData;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLabelService(IFedExShippingClerkFactory shippingClerkFactory,
            Func<IEnumerable<ICarrierResponse>, FedExDownloadedLabelData> createDownloadedLabelData)
        {
            this.shippingClerkFactory = shippingClerkFactory;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Creates the label
        /// </summary>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
        {
            IShippingClerk shippingClerk = shippingClerkFactory.Create(shipment);

            try
            {
                IEnumerable<ICarrierResponse> carrierResponses = shippingClerk.Ship(shipment);
                return createDownloadedLabelData(carrierResponses);
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