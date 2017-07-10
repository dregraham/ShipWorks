using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class AutoWeighServicePipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly SingleScanFilterUpdateCompleteMessage singleScanFilterUpdateCompleteMessage;
        private bool autoPrint = false;
        private int filterCount = 0;

        public AutoWeighServicePipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            mock.Mock<ISingleScanAutomationSettings>()
                .Setup(s => s.IsAutoPrintEnabled())
                .Returns(() => autoPrint);

            mock.Mock<IFilterNodeContentEntity>()
                .Setup(f => f.Count)
                .Returns(() => filterCount);

 

            singleScanFilterUpdateCompleteMessage = new SingleScanFilterUpdateCompleteMessage(this,
                mock.Mock<IFilterNodeContentEntity>().Object, 42);
        }

        [Fact]
        public void CallsApplyWeight_WhenAutoPrintIsOff_AndFilterNodeContentCountIs1()
        {
            autoPrint = false;
            filterCount = 1;

            List<ShipmentEntity> shipments = new List<ShipmentEntity>() {new ShipmentEntity(5)};
            mock.Mock<IOrderLoader>()
                .Setup(
                    o =>
                        o.LoadAsync(It.Is<IEnumerable<long>>(orderIds => orderIds.SingleOrDefault() == 42),
                            ProgressDisplayOptions.NeverShow, false, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, null,
                    shipments));

            var testObject = mock.Create<AutoWeighServicePipeline>();
            testObject.InitializeForCurrentSession();
            testMessenger.Send(singleScanFilterUpdateCompleteMessage);

            mock.Mock<IAutoWeighService>().Verify(a=>a.ApplyWeight(shipments, It.IsAny<ITrackedEvent>()), Times.Once);
        }

        [Fact]
        public void SendShipmentChangedMessage_WhenAutoPrintIsOff_AndFilterNodeContentCountIs1()
        {
            autoPrint = false;
            filterCount = 1;
            ShipmentEntity shipment = new ShipmentEntity(5);

            List<ShipmentEntity> shipments = new List<ShipmentEntity>() { shipment };
            mock.Mock<IOrderLoader>()
                .Setup(
                    o =>
                        o.LoadAsync(It.Is<IEnumerable<long>>(orderIds => orderIds.SingleOrDefault() == 42),
                            ProgressDisplayOptions.NeverShow, false, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, null,
                    shipments));

            var shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.SetupGet(s => s.Shipment)
                .Returns(shipment);

            mock.Mock<IShippingManager>()
                .Setup(m => m.GetShipmentAdapter(shipment))
                .Returns(shipmentAdapter.Object);


            var testObject = mock.Create<AutoWeighServicePipeline>(); ;
            testObject.InitializeForCurrentSession();
            testMessenger.Send(singleScanFilterUpdateCompleteMessage);

            Assert.Equal(shipment,
                testMessenger.SentMessages.OfType<ShipmentChangedMessage>().Single().ShipmentAdapter.Shipment);
        }

        [Fact]
        public void DoesNotWeighShipment_WhenAutoPrintIsOn_AndFilterNodeContentCountIs1()
        {
            autoPrint = true;
            filterCount = 1;

            List<ShipmentEntity> shipments = new List<ShipmentEntity>() { new ShipmentEntity(5) };
            mock.Mock<IOrderLoader>()
                .Setup(
                    o =>
                        o.LoadAsync(It.Is<IEnumerable<long>>(orderIds => orderIds.SingleOrDefault() == 42),
                            ProgressDisplayOptions.NeverShow, false, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, null,
                    shipments));

            var testObject = mock.Create<AutoWeighServicePipeline>();
            testObject.InitializeForCurrentSession();
            testMessenger.Send(singleScanFilterUpdateCompleteMessage);

            mock.Mock<IAutoWeighService>().Verify(a => a.ApplyWeight(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<ITrackedEvent>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void DoesNotWeighShipment_WhenAutoPrintIsOff_AndFilterNodeContentCountIsNotOne(int filterCount)
        {
            autoPrint = false;
            
            List<ShipmentEntity> shipments = new List<ShipmentEntity>() { new ShipmentEntity(5) };
            mock.Mock<IOrderLoader>()
                .Setup(
                    o =>
                        o.LoadAsync(It.Is<IEnumerable<long>>(orderIds => orderIds.SingleOrDefault() == 42),
                            ProgressDisplayOptions.NeverShow, false, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, null,
                    shipments));

            var testObject = mock.Create<AutoWeighServicePipeline>();
            testObject.InitializeForCurrentSession();
            testMessenger.Send(singleScanFilterUpdateCompleteMessage);

            mock.Mock<IAutoWeighService>().Verify(a => a.ApplyWeight(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<ITrackedEvent>()), Times.Never);
        }

        [Fact]
        public void DoesNotSendShipmentChangedMessage_WhenAutoPrintIsOn_AndFilterNodeContentCountIs1_AndShipmentsAreNotDirty()
        {
            autoPrint = false;
            filterCount = 1;
            ShipmentEntity shipment = new ShipmentEntity();

            List<ShipmentEntity> shipments = new List<ShipmentEntity>() { shipment };
            mock.Mock<IOrderLoader>()
                .Setup(
                    o =>
                        o.LoadAsync(It.Is<IEnumerable<long>>(orderIds => orderIds.SingleOrDefault() == 42),
                            ProgressDisplayOptions.NeverShow, false, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, null,
                    shipments));

            var shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.SetupGet(s => s.Shipment)
                .Returns(shipment);

            mock.Mock<IShippingManager>()
                .Setup(m => m.GetShipmentAdapter(shipment))
                .Returns(shipmentAdapter.Object);


            var testObject = mock.Create<AutoWeighServicePipeline>(); ;
            testObject.InitializeForCurrentSession();
            testMessenger.Send(singleScanFilterUpdateCompleteMessage);

            Assert.True(testMessenger.SentMessages.OfType<ShipmentChangedMessage>().None());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}