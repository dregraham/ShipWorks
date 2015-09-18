﻿using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using System;

namespace ShipWorks.Shipping.Tests
{
    /// <summary>
    /// Additional methods for AutoMocks
    /// </summary>
    public static class AutoMockExtensions
    {
        /// <summary>
        /// Get an AutoMock that is loose and will try and return meaningful defaults instead of null
        /// </summary>
        /// <returns></returns>
        public static AutoMock GetLooseThatReturnsMocks() =>
            AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });

        /// <summary>
        /// Configure a shipment type that will be returned by an instance of IShipmentTypeFactory
        /// </summary>
        public static void WithShipmentTypeFromFactory(this AutoMock mock, Action<Mock<ShipmentType>> shipmentTypeConfiguration)
        {
            var shipmentTypeMock = mock.MockRepository.Create<ShipmentType>();
            shipmentTypeConfiguration(shipmentTypeMock);

            mock.Mock<IShipmentTypeFactory>()
                .Setup(x => x.Get(It.IsAny<ShipmentTypeCode>()))
                .Returns(shipmentTypeMock.Object);

            mock.Mock<IShipmentTypeFactory>()
                .Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentTypeMock.Object);
        }
    }
}
