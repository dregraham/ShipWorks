using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Stores.Platforms.LemonStand.WizardPages;
using Xunit;

namespace ShipWorks.Tests.Stores.LemonStand
{
    public class LemonStandStoreTypeTest
    {
        Mock<StoreEntity> store = new Mock<StoreEntity>();
        Mock<LemonStandStoreEntity> lemonStandStore = new Mock<LemonStandStoreEntity>();
        string storeUrl = "shipworks.lemonstand.com";
        LemonStandStoreType testObject;
        
        public LemonStandStoreTypeTest()
        {
            // LemonStand StoreTypeCode value is 68
            lemonStandStore.Setup(s => s.TypeCode).Returns(68);
            lemonStandStore.Setup(s => s.StoreURL).Returns(storeUrl);
        }
        
        [Fact]
        public void Constructor_ThrowsInvalidOperationException_WhenGivenNonLemonStandStoreEntity_Test()
        {
            Assert.Throws<InvalidOperationException>(() => new LemonStandStoreType(store.Object));
        }

        [Fact]
        public void InternalLicenseIdentifier_LicenseIdentifierMatchesStoreUrl_WhenStoreIsGivenStoreUrl_Test()
        {
            testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.Equal(storeUrl, testObject.LicenseIdentifier);
        }

        [Fact]
        public void CreateOnlineUpdateInstanceCommands_OnlyReturnsOneCommand_WhenStoreIsInstantiated_Test()
        {
            testObject = new LemonStandStoreType(lemonStandStore.Object);

            List<MenuCommand> commands = testObject.CreateOnlineUpdateInstanceCommands();

            Assert.Equal(1, commands.Count);
        }

        [Fact]
        public void TypeCode_ThrowsInvalidOperationException_WhenGivenNonLemonStandTypeCode_Test()
        {
            lemonStandStore.Setup(s => s.TypeCode).Returns(69);
            Assert.Throws<InvalidOperationException>(() => new LemonStandStoreType(lemonStandStore.Object));
        }

        [Fact]
        public void CreateAccountSettingsControl_ReturnsLemonStandAccountSettingsControl_WhenCalled_Test()
        {
            testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.IsAssignableFrom<LemonStandAccountSettingsControl>(testObject.CreateAccountSettingsControl());
        }

        [Fact]
        public void CreateAddStoreWizardPages_ReturnsLemonStandAccountPage_WhenCalled_Test()
        {
            testObject = new LemonStandStoreType(lemonStandStore.Object);

            var pages = testObject.CreateAddStoreWizardPages();
            
            Assert.IsAssignableFrom<LemonStandAccountPage>(pages.First());
        }

        [Fact]
        public void CreateDownloader_ReturnsLemonStandDownloader_WhenCalled_Test()
        {
            testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.IsAssignableFrom<LemonStandDownloader>(testObject.CreateDownloader());
        }

        [Fact]
        public void CreateStoreInstance_ReturnsLemonStandStoreEntity_WhenCalled_Test()
        {
            testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.IsAssignableFrom<LemonStandStoreEntity>(testObject.CreateStoreInstance());
        }

        [Fact]
        public void CreateBasicSearchOrderConditions_ReturnsConditionGroup_WhenCalled_Test()
        {
            testObject = new LemonStandStoreType(lemonStandStore.Object);
            Assert.IsAssignableFrom<ConditionGroup>(testObject.CreateBasicSearchOrderConditions("1"));
        }
    }
}
