﻿using System;
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
        public void CreateFilters_ReturnsFiveFilters_Test()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.AreEqual(4, filters.Count());
        }
        
        [TestMethod]
        public void CreateFilters_ContainsExpress1PromotionFootnoteFilter_Test()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.AreEqual(1, filters.OfType<BestRateExpress1PromotionFootnoteFilter>().Count());
        }

        [TestMethod]
        public void CreateFilters_ContainsBestRateNonExistentShipmentTypeFootnoteFilter_Test()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.AreEqual(1, filters.OfType<BestRateNonExistentShipmentTypeFootnoteFilter>().Count());
        }

        [TestMethod]
        public void CreateFilters_Express1PromoFilter_IsAfterNonExistentFootnoteFilter_Test()
        {
            List<IRateGroupFilter> filters = testObject.CreateFilters(shipment).ToList();
            int nonExistentFilterIndex = 0;
            int promoFilterIndex = 0;

            for (int i = 0; i < filters.Count; i++)
            {
                if (filters[i].GetType() == typeof(BestRateNonExistentShipmentTypeFootnoteFilter))
                {
                    nonExistentFilterIndex = i;
                }
                else if (filters[i].GetType() == typeof(BestRateExpress1PromotionFootnoteFilter))
                {
                    promoFilterIndex = i;
                }
            }
            
            Assert.IsTrue(nonExistentFilterIndex < promoFilterIndex);
        }

        [TestMethod]
        public void CreateFilters_ContainsCounterRatesInvalidStoreAddressFootnoteFilter_Test()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.AreEqual(1, filters.OfType<CounterRatesInvalidStoreAddressFootnoteFilter>().Count());
        }
    }
}
