using System;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.LemonStand
{
    public class LemonStandStoreTypeTest
    {
        readonly Mock<StoreEntity> store = new Mock<StoreEntity>();
        readonly Mock<LemonStandStoreEntity> lemonStandStore = new Mock<LemonStandStoreEntity>();
        readonly string storeUrl = "shipworks.lemonstand.com";
        private readonly AutoMock mock;

        public LemonStandStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            // LemonStand StoreTypeCode value is 68
            lemonStandStore.Setup(s => s.TypeCode).Returns(68);
            lemonStandStore.Setup(s => s.StoreURL).Returns(storeUrl);
        }

        [Fact]
        public void Constructor_ThrowsInvalidOperationException_WhenGivenNonLemonStandStoreEntity()
        {
            Assert.Throws<InvalidOperationException>(() => new LemonStandStoreType(store.Object));
        }

        [Fact]
        public void InternalLicenseIdentifier_LicenseIdentifierMatchesStoreUrl_WhenStoreIsGivenStoreUrl()
        {
            var testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.Equal(storeUrl, testObject.LicenseIdentifier);
        }

        [Fact]
        public void TypeCode_ThrowsInvalidOperationException_WhenGivenNonLemonStandTypeCode()
        {
            lemonStandStore.Setup(s => s.TypeCode).Returns(69);
            Assert.Throws<InvalidOperationException>(() => new LemonStandStoreType(lemonStandStore.Object));
        }

        [Fact]
        public void CreateStoreInstance_ReturnsLemonStandStoreEntity_WhenCalled()
        {
            var testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.IsAssignableFrom<LemonStandStoreEntity>(testObject.CreateStoreInstance());
        }

        [Fact]
        public void CreateBasicSearchOrderConditions_ReturnsConditionGroup_WhenCalled()
        {
            var testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.IsAssignableFrom<ConditionGroup>(testObject.CreateBasicSearchOrderConditions("1"));
        }

        [Fact]
        public void GetAuditDescription_ReturnsValueWhenOrderIsCorrectType()
        {
            var testObject = mock.Create<LemonStandStoreType>(TypedParameter.From<StoreEntity>(null));
            var identifier = testObject.GetAuditDescription(new LemonStandOrderEntity { LemonStandOrderID = "ABC-123" });
            Assert.Equal("ABC-123", identifier);
        }

        [Fact]
        public void GetAuditDescription_ReturnsEmptyWhenOrderIsNull()
        {
            var testObject = mock.Create<LemonStandStoreType>(TypedParameter.From<StoreEntity>(null));
            var identifier = testObject.GetAuditDescription(null);
            Assert.Empty(identifier);
        }

        [Fact]
        public void GetAuditDescription_ReturnsEmptyWhenOrderIsNotCorrectType()
        {
            var testObject = mock.Create<LemonStandStoreType>(TypedParameter.From<StoreEntity>(null));
            var identifier = testObject.GetAuditDescription(new OrderEntity());
            Assert.Empty(identifier);
        }
    }
}
