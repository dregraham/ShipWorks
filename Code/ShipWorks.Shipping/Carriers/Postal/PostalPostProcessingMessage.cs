using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Show message for Postal Shipments after processing
    /// </summary>
    [KeyedComponent(typeof(ICarrierPostProcessingMessage), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(ICarrierPostProcessingMessage), ShipmentTypeCode.Endicia)]
    public class PostalPostProcessingMessage : ICarrierPostProcessingMessage
    {
        private readonly IGlobalPostLabelNotification globalPostNotification;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> endiciaAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalPostProcessingMessage(
            IGlobalPostLabelNotification globalPostNotification, 
            IDateTimeProvider dateTimeProvider, 
            ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> endiciaAccountRepository)
        {
            this.globalPostNotification = globalPostNotification;
            this.dateTimeProvider = dateTimeProvider;
            this.endiciaAccountRepository = endiciaAccountRepository;
        }

        /// <summary>
        /// Show messages that apply to the given shipment
        /// </summary>
        public void Show(IEnumerable<IShipmentEntity> processedShipments)
        {
            if (processedShipments.Any(ShowNotifiactionForShipment) && globalPostNotification.AppliesToCurrentUser())
            {
                globalPostNotification.Show();
            }
        }

        /// <summary>
        /// Should we show the notification
        /// </summary>
        private bool ShowNotifiactionForShipment(IShipmentEntity shipment)
        {
            bool showNotifiaction = IsGapLabel(shipment.Postal) || 
                PostalUtility.IsGlobalPost((PostalServiceType) shipment.Postal.Service);

            if (shipment.ShipmentTypeCode == ShipmentTypeCode.Endicia)
            {
                return !IsEndiciaReseller(shipment) && showNotifiaction;
            }

            return showNotifiaction;
        }

        /// <summary>
        /// Is this shipment an Endicia Express 1 shipment
        /// </summary>
        private bool IsEndiciaReseller(IShipmentEntity shipment)
        {
            return endiciaAccountRepository.GetAccountReadOnly(shipment)?.EndiciaReseller != (int)EndiciaReseller.None;
        }

        /// <summary>
        /// Determines whether the shipment is a Gap shipment.
        /// </summary>
        private bool IsGapLabel(IPostalShipmentEntity shipment)
        {
            if (dateTimeProvider.Now < new DateTime(2018, 1, 21))
            {
                return false;
            }

            if (shipment.Service == (int) PostalServiceType.InternationalFirst &&
                shipment.CustomsContentType != (int) PostalCustomsContentType.Documents &&
                (shipment.PackagingType == (int) PostalPackagingType.Envelope ||
                    shipment.PackagingType == (int) PostalPackagingType.LargeEnvelope))
            {
                return true;
            }

            return false;
        }
    }
}
