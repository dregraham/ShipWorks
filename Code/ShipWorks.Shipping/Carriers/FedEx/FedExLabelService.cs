using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
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
        private readonly FedExShippingClerkFactory shippingClerkFactory;
        private readonly IIndex<ShipmentTypeCode, ICarrierSettingsRepository> settingsRepository;
        private readonly Func<IEnumerable<ICarrierResponse>, FedExDownloadedLabelData> createDownloadedLabelData;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLabelService(FedExShippingClerkFactory shippingClerkFactory,
            IIndex<ShipmentTypeCode, ICarrierSettingsRepository> settingsRepository,
            Func<IEnumerable<ICarrierResponse>, FedExDownloadedLabelData> createDownloadedLabelData)
        {
            this.shippingClerkFactory = shippingClerkFactory;
            this.settingsRepository = settingsRepository;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Creates the label
        /// </summary>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
        {
            IShippingClerk shippingClerk = shippingClerkFactory.CreateShippingClerk(shipment, settingsRepository[ShipmentTypeCode.FedEx]);

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