using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.ServiceFilters
{
    public class DimensionServiceFilterTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DimensionServiceFilter testObject;

        public DimensionServiceFilterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new DimensionServiceFilter();
        }

        [Theory]
        [InlineData(1, 2, 3, false)]
        [InlineData(5, 5, 5, false)]
        [InlineData(10, 10, 10, false)]
        [InlineData(1, 2, 109, true)]
        [InlineData(10, 10, 100, false)]
        [InlineData(30, 10, 100, true)]
        [InlineData(80, 20, 20, false)]
        public void GetEligibleServices_ReturnsEmptyList_WhenPackageDimensionsAreNotValidForUPS(double length, double width, double height, bool emptyResult)
        {
            IEnumerable<UpsServiceType> services = new List<UpsServiceType>()
            {
                UpsServiceType.Ups2DayAir,
                UpsServiceType.UpsMailInnovationsExpedited,
                UpsServiceType.Ups3DaySelect
            };

            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Packages = {new UpsPackageEntity {DimsLength = length, DimsWidth = width, DimsHeight = height}}
            };

            IEnumerable<UpsServiceType> result = testObject.GetEligibleServices(shipment, services);
            
            Assert.Equal(emptyResult, result.None());
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(151, true)]
        public void GetEligibleServices_ReturnsEmptyList_WhenPackageWeightAreNotValidForUPS(double weight, bool emptyResult)
        {
            IEnumerable<UpsServiceType> services = new List<UpsServiceType>()
            {
                UpsServiceType.Ups2DayAir,
                UpsServiceType.UpsMailInnovationsExpedited,
                UpsServiceType.Ups3DaySelect
            };

            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Packages = {new UpsPackageEntity {DimsLength = 7, DimsWidth = 7, DimsHeight = 7, Weight = weight}}
            };

            IEnumerable<UpsServiceType> result = testObject.GetEligibleServices(shipment, services);

            Assert.Equal(!emptyResult, result.Any());
        }

        [Fact]
        public void GetEligibleServices_ReturnsEmptyList_WhenPackageDimensionsAreNotValidForUPSWithMultiplePackages()
        {
            IEnumerable<UpsServiceType> services = new List<UpsServiceType>()
            {
                UpsServiceType.Ups2DayAir,
                UpsServiceType.UpsMailInnovationsExpedited,
                UpsServiceType.Ups3DaySelect
            };

            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Packages =
                {
                    new UpsPackageEntity {DimsLength = 12, DimsWidth = 19, DimsHeight = 7},
                    new UpsPackageEntity {DimsLength = 22, DimsWidth = 98, DimsHeight = 100},
                    new UpsPackageEntity {DimsLength = 22, DimsWidth = 98, DimsHeight = 106}
                }
            };

            IEnumerable<UpsServiceType> result = testObject.GetEligibleServices(shipment, services);


            Assert.Empty(result);
        }

        [Fact]
        public void GetEligibleServices_ReturnsServiceList_WhenPackageDimensionsAreValidForUPSWithMultiplePackages()
        {
            IEnumerable<UpsServiceType> services = new List<UpsServiceType>()
            {
                UpsServiceType.Ups2DayAir,
                UpsServiceType.UpsMailInnovationsExpedited,
                UpsServiceType.Ups3DaySelect
            };

            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Packages =
                {
                    new UpsPackageEntity {DimsLength = 12, DimsWidth = 19, DimsHeight = 7},
                    new UpsPackageEntity {DimsLength = 22, DimsWidth = 20, DimsHeight = 2},
                    new UpsPackageEntity {DimsLength = 22, DimsWidth = 20, DimsHeight = 2}
                }
            };

            IEnumerable<UpsServiceType> result = testObject.GetEligibleServices(shipment, services);


            Assert.Equal(services, result);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}