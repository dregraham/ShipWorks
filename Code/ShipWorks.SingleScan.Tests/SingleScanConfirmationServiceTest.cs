using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using Xunit;
using ShipWorks.Shipping.Services;

namespace ShipWorks.SingleScan.Tests
{
    public class SingleScanConfirmationServiceTest : IDisposable
    {
        readonly AutoMock mock;

        public SingleScanConfirmationServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ConfirmOrder_DelegatesToOrderConfirmationService()
        {
            var testObject = mock.Create<SingleScanConfirmationService>();
            mock.Mock<ISingleScanOrderConfirmationService>()
                .Setup(o => o.Confirm(10, 12, "foo"))
                .Returns(true);

            Assert.True(testObject.ConfirmOrder(10, 12, "foo"));

            mock.Mock<ISingleScanOrderConfirmationService>()
                .Verify(o => o.Confirm(10, 12, "foo"), Times.Once);
        }

        [Fact]
        public async void GetShipments_DelegatesToSingleScanShipmentConfirmationService()
        {
            var testObject = mock.Create<SingleScanConfirmationService>();
            var shipments = new List<ShipmentEntity>();

            mock.Mock<ISingleScanShipmentConfirmationService>()
                .Setup(s => s.GetShipments(42, "bar"))
                .ReturnsAsync(shipments);

            var shipmentResults = await testObject.GetShipments(42, "bar");

            Assert.Equal(shipments, (List<ShipmentEntity>) shipmentResults);
            mock.Mock<ISingleScanShipmentConfirmationService>()
                .Verify(s => s.GetShipments(42, "bar"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}