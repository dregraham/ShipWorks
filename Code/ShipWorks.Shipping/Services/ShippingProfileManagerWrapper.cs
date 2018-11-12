using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Wraps the static ShippingProfileManager with an instance that implements an interface
    /// </summary>
    public class ShippingProfileManagerWrapper : IShippingProfileManager
    {
        private static readonly object syncLock = new object();
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerWrapper(
            IShipmentTypeManager shipmentTypeManager,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Initialize ShippingProfileManager
        /// </summary>
        public void InitializeForCurrentSession()
        {
            ShippingProfileManager.InitializeForCurrentSession();
        }

        /// <summary>
        /// Delete the given profile
        /// </summary>
        public void DeleteProfile(ShippingProfileEntity profile)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                DeleteProfile(profile, adapter);
                adapter.Commit();
            }

            ShippingProfileManager.CheckForChangesNeeded();
        }

        /// <summary>
        /// Delete the given profile
        /// </summary>
        public void DeleteProfile(ShippingProfileEntity profile, ISqlAdapter adapter)
        {
            adapter.DeleteEntity(profile);
            ShippingProfileManager.CheckForChangesNeeded();
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
                profile.ShipmentType = shipmentType.ShipmentTypeCode;
                profile.ShipmentTypePrimary = true;

                // Load the shipmentType specific profile data
                LoadProfileData(profile, true);

                // Configure it as a primary profile
                shipmentType.ConfigurePrimaryProfile(profile);
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
        public IEnumerable<IShippingProfileEntity> GetProfilesFor(ShipmentTypeCode shipmentType, bool includeDefaultProfiles) =>
            ShippingProfileManager.GetProfilesFor(shipmentType, includeDefaultProfiles);

        /// <summary>
        /// Return the active list of all profiles
        /// </summary>
        public IEnumerable<ShippingProfileEntity> Profiles => ShippingProfileManager.Profiles;

        /// <summary>
        /// Return the active list of all profiles
        /// </summary>
        public IEnumerable<IShippingProfileEntity> ProfilesReadOnly => ShippingProfileManager.ProfilesReadOnly;

        /// <summary>
        /// Get profiles for the given shipment type
        /// </summary>
        public IEnumerable<IShippingProfileEntity> GetProfilesReadOnlyFor(ShipmentTypeCode value)
        {
            return ShippingProfileManager.ProfilesReadOnly.Where(x => x.ShipmentType == value);
        }

        /// <summary>
        /// Saves the given profile
        /// </summary>
        public void SaveProfile(ShippingProfileEntity profile)
        {
            ShippingProfileManager.SaveProfile(profile);
        }

        /// <summary>
        /// Saves the given profile
        /// </summary>
        public void SaveProfile(ShippingProfileEntity profile, ISqlAdapter adapter)
        {
            ShippingProfileManager.SaveProfile(profile, adapter);
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public void LoadProfileData(ShippingProfileEntity shippingProfileEntity, bool refreshIfPresent)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                ShippingProfileManager.LoadProfileData(shippingProfileEntity, refreshIfPresent, adapter);
            }
        }
    }
}