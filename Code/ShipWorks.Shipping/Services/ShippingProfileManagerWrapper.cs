using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Wraps the static ShippingProfileManager with an instance that implements an interface
    /// </summary>
    public class ShippingProfileManagerWrapper : IShippingProfileManager
    {
        private static object syncLock = new object();

        /// <summary>
        /// Initialize ShippingProfileManager
        /// </summary>
        public void InitializeForCurrentSession()
        {
            ShippingProfileManager.InitializeForCurrentSession();
        }

        /// <summary>
        /// Get the default profile for the given shipment type
        /// </summary>
        public ShippingProfileEntity GetDefaultProfile(ShipmentTypeCode shipmentTypeCode)
        {
            return ShippingProfileManager.GetDefaultProfile(shipmentTypeCode);
        }

        /// <summary>
        /// Get the default profile for the given shipment type
        /// </summary>
        public IShippingProfileEntity GetDefaultProfileReadOnly(ShipmentTypeCode shipmentTypeCode) =>
            ShippingProfileManager.GetDefaultProfileReadOnly(shipmentTypeCode);

        /// <summary>
        /// Create a profile with the default settings for the shipment type
        /// </summary>
        public ShippingProfileEntity GetOrCreatePrimaryProfile(ShipmentType shipmentType)
        {
            ShippingProfileEntity profile = GetDefaultProfile(shipmentType.ShipmentTypeCode);
            if (profile != null)
            {
                return profile;
            }

            lock (syncLock)
            {
                profile = GetDefaultProfile(shipmentType.ShipmentTypeCode);
                if (profile != null)
                {
                    return profile;
                }

                profile = new ShippingProfileEntity();
                profile.Name = string.Format("Defaults - {0}", shipmentType.ShipmentTypeName);
                profile.ShipmentTypeCode = shipmentType.ShipmentTypeCode;
                profile.ShipmentTypePrimary = true;

                // Load the shipmentType specific profile data
                shipmentType.LoadProfileData(profile, true);

                // Configure it as a primary profile
                shipmentType.ConfigurePrimaryProfile(profile);

                // Save the profile
                ShippingProfileManager.SaveProfile(profile);
            }

            return profile;
        }

        /// <summary>
        /// Create a profile with the default settings for the shipment type
        /// </summary>
        public IShippingProfileEntity GetOrCreatePrimaryProfileReadOnly(ShipmentType shipmentType)
        {
            return GetDefaultProfileReadOnly(shipmentType.ShipmentTypeCode) ??
                GetOrCreatePrimaryProfile(shipmentType).AsReadOnly();
        }

        /// <summary>
        /// Get the specified profile
        /// </summary>
        public IShippingProfileEntity GetProfileReadOnly(long profileID) =>
            ShippingProfileManager.GetProfileReadOnly(profileID);

        /// <summary>
        /// Get profiles for the given shipment type
        /// </summary>
        public IEnumerable<ShippingProfileEntity> GetProfilesFor(ShipmentTypeCode value)
        {
            return ShippingProfileManager.Profiles.Where(x => x.ShipmentTypeCode == value);
        }

        /// <summary>
        /// Get profiles for the given shipment type
        /// </summary>
        public IEnumerable<IShippingProfileEntity> GetProfilesReadOnlyFor(ShipmentTypeCode value)
        {
            return ShippingProfileManager.ProfilesReadOnly.Where(x => x.ShipmentTypeCode == value);
        }

        /// <summary>
        /// Saves the given profile
        /// </summary>
        public void SaveProfile(ShippingProfileEntity profile)
        {
            ShippingProfileManager.SaveProfile(profile);
        }
    }
}