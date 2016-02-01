using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Platforms.LemonStand;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.LemonStand
{
    public class LemonStandStoreTypeTest
    {
        readonly Mock<StoreEntity> store = new Mock<StoreEntity>();
        readonly Mock<LemonStandStoreEntity> lemonStandStore = new Mock<LemonStandStoreEntity>();
        string storeUrl = "shipworks.lemonstand.com";
        LemonStandStoreType testObject;
        
        public LemonStandStoreTypeTest()
        {
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
            testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.Equal(storeUrl, testObject.LicenseIdentifier);
        }

        [Fact]
        public void TypeCode_ThrowsInvalidOperationException_WhenGivenNonLemonStandTypeCode()
        {
            lemonStandStore.Setup(s => s.TypeCode).Returns(69);
            Assert.Throws<InvalidOperationException>(() => new LemonStandStoreType(lemonStandStore.Object));
        }

        [Fact]
        public void CreateDownloader_ReturnsLemonStandDownloader_WhenCalled()
        {
            testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.IsAssignableFrom<LemonStandDownloader>(testObject.CreateDownloader());
        }

        [Fact]
        public void CreateStoreInstance_ReturnsLemonStandStoreEntity_WhenCalled()
        {
            testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.IsAssignableFrom<LemonStandStoreEntity>(testObject.CreateStoreInstance());
        }

        [Fact]
        public void CreateBasicSearchOrderConditions_ReturnsConditionGroup_WhenCalled()
        {
            testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.IsAssignableFrom<ConditionGroup>(testObject.CreateBasicSearchOrderConditions("1"));
        }
    }
}
