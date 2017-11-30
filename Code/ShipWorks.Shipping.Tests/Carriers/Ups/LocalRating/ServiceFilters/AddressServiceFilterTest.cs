﻿using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.ServiceFilters
{
    public class 
        AddressServiceFilterTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly List<UpsServiceType> upsServices;

        public AddressServiceFilterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            upsServices = new List<UpsServiceType>()
            {
                UpsServiceType.Ups2DayAir,
                UpsServiceType.Ups2DayAirAM,
                UpsServiceType.Ups3DaySelect,
                UpsServiceType.UpsGround
            };
        }

        [Fact]
        public void GetEligibleServices_ReturnsServicesWithoutUps2DayAirAM_WhenShipmentIsResidential()
        {
            mock.Mock<IResidentialDeterminationService>()
                .Setup(r => r.IsResidentialAddress(It.IsAny<ShipmentEntity>()))
                .Returns(true);
            UpsShipmentEntity shipment = new UpsShipmentEntity() { Shipment = new ShipmentEntity() };

            AddressServiceFilter testObject = mock.Create<AddressServiceFilter>();
            IEnumerable<UpsServiceType> result = testObject.GetEligibleServices(shipment, upsServices);

            Assert.DoesNotContain(UpsServiceType.Ups2DayAirAM, result);
        }

        [Fact]
        public void GetEligibleServices_ReturnsServicesWithUps2DayAirAM_WhenShipmentIsCommercial()
        {
            mock.Mock<UpsOltShipmentType>()
                .Setup(x => x.IsDomestic(AnyIShipment))
                .Returns(true);
            mock.Mock<IResidentialDeterminationService>()
                .Setup(r => r.IsResidentialAddress(It.IsAny<ShipmentEntity>()))
                .Returns(false);
            UpsShipmentEntity shipment = new UpsShipmentEntity { Shipment = new ShipmentEntity() };

            AddressServiceFilter testObject = mock.Create<AddressServiceFilter>();
            IEnumerable<UpsServiceType> result = testObject.GetEligibleServices(shipment, upsServices);

            Assert.Contains(UpsServiceType.Ups2DayAirAM, result);
        }

        [Fact]
        public void GetEligibleServices_EmptyList_WhenShipmentIsInternational()
        {
            mock.Mock<IResidentialDeterminationService>()
                .Setup(r => r.IsResidentialAddress(It.IsAny<ShipmentEntity>()))
                .Returns(true);
            UpsShipmentEntity shipment = new UpsShipmentEntity()
            {
                Shipment = new ShipmentEntity()
                {
                    OriginCountryCode = "US",
                    ShipCountryCode = "CA"
                }
            };

            AddressServiceFilter testObject = mock.Create<AddressServiceFilter>();
            IEnumerable<UpsServiceType> result = testObject.GetEligibleServices(shipment, upsServices);

            Assert.Empty(result);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}