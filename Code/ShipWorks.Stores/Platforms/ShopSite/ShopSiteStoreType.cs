using System;
using Autofac.Features.Indexed;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.ShopSite, ExternallyOwned = true)]
    public class ShopSiteStoreType : StoreType
    {
        private readonly IIndex<StoreTypeCode, AccountSettingsControlBase> accountSettingsControlIndex;
        private readonly Func<IShopSiteStoreEntity, ShopSiteDownloader> createDownloader;
        private readonly IShopSiteIdentifier identifier;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteStoreType(StoreEntity store, 
            IIndex<StoreTypeCode, AccountSettingsControlBase> accountSettingsControlIndex,
            Func<IShopSiteStoreEntity, ShopSiteDownloader> createDownloader,
            IShopSiteIdentifier identifier)
            : base(store)
        {
            if (store != null && !(store is ShopSiteStoreEntity))
            {
                throw new ArgumentException("StoreEntity is not instance of ShopSiteStoreEntity.");
            }

            this.accountSettingsControlIndex = accountSettingsControlIndex;
            this.createDownloader = createDownloader;
            this.identifier = identifier;
        }

        /// <summary>
        /// Creates a new instance of an ShopSite store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            ShopSiteStoreEntity shopSiteStore = new ShopSiteStoreEntity();

            InitializeStoreDefaults(shopSiteStore);

            shopSiteStore.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;
            shopSiteStore.ApiUrl = "";
            shopSiteStore.RequireSSL = true;
            shopSiteStore.RequestTimeout = 60;
            shopSiteStore.DownloadPageSize = 50;
            shopSiteStore.OauthAuthorizationCode = string.Empty;
            shopSiteStore.Identifier = string.Empty;
            shopSiteStore.OauthClientID = string.Empty;
            shopSiteStore.OauthSecretKey = string.Empty;

            return shopSiteStore;
        }

        /// <summary>
        /// Get the identifier object that is used to uniquely identify the specified order for the store.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Create the fields required to uniquely identify the online customer
        /// </summary>
        public override IEntityField2[] CreateCustomerIdentifierFields(out bool instanceLookup)
        {
            instanceLookup = true;

            return new IEntityField2[] { OrderFields.OnlineCustomerID };
        }

        /// <summary>
        /// Create the downloader instance that is used to retrieve data from the store.
        /// </summary>
        public override StoreDownloader CreateDownloader() => createDownloader(Store as IShopSiteStoreEntity);

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl() =>
            accountSettingsControlIndex[StoreTypeCode.ShopSite];

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.ShopSite;

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        protected override string InternalLicenseIdentifier => identifier.Get((ShopSiteStoreEntity) Store);

        /// <summary>
        /// Initial download policy of the store
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy => new InitialDownloadPolicy(InitialDownloadRestrictionType.OrderNumber);
    }
}
