using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Stores.Platforms.LemonStand.WizardPages;

namespace ShipWorks.Tests.Stores.LemonStand
{
    [TestClass]
    public class LemonStandStoreTypeTest
    {
        Mock<LemonStandStoreEntity> store = new Mock<LemonStandStoreEntity>();
        string storeUrl = "shipworks.lemonstand.com";
        LemonStandStoreType testObject;

        [TestInitialize]
        public void Initialize()
        {
            // LemonStand StoreTypeCode value is 68
            store.Setup(s => s.TypeCode).Returns(68);
            store.Setup(s => s.StoreURL).Returns(storeUrl);
        }

        [TestMethod]
        public void InternalLicenseIdentifier_LicenseIdentifierMatchesStoreUrl_WhenStoreIsGivenStoreUrl_Test()
        {
            testObject = new LemonStandStoreType(store.Object);
            Assert.AreEqual(storeUrl, testObject.LicenseIdentifier);
        }

        [TestMethod]
        public void CreateOnlineUpdateInstanceCommands_OnlyReturnsOneCommand_WhenStoreIsInstantiated_Test()
        {
            testObject = new LemonStandStoreType(store.Object);

            List<MenuCommand> commands = testObject.CreateOnlineUpdateInstanceCommands();

            Assert.AreEqual(1, commands.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TypeCode_ThrowsInvalidOperationException_WhenGivenNonLemonStandTypeCode_Test()
        {
            store.Setup(s => s.TypeCode).Returns(69);
            new LemonStandStoreType(store.Object);
        }

        [TestMethod]
        public void CreateAccountSettingsControl_ReturnsLemonStandAccountSettingsControl_WhenCalled_Test()
        {
            testObject = new LemonStandStoreType(store.Object);
            Assert.IsInstanceOfType(testObject.CreateAccountSettingsControl(), typeof(LemonStandAccountSettingsControl));
        }

        [TestMethod]
        public void CreateAddStoreWizardPages_ReturnsLemonStandAccountPage_WhenCalled_Test()
        {
            testObject = new LemonStandStoreType(store.Object);

            var pages = testObject.CreateAddStoreWizardPages();
            
            Assert.IsInstanceOfType(pages.First(), typeof(LemonStandAccountPage));
        }

        [TestMethod]
        public void CreateDownloader_ReturnsLemonStandDownloader_WhenCalled_Test()
        {
            testObject = new LemonStandStoreType(store.Object);
            Assert.IsInstanceOfType(testObject.CreateDownloader(), typeof(LemonStandDownloader));
        }

        [TestMethod]
        public void CreateStoreInstance_ReturnsLemonStandStoreEntity_WhenCalled_Test()
        {
            testObject = new LemonStandStoreType(store.Object);
            Assert.IsInstanceOfType(testObject.CreateStoreInstance(), typeof(LemonStandStoreEntity));
        }

        [TestMethod]
        public void CreateBasicSearchOrderConditions_ReturnsConditionGroup_WhenCalled_Test()
        {
            testObject = new LemonStandStoreType(store.Object);
            Assert.IsInstanceOfType(testObject.CreateBasicSearchOrderConditions("1"), typeof(ConditionGroup));
        }
    }
}
