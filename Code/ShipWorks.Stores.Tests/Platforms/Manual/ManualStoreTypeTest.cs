using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Manual;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
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
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
