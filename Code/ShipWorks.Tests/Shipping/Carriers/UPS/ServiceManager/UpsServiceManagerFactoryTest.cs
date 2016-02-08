using System.Collections.Generic;
using Xunit;
using ShipWorks.Shipping.Carriers.UPS;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.ServiceManager
{
    public class UpsServiceManagerFactoryTest
    {
        private UpsServiceManagerFactory testObject;

        private Mock<IUpsServiceManager> canadaServiceManager;
        private Mock<IUpsServiceManager> unitedStatesServiceManager;
        private Mock<IUpsServiceManager> puertoRicoServiceManager;

        public UpsServiceManagerFactoryTest()
        {
            canadaServiceManager = new Mock<IUpsServiceManager>();
            canadaServiceManager.Setup(m => m.CountryCode).Returns("CA");

            unitedStatesServiceManager = new Mock<IUpsServiceManager>();
            unitedStatesServiceManager.Setup(m => m.CountryCode).Returns("US");

            puertoRicoServiceManager = new Mock<IUpsServiceManager>();
            puertoRicoServiceManager.Setup(m => m.CountryCode).Returns("PR");
            

            testObject = UpsServiceManagerFactory.CreateForTesting(new List<IUpsServiceManager> { canadaServiceManager.Object, unitedStatesServiceManager.Object, puertoRicoServiceManager.Object });
        }

        [Fact]
        public void Create_UsesServiceManagerCountryCode_ToDetermineServiceManagerToUse()
        {
            ShipmentEntity shipment = new ShipmentEntity {OriginCountryCode = "CA"};
            testObject.Create(shipment);

            canadaServiceManager.VerifyGet(m => m.CountryCode, Times.Once());
        }

        [Fact]
        public void Create_ReturnsCanadaServiceManager_WhenOriginCountryCodeIsCA()
        {
            ShipmentEntity shipment = new ShipmentEntity { OriginCountryCode = "CA" };
            IUpsServiceManager serviceManager = testObject.Create(shipment);

            Assert.Equal("CA", serviceManager.CountryCode);
            Assert.Equal(canadaServiceManager.Object, serviceManager);
        }

        [Fact]
        public void Create_ReturnsPRServiceManager_WhenOriginCountryCodeIsPR()
        {
            ShipmentEntity shipment = new ShipmentEntity { OriginCountryCode = "PR" };
            IUpsServiceManager serviceManager = testObject.Create(shipment);

            Assert.Equal("PR", serviceManager.CountryCode);
            Assert.Equal(puertoRicoServiceManager.Object, serviceManager);
        }


        [Fact]
        public void Create_ReturnsUnitedStatesServiceManager_WhenOriginCountryCodeIsUS()
        {
            ShipmentEntity shipment = new ShipmentEntity { OriginCountryCode = "US" };
            IUpsServiceManager serviceManager = testObject.Create(shipment);

            Assert.Equal("US", serviceManager.CountryCode);
            Assert.Equal(unitedStatesServiceManager.Object, serviceManager);
        }

        [Fact]
        public void Create_ReturnsUnitedStatesServiceManager_WhenOriginCountryCodeIsNotUS_AndNotCA()
        {
            ShipmentEntity shipment = new ShipmentEntity { OriginCountryCode = "GB" };
            IUpsServiceManager serviceManager = testObject.Create(shipment);

            Assert.Equal("US", serviceManager.CountryCode);
            Assert.Equal(unitedStatesServiceManager.Object, serviceManager);
        }
    }
}
