using Autofac.Features.Indexed;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.SparkPay.Factories;
using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    public class SparkPayStoreType : StoreType
    {
        private readonly StoreEntity store;
        private readonly IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory;
        private readonly Func<SparkPayStoreEntity, SparkPayOnlineUpdateInstanceCommandsFactory> onlineUpdateInstanceCommandsFactory;
        private readonly Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory;
        private readonly SparkPayStoreEntity sparkPayStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayStoreType(
            StoreEntity store,
            IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory,
            Func<SparkPayStoreEntity, SparkPayOnlineUpdateInstanceCommandsFactory> onlineUpdateInstanceCommandsFactory,
            Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory
            ) : base(store)
        {

            sparkPayStore = (SparkPayStoreEntity)store;

            this.downloaderFactory = downloaderFactory;
            this.onlineUpdateInstanceCommandsFactory = onlineUpdateInstanceCommandsFactory;
            this.statusCodeProviderFactory = statusCodeProviderFactory;
            this.store = store;
        }

        /// <summary>
        /// StoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SparkPay;

        /// <summary>
        /// Create the SparkPay download policy
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy => new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);

        /// <summary>
        /// Creates the license identifier
        /// </summary>
        protected override string InternalLicenseIdentifier => sparkPayStore.StoreUrl;

        /// <summary>
        /// Creates a downloader
        /// </summary>
        public override StoreDownloader CreateDownloader() => downloaderFactory[TypeCode](store);

        /// <summary>
        /// Creates the order identifier
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order) => new OrderNumberIdentifier(order.OrderNumber);

        /// <summary>
        /// Creates the store instance
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            return new SparkPayStoreEntity
            {
                Enabled = true,
                SetupComplete = false,
                Edition = "",

                TypeCode = (int)StoreTypeCode.SparkPay,
                CountryCode = "US",

                AutoDownload = false,
                AutoDownloadMinutes = 2,
                AutoDownloadOnlyAway = true,

                ComputerDownloadPolicy = "",

                ManualOrderPrefix = "",
                ManualOrderPostfix = "-M",

                DefaultEmailAccountID = -1,

                AddressValidationSetting = (int)AddressValidationStoreSettingType.ValidateAndApply,

                Token = "",
                StoreUrl = "",
                StoreName = "My SparkPay Store",
            };
        }

        /// <summary>
        /// Create menu commands for uploading shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            if (sparkPayStore == null)
            {
                throw new NullReferenceException("Non SparkPay store passed to SparkPay license identifier");
            }

            return onlineUpdateInstanceCommandsFactory(sparkPayStore).Create();
        }

        /// <summary>
        /// Get a list of supported online SparkPay statuses
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices() => statusCodeProviderFactory((SparkPayStoreEntity)store).CodeNames;

        /// <summary>
        /// Indicates if the StoreType supports the display of the given "Online" column.
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus || column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }
    }
}
