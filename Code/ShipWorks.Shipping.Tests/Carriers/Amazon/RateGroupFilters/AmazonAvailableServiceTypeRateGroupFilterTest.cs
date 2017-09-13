﻿using System;
using System.Collections.Generic;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
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
            mock.Mock<IAmazonServiceTypeRepository>()
                .Setup(r => r.Get())
                .Returns(new List<AmazonServiceTypeEntity>{new AmazonServiceTypeEntity()
                {
                    ApiValue = "FEDEX_PTP_SECOND_DAY_AM"
                } });

            RateResult rate = new RateResult("Foo", "1", 1,
                new AmazonRateTag { ShippingServiceId = "FEDEX_PTP_SECOND_DAY_AM" });

            var testObject = mock.Create<AmazonAvailableServiceTypeRateGroupFilter>();

            RateGroup rates = testObject.Filter(new RateGroup(new[] { rate }));
            Assert.Contains(rate, rates.Rates);
        }

        [Fact]
        public void Filter_ExcludesRateWithExcludedService()
        {
            mock.Mock<IAmazonServiceTypeRepository>()
                .Setup(r => r.Get())
                .Returns(new List<AmazonServiceTypeEntity>{new AmazonServiceTypeEntity()
                {
                    ApiValue = "FEDEX_PTP_SECOND_DAY_AM"
                } });

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