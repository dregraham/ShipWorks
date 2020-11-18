using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Common.Logging;
using Interapptive.Shared.Utility;
using ShipWorks.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Class to configure stores downloaded from the Hub
    /// </summary>
    public class HubStoreConfigurator : IHubStoreConfigurator
    {
        private readonly IIndex<StoreTypeCode, IStoreSetup> storeSetupFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubStoreConfigurator(IIndex<StoreTypeCode, IStoreSetup> storeSetupFactory, Func<Type, ILog> logFactory)
        {
            this.storeSetupFactory = storeSetupFactory;
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
                    await storeSetupFactory[config.StoreType]?.Setup(config);
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to import configuration for {EnumHelper.GetDescription(config.StoreType)}: {ex.Message}", ex);
                }
            }
        }
    }
}
