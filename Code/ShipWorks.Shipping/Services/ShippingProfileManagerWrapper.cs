using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Wraps the static ShippingProfileManager with an instance that implements an interface
    /// </summary>
    public class ShippingProfileManagerWrapper : IShippingProfileManager
    {
        private static readonly object syncLock = new object();
        private readonly Dictionary<string, PropertyInfo> propertyMap = new Dictionary<string, PropertyInfo>();
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
        public void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                // If this is the first time loading it, or we are supposed to refresh, do it now
                if (!profile.IsNew && refreshIfPresent)
                {
                    LoadPackages(profile, sqlAdapter);
                }

                if (profile.Packages.None() &&
                    (profile.ShipmentType == null || !shipmentTypeManager.Get(profile.ShipmentType.Value).SupportsMultiplePackages))
                {
                    profile.Packages.Add(new PackageProfileEntity());
                }

                LoadChildProfiles(profile, refreshIfPresent, sqlAdapter);
            }
        }

        /// <summary>
        /// Load the profiles children
        /// </summary>
        private void LoadChildProfiles(ShippingProfileEntity profile, bool refreshIfPresent, ISqlAdapter sqlAdapter)
        {
            if (profile.ShipmentType != null && profile.ShipmentType.Value != ShipmentTypeCode.None)
            {
                ShipmentTypeCode shipmentType = profile.ShipmentType.Value;
                (string propertyName, Type type) = GetChildPropertyNameAndType(shipmentType);
                LoadProfileData(profile, propertyName, type, refreshIfPresent, sqlAdapter);

                if (PostalUtility.IsPostalShipmentType(shipmentType) && shipmentType != ShipmentTypeCode.PostalWebTools)
                {
                    (string postalChildPropertyName, Type postalChildType) = GetPostalChildPropertyNameAndType(shipmentType);

                    LoadProfileData(profile.Postal, postalChildPropertyName, postalChildType, refreshIfPresent, sqlAdapter);
                }
            }
        }

        /// <summary>
        /// Load the profiles packages
        /// </summary>
        private void LoadPackages(ShippingProfileEntity profile, ISqlAdapter sqlAdapter)
        {
            profile.Packages.Clear();

            sqlAdapter.FetchEntityCollection(profile.Packages,
                new RelationPredicateBucket(PackageProfileFields.ShippingProfileID == profile.ShippingProfileID));
            profile.Packages.Sort((int) PackageProfileFieldIndex.PackageProfileID, ListSortDirection.Ascending);
        }

        /// <summary>
        /// Get the profiles child property name and type for the ShipmentTypeCode
        /// </summary>
        private static (string, Type) GetPostalChildPropertyNameAndType(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Endicia:
                    return ("Endicia", typeof(EndiciaProfileEntity));
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    return ("Usps", typeof(UspsProfileEntity));
                default:
                    throw new InvalidOperationException("Unknown child property for ShipmentTypeCode");
            }
        }

        /// <summary>
        /// Get the profiles child property name and type for the ShipmentTypeCode
        /// </summary>
        private static (string, Type) GetChildPropertyNameAndType(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return ("Ups", typeof(UpsProfileEntity));
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    return ("Postal", typeof(PostalProfileEntity));
                case ShipmentTypeCode.FedEx:
                    return ("FedEx", typeof(FedExProfileEntity));
                case ShipmentTypeCode.OnTrac:
                    return ("OnTrac", typeof(OnTracProfileEntity));
                case ShipmentTypeCode.iParcel:
                    return ("IParcel", typeof(IParcelProfileEntity));
                case ShipmentTypeCode.Other:
                    return ("Other", typeof(OtherProfileEntity));
                case ShipmentTypeCode.BestRate:
                    return ("BestRate", typeof(BestRateProfileEntity));
                case ShipmentTypeCode.Amazon:
                    return ("Amazon", typeof(AmazonProfileEntity));
                case ShipmentTypeCode.DhlExpress:
                    return ("DhlExpress", typeof(DhlExpressProfileEntity));
                case ShipmentTypeCode.Asendia:
                    return ("Asendia", typeof(AsendiaProfileEntity));
                default:
                    throw new InvalidOperationException($"Unknown child property for ShipmentTypeCode {shipmentTypeCode}");
            }
        }

        /// <summary>
        /// Load an existing profile data into the parent entity, or create if it doesn't exist.  If its already loaded and present
        /// it can be optionally refreshed.
        /// </summary>
        private void LoadProfileData(EntityBase2 parent, string childProperty, Type profileType, bool refreshIfPresent, ISqlAdapter sqlAdapter)
        {
            PropertyInfo property = GetChildProperty(parent, childProperty);

            // See if the profile data is already present
            if (refreshIfPresent)
            {
                property.SetValue(parent, null, null);
            }

            // If the profile isn't there we have to fetch it
            if (property.GetValue(parent, null) == null)
            {
                EntityBase2 childEntity;

                // Try to fetch the existing profile data for the shipment
                if (parent.Fields.State != EntityState.New)
                {
                    childEntity = (EntityBase2) Activator.CreateInstance(profileType, parent.Fields["ShippingProfileID"].CurrentValue);
                    sqlAdapter.FetchEntity(childEntity);
                }
                // If the parent is new, just create a new child.
                else
                {
                    childEntity = (EntityBase2) Activator.CreateInstance(profileType);
                }

                // Apply the reference
                property.SetValue(parent, childEntity, null);

                // If it doesn't exist, then we have to create to save it as new
                if (childEntity.Fields.State != EntityState.Fetched)
                {
                    // Reset the object to new and apply
                    childEntity.Fields.State = EntityState.New;
                }
            }
        }

        /// <summary>
        /// Get the reflected property represented the given property name
        /// </summary>
        private PropertyInfo GetChildProperty(EntityBase2 parent, string childProperty)
        {
            Type type = parent.GetType();
            string identifier = type.FullName + "." + childProperty;

            if (!propertyMap.TryGetValue(identifier, out PropertyInfo property))
            {
                property = type.GetProperty(childProperty);

                propertyMap[identifier] = property;
            }

            return property;
        }
    }
}