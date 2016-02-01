using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
    public class CounterRatesInvalidStoreAddressFootnoteFilterTest
    {
        private CounterRatesInvalidStoreAddressFootnoteFilter testObject;

        private Mock<IRateFootnoteFactory> nonInvalidAddressFootnote;
        private Mock<IRateFootnoteFactory> anotherNonInvalidAddressFootnote;
        private Mock<IRateFootnoteFactory> aThirdInvalidAddressFootnote;

        private RateGroup rateGroup;

        public CounterRatesInvalidStoreAddressFootnoteFilterTest()
        {
            nonInvalidAddressFootnote = new Mock<IRateFootnoteFactory>();
            anotherNonInvalidAddressFootnote = new Mock<IRateFootnoteFactory>();
            aThirdInvalidAddressFootnote = new Mock<IRateFootnoteFactory>();

            rateGroup = new RateGroup(new List<RateResult>());
            rateGroup.AddFootnoteFactory(nonInvalidAddressFootnote.Object);
            rateGroup.AddFootnoteFactory(anotherNonInvalidAddressFootnote.Object);
            rateGroup.AddFootnoteFactory(aThirdInvalidAddressFootnote.Object);

            testObject = new CounterRatesInvalidStoreAddressFootnoteFilter();
        }

        [Fact]
        public void Filter_RemovesDuplicateCounterRatesInvalidStoreAddressFootnoteFactory_Test()
        {
            rateGroup.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Other));
            rateGroup.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Other));
            rateGroup.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Other));
            rateGroup.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Other));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            Assert.Equal(1, filteredRateGroup.FootnoteFactories.OfType<CounterRatesInvalidStoreAddressFootnoteFactory>().Count());
        }

        [Fact]
        public void Filter_RetainsOtherFootnotes_WhenDuplicateCounterRatesInvalidStoreAddressFootnoteFactoryAreRemoved_Test()
        {
            rateGroup.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Other));
            rateGroup.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Other));
            rateGroup.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Other));
            rateGroup.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Other));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            Assert.Equal(4, filteredRateGroup.FootnoteFactories.Count());
            Assert.Equal(3, filteredRateGroup.FootnoteFactories.Count(f => f.GetType() != typeof(CounterRatesInvalidStoreAddressFootnoteFactory)));
        }

        [Fact]
        public void Filter_RetainsCounterRatesInvalidStoreAddressFootnoteFactory_WhenOnlyOneExists_Test()
        {
            rateGroup.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Other));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            Assert.Equal(4, filteredRateGroup.FootnoteFactories.Count());
            Assert.Equal(1, filteredRateGroup.FootnoteFactories.OfType<CounterRatesInvalidStoreAddressFootnoteFactory>().Count());
        }

        [Fact]
        public void Filter_RetainsOtherFootnoteFactories_WhenThereAreNotAnyCounterRatesInvalidStoreAddressFootnoteFactories_Test()
        {
            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            Assert.Equal(3, filteredRateGroup.FootnoteFactories.Count());
            Assert.Equal(3, filteredRateGroup.FootnoteFactories.Count(f => f.GetType() != typeof(CounterRatesInvalidStoreAddressFootnoteFactory)));
        }
    }
}
