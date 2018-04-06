﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private static IShippingProfileManager shippingProfileManager;

        /// <summary>
        /// Initialize ShippingProfileManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<ShippingProfileEntity>();
            shippingProfileManager = IoC.UnsafeGlobalLifetimeScope.Resolve<IShippingProfileManager>();
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

                    foreach (ShippingProfileEntity profile in modified.Concat(added))
                    {
                        shippingProfileManager.LoadProfileData(profile, true);
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
    }
}
