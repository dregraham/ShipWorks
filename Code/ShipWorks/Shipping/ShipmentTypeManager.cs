﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.None;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.EquaShip;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.iParcel;

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

                    // Jerks
                    if (typeCode == ShipmentTypeCode.EquaShip)
                    {
                        continue;
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

                case ShipmentTypeCode.EquaShip:
                    return new EquaShipShipmentType();

                case ShipmentTypeCode.FedEx:
                    return new FedExShipmentType();

                case ShipmentTypeCode.Endicia:
                    return new EndiciaShipmentType();

                case ShipmentTypeCode.PostalExpress1:
                    return new Express1ShipmentType();

                case ShipmentTypeCode.Stamps:
                    return new StampsShipmentType();

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
                case ShipmentTypeCode.FedEx: return 1;
                case ShipmentTypeCode.UpsOnLineTools: return 2;
                case ShipmentTypeCode.UpsWorldShip: return 3;
                case ShipmentTypeCode.Endicia: return 4;
                case ShipmentTypeCode.PostalExpress1: return 5;
                case ShipmentTypeCode.Stamps: return 6;
                case ShipmentTypeCode.PostalWebTools: return 7;
                case ShipmentTypeCode.EquaShip: return 8;
                case ShipmentTypeCode.OnTrac: return 9;
                case ShipmentTypeCode.iParcel: return 10;
                case ShipmentTypeCode.Other: return 11;
                case ShipmentTypeCode.None: return 12;
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
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.PostalExpress1:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Stamps:
                    return true;
            }

            return false;
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
                    return true;
            }

            return false;
        }
    }
}
