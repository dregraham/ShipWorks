﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Class to configure stores downloaded from the Hub
    /// </summary>
    [Component]
    public class HubStoreConfigurator : IHubStoreConfigurator
    {
        private readonly IIndex<StoreTypeCode, IStoreSetup> storeSetupFactory;
        private readonly IStoreManager storeManager;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubStoreConfigurator(IIndex<StoreTypeCode, IStoreSetup> storeSetupFactory,
            IStoreManager storeManager,
            IStoreTypeManager storeTypeManager,
            Func<Type, ILog> logFactory)
        {
            this.storeSetupFactory = storeSetupFactory;
            this.storeManager = storeManager;
            this.storeTypeManager = storeTypeManager;
            log = logFactory(typeof(HubStoreConfigurator));
        }

        /// <summary>
        /// Configure stores
        /// </summary>
        public async Task Configure(IEnumerable<StoreConfiguration> storeConfigurations)
        {
            foreach (var config in storeConfigurations)
            {
                try
                {
                    var storeType = storeTypeManager.GetType(config.StoreType);
                    var type = storeType.StoreReadOnly.GetType();

                    var store = storeSetupFactory[config.StoreType].Setup<StoreEntity>(config);

                    // Fill in non-null values for anything the store is not yet configured for
                    store.InitializeNullsToDefault();
                    store.StartSetup();

                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        // Create the default presets
                        CreateDefaultStatusPreset(store, StatusPresetTarget.Order, adapter);
                        CreateDefaultStatusPreset(store, StatusPresetTarget.OrderItem, adapter);

                        StoreFilterRepository storeFilterRepository = new StoreFilterRepository(store);
                        storeFilterRepository.Save(true);

                        // Mark that this store is now ready
                        store.CompleteSetup();
                        storeManager.SaveStore(store, adapter);

                        CreateOrigin(store, adapter);

                        adapter.Commit();
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to import configuration for {EnumHelper.GetDescription(config.StoreType)}: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Create a default status preset for the store
        /// </summary>
        private void CreateDefaultStatusPreset(StoreEntity store, StatusPresetTarget presetTarget, SqlAdapter adapter)
        {
            StatusPresetEntity preset = new StatusPresetEntity();
            preset.StoreID = store.StoreID;
            preset.StatusTarget = (int) presetTarget;
            preset.StatusText = "";
            preset.IsDefault = true;

            adapter.SaveEntity(preset);
        }

        /// <summary>
        /// Create an origin address for the store
        /// </summary>
        private void CreateOrigin(StoreEntity store, SqlAdapter adapter)
        {
            var origin = new ShippingOriginEntity();
            origin.InitializeNullsToDefault();
            origin.Description = store.StoreName;

            new PersonAdapter(store, string.Empty).CopyTo(origin, string.Empty);

            var value = store.StoreName;

            PersonName name = PersonName.Parse(value);

            int maxFirst = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonFirst);
            if (name.First.Length > maxFirst)
            {
                name.Middle = name.First.Substring(maxFirst) + name.Middle;
                name.First = name.First.Substring(0, maxFirst);
            }

            int maxMiddle = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonMiddle);
            if (name.Middle.Length > maxMiddle)
            {
                name.Last = name.Middle.Substring(maxMiddle) + name.Last;
                name.Middle = name.Middle.Substring(0, maxMiddle);
            }

            int maxLast = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonLast);
            if (name.Last.Length > maxLast)
            {
                name.Last = name.Last.Substring(0, maxLast);
            }

            origin.FirstName = name.First;
            origin.MiddleName = name.Middle;
            origin.LastName = name.Last;

            adapter.SaveEntity(origin);
        }
    }
}
