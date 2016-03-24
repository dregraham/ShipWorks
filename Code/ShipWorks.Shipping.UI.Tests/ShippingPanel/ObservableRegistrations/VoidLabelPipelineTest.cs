using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class VoidLabelPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly TestMessenger messenger;

        public VoidLabelPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());

            mock.Mock<IMessageHelper>().Setup(x => x.ShowDialog(It.IsAny<Func<Form>>())).Returns(DialogResult.OK);
        }

        [Fact]
        public void Register_CallsVoidLabel_WhenShipmentIDofMessageMatchesViewModel()
        {
            CreateTestObjectWithViewModel(shipmentID: 123);

            VoidLabelMessage voidLabelMessage = new VoidLabelMessage(this, 123);
            messenger.Send(voidLabelMessage);

            mock.Mock<IShippingManager>()
                .Verify(x => x.VoidShipment(123, It.IsAny<IShippingErrorManager>()));
        }

        [Fact]
        public void Register_DoesNotCallVoidLabel_WhenUserCancelsConfirmation()
        {
            CreateTestObjectWithViewModel(shipmentID: 123);

            mock.Mock<IMessageHelper>().Setup(x => x.ShowDialog(It.IsAny<Func<Form>>())).Returns(DialogResult.Cancel);

            VoidLabelMessage voidLabelMessage = new VoidLabelMessage(this, 123);
            messenger.Send(voidLabelMessage);

            mock.Mock<IShippingManager>()
                .Verify(x => x.VoidShipment(It.IsAny<long>(), It.IsAny<IShippingErrorManager>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallVoidLabel_WhenShipmentIDofMessageDoesNotMatchViewModel()
        {
            CreateTestObjectWithViewModel(shipmentID: 456);

            VoidLabelMessage voidLabelMessage = new VoidLabelMessage(this, 123);
            messenger.Send(voidLabelMessage);

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowDialog(() => new ShipmentVoidConfirmDlg()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotCallVoidLabel_WhenNoShipmentIsLoaded()
        {
            var testObject = mock.Create<VoidLabelPipeline>();
            testObject.Register(mock.Create<ShippingPanelViewModel>());

            VoidLabelMessage voidLabelMessage = new VoidLabelMessage(this, 123);
            messenger.Send(voidLabelMessage);

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowQuestion(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Register_ShowsError_WhenVoidFails()
        {
            CreateTestObjectWithViewModel(shipmentID: 123);

            mock.Mock<IShippingManager>()
                .Setup(x => x.VoidShipment(It.IsAny<long>(), It.IsAny<IShippingErrorManager>()))
                .Returns(GenericResult.FromError<ICarrierShipmentAdapter>("Error"));

            VoidLabelMessage voidLabelMessage = new VoidLabelMessage(this, 123);
            messenger.Send(voidLabelMessage);

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError("Error"));
        }

        [Fact]
        public void Register_SendsShipmentsVoidedMessage_WhenVoidSucceeds()
        {
            ShipmentEntity voidedEntity = new ShipmentEntity();
            ShipmentEntity calledShipment = null;

            messenger.OfType<ShipmentsVoidedMessage>()
                .Subscribe(x => calledShipment = x.VoidShipmentResults.First().Shipment);

            var shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>(m => m.Setup(x => x.Shipment).Returns(voidedEntity)).Object;
            mock.Mock<IShippingManager>()
                .Setup(x => x.VoidShipment(It.IsAny<long>(), It.IsAny<IShippingErrorManager>()))
                .Returns(GenericResult.FromSuccess(shipmentAdapter));

            CreateTestObjectWithViewModel(shipmentID: 123);

            VoidLabelMessage voidLabelMessage = new VoidLabelMessage(this, 123);
            messenger.Send(voidLabelMessage);

            Assert.Equal(voidedEntity, calledShipment);
        }

        private void CreateTestObjectWithViewModel(long shipmentID = 123)
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();
            viewModel.Setup(x => x.Shipment).Returns(new ShipmentEntity { ShipmentID = shipmentID });
            var testObject = mock.Create<VoidLabelPipeline>();
            testObject.Register(viewModel.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
