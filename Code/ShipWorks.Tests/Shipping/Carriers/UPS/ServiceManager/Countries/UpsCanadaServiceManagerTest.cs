﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager.Countries;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.ServiceManager.Countries
{
    [TestClass]
    public class UpsCanadaServiceManagerTest
    {
        private UpsCanadaServiceManager testObject;

        private Mock<ILog> logger;

        [TestInitialize]
        public void Initialize()
        {
            logger = new Mock<ILog>();
            logger.Setup(l => l.Error(It.IsAny<string>()));

            testObject = new UpsCanadaServiceManager(logger.Object);
        }

        [TestMethod]
        public void CountryCode_ReturnsCA_Test()
        {
            Assert.AreEqual("CA", testObject.CountryCode);
        }


        #region GetServices tests

        // Canada tests
        [TestMethod]
        public void GetServices_ContainsFiveItems_WhenShipCountryIsCanada_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(5, mappings.Count);
        }

        [TestMethod]
        public void GetServices_ContainsUpsExpress_WhenShipCountryIsCanada_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpress));
        }

        [TestMethod]
        public void GetServices_ContainsUpsExpedited_WhenShipCountryIsCanada_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpedited));
        }

        [TestMethod]
        public void GetServices_ContainsUpsStandard_WhenShipCountryIsCanada_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsStandard));
        }

        [TestMethod]
        public void GetServices_ContainsUpsExpressEarlyAm_WhenShipCountryIsCanada_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressEarlyAm));
        }

        [TestMethod]
        public void GetServices_ContainsUpsExpressSaver_WhenShipCountryIsCanada_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressSaver));
        }

        // US tests
        [TestMethod]
        public void GetServices_ContainsSixItems_WhenShipCountryIsUnitedStates_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(6, mappings.Count);
        }

        [TestMethod]
        public void GetServices_ContainsUpsExpedited_WhenShipCountryIsUnitedStates_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpedited));
        }

        [TestMethod]
        public void GetServices_ContainsUpsExpress_WhenShipCountryIsUnitedStates_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpress));
        }
        
        [TestMethod]
        public void GetServices_ContainsUpsStandard_WhenShipCountryIsUnitedStates_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsStandard));
        }

        [TestMethod]
        public void GetServices_ContainsUps3DaySelectFromCanaada_WhenShipCountryIsUnitedStates_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.Ups3DaySelectFromCanada));
        }

        [TestMethod]
        public void GetServices_ContainsUpsExpressSaver_WhenShipCountryIsUnitedStates_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressSaver));
        }

        [TestMethod]
        public void GetServices_ContainsUpsExpressEarlyAm_WhenShipCountryIsUnitedStates_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressEarlyAm));
        }


        // Puerto Rico tests
        [TestMethod]
        public void GetServices_ContainsThreeItems_WhenShipCountryIsPuertoRico_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "PR" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(2, mappings.Count);
        }        

        [TestMethod]
        public void GetServices_ContainsUpsExpedited_WhenShipCountryIsPuertoRico_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "PR" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpedited));
        }

        [TestMethod]
        public void GetServices_ContainsUpsCaWorldWideExpressSaver_WhenShipCountryIsPuertoRico_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "PR" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsCaWorldWideExpressSaver));
        }


        // International tests
        [TestMethod]
        public void GetServices_ContainsThreeItems_WhenShipCountryIsInternational_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "GB" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(3, mappings.Count);
        }

        [TestMethod]
        public void GetServices_ContainsUpsCaWorldWideExpress_WhenShipCountryIsInternational_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "RU" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsCaWorldWideExpress));
        }

        [TestMethod]
        public void GetServices_ContainsUpsCaWorldWideExpressSaver_WhenShipCountryIsInternational_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "RU" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsCaWorldWideExpressSaver));
        }

        [TestMethod]
        public void GetServices_ContainsUpsCaWorldWideExpressPlus_WhenShipCountryIsInternational_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "RU" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsCaWorldWideExpressPlus));
        }

        // Does not appear to be a valid service type for international - commenting out until can be confirmed
        //[TestMethod]
        //public void GetServices_ContainsUpsExpressEarlyAm_WhenShipCountryIsInternational_Test()
        //{
        //    ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "RU" };

        //    List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

        //    Assert.AreEqual(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressEarlyAm));
        //}

        #endregion GetServices tests


        #region GetServicesByRateCode tests

        // Canada Tests
        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpress_WhenDestinationCountryIsCanada_AndRateCodeIs01_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("01", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpedited_WhenDestinationCountryIsCanada_AndRateCodeIs02_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("02", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsStandard_WhenDestinationCountryIsCanada_AndRateCodeIs11_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("11", "CA");

            Assert.AreEqual(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpressSaver_WhenDestinationCountryIsCanada_AndRateCodeIs13_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("13", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpressEarlyAm_WhenDestinationCountryIsCanada_AndRateCodeIs14_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("14", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }


        // US Tests
        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpedited_WhenDestinationCountryIsUnitedStates_AndRateCodeIs02_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("02", "US");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpress_WhenDestinationCountryIsUnitedStates_AndRateCodeIs07_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("07", "US");

            Assert.AreEqual(UpsServiceType.UpsExpress, mapping.UpsServiceType);
        }
        
        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsStandard_WhenDestinationCountryIsUnitedStates_AndRateCodeIs11_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("11", "US");

            Assert.AreEqual(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUps3DaySelectFromCanada_WhenDestinationCountryIsUnitedStates_AndRateCodeIs12_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("12", "US");

            Assert.AreEqual(UpsServiceType.Ups3DaySelectFromCanada, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpressSaver_WhenDestinationCountryIsUnitedStates_AndRateCodeIs13_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("13", "US");

            Assert.AreEqual(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpressEarlyAm_WhenDestinationCountryIsUnitedStates_AndRateCodeIs54_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("54", "US");

            Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }
        
        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpedited_WhenDestinationCountryIsPuertoRico_AndRateCodeIs02_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("02", "PR");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsExpressSaver_WhenDestinationCountryIsPuertoRico_AndRateCodeIs65_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("65", "PR");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }


        // International tests
        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsCaWorldWideExpress_WhenDestinationCountryIsInternational_AndRateCodeIs07_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("07", "RU");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsCaWorldWideExpressPlus_WhenDestinationCountryIsInternational_AndRateCodeIs21_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("21", "RU");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressPlus, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByRateCode_ReturnsUpsCaWorldWideExpressSaver_WhenDestinationCountryIsInternational_AndRateCodeIs65_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("65", "RU");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        // Does not appear to be a valid service type for international - commenting out until can be confirmed
        //[TestMethod]
        //public void GetServicesByRateCode_ReturnsUpsExpressEarlyAm_WhenDestinationCountryIsInternational_AndRateCodeIs14_Test()
        //{
        //    UpsServiceMapping mapping = testObject.GetServicesByRateCode("14", "RU");

        //    Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        //}


        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServicesByRateCode_ThrowsUpsException_WhenRateCodeIsNotFound_Test()
        {
            testObject.GetServicesByRateCode("999", "CA");
        }


        [TestMethod]
        public void GetServicesByRateCode_WritesToLog_WhenRateCodeIsNotFound_Test()
        {
            try
            {
                testObject.GetServicesByRateCode("999", "CA");
            }
            catch (UpsException)
            {
                logger.Verify(l => l.Error(It.IsAny<string>()), Times.Once());
            }
        }

        #endregion GetServicesByRateCode tests
        

        #region GetServicesByWorldShipDescription tests

        // Canada tests
        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpress_WhenCountryCodeIsCanada_AndDescriptionIsExpress_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpedited_WhenCountryCodeIsCanada_AndDescriptionIsUpsExpedited_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Expedited", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsStandard_WhenCountryCodeIsCanada_AndDescriptionIsStandard_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Standard", "CA");

            Assert.AreEqual(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenCountryCodeIsCanada_AndDescriptionIsExpressSaver_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Saver", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressEarlyAm_WhenCountryCodeIsCanada_AndDescriptionIsExpressEarlyAM_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Early AM", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressEarlyAm_WhenCountryCodeIsLowerCaseCanada_AndDescriptionIsExpressEarlyAM_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Early AM", "ca");

            Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServicesByWorldShipDescription_ThrowsUpsException_WhenCountryCodeIsCanada_AndDescriptionIsNotFound_Test()
        {
            testObject.GetServicesByWorldShipDescription("This is not going to be found", "CA");
        }


        // US tests
        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpedited_WhenCountryCodeIsUnitedStates_AndDescriptionIsUpsExpedited_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Expedited", "US");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsStandard_WhenCountryCodeIsUnitedStates_AndDescriptionIsStandard_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Standard", "US");

            Assert.AreEqual(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUps3DaySelect_WhenCountryCodeIsUnitedStates_AndDescriptionIs3DaySelect_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("3 Day Select", "US");

            Assert.AreEqual(UpsServiceType.Ups3DaySelectFromCanada, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenCountryCodeIsUnitedStates_AndDescriptionIsExpressSaver_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Saver", "US");

            Assert.AreEqual(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressEarlyAm_WhenCountryCodeIsUnitedStates_AndDescriptionIsExpressEarlyAM_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("EXPRESS EARLY AM", "US");

            Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressEarlyAm_WhenCountryCodeIsLowerCaseUnitedStates_AndDescriptionIsExpressEarlyAM_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("EXPRESS EARLY AM", "us");

            Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }
        
        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServicesByWorldShipDescription_ThrowsUpsException_WhenCountryCodeIsUnitedStates_AndDescriptionIsNotFound_Test()
        {
            testObject.GetServicesByWorldShipDescription("This is not going to be found", "US");
        }


        //Puerto Rico tests
        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpedited_WhenCountryCodeIsPuertoRico_AndDescriptionIsUpsExpedited_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Expedited", "PR");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenCountryCodeIsPuertoRico_AndDescriptionIsExpressSaver_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Saver", "PR");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenCountryCodeIsLowerCasePuertoRico_AndDescriptionIsExpressSaver_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Saver", "pr");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServicesByWorldShipDescription_ThrowsUpsException_WhenCountryCodeIsPuertoRico_AndDescriptionIsNotFound_Test()
        {
            testObject.GetServicesByWorldShipDescription("This is not going to be found", "PR");
        }


        // International tests
        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsCaWorldWideExpress_WhenCountryCodeIsInternational_AndDescriptionIsUpsExpress_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express", "FR");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsCaWorldWideExpressPlus_WhenCountryCodeIsInternational_AndDescriptionIsUpsExpedited_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("WORLDWIDE EXPRESS PLUS", "RU");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressPlus, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsCAWorldWideExpressSaver_WhenCountryCodeIsInternational_AndDescriptionIsExpressSaver_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Saver", "RU");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        // Does not appear to be a valid service type for international - commenting out until can be confirmed
        //[TestMethod]
        //public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenCountryCodeIsInternational_AndDescriptionIsExpressEarlyAM_Test()
        //{
        //    UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("EXPRESS EARLY AM", "RU");

        //    Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        //}

        [TestMethod]
        public void GetServicesByWorldShipDescription_ReturnsUpsCaWorldWideExpressSaver_WhenCountryCodeIsInternational_AndDescriptionIsAllLowerCase_Test()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("express saver", "RU");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServicesByWorldShipDescription_ThrowsUpsException_WhenCountryCodeIsInternational_AndDescriptionIsNotFound_Test()
        {
            testObject.GetServicesByWorldShipDescription("This is not going to be found", "RU");
        }

        #endregion GetServicesByWorldShipDescription tests


        #region GetServiceByTransitCode tests

        // Canada Tests
        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpress_WhenCountryIsCanada_AndTransitCodeIs24_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("24", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpedited_WhenCountryIsCanada_AndTransitCodeIs19_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("19", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsStandard_WhenCountryIsCanada_AndTransitCodeIs25_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("25", "CA");

            Assert.AreEqual(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpressSaver_WhenCountryIsCanada_AndTransitCodeIs20_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("20", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpressEarlyAm_WhenCountryIsCanada_AndTransitCodeIs23_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("23", "CA");

            Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpressEarlyAm_WhenCountryIsLowerCaseCanada_AndTransitCodeIs23_Test()
        {
            // Primarily testing that casing doesn't matter for the country code
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("23", "ca");

            Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServiceByTransitCode_ThrowsUpsException_WhenCountryIsCanada_AndTransitCodeIsNotFound_Test()
        {
            testObject.GetServiceByTransitCode("This will not be found.", "CA");
        }


        // US tests
        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpedited_WhenCountryIsUnitedStates_AndTransitCodeIs05_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("05", "US");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpress_WhenCountryIsUnitedStates_AndTransitCodeIs01_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("01", "US");

            Assert.AreEqual(UpsServiceType.UpsExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsStandard_WhenCountryIsUnitedStates_AndTransitCodeIs03_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("03", "US");

            Assert.AreEqual(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUps3DaySelectFromCanada_WhenCountryIsUnitedStates_AndTransitCodeIs33_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("33", "US");

            Assert.AreEqual(UpsServiceType.Ups3DaySelectFromCanada, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpressSaver_WhenCountryIsUnitedStates_AndTransitCodeIs28_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("28", "US");

            Assert.AreEqual(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpressEarlyAm_WhenCountryIsUnitedStates_AndTransitCodeIs21_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("21", "US");

            Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpedited_WhenCountryIsLowerCaseUnitedStates_AndTransitCodeIs05_Test()
        {
            // Just provide a valid trasit code and lower case country code for US
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("05", "us");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServiceByTransitCode_ThrowsUpsException_WhenCountryIsUnitedStates_AndTransitCodeIsNotFound_Test()
        {
            testObject.GetServiceByTransitCode("This will not be found.", "US");
        }


        // Puerto Rico tests
        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsCAWorldWideExpressSaver_WhenCountryIsPuertoRico_AndTransitCodeIs65_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("65", "PR");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpedited_WhenCountryIsPuertoRico_AndTransitCodeIs05_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("05", "PR");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpedited_WhenCountryIsLowerCasePuertoRico_AndTransitCodeIs05_Test()
        {
            // Just provide a valid trasit code and lower case country code for PR
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("05", "pr");

            Assert.AreEqual(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }
        
        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServiceByTransitCode_ThrowsUpsException_WhenCountryIsPuertoRico_AndTransitCodeIsNotFound_Test()
        {
            testObject.GetServiceByTransitCode("This will not be found.", "PR");
        }


        // International tests
        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsCaWorldWideExpress_WhenCountryIsInternational_AndTransitCodeIs01_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("01", "GB");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsWorldWideExpress_WhenCountryIsInternational_AndTransitCodeIs21_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("21", "GB");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressPlus, mapping.UpsServiceType);
        }

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsExpressSaver_WhenCountryIsInternational_AndTransitCodeIs28_Test()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("28", "GB");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        // Does not appear to be a valid service type for international - commenting out until can be confirmed
        //[TestMethod]
        //public void GetServiceByTransitCode_ReturnsUpsExpressEarlyAm_WhenCountryIsInternational_AndTransitCodeIs54_Test()
        //{
        //    UpsServiceMapping mapping = testObject.GetServiceByTransitCode("54", "RU");

        //    Assert.AreEqual(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        //}

        [TestMethod]
        public void GetServiceByTransitCode_ReturnsUpsCaWorldWideExpress_WhenCountryIsLowerCaseInternational_AndTransitCodeIs01_Test()
        {
            // Just provide a valid trasit code and lower case country code for PR
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("01", "gb");

            Assert.AreEqual(UpsServiceType.UpsCaWorldWideExpress, mapping.UpsServiceType);
        }

        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void GetServiceByTransitCode_ThrowsUpsException_WhenCountryIsInternational_AndTransitCodeIsNotFound_Test()
        {
            testObject.GetServiceByTransitCode("This will not be found.", "FR");
        }

        #endregion GetServiceByTransitCode tests
    }
}
