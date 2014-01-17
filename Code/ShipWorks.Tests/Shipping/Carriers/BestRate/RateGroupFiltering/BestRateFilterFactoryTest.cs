using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
    [TestClass]
    public class BestRateFilterFactoryTest
    {
        private readonly BestRateFilterFactory testObject;

        private readonly ShipmentEntity shipment;

        public BestRateFilterFactoryTest()
        {
            testObject = new BestRateFilterFactory();

            shipment = new ShipmentEntity
            {
                BestRate = new BestRateShipmentEntity
                {
                    ServiceLevel = (int)ServiceLevelType.Anytime
                }
            };
        }

        [TestMethod]
        public void CreateFilters_ReturnsOneFilter_Test()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.AreEqual(2, filters.Count());
        }

        [TestMethod]
        public void CreateFilters_ContainsRateGroupFilter_Test()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.AreEqual(1, filters.OfType<RateGroupFilter>().Count());
        }

        [TestMethod]
        public void CreateFilters_ContainsExpress1PromotionFootnoteFilter_Test()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.AreEqual(1, filters.OfType<BestRateExpress1PromotionFootnoteFilter>().Count());
        }
    }
}
