﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Primary controller for edition management
    /// </summary>
    public static class EditionManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EditionManager));

        // We default to having no restrictions
        static EditionRestrictionSet _restrictionSet = new EditionRestrictionSet();
        static object restrictionLock = new object();

        /// <summary>
        /// Raised when the edition has changed
        /// </summary>
        public static event EventHandler RestrictionsChanged;

        /// <summary>
        /// One-time application initialization
        /// </summary>
        public static void Initialize()
        {
            UpdateRestrictions(new List<StoreEntity>());
        }

        /// <summary>
        /// Get the edition that the app.config represents is the current edition
        /// </summary>
        public static EditionInstalledType InstalledEditionType
        {
            get
            {
                string editionValue = ConfigurationManager.AppSettings["edition"] ?? "standard";

                log.InfoFormat("Installed edition: {0}", editionValue);

                foreach (var value in EnumHelper.GetEnumList<EditionInstalledType>())
                {
                    if (value.Description == editionValue)
                    {
                        return value.Value;
                    }
                }

                return EditionInstalledType.Standard;
            }
        }

        /// <summary>
        /// Update the active edition based on the edition information found in each of the stores
        /// </summary>
        public static void UpdateRestrictions()
        {
            UpdateRestrictions(StoreManager.GetAllStores());
        }

        /// <summary>
        /// Update the current edition restriction set based on the given store list
        /// </summary>
        private static void UpdateRestrictions(IEnumerable<StoreEntity> stores)
        {
            // Convert to a List so we can iterate it more than once.
            List<StoreEntity> storeEntities = (List<StoreEntity>) (stores as IList<StoreEntity> ?? stores.ToList());

            // If no stores were passed, like from Initialization, just return.
            if (!storeEntities.Any())
            {
                return;
            }

            List<EditionRestriction> restrictions = new List<EditionRestriction>();

            foreach (StoreEntity store in storeEntities)
            {
                Edition edition = EditionSerializer.Restore(store);

                restrictions.AddRange(edition.GetRestrictions());
            }

            // Now that we have the full list of restrictions, remove any registration restrictions if needed.
            restrictions = RemoveRestrictionIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, storeEntities);

            // If there weren't any accounts for Endicia and registration was restricted, we disable the shipment type too.  So remove that restriction
            // if needed.
            restrictions = RemoveRestrictionIfNeeded(EditionFeature.ShipmentType, ShipmentTypeCode.Endicia, restrictions, storeEntities);

            // Remove any Stamps consolidator restrictions if needed
            restrictions = RemoveRestrictionIfNeeded(EditionFeature.StampsAscendiaConsolidator, null, restrictions, storeEntities);
            restrictions = RemoveRestrictionIfNeeded(EditionFeature.StampsDhlConsolidator, null, restrictions, storeEntities);
            restrictions = RemoveRestrictionIfNeeded(EditionFeature.StampsGlobegisticsConsolidator, null, restrictions, storeEntities);
            restrictions = RemoveRestrictionIfNeeded(EditionFeature.StampsIbcConsolidator, null, restrictions, storeEntities);
            restrictions = RemoveRestrictionIfNeeded(EditionFeature.StampsRrDonnelleyConsolidator, null, restrictions, storeEntities);

            UpdateDefaultShippingType();
            ActiveRestrictions = new EditionRestrictionSet(restrictions);

            // Let anyone who cares know that enabled carriers may have changed.
            Messenger.Current.Send(new EnabledCarriersChangedMessage(new object(), new List<ShipmentTypeCode>(), new List<ShipmentTypeCode>()));
        }

        /// <summary>
        /// Updates the shipping setting default shipment type.  This is to keep restricted shipment types in sync with
        /// the selected default type.  i.e. if UPS is restricted and it was the default type, the next created shipment
        /// would be created as UPS.  So instead, we'll update the default type to be None.
        /// </summary>
        private static void UpdateDefaultShippingType()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingSettings shippingSettings = lifetimeScope.Resolve<IShippingSettings>();
                IShipmentTypeManager shipmentTypeManager = lifetimeScope.Resolve<IShipmentTypeManager>();

                IShippingSettingsEntity shippingSettingsEntity = shippingSettings.FetchReadOnly();

                if (shipmentTypeManager.ShipmentTypeCodes.None(x => x == shippingSettingsEntity.DefaultShipmentTypeCode))
                {
                    shippingSettings.SetDefaultProvider(ShipmentTypeCode.None);
                }
            }
        }

        /// <summary>
        /// If the only feature restrictions are for trial stores, and no restrictions for non-trial stores exist,
        /// the user should be able to use the feature.
        ///
        /// This will return an modified list of restrictions if only trial restrictions exist.
        /// </summary>
        public static List<EditionRestriction> RemoveRestrictionIfNeeded(EditionFeature editionFeature,
            object restrictionData, List<EditionRestriction> restrictions, List<StoreEntity> stores)
        {
            // Get the feature restrictions
            List<EditionRestriction> allFeatureRestrictions = restrictions
                .Where(r => r.Feature == editionFeature).ToList();

            if (restrictionData != null)
            {
                allFeatureRestrictions = allFeatureRestrictions.Where(r => Equals(r.Data, restrictionData)).ToList();
            }

            List<StoreEntity> restrictedStores = allFeatureRestrictions.Select(r => r.Edition.Store).ToList();

            // Only check for enabled stores, as they are the only ones with up to date restrictions.
            // If there are any stores left over that don't have restrictions, then we should allow the feature, so
            // remove the restrictions from the list.
            if (stores.Where(s => s.Enabled).Except(restrictedStores).Any())
            {
                restrictions = restrictions.Except(allFeatureRestrictions).ToList();
            }

            return restrictions;
        }

        /// <summary>
        /// The current set of edition restrictions
        /// </summary>
        public static EditionRestrictionSet ActiveRestrictions
        {
            get
            {
                lock (restrictionLock)
                {
                    return _restrictionSet;
                }
            }
            private set
            {
                lock (restrictionLock)
                {
                    if (object.Equals(_restrictionSet, value))
                    {
                        return;
                    }

                    _restrictionSet = value;
                }

                if (RestrictionsChanged != null)
                {
                    RestrictionsChanged(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Update the given store to make sure it's set with the given edition data.  The store is not edited directly - any change is saved directly to the database.
        /// </summary>
        public static bool UpdateStoreEdition(StoreEntity store, IEdition edition)
        {
            // See if the edition changed
            string updatedEdition = edition.Serialize();
            if (store.Edition != updatedEdition)
            {
                StoreEntity prototype = new StoreEntity(store.StoreID) { IsNew = false };
                prototype.Edition = updatedEdition;

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(prototype);
                }

                // Get our in memory stores set up-to-date
                StoreManager.CheckForChanges();
                UpdateRestrictions();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Return true if unrestricted on exit, or false if restricted. Shows the edition upgrade window if applicable.
        /// </summary>
        public static bool HandleRestrictionIssue(IWin32Window owner, EditionRestrictionIssue issue)
        {
            if (issue.Level == EditionRestrictionLevel.None)
            {
                return true;
            }
            else if (issue.Level == EditionRestrictionLevel.Hidden)
            {
                Debug.Fail("Shouldn't be able to get to this point on a hidden GUI element.");

                return false;
            }
            else if (issue.Level == EditionRestrictionLevel.Forbidden)
            {
                MessageHelper.ShowError(owner, issue.GetDescription());

                return false;
            }
            else
            {
                Edition edition = issue.Edition;

                if (edition.PromptForUpgrade(owner, issue))
                {
                    // Update the stores and restriction set based on the now upgraded edition
                    StoreManager.CheckForChanges();
                    UpdateRestrictions();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
