using System.Collections.Generic;
using System.Linq;
using Xunit;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager.Countries;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.ServiceManager.Countries
{
    public class UpsCanadaServiceManagerTest
    {
        private UpsCanadaServiceManager testObject;

        private Mock<ILog> logger;

        public UpsCanadaServiceManagerTest()
        {
            logger = new Mock<ILog>();
            logger.Setup(l => l.Error(It.IsAny<string>()));

            testObject = new UpsCanadaServiceManager(logger.Object);
        }

        [Fact]
        public void CountryCode_ReturnsCA()
        {
            Assert.Equal("CA", testObject.CountryCode);
        }


        #region GetServices tests

        // Canada tests
        [Fact]
        public void GetServices_ContainsFiveItems_WhenShipCountryIsCanada()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(5, mappings.Count);
        }

        [Fact]
        public void GetServices_ContainsUpsExpress_WhenShipCountryIsCanada()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpress));
        }

        [Fact]
        public void GetServices_ContainsUpsExpedited_WhenShipCountryIsCanada()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpedited));
        }

        [Fact]
        public void GetServices_ContainsUpsStandard_WhenShipCountryIsCanada()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsStandard));
        }

        [Fact]
        public void GetServices_ContainsUpsExpressEarlyAm_WhenShipCountryIsCanada()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressEarlyAm));
        }

        [Fact]
        public void GetServices_ContainsUpsExpressSaver_WhenShipCountryIsCanada()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "CA" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressSaver));
        }

        // US tests
        [Fact]
        public void GetServices_ContainsSevenItems_WhenShipCountryIsUnitedStates()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(7, mappings.Count);
        }

        [Fact]
        public void GetServices_ContainsUpsExpedited_WhenShipCountryIsUnitedStates()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpedited));
        }

        [Fact]
        public void GetServices_ContainsUpsExpress_WhenShipCountryIsUnitedStates()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpress));
        }

        [Fact]
        public void GetServices_ContainsUpsStandard_WhenShipCountryIsUnitedStates()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsStandard));
        }

        [Fact]
        public void GetServices_ContainsUps3DaySelectFromCanaada_WhenShipCountryIsUnitedStates()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.Ups3DaySelectFromCanada));
        }

        [Fact]
        public void GetServices_ContainsUpsExpressSaver_WhenShipCountryIsUnitedStates()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressSaver));
        }

        [Fact]
        public void GetServices_ContainsUpsExpressEarlyAm_WhenShipCountryIsUnitedStates()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "US" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressEarlyAm));
        }


        // Puerto Rico tests
        [Fact]
        public void GetServices_ContainsThreeItems_WhenShipCountryIsPuertoRico()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "PR" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(2, mappings.Count);
        }

        [Fact]
        public void GetServices_ContainsUpsExpedited_WhenShipCountryIsPuertoRico()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "PR" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpedited));
        }

        [Fact]
        public void GetServices_ContainsUpsCaWorldWideExpressSaver_WhenShipCountryIsPuertoRico()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "PR" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsCaWorldWideExpressSaver));
        }


        // International tests
        [Fact]
        public void GetServices_ContainsThreeItems_WhenShipCountryIsInternational()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "GB" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(3, mappings.Count);
        }

        [Fact]
        public void GetServices_ContainsUpsCaWorldWideExpress_WhenShipCountryIsInternational()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "RU" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsCaWorldWideExpress));
        }

        [Fact]
        public void GetServices_ContainsUpsCaWorldWideExpressSaver_WhenShipCountryIsInternational()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "RU" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsCaWorldWideExpressSaver));
        }

        [Fact]
        public void GetServices_ContainsUpsCaWorldWideExpressPlus_WhenShipCountryIsInternational()
        {
            ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "RU" };

            List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

            Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsCaWorldWideExpressPlus));
        }

        // Does not appear to be a valid service type for international - commenting out until can be confirmed
        //[Fact]
        //public void GetServices_ContainsUpsExpressEarlyAm_WhenShipCountryIsInternational()
        //{
        //    ShipmentEntity shipment = new ShipmentEntity { ShipCountryCode = "RU" };

        //    List<UpsServiceMapping> mappings = testObject.GetServices(shipment);

        //    Assert.Equal(1, mappings.Count(m => m.UpsServiceType == UpsServiceType.UpsExpressEarlyAm));
        //}

        #endregion GetServices tests


        #region GetServicesByRateCode tests

        // Canada Tests
        [Fact]
        public void GetServicesByRateCode_ReturnsUpsExpress_WhenDestinationCountryIsCanada_AndRateCodeIs01()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("01", "CA");

            Assert.Equal(UpsServiceType.UpsExpress, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsExpedited_WhenDestinationCountryIsCanada_AndRateCodeIs02()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("02", "CA");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsStandard_WhenDestinationCountryIsCanada_AndRateCodeIs11()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("11", "CA");

            Assert.Equal(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsExpressSaver_WhenDestinationCountryIsCanada_AndRateCodeIs13()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("13", "CA");

            Assert.Equal(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsExpressEarlyAm_WhenDestinationCountryIsCanada_AndRateCodeIs14()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("14", "CA");

            Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }


        // US Tests
        [Fact]
        public void GetServicesByRateCode_ReturnsUpsExpedited_WhenDestinationCountryIsUnitedStates_AndRateCodeIs02()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("02", "US");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsExpress_WhenDestinationCountryIsUnitedStates_AndRateCodeIs07()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("07", "US");

            Assert.Equal(UpsServiceType.UpsExpress, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsStandard_WhenDestinationCountryIsUnitedStates_AndRateCodeIs11()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("11", "US");

            Assert.Equal(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUps3DaySelectFromCanada_WhenDestinationCountryIsUnitedStates_AndRateCodeIs12()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("12", "US");

            Assert.Equal(UpsServiceType.Ups3DaySelectFromCanada, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsExpressSaver_WhenDestinationCountryIsUnitedStates_AndRateCodeIs13()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("13", "US");

            Assert.Equal(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsExpressEarlyAm_WhenDestinationCountryIsUnitedStates_AndRateCodeIs54()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("54", "US");

            Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsExpedited_WhenDestinationCountryIsPuertoRico_AndRateCodeIs02()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("02", "PR");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsExpressSaver_WhenDestinationCountryIsPuertoRico_AndRateCodeIs65()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("65", "PR");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }


        // International tests
        [Fact]
        public void GetServicesByRateCode_ReturnsUpsCaWorldWideExpress_WhenDestinationCountryIsInternational_AndRateCodeIs07()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("07", "RU");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpress, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsCaWorldWideExpressPlus_WhenDestinationCountryIsInternational_AndRateCodeIs21()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("21", "RU");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressPlus, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByRateCode_ReturnsUpsCaWorldWideExpressSaver_WhenDestinationCountryIsInternational_AndRateCodeIs65()
        {
            UpsServiceMapping mapping = testObject.GetServicesByRateCode("65", "RU");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        // Does not appear to be a valid service type for international - commenting out until can be confirmed
        //[Fact]
        //public void GetServicesByRateCode_ReturnsUpsExpressEarlyAm_WhenDestinationCountryIsInternational_AndRateCodeIs14()
        //{
        //    UpsServiceMapping mapping = testObject.GetServicesByRateCode("14", "RU");

        //    Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        //}


        [Fact]
        public void GetServicesByRateCode_ThrowsUpsException_WhenRateCodeIsNotFound()
        {
            Assert.Throws<UpsException>(() => testObject.GetServicesByRateCode("999", "CA"));
        }

        [Fact]
        public void GetServicesByRateCode_WritesToLog_WhenRateCodeIsNotFound()
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
        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpress_WhenCountryCodeIsCanada_AndDescriptionIsExpress()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express", "CA");

            Assert.Equal(UpsServiceType.UpsExpress, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpedited_WhenCountryCodeIsCanada_AndDescriptionIsUpsExpedited()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Expedited", "CA");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsStandard_WhenCountryCodeIsCanada_AndDescriptionIsStandard()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Standard", "CA");

            Assert.Equal(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenCountryCodeIsCanada_AndDescriptionIsExpressSaver()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Saver", "CA");

            Assert.Equal(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressEarlyAm_WhenCountryCodeIsCanada_AndDescriptionIsExpressEarlyAM()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Early AM", "CA");

            Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressEarlyAm_WhenCountryCodeIsLowerCaseCanada_AndDescriptionIsExpressEarlyAM()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Early AM", "ca");

            Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ThrowsUpsException_WhenCountryCodeIsCanada_AndDescriptionIsNotFound()
        {
            Assert.Throws<UpsException>(() => testObject.GetServicesByWorldShipDescription("This is not going to be found", "CA"));
        }


        // US tests
        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpedited_WhenCountryCodeIsUnitedStates_AndDescriptionIsUpsExpedited()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Expedited", "US");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsStandard_WhenCountryCodeIsUnitedStates_AndDescriptionIsStandard()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Standard", "US");

            Assert.Equal(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUps3DaySelect_WhenCountryCodeIsUnitedStates_AndDescriptionIs3DaySelect()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("3 Day Select", "US");

            Assert.Equal(UpsServiceType.Ups3DaySelectFromCanada, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenCountryCodeIsUnitedStates_AndDescriptionIsExpressSaver()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Saver", "US");

            Assert.Equal(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressEarlyAm_WhenCountryCodeIsUnitedStates_AndDescriptionIsExpressEarlyAM()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("EXPRESS EARLY AM", "US");

            Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressEarlyAm_WhenCountryCodeIsLowerCaseUnitedStates_AndDescriptionIsExpressEarlyAM()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("EXPRESS EARLY AM", "us");

            Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ThrowsUpsException_WhenCountryCodeIsUnitedStates_AndDescriptionIsNotFound()
        {
            Assert.Throws<UpsException>(() => testObject.GetServicesByWorldShipDescription("This is not going to be found", "US"));
        }


        //Puerto Rico tests
        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpedited_WhenCountryCodeIsPuertoRico_AndDescriptionIsUpsExpedited()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Expedited", "PR");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenCountryCodeIsPuertoRico_AndDescriptionIsExpressSaver()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Saver", "PR");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenCountryCodeIsLowerCasePuertoRico_AndDescriptionIsExpressSaver()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Saver", "pr");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ThrowsUpsException_WhenCountryCodeIsPuertoRico_AndDescriptionIsNotFound()
        {
            Assert.Throws<UpsException>(() => testObject.GetServicesByWorldShipDescription("This is not going to be found", "PR"));
        }


        // International tests
        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsCaWorldWideExpress_WhenCountryCodeIsInternational_AndDescriptionIsUpsExpress()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express", "FR");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpress, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsCaWorldWideExpressPlus_WhenCountryCodeIsInternational_AndDescriptionIsUpsExpedited()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("WORLDWIDE EXPRESS PLUS", "RU");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressPlus, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsCAWorldWideExpressSaver_WhenCountryCodeIsInternational_AndDescriptionIsExpressSaver()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("Express Saver", "RU");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        // Does not appear to be a valid service type for international - commenting out until can be confirmed
        //[Fact]
        //public void GetServicesByWorldShipDescription_ReturnsUpsExpressSaver_WhenCountryCodeIsInternational_AndDescriptionIsExpressEarlyAM()
        //{
        //    UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("EXPRESS EARLY AM", "RU");

        //    Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        //}

        [Fact]
        public void GetServicesByWorldShipDescription_ReturnsUpsCaWorldWideExpressSaver_WhenCountryCodeIsInternational_AndDescriptionIsAllLowerCase()
        {
            UpsServiceMapping mapping = testObject.GetServicesByWorldShipDescription("express saver", "RU");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServicesByWorldShipDescription_ThrowsUpsException_WhenCountryCodeIsInternational_AndDescriptionIsNotFound()
        {
            Assert.Throws<UpsException>(() => testObject.GetServicesByWorldShipDescription("This is not going to be found", "RU"));
        }

        #endregion GetServicesByWorldShipDescription tests


        #region GetServiceByTransitCode tests

        // Canada Tests
        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpress_WhenCountryIsCanada_AndTransitCodeIs24()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("24", "CA");

            Assert.Equal(UpsServiceType.UpsExpress, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpedited_WhenCountryIsCanada_AndTransitCodeIs19()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("19", "CA");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsStandard_WhenCountryIsCanada_AndTransitCodeIs25()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("25", "CA");

            Assert.Equal(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpressSaver_WhenCountryIsCanada_AndTransitCodeIs20()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("20", "CA");

            Assert.Equal(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpressEarlyAm_WhenCountryIsCanada_AndTransitCodeIs23()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("23", "CA");

            Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpressEarlyAm_WhenCountryIsLowerCaseCanada_AndTransitCodeIs23()
        {
            // Primarily testing that casing doesn't matter for the country code
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("23", "ca");

            Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ThrowsUpsException_WhenCountryIsCanada_AndTransitCodeIsNotFound()
        {
            Assert.Throws<UpsException>(() => testObject.GetServiceByTransitCode("This will not be found.", "CA"));
        }


        // US tests
        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpedited_WhenCountryIsUnitedStates_AndTransitCodeIs05()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("05", "US");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpress_WhenCountryIsUnitedStates_AndTransitCodeIs01()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("01", "US");

            Assert.Equal(UpsServiceType.UpsExpress, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsStandard_WhenCountryIsUnitedStates_AndTransitCodeIs03()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("03", "US");

            Assert.Equal(UpsServiceType.UpsStandard, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUps3DaySelectFromCanada_WhenCountryIsUnitedStates_AndTransitCodeIs33()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("33", "US");

            Assert.Equal(UpsServiceType.Ups3DaySelectFromCanada, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpressSaver_WhenCountryIsUnitedStates_AndTransitCodeIs28()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("28", "US");

            Assert.Equal(UpsServiceType.UpsExpressSaver, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpressEarlyAm_WhenCountryIsUnitedStates_AndTransitCodeIs21()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("21", "US");

            Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpedited_WhenCountryIsLowerCaseUnitedStates_AndTransitCodeIs05()
        {
            // Just provide a valid trasit code and lower case country code for US
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("05", "us");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ThrowsUpsException_WhenCountryIsUnitedStates_AndTransitCodeIsNotFound()
        {
            Assert.Throws<UpsException>(() => testObject.GetServiceByTransitCode("This will not be found.", "US"));
        }


        // Puerto Rico tests
        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsCAWorldWideExpressSaver_WhenCountryIsPuertoRico_AndTransitCodeIs65()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("65", "PR");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpedited_WhenCountryIsPuertoRico_AndTransitCodeIs05()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("05", "PR");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpedited_WhenCountryIsLowerCasePuertoRico_AndTransitCodeIs05()
        {
            // Just provide a valid trasit code and lower case country code for PR
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("05", "pr");

            Assert.Equal(UpsServiceType.UpsExpedited, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ThrowsUpsException_WhenCountryIsPuertoRico_AndTransitCodeIsNotFound()
        {
            Assert.Throws<UpsException>(() => testObject.GetServiceByTransitCode("This will not be found.", "PR"));
        }


        // International tests
        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsCaWorldWideExpress_WhenCountryIsInternational_AndTransitCodeIs01()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("01", "GB");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpress, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsWorldWideExpress_WhenCountryIsInternational_AndTransitCodeIs21()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("21", "GB");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressPlus, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsExpressSaver_WhenCountryIsInternational_AndTransitCodeIs28()
        {
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("28", "GB");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpressSaver, mapping.UpsServiceType);
        }

        // Does not appear to be a valid service type for international - commenting out until can be confirmed
        //[Fact]
        //public void GetServiceByTransitCode_ReturnsUpsExpressEarlyAm_WhenCountryIsInternational_AndTransitCodeIs54()
        //{
        //    UpsServiceMapping mapping = testObject.GetServiceByTransitCode("54", "RU");

        //    Assert.Equal(UpsServiceType.UpsExpressEarlyAm, mapping.UpsServiceType);
        //}

        [Fact]
        public void GetServiceByTransitCode_ReturnsUpsCaWorldWideExpress_WhenCountryIsLowerCaseInternational_AndTransitCodeIs01()
        {
            // Just provide a valid trasit code and lower case country code for PR
            UpsServiceMapping mapping = testObject.GetServiceByTransitCode("01", "gb");

            Assert.Equal(UpsServiceType.UpsCaWorldWideExpress, mapping.UpsServiceType);
        }

        [Fact]
        public void GetServiceByTransitCode_ThrowsUpsException_WhenCountryIsInternational_AndTransitCodeIsNotFound()
        {
            Assert.Throws<UpsException>(() => testObject.GetServiceByTransitCode("This will not be found.", "FR"));
        }

        #endregion GetServiceByTransitCode tests
    }
}
