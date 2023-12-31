﻿using System;
using Interapptive.Shared.ComponentRegistration;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Serialization;
using ShipWorks.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Base class for setting up a store from a getConfig call
    /// </summary>
    [KeyAllComponent(typeof(IStoreSetup), typeof(StoreTypeCode), new object[] { StoreTypeCode.Manual, StoreTypeCode.Odbc, StoreTypeCode.GenericFile, StoreTypeCode.ThreeDCart, StoreTypeCode.Api, StoreTypeCode.BrightpearlHub, StoreTypeCode.WalmartHub, StoreTypeCode.ChannelAdvisorHub, StoreTypeCode.VolusionHub, StoreTypeCode.GrouponHub })]
    public class BaseStoreSetup : IStoreSetup
    {
        /// <summary>
        /// Setup the specified store type
        /// </summary>
        public virtual StoreEntity Setup(StoreConfiguration config, Type storeType, StoreEntity existingStore)
        {
            var deserializedStore = (StoreEntity) JsonConvert.DeserializeObject(config.SyncPayload, storeType, new EntityJsonSerializerSettings());

            if (existingStore != null)
            {
                deserializedStore.StoreID = existingStore.StoreID;
            }

            return deserializedStore;
        }
    }
}
