using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores;
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

        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            return null;
        }

        protected override string InternalLicenseIdentifier { get; }
    }
}
