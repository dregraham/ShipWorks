using System;
using Autofac.Features.Indexed;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Utility;
using log4net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Stores.Platforms.ShopSite.AccountSettings;

namespace ShipWorks.Stores.Platforms.ShopSite.AccountSettings
{
    /// <summary>
    /// Verify a connection to ShopSite
    /// </summary>
    [Component]
    public class ShopSiteConnectionVerifier : IShopSiteConnectionVerifier
    {
        readonly IIndex<ShopSiteAuthenticationType, IShopSiteWebClient> webClientFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteConnectionVerifier(IIndex<ShopSiteAuthenticationType, IShopSiteWebClient> webClientFactory,
            Func<Type, ILog> createLogger)
        {
            this.webClientFactory = webClientFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Verify the connection
        /// </summary>
        public IResult Verify(ShopSiteStoreEntity store, IShopSiteAuthenticationPersistenceStrategy persistenceStrategy)
        {
            return ConnectionVerificationNeeded(store, persistenceStrategy) ?
                TestConnection(store) :
                Result.FromSuccess();
        }

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        private static bool ConnectionVerificationNeeded(ShopSiteStoreEntity store, IShopSiteAuthenticationPersistenceStrategy strategy)
        {
            return store.Fields[(int) ShopSiteStoreFieldIndex.ApiUrl].IsChanged ||
                strategy.ConnectionVerificationNeeded(store);
        }

        /// <summary>
        /// Update the connection information
        /// </summary>
        private Result TestConnection(ShopSiteStoreEntity store)
        {
            try
            {
                IShopSiteWebClient webClient = webClientFactory[store.Authentication];
                webClient.TestConnection();

                return Result.FromSuccess();
            }
            catch (ShopSiteException ex)
            {
                log.Error(ex);
                return Result.FromError(ex);
            }
        }
    }
}
