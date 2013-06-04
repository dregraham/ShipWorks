using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager.Countries;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using log4net;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.ServiceManager.Countries
{
    [TestClass]
    public class UpsPuertoRicoServiceManagerTest
    {
        private UpsPuertoRicoServiceManager testObject;

        private Mock<ILog> logger;

        [TestInitialize]
        public void Initialize()
        {
            logger = new Mock<ILog>();
            logger.Setup(l => l.Error(It.IsAny<string>()));

            testObject = new UpsPuertoRicoServiceManager(logger.Object);
        }

        [TestMethod]
        public void CountryCode_ReturnsPR_Test()
        {
            Assert.AreEqual("PR", testObject.CountryCode);
        }

        #region GetServices tests

        [TestMethod]
        public void GetServices_ContainsUpsNextDayAir_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.ShipCountryCode = "US";

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsNextDayAir));
        }

        [TestMethod]
        public void GetServices_ContainsUps2DayAir_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.ShipCountryCode = "US";

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.Ups2DayAir));
        }

        [TestMethod]
        public void GetServices_ContainsUpsGround_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.ShipCountryCode = "US";

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsGround));
        }

        [TestMethod]
        public void GetServices_ContainsWorldwideExpress_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.WorldwideExpress));
        }

        [TestMethod]
        public void GetServices_ContainsWorldwideExpedited_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.WorldwideExpedited));
        }

        [TestMethod]
        public void GetServices_ContainsUpsNextDayAirAM_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.ShipCountryCode = "US";

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsNextDayAirAM));
        }

        [TestMethod]
        public void GetServices_ContainsWorldwideExpressPlus_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.WorldwideExpressPlus));
        }

        [TestMethod]
        public void GetServices_ContainsUpsExpressSaver_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressSaver));
        }

        #endregion GetServices tests

        #region GetServicesByRateCode tests

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpress_AndRateCodeIs01_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("01", "US");

            Assert.AreEqual(UpsServiceType.UpsNextDayAir, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUps2DayAir_AndRateCodeIs02AndCountryIsUs_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("02", "US");

            Assert.AreEqual(UpsServiceType.Ups2DayAir, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsGround_AndRateCodeIs03_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("03", "US");

            Assert.AreEqual(UpsServiceType.UpsGround, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsWorldwideExpress_AndRateCodeIs07_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("07", "XXXX");

            Assert.AreEqual(UpsServiceType.WorldwideExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsWorldwideExpedited_AndRateCodeIs08_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("08", "XXXX");

            Assert.AreEqual(UpsServiceType.WorldwideExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsNextDayAirAmAndRateCodeIs14_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("14", "US");

            Assert.AreEqual(UpsServiceType.UpsNextDayAirAM, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsWorldwideExpressPlus_AndRateCodeIs54_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("54", "XXXX");

            Assert.AreEqual(UpsServiceType.WorldwideExpressPlus, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpressSaver_AndRateCodeIs65_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("65", "XXXX");

            Assert.AreEqual(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }


        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServicesByRateCode_ThrowsUpsException_AndRateCodeIs42_Test()
        {
            testObject.GetServicesByRateCode("42", "XXXX");
        }

        #endregion GetServicesByRateCode tests

        #region GetServicesByWorldShipDescription tests

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsNextDayAir_WhenDescriptionIsNextDayAirAndCountryIsUS()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription(WorldShipServiceDescriptions.UpsNextDayAir, "US");

            Assert.AreEqual(UpsServiceType.UpsNextDayAir, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUps2DayAir_WhenDescriptionIs2ndDayAirAndCountryIsUS()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription(WorldShipServiceDescriptions.Ups2DayAir, "US");

            Assert.AreEqual(UpsServiceType.Ups2DayAir, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsGround_WhenDescriptionIsGroundServiceAndCountryIsUS()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription(WorldShipServiceDescriptions.Ground, "US");

            Assert.AreEqual(UpsServiceType.UpsGround, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsWorldwideExpress_WhenDescriptionIsExpressOrWorldwideExpress()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription(WorldShipServiceDescriptions.WorldwideExpress, "RU");

            Assert.AreEqual(UpsServiceType.WorldwideExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsWorldwideExpedited_WhenDescriptionIsExpeditedOrWorldwideExpedited()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription(WorldShipServiceDescriptions.WorldwideExpedited, "RU");

            Assert.AreEqual(UpsServiceType.WorldwideExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsUpsNextDayAirAM_WhenDescriptionIsNextDayAirEarlyAMAndCountryIsUS()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription(WorldShipServiceDescriptions.UpsNextDayAirAm, "US");

            Assert.AreEqual(UpsServiceType.UpsNextDayAirAM, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsWorldwideExpressPlus_WhenDescriptionIsExpressPlusOrWorldwideExpressPlus()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription(WorldShipServiceDescriptions.WorldwideExpressPlus, "RU");

            Assert.AreEqual(UpsServiceType.WorldwideExpressPlus, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenDescriptionIsExpressSaverOrWorldwideSaver()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription(WorldShipServiceDescriptions.UpsExpressSaver, "RU");

            Assert.AreEqual(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServicesByWorldShipDescription_ThrowsUpsException_WhenDescriptionIsNotFound_Test()
        {
            testObject.GetServicesByWorldShipDescription("This is not going to be found", "CA");
        }

        #endregion GetServicesByWorldShipDescription tests

        #region GetServicesByTransitService tests

        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServiceByTransitCode_ThrowUpsException_WhenTransitCodeIs02AndCountryCodeIsPr_Test()
        {
            testObject.GetServiceByTransitCode("02", "PR");
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnUps2DayAirIntra_WhenTransitCodeIs01AndCountryCodeIsPr_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("01", "PR");

            Assert.AreEqual(UpsServiceType.Ups2nDayAirIntra, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsNextDayAirAm_WhenTransitCodeIs21AndCountryCodeIsUs_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("21", "US");

            Assert.AreEqual(UpsServiceType.UpsNextDayAirAM, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsNextDayAir_WhenTransitCodeIs01AndCountryCodeIsUs_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("01", "US");

            Assert.AreEqual(UpsServiceType.UpsNextDayAir,mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUps2DayAir_WhenTransitCodeIs02AndCountryCodeIsUs_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("02", "US");

            Assert.AreEqual(UpsServiceType.Ups2DayAir, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsGround_WhenTransitCodeIsG_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("G", "US");

            Assert.AreEqual(UpsServiceType.UpsGround, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsWorldwideExpressPlus_WhenTransitCodeIs21AndCountryCodeIsNotUsOrPr_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("21", "asfdsaf");

            Assert.AreEqual(UpsServiceType.WorldwideExpressPlus, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsWorldwideExpress_WhenTransitCodeIs01AndCountryCodeIsNotUsOrPr_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("01", "asfdsaf");

            Assert.AreEqual(UpsServiceType.WorldwideExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpressSaver_WhenTransitCodeIs28AndCountryCodeIsNotUsOrPr_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("28", "asfdsaf");

            Assert.AreEqual(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsWordwideExpedited_WhenTransitCodeIs05AndCountryCodeIsNotUsOrPr_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("05", "asfdsaf");

            Assert.AreEqual(UpsServiceType.WorldwideExpedited, mapping.UpsServiceType);
        }
        #endregion GetServicesByTransitService tests
    }
}
