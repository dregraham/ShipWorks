using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using System;
using System.Linq;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonAllowedCarriersRateGroupFilterTest : IDisposable
    {
        AutoMock mock;

        public AmazonAllowedCarriersRateGroupFilterTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Theory]
        [InlineData("FOO")]
        [InlineData("BAR")]
        public void Filter_DoesNotIncludeRate_WhenRateIsFromDisallowedCarrier(string carrierName)
        {
            RateResult rate = new RateResult("Foo", "1", 1, new AmazonRateTag { CarrierName = carrierName });
            AmazonAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(new RateGroup(new[] { rate }));
            Assert.DoesNotContain(rate, rates.Rates);
        }

        [Theory]
        [InlineData("STAMPS_DOT_COM")]
        [InlineData("FEDEX")]
        [InlineData("UPS")]
        [InlineData("stamps_dot_com")]
        [InlineData("fedeX")]
        [InlineData("uPs")]
        public void Filter_IncludesRate_WhenRateIsFromAllowedCarrier(string carrierName)
        {
            RateResult rate = new RateResult("Foo", "1", 1, new AmazonRateTag { CarrierName = carrierName });
            AmazonAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(new RateGroup(new[] { rate }));
            Assert.Contains(rate, rates.Rates);
        }

        [Fact]
        public void Filter_DoesNotAffectOtherFootnotes()
        {
            IRateFootnoteFactory factory = mock.Create<IRateFootnoteFactory>();
            RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
            rateGroup.AddFootnoteFactory(factory);

            AmazonAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(rateGroup);

            Assert.Contains(factory, rates.FootnoteFactories);
        }

        [Fact]
        public void Filter_CopiesCarrierAndOutOfDate()
        {
            AmazonAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(new RateGroup(Enumerable.Empty<RateResult>()) { Carrier = ShipmentTypeCode.iParcel, OutOfDate = true });

            Assert.Equal(ShipmentTypeCode.iParcel, rates.Carrier);
            Assert.True(rates.OutOfDate);
        }

        [Fact]
        public void Filter_RemovesCarriersFromTandCFootnoteFactory_WhenFactoryIsInList()
        {
            AmazonShipmentType shipmentType = mock.Create<AmazonShipmentType>();
            var factory = new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(new[] { "FOO", "UPS" });

            RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
            rateGroup.AddFootnoteFactory(factory);

            AmazonAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(rateGroup);

            var alteredFactory = rates.FootnoteFactories.OfType<AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory>().Single();
            Assert.Contains("UPS", alteredFactory.CarrierNames);
            Assert.DoesNotContain("FOO", alteredFactory.CarrierNames);
        }

        [Fact]
        public void Filter_RemovesTandCFootnoteFactory_WhenFilteredCarrierListIsEmpty()
        {
            AmazonShipmentType shipmentType = mock.Create<AmazonShipmentType>();
            var factory = new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(new[] { "FOO" });

            RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
            rateGroup.AddFootnoteFactory(factory);

            AmazonAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(rateGroup);

            Assert.Empty(rates.FootnoteFactories.OfType<AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory>());
        }

        public void Dispose() => mock?.Dispose();
    }
}
