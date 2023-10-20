﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Manages all ShipmentTypes available in ShipWorks
    /// </summary>
    public static class ShipmentTypeManager
    {
        private static readonly List<ShipmentTypeCode> shipmentTypeCodes = EnumHelper.GetEnumList<ShipmentTypeCode>()
            .Select(s => s.Value)
            .ToList();

        /// <summary>
        /// Returns all shipment types in ShipWorks
        /// </summary>
        public static IEnumerable<ShipmentTypeCode> ShipmentTypeCodes =>
            shipmentTypeCodes
                .Where(IsCarrierAllowed)
                .OrderBy(GetSortValue);

        public static List<ShipmentType> ShipmentTypes =>
            ShipmentTypeCodes.Select(GetType).ToList();

        /// <summary>
        /// Gets a list of enabled shipment types
        /// </summary>
        public static List<ShipmentType> EnabledShipmentTypes =>
            ShipmentTypeCodes.Where(ShippingManager.IsShipmentTypeEnabled).Select(GetType).ToList();

        /// <summary>
        /// Gets a list of enabled shipment types
        /// </summary>
        public static IEnumerable<ShipmentTypeCode> EnabledShipmentTypeCodes =>
            ShipmentTypeCodes.Where(ShippingManager.IsShipmentTypeEnabled);

        /// <summary>
        /// Get the ShipmentTypeCode instance of the specified ShipmentEntity
        /// </summary>
        public static ShipmentType GetType(IShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            return GetType(shipment.ShipmentTypeCode);
        }

        /// <summary>
        /// Get the ShipmentType based on the given type code
        /// </summary>
        /// <remarks>The three shipment type codes remaining are to satisfy about 70 tests.</remarks>
        public static ShipmentType GetType(ShipmentTypeCode typeCode)
        {
            switch (typeCode)
            {
                case ShipmentTypeCode.Endicia:
                    return new EndiciaShipmentType();

                case ShipmentTypeCode.FedEx:
                    return new FedExShipmentType();
            }

            if (IoC.UnsafeGlobalLifetimeScope.IsRegisteredWithKey<ShipmentType>(typeCode))
            {
                return IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<Owned<ShipmentType>>(typeCode).Value;
            }

            throw new InvalidOperationException($"Invalid shipment type {typeCode}.");
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
                case ShipmentTypeCode.DhlExpress: return 12;
                case ShipmentTypeCode.DhlEcommerce: return 13;
                case ShipmentTypeCode.AmazonSFP: return 14;
                case ShipmentTypeCode.AmazonSWA: return 15;
                case ShipmentTypeCode.Asendia: return 16;
                case ShipmentTypeCode.Other: return 17;
                case ShipmentTypeCode.None: return 18;
            }

            throw new InvalidOperationException("Unhandled shipment type in GetSortValue");
        }

        /// <summary>
        /// Indicates if the shipment type is FedEx
        /// </summary>
        public static bool IsFedEx(ShipmentTypeCode shipmentTypeCode) => shipmentTypeCode == ShipmentTypeCode.FedEx;

        /// <summary>
        /// Indicates if the shipment type is iParcel
        /// </summary>
        public static bool IsiParcel(ShipmentTypeCode shipmentTypeCode) => shipmentTypeCode == ShipmentTypeCode.iParcel;

        /// <summary>
        /// Determines whether shipment is BestRate Shipment
        /// </summary>
        public static bool IsBestRate(ShipmentTypeCode shipmentTypeCode) => shipmentTypeCode == ShipmentTypeCode.BestRate;

        /// <summary>
        /// Indicates if the shipment type is UPS
        /// </summary>
        public static bool IsUps(ShipmentTypeCode shipmentTypeCode) =>
            shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools || shipmentTypeCode == ShipmentTypeCode.UpsWorldShip;

        /// <summary>
        /// Indicates if the shipment type is postal
        /// </summary>
        public static bool IsPostal(ShipmentTypeCode shipmentTypeCode) => PostalUtility.IsPostalShipmentType(shipmentTypeCode);

        /// <summary>
        /// Indicates if the shipment type code supports DHL
        /// </summary>
        public static bool ShipmentTypeCodeSupportsDhl(ShipmentTypeCode shipmentTypeCode) =>
            shipmentTypeCode == ShipmentTypeCode.Endicia || shipmentTypeCode == ShipmentTypeCode.Usps;

        /// <summary>
        /// Indicates if the given service represents a DHL service provided through Endicia
        /// </summary>
        public static bool IsEndiciaDhl(PostalServiceType postalService)
        {
            switch (postalService)
            {
                case PostalServiceType.DhlParcelGround:
                case PostalServiceType.DhlParcelExpedited:
                case PostalServiceType.DhlParcelPlusGround:
                case PostalServiceType.DhlParcelPlusExpedited:
                case PostalServiceType.DhlBpmGround:
                case PostalServiceType.DhlBpmExpedited:
                case PostalServiceType.DhlCatalogGround:
                case PostalServiceType.DhlCatalogExpedited:
                case PostalServiceType.DhlMediaMailGround:
                case PostalServiceType.DhlMarketingGround:
                case PostalServiceType.DhlMarketingExpedited:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is stamps DHL] [the specified postal service].
        /// </summary>
        public static bool IsStampsDhl(PostalServiceType postalService)
        {
            switch (postalService)
            {
                case PostalServiceType.DhlParcelExpedited:
                case PostalServiceType.DhlParcelGround:
                case PostalServiceType.DhlParcelPlusExpedited:
                case PostalServiceType.DhlParcelPlusGround:
                case PostalServiceType.DhlBpmExpedited:
                case PostalServiceType.DhlBpmGround:
                case PostalServiceType.DhlMarketingExpedited:
                case PostalServiceType.DhlMarketingGround:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is stamps DHL] or [is endicia DHL] [the specified postal service].
        /// </summary>
        public static bool IsDhl(PostalServiceType postalService) =>
            IsEndiciaDhl(postalService) || IsStampsDhl(postalService);

        /// <summary>
        /// Determines whether [is DHL smart mail] [the specified postal service].
        /// </summary>
        public static bool IsDhlSmartMail(PostalServiceType postalService)
        {
            switch (postalService)
            {
                case PostalServiceType.DhlParcelExpedited:
                case PostalServiceType.DhlParcelGround:
                case PostalServiceType.DhlParcelPlusExpedited:
                case PostalServiceType.DhlParcelPlusGround:
                case PostalServiceType.DhlBpmExpedited:
                case PostalServiceType.DhlBpmGround:
                case PostalServiceType.DhlCatalogExpedited:
                case PostalServiceType.DhlCatalogGround:
                case PostalServiceType.DhlMediaMailGround:
                case PostalServiceType.DhlMarketingGround:
                case PostalServiceType.DhlMarketingExpedited:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if the given service represents an Endicia consolidator service (that is NOT DHL GM)
        /// </summary>
        public static bool IsConsolidator(PostalServiceType service)
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

            return PostalUtility.UspsConsolidatorTypes.Contains(service);
        }

        private static bool IsCarrierAllowed(ShipmentTypeCode typeCode)
        {
            if(typeCode == ShipmentTypeCode.AmazonSWA) return true;
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();

                // If the active edition doesn't allow this ShipmentType, skip it
                if (licenseService.CheckRestriction(EditionFeature.ShipmentType, typeCode) == EditionRestrictionLevel.Hidden)
                {
                    return false;
                }

                if (typeCode == ShipmentTypeCode.Express1Usps)
                {
                    // The only time Express1 for USPS should be excluded is when USPS has never been setup but Endicia has been setup
                    if (!ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Usps) &&
                        !ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Express1Usps) && ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Endicia))
                    {
                        // USPS has never been setup, so we want to exclude the Express1/USPS type since Endicia IS setup in ShipWorks
                        return false;
                    }
                }
                else if (typeCode == ShipmentTypeCode.Express1Endicia)
                {
                    // We have an Express1 for Endicia shipment type which should be excluded if Endicia has never been setup
                    if (!ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Endicia) && !ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Express1Endicia))
                    {
                        // The Endicia type has never been setup, so we want to exclude the Express1 for Endicia type
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
