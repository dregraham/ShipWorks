using System;
using System.Collections.Generic;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.RateGroupFilters;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon.RateGroupFilters
{
    public class AmazonAvailableServiceTypeRateGroupFilterTest : IDisposable
    {
        private readonly AutoMock mock;

        public AmazonAvailableServiceTypeRateGroupFilterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Filter_IncludesRateWithAvailableService()
        {
            mock.FromFactory<IShipmentTypeManager>()
                .Mock(x => x.Get(ShipmentTypeCode.Amazon))
                .Setup(x => x.GetAvailableServiceTypes())
                .Returns(new List<int>{1});

            RateResult rate = new RateResult("Foo", "1", 1,
                new AmazonRateTag { ShippingServiceId = "FEDEX_PTP_SECOND_DAY_AM" });

            var testObject = mock.Create<AmazonAvailableServiceTypeRateGroupFilter>();

            RateGroup rates = testObject.Filter(new RateGroup(new[] { rate }));
            Assert.Contains(rate, rates.Rates);
        }

        [Fact]
        public void Filter_ExcludesRateWithExcludedService()
        {
            mock.FromFactory<IShipmentTypeManager>()
                .Mock(x => x.Get(ShipmentTypeCode.Amazon))
                .Setup(x => x.GetAvailableServiceTypes())
                .Returns(new List<int> { 1 });

            RateResult rate = new RateResult("Foo", "1", 1,
                new AmazonRateTag { ShippingServiceId = "Ground" });

            var testObject = mock.Create<AmazonAvailableServiceTypeRateGroupFilter>();

            RateGroup rates = testObject.Filter(new RateGroup(new[] { rate }));
            Assert.DoesNotContain(rate, rates.Rates);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}