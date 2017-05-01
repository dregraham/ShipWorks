using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class ValueAddServiceFilterTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly List<UpsServiceType> allServiceTypes;

        public ValueAddServiceFilterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            allServiceTypes = new List<UpsServiceType>()
            {
                UpsServiceType.UpsGround,
                UpsServiceType.Ups3DaySelect,
                UpsServiceType.Ups2DayAir,
                UpsServiceType.Ups2DayAirAM,
                UpsServiceType.UpsNextDayAirSaver,
                UpsServiceType.UpsNextDayAir,
            };
        }

        [Fact]
        public void GetEligibleServices_OnlyReturnsExpectedServices_WhenSaturdayDelivery()
        {
            var testObject = mock.Create<ValueAddServiceFilter>();

            var shipment = Create.Shipment().AsUps(x => x.Set(ups => ups.SaturdayDelivery = true))
                .Build();

            var eligibleServiceTypes = testObject.GetEligibleServices(shipment.Ups, allServiceTypes).ToList();

            List<UpsServiceType> expectedServiceTypes = new List<UpsServiceType>()
            {
                UpsServiceType.UpsGround,
                UpsServiceType.Ups3DaySelect,
                UpsServiceType.Ups2DayAir,
                UpsServiceType.UpsNextDayAir,
            };

            ConfirmExpectedServices(expectedServiceTypes, eligibleServiceTypes);
        }

        [Fact]
        public void GetEligibleServices_OnlyReturnsExpectedServices_WhenReturn()
        {
            var testObject = mock.Create<ValueAddServiceFilter>();

            var shipment = Create.Shipment().AsUps().Set(s => s.ReturnShipment = true)
                .Build();

            var eligibleServiceTypes = testObject.GetEligibleServices(shipment.Ups, allServiceTypes).ToList();

            List<UpsServiceType> expectedServiceTypes = new List<UpsServiceType>()
            {
                UpsServiceType.UpsGround,
                UpsServiceType.Ups3DaySelect,
                UpsServiceType.Ups2DayAir,
                UpsServiceType.UpsNextDayAir,
            };

            ConfirmExpectedServices(expectedServiceTypes, eligibleServiceTypes);
        }

        [Fact]
        public void GetEligibleServices_ReturnsAllServices_WhenNotReturnOrSaturdayDelivery()
        {
            var testObject = mock.Create<ValueAddServiceFilter>();

            var shipment = Create.Shipment().AsUps().Build();

            var eligibleServiceTypes = testObject.GetEligibleServices(shipment.Ups, allServiceTypes.ToList()).ToList();

            ConfirmExpectedServices(allServiceTypes, eligibleServiceTypes);
        }

        [Fact]
        public void GetEligibleServices_ReturnsSingleService_WhenOnlyGivenOneService()
        {
            var testObject = mock.Create<ValueAddServiceFilter>();

            var shipment = Create.Shipment().AsUps().Build();

            var eligibleServiceTypes = testObject.GetEligibleServices(shipment.Ups, new []{UpsServiceType.UpsGround}).ToList();

            Assert.Equal(UpsServiceType.UpsGround, eligibleServiceTypes.Single());
        }

        private static void ConfirmExpectedServices(IReadOnlyCollection<UpsServiceType> expectedServiceTypes,
            IReadOnlyCollection<UpsServiceType> eligibleServiceTypes)
        {
            Assert.Equal(expectedServiceTypes.Count, eligibleServiceTypes.Count);
            Assert.True(new HashSet<UpsServiceType>(expectedServiceTypes).SetEquals(eligibleServiceTypes));
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}