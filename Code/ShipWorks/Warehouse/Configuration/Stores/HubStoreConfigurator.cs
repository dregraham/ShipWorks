using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
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
        private readonly IActionManager actionManager;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubStoreConfigurator(IIndex<StoreTypeCode, IStoreSetup> storeSetupFactory,
            IStoreManager storeManager,
            IStoreTypeManager storeTypeManager,
            IActionManager actionManager,
            Func<Type, ILog> logFactory)
        {
            this.storeSetupFactory = storeSetupFactory;
            this.storeManager = storeManager;
            this.storeTypeManager = storeTypeManager;
            this.actionManager = actionManager;
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
                    //Convert the config store to a store entity
                    var storeType = storeTypeManager.GetType(config.StoreType).CreateStoreInstance().GetType();
                    var deserializedStore = storeSetupFactory[config.StoreType].Setup(config, storeType);

                    if (storeManager.GetAllStores().Any(x => storeTypeManager.GetType(x).LicenseIdentifier == config.UniqueIdentifier))
                    {
                        UpdateStore(deserializedStore);
                    }
                    else
                    {
                        var storeID = ConfigureNewStore(deserializedStore);
                        ConfigureDefaultAction(storeID, config.ActionsPayload);
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to import configuration for {EnumHelper.GetDescription(config.StoreType)}: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Updates an existing store and saves it to the DB
        /// </summary>
        private void UpdateStore(StoreEntity deserializedStore)
        {
            deserializedStore.IsNew = false;
            storeManager.SaveStore(deserializedStore);
        }

        /// <summary>
        /// Configures a new store and saves it to the DB
        /// </summary>
        private long ConfigureNewStore(StoreEntity store)
        {
            // Fill in non-null values for anything the store is not yet configured for
            store.InitializeNullsToDefault();
            store.StartSetup();

            // Mark all fields as changed so they will be saved
            store.IsNew = true;
            store.IsDirty = true;
            store.Fields.ForEach(x => x.IsChanged = true);

            storeManager.SaveStore(store);

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

            return store.StoreID;
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

        /// <summary>
        /// Create the default store action
        /// </summary>
        private void ConfigureDefaultAction(long storeID, string actionPayload)
        {
            var actionConfiguration = JsonConvert.DeserializeObject<ActionConfiguration>(actionPayload);
            var action = JsonConvert.DeserializeObject<ActionEntity>(actionConfiguration.SerializedAction);
            var tasks = new List<ActionTask>();

            foreach (var serializedTask in actionConfiguration.SerializedTasks)
            {
                var task = JsonConvert.DeserializeObject<ActionTask>(serializedTask);

                // Mark all fields as changed so they will be saved
                task.Entity.IsNew = true;
                task.Entity.IsDirty = true;
                task.Entity.Fields.ForEach(x => x.IsChanged = true);
                tasks.Add(task);
            }

            action.IsNew = true;
            action.IsDirty = true;
            action.Fields.ForEach(x => x.IsChanged = true);

            action.StoreLimitedList = new long[] { storeID };

            using (var adapter = new SqlAdapter(true))
            {
                actionManager.SaveAction(action, adapter);

                foreach (var task in tasks)
                {
                    task.Save(action, adapter);
                }

                adapter.Commit();
            }
        }
    }
}
