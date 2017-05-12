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

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.ServiceFilters
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
                UpsServiceType.UpsNextDayAir
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

        [Fact]
        public void GetEligibleServices_ReturnsServicesWithoutNextDayAirAM_WhenShipmentHasDeliveryConfirmation()
        {
            var testObject = mock.Create<ValueAddServiceFilter>();

            var shipment = Create.Shipment().AsUps().Build();
            shipment.Ups.DeliveryConfirmation = (int) UpsDeliveryConfirmationType.NoSignature;

            var eligibleServiceTypes = testObject.GetEligibleServices(shipment.Ups, new[] { UpsServiceType.UpsGround, UpsServiceType.UpsNextDayAirAM }).ToList();

            Assert.Equal(UpsServiceType.UpsGround, eligibleServiceTypes.Single());
        }

        [Fact]
        public void GetEligibleServices_ReturnsNextDayAirAM_WhenShipmentHasVerbalConfirmationEnabled()
        {
            var testObject = mock.Create<ValueAddServiceFilter>();

            var shipment = Create.Shipment().AsUps().Build();
            shipment.Ups.Packages.Add(new UpsPackageEntity() {VerbalConfirmationEnabled = true});

            var eligibleServiceTypes = testObject.GetEligibleServices(shipment.Ups, new[] { UpsServiceType.UpsGround, UpsServiceType.UpsNextDayAirAM }).ToList();

            Assert.Equal(UpsServiceType.UpsNextDayAirAM, eligibleServiceTypes.Single());
        }

        [Fact]
        public void GetEligibleServices_RemovesUpsGround_WhenShipmentHasCodEnabled()
        {
            ValueAddServiceFilter testObject = mock.Create<ValueAddServiceFilter>();

            ShipmentEntity shipment = Create.Shipment().AsUps().Build();
            shipment.Ups.CodEnabled = true;

            List<UpsServiceType> eligibleServiceTypes = testObject.GetEligibleServices(shipment.Ups, new[] { UpsServiceType.UpsGround, UpsServiceType.UpsNextDayAirAM }).ToList();
            
            Assert.DoesNotContain(UpsServiceType.UpsGround, eligibleServiceTypes);
        }

        [Fact]
        public void GetEligibleServices_RemovesUps3DaySelect_WhenShipmentHasCodEnabled()
        {
            ValueAddServiceFilter testObject = mock.Create<ValueAddServiceFilter>();

            ShipmentEntity shipment = Create.Shipment().AsUps().Build();
            shipment.Ups.CodEnabled = true;

            List<UpsServiceType> eligibleServiceTypes = testObject.GetEligibleServices(shipment.Ups, new[] { UpsServiceType.Ups3DaySelect, UpsServiceType.UpsGround, UpsServiceType.UpsNextDayAirAM }).ToList();

            Assert.DoesNotContain(UpsServiceType.Ups3DaySelect, eligibleServiceTypes);
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