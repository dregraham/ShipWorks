﻿using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Tests.Shared;
using System;
using ShipWorks.Shipping.Carriers.Dhl.API;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressLabelServiceTest
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly Mock<IDhlExpressLabelClientFactory> labelClientFactory;
        private readonly Mock<IDhlExpressLabelClient> labelClient;
        private readonly DhlExpressLabelService testObject;

        public DhlExpressLabelServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };

            labelClient = mock.Mock<IDhlExpressLabelClient>();
            labelClientFactory = mock.Mock<IDhlExpressLabelClientFactory>();
            labelClientFactory.Setup(l => l.Create(shipment)).Returns(labelClient);

            testObject = mock.Create<DhlExpressLabelService>();
        }

        [Fact]
        public void Create_ThrowsShippingException_WhenClientThrowsException()
        {
            labelClient.Setup(c => c.Create(shipment)).Throws(new Exception("Stuff Broke"));

            Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment));
        }

        [Fact]
        public void Create_DelegatesToLabelClientFactoryForLabelClient()
        {
            testObject.Create(shipment);

            labelClientFactory.Verify(l => l.Create(shipment));
        }

        [Fact]
        public void Create_DelegatesToLabelClientWithShipment()
        {
            testObject.Create(shipment);

            labelClient.Verify(l => l.Create(shipment));
        }

        [Fact]
        public void Void_DelegatesToLabelClientFactoryForLabelClient()
        {
            testObject.Void(shipment);

            labelClientFactory.Verify(l => l.Create(shipment));
        }

        [Fact]
        public void Void_DelegatesToLabelClientWithShipment()
        {
            testObject.Void(shipment);

            labelClient.Verify(l => l.Void(shipment));
        }
    }
}
