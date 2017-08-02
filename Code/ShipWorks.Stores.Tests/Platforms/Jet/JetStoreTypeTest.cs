using System;
using Autofac;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Jet;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetStoreTypeTest : IDisposable
    {
        private readonly AutoMock mock;

        public JetStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void TypeCode_IsJet()
        {
            JetStoreType testObject = mock.Create<JetStoreType>(new TypedParameter(typeof(StoreEntity),
                new JetStoreEntity() { StoreTypeCode = StoreTypeCode.Jet }));

            var typeCode = testObject.TypeCode;

            Assert.Equal(StoreTypeCode.Jet, typeCode);
        }

        [Fact]
        public void CreateStoreInstance_ReturnsJetStoreEntity()
        {
            JetStoreType testObject = mock.Create<JetStoreType>(new TypedParameter(typeof(StoreEntity),
                new JetStoreEntity() { StoreTypeCode = StoreTypeCode.Jet }));

            StoreEntity store = testObject.CreateStoreInstance();

            Assert.IsType<JetStoreEntity>(store);
        }

        [Fact]
        public void CreateStoreInstance_InitializesStoreValues()
        {
            JetStoreType testObject = mock.Create<JetStoreType>(new TypedParameter(typeof(StoreEntity),
                new JetStoreEntity() { StoreTypeCode = StoreTypeCode.Jet }));

            JetStoreEntity store = testObject.CreateStoreInstance() as JetStoreEntity;

            Assert.Empty(store.ApiUser);
            Assert.Empty(store.Secret);
            Assert.Equal("My Jet Store", store.StoreName);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}