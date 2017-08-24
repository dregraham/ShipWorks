using System;
using System.Collections.Generic;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.SparkPay.Factories;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    public class SparkPayStoreType : StoreType
    {
        private readonly StoreEntity store;
        private readonly Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory;
        private readonly SparkPayStoreEntity sparkPayStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayStoreType(
            StoreEntity store,
            Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory
            ) : base(store)
        {

            sparkPayStore = (SparkPayStoreEntity) store;

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

                TypeCode = (int) StoreTypeCode.SparkPay,
                CountryCode = "US",

                AutoDownload = false,
                AutoDownloadMinutes = 2,
                AutoDownloadOnlyAway = true,

                ComputerDownloadPolicy = "",

                ManualOrderPrefix = "",
                ManualOrderPostfix = "-M",

                DefaultEmailAccountID = -1,

                AddressValidationSetting = (int) AddressValidationStoreSettingType.ValidateAndApply,

                Token = "",
                StoreUrl = "",
                StoreName = "My SparkPay Store",
            };
        }

        /// <summary>
        /// Get a list of supported online SparkPay statuses
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices() => statusCodeProviderFactory((SparkPayStoreEntity) store).CodeNames;

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
