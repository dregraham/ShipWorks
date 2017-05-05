using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.UI;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.ShopSite, ExternallyOwned = true)]
    public class ShopSiteStoreType : StoreType
    {
        private readonly IIndex<StoreTypeCode, AccountSettingsControlBase> accountSettingsControlIndex;
        private readonly IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteStoreType(StoreEntity store, 
            IIndex<StoreTypeCode, AccountSettingsControlBase> accountSettingsControlIndex,
            IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory)
            : base(store)
        {
            if (store != null && !(store is ShopSiteStoreEntity))
            {
                throw new ArgumentException("StoreEntity is not instance of ShopSiteStoreEntity.");
            }

            this.accountSettingsControlIndex = accountSettingsControlIndex;
            this.downloaderFactory = downloaderFactory;
        }

        /// <summary>
        /// Creates a new instance of an ShopSite store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            ShopSiteStoreEntity shopSiteStore = new ShopSiteStoreEntity();

            InitializeStoreDefaults(shopSiteStore);

            shopSiteStore.Authentication = ShopSiteAuthenticationType.Oauth;
            shopSiteStore.ApiUrl = "";
            shopSiteStore.RequireSSL = true;
            shopSiteStore.RequestTimeout = 60;
            shopSiteStore.DownloadPageSize = 50;

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
        public override StoreDownloader CreateDownloader() => downloaderFactory[TypeCode](Store);

        /// <summary>
        /// Create the control that is used for editing the account settings in the Store Settings window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl() =>
            accountSettingsControlIndex[StoreTypeCode.ShopSite];

        /// <summary>
        /// StoreType enum value
        /// </summary>
        override public StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.ShopSite;
            }
        }

        /// <summary>
        /// This is a string that uniquely identifies the store.
        /// </summary>
        override protected string InternalLicenseIdentifier
        {
            get
            {
                string identifier = ((ShopSiteStoreEntity) Store).ApiUrl.ToLowerInvariant();

                // Remove the db_xml.cgi part
                identifier = identifier.Replace("db_xml.cgi", "");

                return identifier;
            }
        }

        /// <summary>
        /// Initial download policy of the store
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.OrderNumber);
            }
        }
    }
}
