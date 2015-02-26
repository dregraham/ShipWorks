using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.None;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Manages all ShipmentTypes available in ShipWorks
    /// </summary>
    public static class ShipmentTypeManager
    {        
        /// <summary>
        /// Returns all shipment types in ShipWorks
        /// </summary>
        public static List<ShipmentType> ShipmentTypes
        {
            get
            {
                List<ShipmentType> shipmentTypes = new List<ShipmentType>();

                foreach (ShipmentTypeCode typeCode in Enum.GetValues(typeof(ShipmentTypeCode)))
                {
                    // If the active edition doesn't allow this ShipmentType, skip it
                    if (EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.ShipmentType, typeCode).Level == EditionRestrictionLevel.Hidden)
                    {
                        continue;
                    }

                    if (typeCode == ShipmentTypeCode.Express1Usps)
                    {
                        // The only time Express1 for USPS should be excluded is when USPS has never been setup but Endicia has been setup
                        if (!ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Usps) &&
                            !ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Express1Usps) && ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Endicia))
                        {
                            // USPS has never been setup, so we want to exclude the Express1/USPS type since Endicia IS setup in ShipWorks
                            continue;
                        }
                        
                    }
                    else if (typeCode == ShipmentTypeCode.Express1Endicia)
                    {
                        // We have an Express1 for Endicia shipment type which should be excluded if Endicia has never been setup
                        if (!ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Endicia) && !ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Express1Endicia))
                        {
                            // The Endicia type has never been setup, so we want to exclude the Express1 for Endicia type
                            continue;
                        }
                    }

                    shipmentTypes.Add(GetType(typeCode));
                }

                // Sort based on the shipment type name
                shipmentTypes.Sort(new Comparison<ShipmentType>(delegate(ShipmentType left, ShipmentType right)
                {
                    return GetSortValue(left.ShipmentTypeCode).CompareTo(GetSortValue(right.ShipmentTypeCode));
                }));

                return shipmentTypes;
            }
        }

        /// <summary>
        /// Gets a list of enabled shipment types
        /// </summary>
        public static List<ShipmentType> EnabledShipmentTypes
        {
            get { return ShipmentTypes.Where(s => ShippingManager.IsShipmentTypeEnabled(s.ShipmentTypeCode)).ToList(); }
        }

        /// <summary>
        /// Get the StoreType instance of the specified ShipmentEntity
        /// </summary>
        public static ShipmentType GetType(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return GetType((ShipmentTypeCode) shipment.ShipmentType);
        }

        /// <summary>
        /// Get the ShipmentType based on the given type code
        /// </summary>
        public static ShipmentType GetType(ShipmentTypeCode typeCode)
        {
            switch (typeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                    return new UpsOltShipmentType();

                case ShipmentTypeCode.UpsWorldShip:
                    return new WorldShipShipmentType();

                case ShipmentTypeCode.FedEx:
                    return new FedExShipmentType();

                case ShipmentTypeCode.Endicia:
                    return new EndiciaShipmentType();

                case ShipmentTypeCode.Express1Endicia:
                    return new Express1EndiciaShipmentType();

                case ShipmentTypeCode.Express1Usps:
                    return new Express1UspsShipmentType();

                case ShipmentTypeCode.PostalWebTools:
                    return new PostalWebShipmentType();

                case ShipmentTypeCode.Other:
                    return new OtherShipmentType();

                case ShipmentTypeCode.None:
                    return new NoneShipmentType();

                case ShipmentTypeCode.OnTrac:
                    return new OnTracShipmentType();

                case ShipmentTypeCode.iParcel:
                    return new iParcelShipmentType();

                case ShipmentTypeCode.BestRate:
                    return new BestRateShipmentType();

                case ShipmentTypeCode.Usps:
                    return new UspsShipmentType();
            }

            throw new InvalidOperationException("Invalid shipment type.");
        }

        /// <summary>
        /// Get the effective sort value of the given type code.  This provides our custom sorting
        /// </summary>
        public static int GetSortValue(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.BestRate: return 1;
                case ShipmentTypeCode.Usps: return 2;
                case ShipmentTypeCode.PostalWebTools: return 3;
                case ShipmentTypeCode.Express1Usps: return 4;
                case ShipmentTypeCode.FedEx: return 5;
                case ShipmentTypeCode.UpsOnLineTools: return 6;
                case ShipmentTypeCode.UpsWorldShip: return 7;
                case ShipmentTypeCode.Endicia: return 8;
                case ShipmentTypeCode.Express1Endicia: return 9;
                case ShipmentTypeCode.OnTrac: return 10;
                case ShipmentTypeCode.iParcel: return 11;
                case ShipmentTypeCode.Other: return 12;
                case ShipmentTypeCode.None: return 13;
            }

            throw new InvalidOperationException("Unhandled shipment type in GetSortValue");
        }

        /// <summary>
        /// Indicates if the shipment type is FedEx
        /// </summary>
        public static bool IsFedEx(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.FedEx: return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if the shipment type is iParcel
        /// </summary>
        public static bool IsiParcel(ShipmentTypeCode shipmentTypeCode)
        {
            return shipmentTypeCode == ShipmentTypeCode.iParcel;
        }

        /// <summary>
        /// Determines whether shipment is BestRate Shipment
        /// </summary>
        public static bool IsBestRate(ShipmentTypeCode shipmentTypeCode)
        {
            return shipmentTypeCode == ShipmentTypeCode.BestRate;

        }

        /// <summary>
        /// Indicates if the shipment type is UPS
        /// </summary>
        public static bool IsUps(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if the shipment type is postal
        /// </summary>
        public static bool IsPostal(ShipmentTypeCode shipmentTypeCode)
        {
            return PostalUtility.IsPostalShipmentType(shipmentTypeCode);
        }

        /// <summary>
        /// Indicates if the given service represents a DHL service provided through Endicia
        /// </summary>
        public static bool IsEndiciaDhl(PostalServiceType postalService)
        {
            switch (postalService)
            {
                case PostalServiceType.DhlParcelStandard:
                case PostalServiceType.DhlParcelExpedited:
                case PostalServiceType.DhlParcelPlusStandard:
                case PostalServiceType.DhlParcelPlusExpedited:
                case PostalServiceType.DhlBpmStandard:
                case PostalServiceType.DhlBpmExpedited:
                case PostalServiceType.DhlCatalogStandard:
                case PostalServiceType.DhlCatalogExpedited:
                case PostalServiceType.DhlMediaMailStandard:
                case PostalServiceType.DhlMarketingStandard:
                case PostalServiceType.DhlMarketingExpedited:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if the given service represents an Endicia consolidator service (that is NOT DHL GM)
        /// </summary>
        public static bool IsEndiciaConsolidator(PostalServiceType service)
        {
            switch (service)
            {
                case PostalServiceType.ConsolidatorDomestic:
                case PostalServiceType.ConsolidatorInternational:
                case PostalServiceType.ConsolidatorIpa:
                case PostalServiceType.ConsolidatorIsal:
                case PostalServiceType.CommercialePacket:
                    return true;
            }

            return false;
        }

   }
}
