using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.LemonStand;

namespace ShipWorks.Tests.Stores.LemonStand
{
    [TestClass]
    public class LemonStandStoreTypeTest
    {
        Mock<LemonStandStoreEntity> store = new Mock<LemonStandStoreEntity>();
        string storeUrl = "shipworks.lemonstand.com";
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
            LemonStandStoreType testObject = new LemonStandStoreType(store.Object);
            Assert.AreEqual(storeUrl, testObject.LicenseIdentifier);
        }

        [TestMethod]
        public void CreateOnlineUpdateInstanceCommands_IsCalledOnce_WhenStoreIsInstantiated_Test()
        {
            LemonStandStoreType testObject = new LemonStandStoreType(store.Object);
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TypeCode_ThrowsInvalidOperationException_WhenGivenNonLemonStandTypeCode_Test()
        {
            store.Setup(s => s.TypeCode).Returns(69);
            new LemonStandStoreType(store.Object);
        }

        [TestMethod]
        public void CreateAccountSettingsControl()
        {
            LemonStandStoreType testObject = new LemonStandStoreType(store.Object);

        }

        [TestMethod]
        public void CreateAddStoreWizardPages()
        {
            LemonStandStoreType testObject = new LemonStandStoreType(store.Object);

        }

        [TestMethod]
        public void CreateDownloader()
        {
            LemonStandStoreType testObject = new LemonStandStoreType(store.Object);

        }

        [TestMethod]
        public void CreateStroreInstance()
        {
            LemonStandStoreType testObject = new LemonStandStoreType(store.Object);

        }

        [TestMethod]
        public void GenerateTemplateOrderElements()
        {
            LemonStandStoreType testObject = new LemonStandStoreType(store.Object);

        }

        [TestMethod]
        public void CreateBasicSearchOrderConditions()
        {
            LemonStandStoreType testObject = new LemonStandStoreType(store.Object);

        }
    }
}
