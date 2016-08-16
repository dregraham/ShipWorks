using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
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

        [Fact]
        public void CreateFilters_ReturnsFiveFilters()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.Equal(5, filters.Count());
        }
        
        [Fact]
        public void CreateFilters_ContainsExpress1PromotionFootnoteFilter()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.Equal(1, filters.OfType<BestRateExpress1PromotionFootnoteFilter>().Count());
        }

        [Fact]
        public void CreateFilters_ContainsBestRateNonExistentShipmentTypeFootnoteFilter()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.Equal(1, filters.OfType<BestRateNonExistentShipmentTypeFootnoteFilter>().Count());
        }

        [Fact]
        public void CreateFilters_Express1PromoFilter_IsAfterNonExistentFootnoteFilter()
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
            
            Assert.True(nonExistentFilterIndex < promoFilterIndex);
        }

        [Fact]
        public void CreateFilters_ContainsCounterRatesInvalidStoreAddressFootnoteFilter()
        {
            IEnumerable<IRateGroupFilter> filters = testObject.CreateFilters(shipment);

            Assert.Equal(1, filters.OfType<CounterRatesInvalidStoreAddressFootnoteFilter>().Count());
        }
    }
}
