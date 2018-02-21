using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Manual;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Manual
{
    public class ManualStoreTypeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ManualStoreType testObject;

        public ManualStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<ManualStoreType>(new TypedParameter(typeof(StoreEntity),
                new StoreEntity() { StoreTypeCode = StoreTypeCode.Manual }));
        }

        [Fact]
        public void TypeCode_IsManual()
        {
           var typeCode = testObject.TypeCode;
 
           Assert.Equal(StoreTypeCode.Manual, typeCode);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsStoreEntity()
        {
            StoreEntity store = testObject.CreateStoreInstance();

            Assert.IsType<StoreEntity>(store);
        }

        [Fact]
        public void CreateStoreInstance_InitializesStoreValues()
        {
            StoreEntity store = testObject.CreateStoreInstance();

            Assert.Equal("My Manual Store", store.StoreName);
            Assert.Equal(false, store.AutoDownload);
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnOrderIdentifier()
        {
            OrderEntity order = new OrderEntity();

            OrderIdentifier orderIdentifier = testObject.CreateOrderIdentifier(order);

            Assert.IsType<AlphaNumericOrderIdentifier>(orderIdentifier);
        }

        [Theory]
        [InlineData("", "123", "", "123")]
        [InlineData("abc", "123", "", "abc123")]
        [InlineData("", "123", "abc", "123abc")]
        [InlineData("abc", "123", "abc", "abc123abc")]
        public void CreateOrder_VerifyOrderNumber(string prefix, string orderNumber, 
            string postfix, string orderNumberComplete)
        {
            OrderEntity order = new OrderEntity();

            order.ChangeOrderNumber(orderNumber, prefix, postfix);

            Assert.Equal(orderNumberComplete, order.OrderNumberComplete);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
