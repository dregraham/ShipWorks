﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Shipment type for Express 1 shipments.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public class Express1EndiciaShipmentType : EndiciaShipmentType
    {
        /// <summary>
        /// Create an instance of the Express1 Endicia Shipment Type
        /// </summary>
        public Express1EndiciaShipmentType()
        {
            AccountRepository = new Express1EndiciaAccountRepository();
        }

        /// <summary>
        /// Postal Shipment Type
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.Express1Endicia;

        /// <summary>
        /// The user-displayable name of the shipment type
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string ShipmentTypeName
        {
            get
            {
                return
                    (ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Usps) ||
                    ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Express1Usps)) ?
                    "USPS (Express1 for Endicia)" : "USPS (Express1)";
            }
        }

        /// <summary>
        /// Reseller type
        /// </summary>
        public override EndiciaReseller EndiciaReseller => EndiciaReseller.Express1;

        /// <summary>
        /// Create the Service Control
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new Express1EndiciaServiceControl(rateControl);
        }

        /// <summary>
        /// Gets the package types that have been available for this shipment type
        /// </summary>
        public override IEnumerable<int> GetAvailablePackageTypes(IExcludedPackageTypeRepository repository)
        {
            // All package types including cubic are available to Express1/Endicia
            return EnumHelper.GetEnumList<PostalPackagingType>()
                .Select(x => x.Value)
                .Where(x => !EnumHelper.GetDeprecated(x))
                .Cast<int>()
                .Except(GetExcludedPackageTypes(repository));
        }

        /// <summary>
        /// Get the Express1 MailClass code for the given service
        /// </summary>
        public override string GetMailClassCode(PostalServiceType serviceType, PostalPackagingType packagingType)
        {
            // Express1 is not supporting the July 2013 Express/Priority Express updates, so this is the same
            // as the virtual method it overrides with the exception of ExpressMail returns "Express" instead
            // of "PriorityExpress"
            switch (serviceType)
            {
                case PostalServiceType.ExpressMail: return "Express";
                case PostalServiceType.FirstClass: return "First";
                case PostalServiceType.LibraryMail: return "LibraryMail";
                case PostalServiceType.MediaMail: return "MediaMail";
                case PostalServiceType.StandardPost: return "StandardPost";
                case PostalServiceType.ParcelSelect: return "ParcelSelect";
                case PostalServiceType.PriorityMail: return "Priority";
                case PostalServiceType.CriticalMail: return "CriticalMail";

                case PostalServiceType.InternationalExpress: return "ExpressMailInternational";
                case PostalServiceType.InternationalPriority: return "PriorityMailInternational";

                case PostalServiceType.InternationalFirst:
                    {
                        return PostalUtility.IsEnvelopeOrFlat(packagingType) ? "FirstClassMailInternational" : "FirstClassPackageInternationalService";
                    }
            }

            if (ShipmentTypeManager.IsEndiciaDhl(serviceType))
            {
                return EnumHelper.GetApiValue(serviceType);
            }

            throw new EndiciaException($"{PostalUtility.GetPostalServiceTypeDescription(serviceType)} is not supported when shipping with Endicia.");
        }

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates => false;

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            // Don't update Express1 entries because they could overwrite Endicia records
        }

        /// <summary>
        /// Track the given Express1 shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            return PostalWebClientTracking.TrackShipment(shipment.TrackingNumber);
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the Express1 for USPS shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an Express1UspsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository)
        {
            return new NullShippingBroker();
        }
    }
}
