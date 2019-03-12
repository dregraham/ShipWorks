using System;
using System.Collections.Generic;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Amazon.SFP.RateGroupFilters;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon.RateGroupFilters
{
    public class AmazonSFPAvailableServiceTypeRateGroupFilterTest : IDisposable
    {
        private readonly AutoMock mock;
        Mock<ShipmentType> shipmentType;

        public AmazonSFPAvailableServiceTypeRateGroupFilterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentType = mock.CreateKeyedMockOf<ShipmentType>().For(ShipmentTypeCode.AmazonSFP);
        }

        [Fact]
        public void Filter_IncludesRateWithAvailableService()
        {
            mock.Mock<IAmazonSFPServiceTypeRepository>()
                .Setup(r => r.Get())
                .Returns(new List<AmazonServiceTypeEntity>{
                    new AmazonServiceTypeEntity()
                    {
                        AmazonServiceTypeID = 42,
                        ApiValue = "FEDEX_PTP_SECOND_DAY_AM"
                    }
                });

            shipmentType.Setup(s => s.GetAvailableServiceTypes())
                .Returns(new[] { 42 });

            RateResult rate = new RateResult("Foo", "1", 1,
                new AmazonRateTag { ShippingServiceId = "FEDEX_PTP_SECOND_DAY_AM" });

            var testObject = mock.Create<AmazonSFPAvailableServiceTypeRateGroupFilter>();

            RateGroup rates = testObject.Filter(new RateGroup(new[] { rate }));
            Assert.Contains(rate, rates.Rates);
        }

        [Fact]
        public void Filter_ExcludesRateWithExcludedService()
        {
            mock.Mock<IAmazonSFPServiceTypeRepository>()
                .Setup(r => r.Get())
                .Returns(new List<AmazonServiceTypeEntity>{
                    new AmazonServiceTypeEntity()
                    {
                        AmazonServiceTypeID = 42,
                        ApiValue = "FEDEX_PTP_SECOND_DAY_AM"
                    }
                });

            shipmentType.Setup(s => s.GetAvailableServiceTypes())
                .Returns(new[] { 10 });

            RateResult rate = new RateResult("Foo", "1", 1,
                new AmazonRateTag { ShippingServiceId = "Ground" });

            var testObject = mock.Create<AmazonSFPAvailableServiceTypeRateGroupFilter>();

            RateGroup rates = testObject.Filter(new RateGroup(new[] { rate }));
            Assert.DoesNotContain(rate, rates.Rates);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}