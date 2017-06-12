using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Policies;
using Xunit;

namespace ShipWorks.Tests.Shipping.Policies
{
    public class BestRateUpsRestrictionShippingPolicyTest
    {
        private BestRateUpsRestrictionShippingPolicy testObject;

        public BestRateUpsRestrictionShippingPolicyTest()
        {
            testObject = new BestRateUpsRestrictionShippingPolicy();
        }

        [Fact]
        public void Configure_ThrowsArgumentException_WhenValueIsNotTrueOrFalse()
        {
            var ex = Assert.Throws<ArgumentException>(() => testObject.Configure("nottrueorfalse"));
            Assert.Equal("Unknown configuration value 'nottrueorfalse.' Expected 'true' or 'false.'\r\nParameter name: configuration", ex.Message);
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_WhenTargetIsListOfUpsRatingMethod()
        {
            testObject.Configure("true");

            Assert.True(testObject.IsApplicable(new List<UpsRatingMethod>()));
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenTargetIsNotListOfUpsRatingMethod()
        {
            testObject.Configure("true");

            Assert.False(testObject.IsApplicable("TheWrongType"));
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenConfiguredWithFalse()
        {
            testObject.Configure("false");

            Assert.False(testObject.IsApplicable(new List<UpsRatingMethod>()));
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenConfiguredWithTrue()
        {
            testObject.Configure("true");

            Assert.True(testObject.IsApplicable(new List<UpsRatingMethod>()));
        }

        [Fact]
        public void Apply_ThrowsArgumentNullException_WhenTargetIsNull()
        {
            testObject.Configure("true");
            Assert.Throws<ArgumentNullException>(() => testObject.Apply(null));
        }

        [Fact]
        public void Apply_ThrowsArgumentException_WhenTargetIsNotApplicable()
        {
            testObject.Configure("true");
            ArgumentException ex = Assert.Throws<ArgumentException>(() => testObject.Apply("not applicable type"));
            Assert.Equal("target not of type ListList<UpsRatingMethod>\r\nParameter name: target", ex.Message);
        }

        [Fact]
        public void Apply_RemovesAllButLocalOnly_WhenRestricted()
        {
            testObject.Configure("true");

            List<UpsRatingMethod> services = new List<UpsRatingMethod>()
            {
                UpsRatingMethod.LocalOnly,
                UpsRatingMethod.ApiOnly,
                UpsRatingMethod.LocalWithApiFailover
            };

            testObject.Apply(services);

            Assert.Single(services);
            Assert.Contains(UpsRatingMethod.LocalOnly, services);
        }

        [Fact]
        public void Apply_DoesNotRemoveAnything_WhenNotRestricted()
        {
            testObject.Configure("false");

            List<UpsRatingMethod> services = new List<UpsRatingMethod>()
            {
                UpsRatingMethod.LocalOnly,
                UpsRatingMethod.ApiOnly,
                UpsRatingMethod.LocalWithApiFailover
            };

            testObject.Apply(services);

            Assert.Equal(3, services.Count);
            Assert.Contains(UpsRatingMethod.LocalOnly, services);
            Assert.Contains(UpsRatingMethod.ApiOnly, services);
            Assert.Contains(UpsRatingMethod.LocalWithApiFailover, services);
        }
    }
}
