using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    class SparkPayStoreType : StoreType
    {
        public SparkPayStoreType(StoreEntity store) : base(store)
        {

        }

        public override StoreTypeCode TypeCode => StoreTypeCode.AmeriCommerce;

        protected override string InternalLicenseIdentifier
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override StoreDownloader CreateDownloader()
        {
            throw new NotImplementedException();
        }

        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            throw new NotImplementedException();
        }

        public override StoreEntity CreateStoreInstance()
        {
            throw new NotImplementedException();
        }
    }
}
