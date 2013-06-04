﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;
using System.ComponentModel;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Manages shipment profiles
    /// </summary>
    public static class ShippingProfileManager
    {
        static TableSynchronizer<ShippingProfileEntity> synchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            synchronizer = new TableSynchronizer<ShippingProfileEntity>();
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
                        ShipmentType shipmentType = ShipmentTypeManager.GetType((ShipmentTypeCode) profile.ShipmentType);
                        shipmentType.LoadProfileData(profile, true);
                    }
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
        /// Get the profile with the specified ID, or null if it does not exist
        /// </summary>
        public static ShippingProfileEntity GetProfile(long profileID)
        {
            return Profiles.SingleOrDefault(p => p.ShippingProfileID == profileID);
        }

        /// <summary>
        /// Apply the given profile to the given shipment
        /// </summary>
        public static void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            if (shipment.Processed)
            {
                throw new InvalidOperationException("Cannot apply profile to a processed shipment.");
            }

            if (profile.ShipmentType == shipment.ShipmentType)
            {
                ShipmentTypeManager.GetType(shipment).ApplyProfile(shipment, profile);
            }
        }

        /// <summary>
        /// Save the given profile to the database
        /// </summary>
        public static void SaveProfile(ShippingProfileEntity profile)
        {
            // Get the shipment type of the profile
            ShipmentType shipmentType = ShipmentTypeManager.GetType((ShipmentTypeCode) profile.ShipmentType);
            
            bool rootDirty = profile.IsDirty;
            bool anyDirty = new ObjectGraphUtils().ProduceTopologyOrderedList(profile).Any(e => e.IsDirty);

            // Transaction
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                bool extraDirty = shipmentType.SaveProfileData(profile, adapter);

                // Force the profile change if any derived stuff changes
                if ((anyDirty || extraDirty) && !rootDirty)
                {
                    profile.Fields[(int) ShippingProfileFieldIndex.ShipmentType].IsChanged = true;
                    profile.Fields.IsDirty = true;
                }

                // Save the base profile
                adapter.SaveAndRefetch(profile);

                adapter.Commit();
            }

            CheckForChangesNeeded();
        }
    }
}
