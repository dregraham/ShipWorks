using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartStoreTypeTest
    {
        [Fact]
        public void CreateOrderIdentifier_ReturnsOrderNumberIdentifier()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                WalmartStoreEntity store = new WalmartStoreEntity();
                store.TypeCode = (int) StoreTypeCode.Walmart;

                WalmartStoreType testObject = mock.Create<WalmartStoreType>(new TypedParameter(typeof(StoreEntity), store));
                OrderEntity order = new OrderEntity() {OrderNumber = 7};
                OrderNumberIdentifier identifier = new OrderNumberIdentifier(7);

                Assert.Equal(identifier.ToString(), testObject.CreateOrderIdentifier(order).ToString());
            }
        }
    }
}