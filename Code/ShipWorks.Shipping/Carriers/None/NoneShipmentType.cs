﻿using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.None
{
    /// <summary>
    /// "None" shipment type implementation
    /// </summary>
    public class NoneShipmentType : ShipmentType
    {
        /// <summary>
        /// The ShipmentType code of this type
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.None;

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment) =>
            new List<IPackageAdapter> { new NullPackageAdapter() };

        /// <summary>
        /// Ensures that the carrier specific data for the shipment
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            // Loading shipment data does nothing for the none shipment type
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment) => string.Empty;

        /// <summary>
        /// Get the carrier specific description of the shipping service used
        /// </summary>
        public override string GetServiceDescription(string serviceCode) => string.Empty;

        /// <summary>
        /// No parcels for 'None' shipments
        /// </summary>
        public override int GetParcelCount(ShipmentEntity shipment) => 0;

        /// <summary>
        /// Get the parcel data for the shipment
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            throw new NotSupportedException("GetParcelDetail not supported for none.");
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the None shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a NullShippingBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository) => new NullShippingBroker();

        /// <summary>
        /// Apply default settings to the None profile
        /// </summary>
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            // The base configures dimensions for all shipment types. None is an exception and
            // doesn't need it.
            profile.Packages.Clear();
        }
    }
}
