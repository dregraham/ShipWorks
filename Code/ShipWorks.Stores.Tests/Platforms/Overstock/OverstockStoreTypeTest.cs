using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Overstock;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Overstock
{
    public class OverstockStoreTypeTest : IDisposable
    {
        private readonly AutoMock mock;

        public OverstockStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void TypeCode_IsOverstock()
        {
            OverstockStoreType testObject = mock.Create<OverstockStoreType>(new TypedParameter(typeof(StoreEntity),
                new OverstockStoreEntity() { StoreTypeCode = StoreTypeCode.Overstock }));

            var typeCode = testObject.TypeCode;

            Assert.Equal(StoreTypeCode.Overstock, typeCode);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsOverstockStoreEntity()
        {
            OverstockStoreType testObject = mock.Create<OverstockStoreType>(new TypedParameter(typeof(StoreEntity),
                new OverstockStoreEntity() { StoreTypeCode = StoreTypeCode.Overstock }));

            StoreEntity store = testObject.CreateStoreInstance();

            Assert.IsType<OverstockStoreEntity>(store);
        }

        [Fact]
        public void CreateStoreInstance_InitializesStoreValues()
        {
            OverstockStoreType testObject = mock.Create<OverstockStoreType>(new TypedParameter(typeof(StoreEntity),
                new OverstockStoreEntity() { StoreTypeCode = StoreTypeCode.Overstock }));

            OverstockStoreEntity store = testObject.CreateStoreInstance() as OverstockStoreEntity;

            Assert.Empty(store.Username);
            Assert.Empty(store.Password);
            Assert.Equal("My Overstock Store", store.StoreName);
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsOverstockOrderIdentifier()
        {
            OverstockStoreType testObject = mock.Create<OverstockStoreType>(new TypedParameter(typeof(StoreEntity),
                new OverstockStoreEntity() { StoreTypeCode = StoreTypeCode.Overstock }));

            var order = new OverstockOrderEntity();

            var orderIdentifier = testObject.CreateOrderIdentifier(order);

            Assert.IsType<AlphaNumericOrderIdentifier>(orderIdentifier);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}