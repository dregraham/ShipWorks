using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Services;

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
        private readonly IReadOnlyCarrierAccountRetriever<IEndiciaAccountEntity> endiciaAccountRepository;
        private readonly IReadOnlyCarrierAccountRetriever<IUspsAccountEntity> uspsAccountRetriever;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalPostProcessingMessage(
            IGlobalPostLabelNotification globalPostNotification,
            IDateTimeProvider dateTimeProvider,
            IReadOnlyCarrierAccountRetriever<IEndiciaAccountEntity> endiciaAccountRepository,
            IReadOnlyCarrierAccountRetriever<IUspsAccountEntity> uspsAccountRetriever)
        {
            this.globalPostNotification = globalPostNotification;
            this.dateTimeProvider = dateTimeProvider;
            this.endiciaAccountRepository = endiciaAccountRepository;
            this.uspsAccountRetriever = uspsAccountRetriever;
        }

        /// <summary>
        /// Show messages that apply to the given shipment
        /// </summary>
        public void Show(IEnumerable<IShipmentEntity> processedShipments)
        {
            IShipmentEntity gapShipment = processedShipments.FirstOrDefault(s => ShowNotificationForShipment(s));

            if (gapShipment != null && globalPostNotification.AppliesToCurrentUser())
            {
                globalPostNotification.Show(gapShipment);
            }
        }

        /// <summary>
        /// Should we show the notification
        /// </summary>
        private bool ShowNotificationForShipment(IShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Postal == null)
            {
                return false;
            }

            return IsGapShipment(shipment) || IsPresortShipment(shipment);
        }

        /// <summary>
        /// Is the shipment presort?
        /// </summary>
        private bool IsPresortShipment(IShipmentEntity shipment)
        {
            if (shipment.Postal?.Usps == null)
            {
                return false;
            }

            var account = uspsAccountRetriever.GetAccountReadOnly(shipment);

            return (shipment.Postal.Service == (int) PostalServiceType.InternationalFirst &&
                    ((GlobalPostServiceAvailability) account.GlobalPostAvailability).HasFlag(GlobalPostServiceAvailability.InternationalFirst)) ||
                (shipment.Postal.Service == (int) PostalServiceType.InternationalPriority &&
                    ((GlobalPostServiceAvailability) account.GlobalPostAvailability).HasFlag(GlobalPostServiceAvailability.InternationalPriority)) ||
                (shipment.Postal.Service == (int) PostalServiceType.InternationalExpress &&
                    ((GlobalPostServiceAvailability) account.GlobalPostAvailability).HasFlag(GlobalPostServiceAvailability.InternationalExpress));
        }

        /// <summary>
        /// Is the shipment Gap?
        /// </summary>
        private bool IsGapShipment(IShipmentEntity shipment)
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
            return endiciaAccountRepository.GetAccountReadOnly(shipment)?.EndiciaReseller != (int) EndiciaReseller.None;
        }

        /// <summary>
        /// Determines whether the shipment is a Gap shipment.
        /// </summary>
        private bool IsGapLabel(IPostalShipmentEntity shipment)
        {
            if (dateTimeProvider.Now < new DateTime(2018, 1, 21) && !InterapptiveOnly.MagicKeysDown)
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
