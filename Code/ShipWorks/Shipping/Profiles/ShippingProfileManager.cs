using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Manages shipment profiles
    /// </summary>
    public static class ShippingProfileManager
    {
        private static IEnumerable<IShippingProfileEntity> readOnlyEntities;
        private static TableSynchronizer<ShippingProfileEntity> synchronizer;
        private static bool needCheckForChanges = false;
        private static IShipmentTypeManager shipmentTypeManager;
        private readonly static Dictionary<string, PropertyInfo> propertyMap = new Dictionary<string, PropertyInfo>();

        /// <summary>
        /// Initialize ShippingProfileManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<ShippingProfileEntity>();
            shipmentTypeManager = IoC.UnsafeGlobalLifetimeScope.Resolve<IShipmentTypeManager>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            lock (synchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (synchronizer)
            {
                List<ShippingProfileEntity> modified = new List<ShippingProfileEntity>();
                List<ShippingProfileEntity> added = new List<ShippingProfileEntity>();

                if (synchronizer.Synchronize(modified, added))
                {
                    synchronizer.EntityCollection.Sort((int) ShippingProfileFieldIndex.Name, ListSortDirection.Ascending);

                    using (ISqlAdapter adapter = new SqlAdapter(false))
                    {
                        foreach (ShippingProfileEntity profile in modified.Concat(added))
                        {
                            LoadProfileData(profile, true, adapter);
                        }
                    }

                    readOnlyEntities = synchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Return the active list of all profiles
        /// </summary>
        public static List<ShippingProfileEntity> Profiles
        {
            get
            {
                lock (synchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    return EntityUtility.CloneEntityCollection(synchronizer.EntityCollection);
                }
            }
        }

        /// <summary>
        /// Return the active list of all profiles
        /// </summary>
        public static IEnumerable<IShippingProfileEntity> ProfilesReadOnly
        {
            get
            {
                lock (synchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    return readOnlyEntities;
                }
            }
        }

        /// <summary>
        /// Get the profile with the specified ID, or null if it does not exist
        /// </summary>
        public static ShippingProfileEntity GetProfile(long profileID)
        {
            return Profiles.SingleOrDefault(p => p.ShippingProfileID == profileID);
        }

        /// <summary>
        /// Get the profile with the specified ID, or null if it does not exist
        /// </summary>
        public static IShippingProfileEntity GetProfileReadOnly(long profileID)
        {
            return ProfilesReadOnly.SingleOrDefault(p => p.ShippingProfileID == profileID);
        }
        
        /// <summary>
        /// Get profiles for the given shipment type
        /// </summary>
        public static IEnumerable<IShippingProfileEntity> GetProfilesFor(ShipmentTypeCode shipmentTypeCode, 
                                                                         bool includeDefaultProfiles)
        {
            IEnumerable<IShippingProfileEntity> profiles = ProfilesReadOnly.Where(p => p.ShipmentType == null ||
                                                                                       p.ShipmentType == shipmentTypeCode);

            if (!includeDefaultProfiles)
            {
                profiles = profiles.Where(p => !p.ShipmentTypePrimary);
            }
            
            return profiles;
        }
        
        /// <summary>
        /// Save the given profile to the database
        /// </summary>
        public static void SaveProfile(ShippingProfileEntity profile)
        {
            using (SqlAdapter adapter = new SqlAdapter(false))
            {
                SaveProfile(profile, adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Save the given profile to the database
        /// </summary>
        public static void SaveProfile(ShippingProfileEntity profile, ISqlAdapter adapter)
        {
            bool rootDirty = profile.IsDirty;
            bool anyDirty = new ObjectGraphUtils().ProduceTopologyOrderedList<IEntity2>(profile).Any(e => e.IsDirty);
            bool extraDirty = SaveProfilePackages(profile, adapter);

            // Force the profile change if any derived stuff changes
            if ((anyDirty || extraDirty) && !rootDirty)
            {
                profile.Fields[(int) ShippingProfileFieldIndex.ShipmentType].IsChanged = true;
                profile.Fields.IsDirty = true;
            }

            // Save the base profile
            adapter.SaveAndRefetch(profile);

            lock (synchronizer)
            {
                synchronizer.MergeEntity(profile);
                CheckForChangesNeeded();
            }
        }

        /// <summary>
        /// Save the profile packages
        /// </summary>
        private static bool SaveProfilePackages(ShippingProfileEntity profile, ISqlAdapter adapter)
        {
            bool changes = false;

            // First delete out anything that needs deleted
            // Introducing new variable as we will be removing items from PackageProfile
            // and if we used the same colleciton, we would get an exception.
            List<PackageProfileEntity> allPackageProfiles = profile.Packages.Where(package => package.Fields.State == EntityState.Deleted).ToList();
            foreach (PackageProfileEntity package in allPackageProfiles)
            {
                // If its new but deleted, just get rid of it                
                if (package.IsNew)
                {
                    profile.Packages.Remove(package);
                }

                // If its deleted, delete it
                else
                {
                    package.Fields.State = EntityState.Fetched;
                    profile.Packages.Remove(package);

                    adapter.DeleteEntity(package);

                    changes = true;
                }                
            }

            return changes;
        }

        /// <summary>
        /// Checks whether the name of the profile already exists in another profile
        /// </summary>
        public static bool DoesNameExist(IShippingProfileEntity profile)
        {
            if (profile == null)
            {
                return false;
            }

            int profileCount = ShippingProfileCollection.GetCount(SqlAdapter.Default,
                ShippingProfileFields.ShippingProfileID != profile.ShippingProfileID &
                ShippingProfileFields.Name == profile.Name);

            return (profileCount != 0);
        }

        /// <summary>
        /// Get the default profile for the given shipment type
        /// </summary>
        public static ShippingProfileEntity GetDefaultProfile(ShipmentTypeCode shipmentTypeCode)
        {
            return Profiles.FirstOrDefault(p =>
                p.ShipmentType == shipmentTypeCode && p.ShipmentTypePrimary);
        }

        /// <summary>
        /// Get the default profile for the given shipment type
        /// </summary>
        public static IShippingProfileEntity GetDefaultProfileReadOnly(ShipmentTypeCode shipmentTypeCode)
        {
            return ProfilesReadOnly.FirstOrDefault(p =>
                p.ShipmentType == shipmentTypeCode && p.ShipmentTypePrimary);
        }

        /// <summary>
        /// Collect telemetry data for shipping profiles
        /// </summary>
        public static IDictionary<string, string> GetTelemetryData()
        {
            Dictionary<string, string> telemetryData = new Dictionary<string, string>();
            telemetryData.Add("Shipping.Profiles.Count", ProfilesReadOnly.Count().ToString());

            foreach (IGrouping<ShipmentTypeCode?, IShippingProfileEntity> carrierProfiles in ProfilesReadOnly.GroupBy(p => p.ShipmentType))
            {
                string profileType = carrierProfiles.Key.HasValue ? EnumHelper.GetDescription(carrierProfiles.Key) : "Global";
                telemetryData.Add($"Shipping.Profiles.{profileType.Replace(" ", string.Empty)}.Count", carrierProfiles.Count().ToString());
            }

            return telemetryData;
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public static void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent, ISqlAdapter sqlAdapter)
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

        /// <summary>
        /// Load the profiles children
        /// </summary>
        private static void LoadChildProfiles(ShippingProfileEntity profile, bool refreshIfPresent, ISqlAdapter sqlAdapter)
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
        private static void LoadPackages(ShippingProfileEntity profile, ISqlAdapter sqlAdapter)
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
        private static void LoadProfileData(EntityBase2 parent, string childProperty, Type profileType, bool refreshIfPresent, ISqlAdapter sqlAdapter)
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
        private static PropertyInfo GetChildProperty(EntityBase2 parent, string childProperty)
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
