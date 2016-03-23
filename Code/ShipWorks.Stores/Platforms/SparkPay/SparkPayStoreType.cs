using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    public class SparkPayStoreType : StoreType
    {
        readonly StoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store"></param>
        public SparkPayStoreType(StoreEntity store) : base(store)
        {
            this.store = store;
        }

        /// <summary>
        /// StoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SparkPay;

        protected override string InternalLicenseIdentifier
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Creates a downloader
        /// </summary>
        public override StoreDownloader CreateDownloader() => new SparkPayDownloader(store);

        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            throw new NotImplementedException();
        }

        public override StoreEntity CreateStoreInstance()
        {
            SparkPayStoreEntity store = new SparkPayStoreEntity();

            InitializeStoreDefaults(store);
            store.Token = "";
            store.StoreUrl = "";
            store.StoreName = "My SparkPay Store";

            return store;
        }
    }
}
