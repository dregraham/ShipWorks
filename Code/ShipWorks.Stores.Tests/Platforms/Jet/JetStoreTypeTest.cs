using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Tests.Shared;
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

        [Fact]
        public void CreateOrderIdentifier_ReturnsJetOrderIdentifier()
        {
            JetStoreType testObject = mock.Create<JetStoreType>(new TypedParameter(typeof(StoreEntity),
                new JetStoreEntity() { StoreTypeCode = StoreTypeCode.Jet }));

            var order = new JetOrderEntity();

            var orderIdentifier = testObject.CreateOrderIdentifier(order);

            Assert.IsType<JetOrderIdentifier>(orderIdentifier);
        }

        [Fact]
        public void GetAuditDescription_ReturnsValueWhenOrderIsCorrectType()
        {
            var testObject = mock.Create<JetStoreType>(TypedParameter.From<StoreEntity>(null));
            var identifier = testObject.GetAuditDescription(new JetOrderEntity { MerchantOrderId = "ABC-123" });
            Assert.Equal("ABC-123", identifier);
        }

        [Fact]
        public void GetAuditDescription_ReturnsEmptyWhenOrderIsNull()
        {
            var testObject = mock.Create<JetStoreType>(TypedParameter.From<StoreEntity>(null));
            var identifier = testObject.GetAuditDescription(null);
            Assert.Empty(identifier);
        }

        [Fact]
        public void GetAuditDescription_ReturnsEmptyWhenOrderIsNotCorrectType()
        {
            var testObject = mock.Create<JetStoreType>(TypedParameter.From<StoreEntity>(null));
            var identifier = testObject.GetAuditDescription(new OrderEntity());
            Assert.Empty(identifier);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}