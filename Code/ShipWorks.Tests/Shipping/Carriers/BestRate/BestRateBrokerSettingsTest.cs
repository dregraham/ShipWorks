using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    [TestClass]
    public class BestRateBrokerSettingsTest
    {
        private BestRateBrokerSettings testObject;

        private ShippingSettingsEntity settings;
        private List<IBestRateShippingBroker> brokers;
        
        [TestInitialize]
        public void Initialize()
        {
            settings = new ShippingSettingsEntity();
            brokers = new List<IBestRateShippingBroker>();
            

            settings.BestRateExcludedTypes = new int[0];
            settings.ActivatedTypes = new int[]
            {
                (int)ShipmentTypeCode.Express1Endicia,
                (int)ShipmentTypeCode.Express1Usps
            };

            testObject = new BestRateBrokerSettings(settings, null);
            testObject.EnabledShipmentTypeCodes = EnumHelper.GetEnumList<ShipmentTypeCode>().Select(x => x.Value);
        }

        [TestMethod]
        public void CheckExpress1Rates_ReturnsFalse_StampsExpress1EnabledAndNoAccount_Test()
        {
            Assert.AreEqual(false, testObject.CheckExpress1Rates(new UspsShipmentType()));
        }

        [TestMethod]
        public void CheckExpress1Rates_ReturnsFalse_StampsExpress1IsDisabledForStampsInBestRates_Test()
        {
            settings.BestRateExcludedTypes = new[] { (int)ShipmentTypeCode.Express1Usps };
            Assert.AreEqual(false, testObject.CheckExpress1Rates(new UspsShipmentType()));
        }

        [TestMethod]
        public void CheckExpress1Rates_ReturnsFalse_StampsExpress1IsDisabledForStampsInSettings_Test()
        {
            testObject.EnabledShipmentTypeCodes =
                EnumHelper.GetEnumList<ShipmentTypeCode>()
                    .Select(x => x.Value)
                    .Where(x => x != ShipmentTypeCode.Express1Usps);
            Assert.AreEqual(false, testObject.CheckExpress1Rates(new UspsShipmentType()));
        }

        [TestMethod]
        public void CheckExpress1Rates_ReturnsFalse_StampsExpress1IsEnabledAndAccountExists_Test()
        {
            brokers.Add(new Express1UspsBestRateBroker());
            Assert.AreEqual(false, testObject.CheckExpress1Rates(new UspsShipmentType()));
        }

        [TestMethod]
        public void CheckExpress1Rates_ReturnsFalse_EndiciaExpress1EnabledAndNoAccount_Test()
        {
            Assert.AreEqual(false, testObject.CheckExpress1Rates(new EndiciaShipmentType()));
        }

        [TestMethod]
        public void CheckExpress1Rates_ReturnsFalse_EndiciaExpress1IsDisabledForEndiciaInBestRates_Test()
        {
            settings.BestRateExcludedTypes = new[] { (int)ShipmentTypeCode.Express1Endicia };
            Assert.AreEqual(false, testObject.CheckExpress1Rates(new EndiciaShipmentType()));
        }

        [TestMethod]
        public void CheckExpress1Rates_ReturnsFalse_EndiciaExpress1IsDisabledForEndiciaInSettings_Test()
        {
            testObject.EnabledShipmentTypeCodes =
                EnumHelper.GetEnumList<ShipmentTypeCode>()
                    .Select(x => x.Value)
                    .Where(x => x != ShipmentTypeCode.Express1Endicia);
            Assert.AreEqual(false, testObject.CheckExpress1Rates(new EndiciaShipmentType()));
        }

        [TestMethod]
        public void CheckExpress1Rates_ReturnsFalse_EndiciaExpress1IsEnabledAndAccountExists_Test()
        {
            brokers.Add(new Express1EndiciaBestRateBroker());
            Assert.AreEqual(false, testObject.CheckExpress1Rates(new EndiciaShipmentType()));
        }

        [TestMethod]
        public void IsMailInnovationsAvailable_ReturnsTrue_OltEnabled_Test()
        {
            settings.UpsMailInnovationsEnabled = true;
            Assert.AreEqual(true, testObject.IsMailInnovationsAvailable(new UpsOltShipmentType()));
        }

        [TestMethod]
        public void IsMailInnovationsAvailable_ReturnsFalse_OltDisabled_Test()
        {
            Assert.AreEqual(false, testObject.IsMailInnovationsAvailable(new UpsOltShipmentType()));            
        }

        [TestMethod]
        public void IsMailInnovationsAvailable_ReturnsTrue_WorldShipEnabled_Test()
        {
            settings.WorldShipMailInnovationsEnabled = true;
            Assert.AreEqual(true, testObject.IsMailInnovationsAvailable(new WorldShipShipmentType()));
        }

        [TestMethod]
        public void IsMailInnovationsAvailable_ReturnsFalse_WorldShipDisabled_Test()
        {
            Assert.AreEqual(false, testObject.IsMailInnovationsAvailable(new WorldShipShipmentType()));
        }

    }
}