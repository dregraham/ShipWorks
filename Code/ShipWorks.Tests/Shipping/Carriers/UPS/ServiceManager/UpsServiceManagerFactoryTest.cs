using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.UPS;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.ServiceManager
{
    [TestClass]
    public class UpsServiceManagerFactoryTest
    {
        private UpsServiceManagerFactory testObject;

        private Mock<IUpsServiceManager> canadaServiceManager;
        private Mock<IUpsServiceManager> unitedStatesServiceManager;
        private Mock<IUpsServiceManager> puertoRicoServiceManager;

        [TestInitialize]
        public void Initialize()
        {
            canadaServiceManager = new Mock<IUpsServiceManager>();
            canadaServiceManager.Setup(m => m.CountryCode).Returns("CA");

            unitedStatesServiceManager = new Mock<IUpsServiceManager>();
            unitedStatesServiceManager.Setup(m => m.CountryCode).Returns("US");

            puertoRicoServiceManager = new Mock<IUpsServiceManager>();
            puertoRicoServiceManager.Setup(m => m.CountryCode).Returns("PR");
            

            testObject = new UpsServiceManagerFactory(new List<IUpsServiceManager> { canadaServiceManager.Object, unitedStatesServiceManager.Object, puertoRicoServiceManager.Object });
        }

        [TestMethod]
        public void Create_UsesServiceManagerCountryCode_ToDetermineServiceManagerToUse_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity {OriginCountryCode = "CA"};
            testObject.Create(shipment);

            canadaServiceManager.VerifyGet(m => m.CountryCode, Times.Once());
        }

        [TestMethod]
        public void Create_ReturnsCanadaServiceManager_WhenOriginCountryCodeIsCA_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { OriginCountryCode = "CA" };
            IUpsServiceManager serviceManager = testObject.Create(shipment);

            Assert.AreEqual("CA", serviceManager.CountryCode);
            Assert.AreEqual(canadaServiceManager.Object, serviceManager);
        }

        [TestMethod]
        public void Create_ReturnsPRServiceManager_WhenOriginCountryCodeIsPR_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { OriginCountryCode = "PR" };
            IUpsServiceManager serviceManager = testObject.Create(shipment);

            Assert.AreEqual("PR", serviceManager.CountryCode);
            Assert.AreEqual(puertoRicoServiceManager.Object, serviceManager);
        }


        [TestMethod]
        public void Create_ReturnsUnitedStatesServiceManager_WhenOriginCountryCodeIsUS_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { OriginCountryCode = "US" };
            IUpsServiceManager serviceManager = testObject.Create(shipment);

            Assert.AreEqual("US", serviceManager.CountryCode);
            Assert.AreEqual(unitedStatesServiceManager.Object, serviceManager);
        }

        [TestMethod]
        public void Create_ReturnsUnitedStatesServiceManager_WhenOriginCountryCodeIsNotUS_AndNotCA_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { OriginCountryCode = "GB" };
            IUpsServiceManager serviceManager = testObject.Create(shipment);

            Assert.AreEqual("US", serviceManager.CountryCode);
            Assert.AreEqual(unitedStatesServiceManager.Object, serviceManager);
        }
    }
}
