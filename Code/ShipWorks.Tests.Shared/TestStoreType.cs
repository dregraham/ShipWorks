using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Tests.Shared
{
    public class TestStoreType : StoreType
    {
        public TestStoreType() : base(new StoreEntity(1))
        {
        }

        public TestStoreType(StoreEntity store) : base(store)
        {
        }

        public override StoreTypeCode TypeCode { get; }

        public override StoreEntity CreateStoreInstance()
        {
            return null;
        }

        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            return null;
        }

        public override StoreDownloader CreateDownloader()
        {
            return null;
        }

        protected override string InternalLicenseIdentifier { get; }
    }
}
