using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.PlatforInterfaces;
using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    public class SparkPayStoreType : StoreType
    {
        readonly StoreEntity store;
        readonly IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory;
        readonly IIndex<StoreTypeCode, IInternalLicenseIdentifierFactory> licenseIdentifierFactory;
        readonly IIndex<StoreTypeCode, IOrderIdentifierFactory> orderIdentifierFactory;
        readonly IIndex<StoreTypeCode, IStoreInstanceFactory> storeInstanceFactory;
        readonly IIndex<StoreTypeCode, Func<StoreEntity, IOnlineUpdateInstanceCommandsFactory>> onlineUpdateInstanceCommandsFactory;
        readonly Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store"></param>
        public SparkPayStoreType(
            StoreEntity store,
            IIndex<StoreTypeCode, Func<StoreEntity, StoreDownloader>> downloaderFactory,
            IIndex<StoreTypeCode, IInternalLicenseIdentifierFactory> licenseIdentifierFactory,
            IIndex<StoreTypeCode, IOrderIdentifierFactory> orderIdentifierFactory,
            IIndex<StoreTypeCode, IStoreInstanceFactory> storeInstanceFactory,
            IIndex<StoreTypeCode, Func<StoreEntity, IOnlineUpdateInstanceCommandsFactory>> onlineUpdateInstanceCommandsFactory,
            Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory
            ) : base(store)
        {
            this.storeInstanceFactory = storeInstanceFactory;
            this.orderIdentifierFactory = orderIdentifierFactory;
            this.downloaderFactory = downloaderFactory;
            this.licenseIdentifierFactory = licenseIdentifierFactory;
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
        protected override string InternalLicenseIdentifier => licenseIdentifierFactory[TypeCode].Create(store);

        /// <summary>
        /// Creates a downloader
        /// </summary>
        public override StoreDownloader CreateDownloader() => downloaderFactory[TypeCode](store);

        /// <summary>
        /// Creates the order identifier
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order) => orderIdentifierFactory[TypeCode].Create(order);

        /// <summary>
        /// Creates the store instance
        /// </summary>
        public override StoreEntity CreateStoreInstance() => storeInstanceFactory[TypeCode].CreateStoreInstance();

        /// <summary>
        /// Create menu commands for uploading shipment details
        /// </summary>
        public override List<MenuCommand> CreateOnlineUpdateInstanceCommands() => onlineUpdateInstanceCommandsFactory[TypeCode](store).Create();

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
