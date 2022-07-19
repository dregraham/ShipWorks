using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse.Configuration.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// Given a config, creates a store.
    /// Platform stores originate outside of the ShipWorks client, likely from Platform/Hub/Api.
    /// </summary>
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.Api)]
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.BrightpearlHub)]
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.WalmartHub)]
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.ChannelAdvisorHub)]
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.VolusionHub)]
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.GrouponHub)]
    public class PlatformStoreSetup : BaseStoreSetup
    {
        private readonly IStoreTypeManager storeTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformStoreSetup(IStoreTypeManager storeTypeManager)
        {
            this.storeTypeManager = storeTypeManager;
        }

        /// <summary>
        /// Setup the Platform Store
        /// </summary>
        public override StoreEntity Setup(StoreConfiguration config, Type storeType, StoreEntity existingStore)
        {
            // When a user first adds a store, there will be no sync payload. When SW syncs, there will be a payload
            // and we want to sync like any other store.
            if (!string.IsNullOrWhiteSpace(config.SyncPayload))
            {
                return base.Setup(config, storeType, existingStore);
            }

            var store = CreatePlatformStore(config.StoreType);

            var isNewStore = existingStore == null;
            if (!isNewStore)
            {
                store.StoreID = existingStore.StoreID;
            }

            ApplyStoreConfig(store, config);

            return store;
        }


        /// <summary>
        /// Creates a PlatformStoreEntity given a storeType
        /// </summary>
        public StoreEntity CreatePlatformStore(StoreTypeCode storeTypeCode)
        {
            var storeType = storeTypeManager.GetType(storeTypeCode);
            var store = storeType.CreateStoreInstance();
            return store;
        }


        /// <summary>
        /// Populates a PlatformStoreEntity with data from a StoreConfiguration
        /// </summary>
        public void ApplyStoreConfig(StoreEntity store, StoreConfiguration config)
        {
            store.SetupComplete = true;

            store.StoreName = config.Name;
            store.WarehouseStoreID = Guid.Parse(config.Id);
            store.OrderSourceID = config.UniqueIdentifier;
            store.ManagedInHub = config.ManagedInHub;

            if (config.Settings != null)
            {
                store.ManualOrderPrefix = config.Settings.ManualOrdersPrefix;
                store.ManualOrderPostfix = config.Settings.ManualOrdersPostfix;
                store.DomesticAddressValidationSetting = EnumHelper.GetEnumByApiValue<AddressValidationStoreSettingType>(config.Settings.AddressValidationDomestic);
                store.InternationalAddressValidationSetting = EnumHelper.GetEnumByApiValue<AddressValidationStoreSettingType>(config.Settings.AddressValidationInternational);
                store.Enabled = config.Settings.ActivelyShips;
            }

            if (config.Address != null)
            {
                store.Address.City = config.Address.City;
                store.Address.Company = config.Address.Company;
                store.Address.Phone = config.Address.Phone;
                store.Address.Email = config.Address.Email;
                store.Address.StateProvCode = config.Address.StateProvCode;
                store.Address.PostalCode = config.Address.PostalCode;
                store.Address.CountryCode = config.Address.CountryCode;
                store.Address.Street1 = config.Address.Street1;
                store.Address.Street2 = config.Address.Street2;
                store.Address.Website = config.Address.Website;
            }
        }
    }
}
