using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class BestRateBrokerSettingsTest
    {
        private BestRateBrokerSettings testObject;

        private ShippingSettingsEntity settings;
        private List<IBestRateShippingBroker> brokers;

        public BestRateBrokerSettingsTest()
        {
            settings = new ShippingSettingsEntity();
            brokers = new List<IBestRateShippingBroker>();


            settings.BestRateExcludedTypes = Enumerable.Empty<ShipmentTypeCode>();
            settings.ActivatedTypes = new[]
            {
                ShipmentTypeCode.Express1Endicia,
                ShipmentTypeCode.Express1Usps
            };

            testObject = new BestRateBrokerSettings(settings, null);
            testObject.EnabledShipmentTypeCodes = EnumHelper.GetEnumList<ShipmentTypeCode>().Select(x => x.Value);
        }

        [Fact]
        public void CheckExpress1Rates_ReturnsFalse_UspsExpress1EnabledAndNoAccount()
        {
            Assert.Equal(false, testObject.CheckExpress1Rates(new UspsShipmentType()));
        }

        [Fact]
        public void CheckExpress1Rates_ReturnsFalse_UspsExpress1IsDisabledForUspsInBestRates()
        {
            settings.BestRateExcludedTypes = new[] { ShipmentTypeCode.Express1Usps };
            Assert.Equal(false, testObject.CheckExpress1Rates(new UspsShipmentType()));
        }

        [Fact]
        public void CheckExpress1Rates_ReturnsFalse_UspsExpress1IsDisabledForUspsInSettings()
        {
            testObject.EnabledShipmentTypeCodes =
                EnumHelper.GetEnumList<ShipmentTypeCode>()
                    .Select(x => x.Value)
                    .Where(x => x != ShipmentTypeCode.Express1Usps);
            Assert.Equal(false, testObject.CheckExpress1Rates(new UspsShipmentType()));
        }

        [Fact]
        public void CheckExpress1Rates_ReturnsFalse_UspsExpress1IsEnabledAndAccountExists()
        {
            brokers.Add(new Express1UspsBestRateBroker());
            Assert.Equal(false, testObject.CheckExpress1Rates(new UspsShipmentType()));
        }

        [Fact]
        public void CheckExpress1Rates_ReturnsFalse_EndiciaExpress1EnabledAndNoAccount()
        {
            Assert.Equal(false, testObject.CheckExpress1Rates(new EndiciaShipmentType()));
        }

        [Fact]
        public void CheckExpress1Rates_ReturnsFalse_EndiciaExpress1IsDisabledForEndiciaInBestRates()
        {
            settings.BestRateExcludedTypes = new[] { ShipmentTypeCode.Express1Endicia };
            Assert.Equal(false, testObject.CheckExpress1Rates(new EndiciaShipmentType()));
        }

        [Fact]
        public void CheckExpress1Rates_ReturnsFalse_EndiciaExpress1IsDisabledForEndiciaInSettings()
        {
            testObject.EnabledShipmentTypeCodes =
                EnumHelper.GetEnumList<ShipmentTypeCode>()
                    .Select(x => x.Value)
                    .Where(x => x != ShipmentTypeCode.Express1Endicia);
            Assert.Equal(false, testObject.CheckExpress1Rates(new EndiciaShipmentType()));
        }

        [Fact]
        public void CheckExpress1Rates_ReturnsFalse_EndiciaExpress1IsEnabledAndAccountExists()
        {
            brokers.Add(new Express1EndiciaBestRateBroker());
            Assert.Equal(false, testObject.CheckExpress1Rates(new EndiciaShipmentType()));
        }

        [Fact]
        public void IsMailInnovationsAvailable_ReturnsTrue_OltEnabled()
        {
            settings.UpsMailInnovationsEnabled = true;
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Assert.Equal(true, testObject.IsMailInnovationsAvailable(mock.Create<UpsOltShipmentType>()));
            }
        }

        [Fact]
        public void IsMailInnovationsAvailable_ReturnsFalse_OltDisabled()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Assert.Equal(false, testObject.IsMailInnovationsAvailable(mock.Create<UpsOltShipmentType>()));
            }
        }

        [Fact]
        public void IsMailInnovationsAvailable_ReturnsTrue_WorldShipEnabled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                WorldShipShipmentType shipmentType = mock.Create<WorldShipShipmentType>();

                settings.WorldShipMailInnovationsEnabled = true;
                Assert.Equal(true, testObject.IsMailInnovationsAvailable(shipmentType));
            }
        }

        [Fact]
        public void IsMailInnovationsAvailable_ReturnsFalse_WorldShipDisabled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                WorldShipShipmentType shipmentType = mock.Create<WorldShipShipmentType>();

                Assert.Equal(false, testObject.IsMailInnovationsAvailable(shipmentType));
            }
        }

    }
}