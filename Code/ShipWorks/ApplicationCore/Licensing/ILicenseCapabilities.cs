﻿using System;
using System.Collections.Generic;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface ILicenseCapabilities
    {
        /// <summary>
        /// Controls if DHL is enabled for Endicia users
        /// </summary>
        bool EndiciaDhl { get; set; }

        /// <summary>
        /// Controls if using Endicia insurance is enabled for Endicia users
        /// </summary>
        bool EndiciaInsurance { get; set; }

        /// <summary>
        /// ShipmentType, can be forbidden or just restricted to upgrade
        /// </summary>
        Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> ShipmentTypeRestriction { get; }

        /// <summary>
        /// Gets the shipping policy for a specific shipment type.
        /// </summary>
        Dictionary<ShipmentTypeCode, Dictionary<ShippingPolicyType, string>> ShipmentTypeShippingPolicy { get; }

        /// <summary>
        /// Restricted to a specific number of UPS accounts
        /// </summary>
        int UpsAccountLimit { get; set; }

        /// <summary>
        ///  Restricted to a specific UPS account number
        ///  </summary>
        IEnumerable<string> UpsAccountNumbers { get; set; }

        /// <summary>
        /// Restricted to using only postal APO\FPO\POBox services
        /// </summary>
        BrownPostalAvailability PostalAvailability { get; set; }

        /// <summary>
        /// UPS SurePost service type can be restricted
        /// </summary>
        bool UpsSurePost { get; set; }

        /// <summary>
        /// Gets or sets the ups status.
        /// </summary>
        UpsStatus UpsStatus { get; set; }

        /// <summary>
        /// Endicia consolidator
        /// </summary>
        bool EndiciaConsolidator { get; set; }

        /// <summary>
        /// Endicia Scan Based Returns can be Restricted
        /// </summary>
        bool EndiciaScanBasedReturns { get; set; }

        /// <summary>
        /// Controls if using Stamps insurance is enabled for Usps users
        /// </summary>
        bool StampsInsurance { get; set; }

        /// <summary>
        /// Controls if DHL is enabled for Stamps users
        /// </summary>
        bool StampsDhl { get; set; }

        /// <summary>
        /// Stamps Ascendia consolidator
        /// </summary>
        bool StampsAscendiaConsolidator { get; set; }

        /// <summary>
        /// Stamps DHL consolidator
        /// </summary>
        bool StampsDhlConsolidator { get; set; }

        /// <summary>
        /// Stamps Globegistics consolidator
        /// </summary>
        bool StampsGlobegisticsConsolidator { get; set; }

        /// <summary>
        /// Stamps Ibc consolidator
        /// </summary>
        bool StampsIbcConsolidator { get; set; }

        /// <summary>
        /// Stamps RrDonnelley consolidator
        /// </summary>
        bool StampsRrDonnelleyConsolidator { get; set; }

        /// <summary>
        /// Number of selling channels the license allows
        /// </summary>
        int ChannelLimit { get; set; }

        /// <summary>
        ///Number of shipments the license allows
        /// </summary>
        int ShipmentLimit { get; set; }

        /// <summary>
        /// Gets or sets the billing end date.
        /// </summary>
        DateTime BillingEndDate { get; set; }

        /// <summary>
        /// Details about the trial
        /// </summary>
        TrialDetails TrialDetails { get; set; }

        /// <summary>
        /// The number of Active Channels in tango
        /// </summary>
        int ActiveChannels { get; }

        /// <summary>
        /// The number of processed shipments in tango
        /// </summary>
        int ProcessedShipments { get; }

        /// <summary>
        /// Determines whether [is channel allowed] [the specified store type].
        /// </summary>
        bool IsChannelAllowed(StoreTypeCode storeType);

        /// <summary>
        /// Gets a value indicating whether best rate allowed given for this instance.
        /// </summary>
        /// <value><c>true</c> if the capabilities allows best rate; otherwise, <c>false</c>.</value>
        bool IsBestRateAllowed { get; }

        /// <summary>
        /// Gets the ShipmentType Functionality value for the given ShipmentType and PolicyType
        /// </summary>
        string GetShipmentTypeFunctionality(ShipmentTypeCode shipmentType, ShippingPolicyType policy);

        /// <summary>
        /// Controls if customer is allowed to use Warehouse Site
        /// </summary>
        bool Warehouse { get; set; }
    }
}