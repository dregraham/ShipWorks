﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Manages shipment profiles
    /// </summary>
    public interface IShippingProfileManager
    {
        /// <summary>
        /// Return the active list of all profiles
        /// </summary>
        IEnumerable<ShippingProfileEntity> Profiles { get; }

        /// <summary>
        /// Get the default profile for the given shipment type
        /// </summary>
        ShippingProfileEntity GetDefaultProfile(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Get the default profile for the given shipment type
        /// </summary>
        IShippingProfileEntity GetDefaultProfileReadOnly(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Gets or creates a profile with the default settings for the shipment type
        /// </summary>
        ShippingProfileEntity GetOrCreatePrimaryProfile(ShipmentType shipmentType);

        /// <summary>
        /// Gets or creates a profile with the default settings for the shipment type
        /// </summary>
        IShippingProfileEntity GetOrCreatePrimaryProfileReadOnly(ShipmentType shipmentType);

        /// <summary>
        /// Get the specified profile
        /// </summary>
        IShippingProfileEntity GetProfileReadOnly(long profileID);

        /// <summary>
        /// Get profiles for the given shipment type
        /// </summary>
        IEnumerable<ShippingProfileEntity> GetProfilesFor(ShipmentTypeCode value);

        /// <summary>
        /// Get profiles for the given shipment type
        /// </summary>
        IEnumerable<IShippingProfileEntity> GetProfilesReadOnlyFor(ShipmentTypeCode value);

        /// <summary>
        /// Saves the given profile
        /// </summary>
        void SaveProfile(ShippingProfileEntity profile);

        /// <summary>
        /// Initialize ShippingProfileManager
        /// </summary>
        void InitializeForCurrentSession();

        /// <summary>
        /// Deletes the given profile
        /// </summary>
        void DeleteProfile(ShippingProfileEntity profile);
    }
}