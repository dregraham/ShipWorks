using System;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.SFP.RateGroupFilters;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon.RateGroupFilters
{
    public class AmazonSFPAllowedCarriersRateGroupFilterTest : IDisposable
    {
        readonly AutoMock mock;

        public AmazonSFPAllowedCarriersRateGroupFilterTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Theory]
        [InlineData("FOO")]
        [InlineData("BAR")]
        public void Filter_DoesNotIncludeRate_WhenRateIsFromDisallowedCarrier(string carrierName)
        {
            RateResult rate = new RateResult("Foo", "1", 1, new AmazonRateTag { CarrierName = carrierName });
            AmazonSFPAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonSFPAllowedCarriersRateGroupFilter>();
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
        [InlineData("oNtRaC")]
        public void Filter_IncludesRate_WhenRateIsFromAllowedCarrier(string carrierName)
        {
            RateResult rate = new RateResult("Foo", "1", 1, new AmazonRateTag { CarrierName = carrierName });
            AmazonSFPAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonSFPAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(new RateGroup(new[] { rate }));
            Assert.Contains(rate, rates.Rates);
        }

        [Fact]
        public void Filter_DoesNotAffectOtherFootnotes()
        {
            IRateFootnoteFactory factory = mock.Build<IRateFootnoteFactory>();
            RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
            rateGroup.AddFootnoteFactory(factory);

            AmazonSFPAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonSFPAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(rateGroup);

            Assert.Contains(factory, rates.FootnoteFactories);
        }

        [Fact]
        public void Filter_CopiesCarrierAndOutOfDate()
        {
            AmazonSFPAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonSFPAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(new RateGroup(Enumerable.Empty<RateResult>()) { Carrier = ShipmentTypeCode.iParcel, OutOfDate = true });

            Assert.Equal(ShipmentTypeCode.iParcel, rates.Carrier);
            Assert.True(rates.OutOfDate);
        }

        [Fact]
        public void Filter_RemovesCarriersFromTandCFootnoteFactory_WhenFactoryIsInList()
        {
            var factory = new AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory(new[] { "FOO", "UPS", "fedeX" });

            RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
            rateGroup.AddFootnoteFactory(factory);

            AmazonSFPAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonSFPAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(rateGroup);

            var alteredFactory = rates.FootnoteFactories.OfType<AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory>().Single();
            Assert.Contains("UPS", alteredFactory.CarrierNames);
            Assert.Contains("fedeX", alteredFactory.CarrierNames);
            Assert.DoesNotContain("FOO", alteredFactory.CarrierNames);
        }

        [Fact]
        public void Filter_RemovesTandCFootnoteFactory_WhenFilteredCarrierListIsEmpty()
        {
            var factory = new AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory(new[] { "FOO" });

            RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
            rateGroup.AddFootnoteFactory(factory);

            AmazonSFPAllowedCarriersRateGroupFilter testObject = mock.Create<AmazonSFPAllowedCarriersRateGroupFilter>();
            RateGroup rates = testObject.Filter(rateGroup);

            Assert.Empty(rates.FootnoteFactories.OfType<AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory>());
        }

        public void Dispose() => mock?.Dispose();
    }
}
