using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingRibbon;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingRibbon
{
    public class ShippingRibbonServiceTest : IDisposable
    {
        readonly AutoMock mock;
        readonly TestMessenger messenger;
        Mock<IShippingRibbonActions> actions;

        public ShippingRibbonServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            actions = mock.CreateMock<IShippingRibbonActions>();
            actions.SetupAllProperties();
        }

        [Fact]
        public void Register_DisablesAllActions()
        {
            var testObject = mock.Create<ShippingRibbonService>();

            testObject.Register(actions.Object);

            Assert.False(actions.Object.CreateLabel.Enabled);
            Assert.False(actions.Object.Void.Enabled);
            Assert.False(actions.Object.Return.Enabled);
            Assert.False(actions.Object.Reprint.Enabled);
            Assert.False(actions.Object.ShipAgain.Enabled);
        }

        #region Create Label
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void HandleOrderSelectionChanged_DisablesCreateLabel_WhenLoadedShipmentIsProcessed(bool isProcessed, bool expected)
        {
            actions.Object.CreateLabel.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x => x.Processed = isProcessed));

            Assert.Equal(expected, actions.Object.CreateLabel.Enabled);
        }

        [Fact]
        public void HandleOrderSelectionChanged_DisablesCreateLabel_WhenLoadedOrderHasNoShipments()
        {
            actions.Object.CreateLabel.Enabled = true;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection();

            Assert.False(actions.Object.CreateLabel.Enabled);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        public void HandleOrderSelectionChanged_SetsEnabledOnCreateLabel_WhenMessageHasNoLoadedOrders(int selectionCount, bool expected)
        {
            actions.Object.CreateLabel.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            var orderSelections = Enumerable.Range(0, selectionCount)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            Assert.Equal(expected, actions.Object.CreateLabel.Enabled);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void HandleShipmentsProcessedMessage_SetsEnabledOnCreateLabel_WhenSingleUnprocessedShipmentIsLoaded(bool isProcessed, bool expected)
        {
            actions.Object.CreateLabel.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = false;
                x.ShipmentID = 123;
            }));

            messenger.Send(new ShipmentsProcessedMessage(this, new[]
            {
                new ProcessShipmentResult(new ShipmentEntity { ShipmentID = 123, Processed = isProcessed })
            }));

            Assert.Equal(expected, actions.Object.CreateLabel.Enabled);
        }

        [Fact]
        public void CreateLabelClick_SendsCreateLabelMessage_WhenSingleUnprocessedShipmentIsLoaded()
        {
            long shipmentID = 0;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = false;
                x.ShipmentID = 1234;
            }));

            messenger.OfType<CreateLabelMessage>().Subscribe(x => shipmentID = x.ShipmentID);

            Mock.Get(actions.Object.CreateLabel).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Equal(1234, shipmentID);
        }

        [Fact]
        public void CreateLabelClick_DoesNotSendAMessage_WhenSingleProcessedShipmentIsLoaded()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = true;
                x.ShipmentID = 1234;
            }));

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.CreateLabel).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }

        [Fact]
        public void CreateLabelClick_SendsOpenShippingDialogWithOrdersMessage_WhenNoShipmentsAreLoaded()
        {
            IEnumerable<long> orderIDs = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            var orderSelections = Enumerable.Range(0, 3)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            messenger.OfType<OpenShippingDialogWithOrdersMessage>().Subscribe(x => orderIDs = x.OrderIDs);

            Mock.Get(actions.Object.CreateLabel).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Contains(0, orderIDs);
            Assert.Contains(1, orderIDs);
            Assert.Contains(2, orderIDs);
        }

        [Fact]
        public void CreateLabelClick_DoesNotSendAMessage_WhenCurrentSelectionHasNotBeenSet()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.CreateLabel).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }
        #endregion

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void HandleOrderSelectionChanged_DisablesVoid_WhenLoadedShipmentIsProcessed(bool isProcessed, bool isVoided, bool expected)
        {
            actions.Object.Void.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = isProcessed;
                x.Voided = isVoided;
            }));

            Assert.Equal(expected, actions.Object.Void.Enabled);
        }

        [Fact]
        public void HandleOrderSelectionChanged_DisablesVoid_WhenLoadedOrderHasNoShipments()
        {
            actions.Object.Void.Enabled = true;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection();

            Assert.False(actions.Object.Void.Enabled);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        public void HandleOrderSelectionChanged_SetsEnabledOnVoid_WhenMessageHasNoLoadedOrders(int selectionCount, bool expected)
        {
            actions.Object.Void.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            var orderSelections = Enumerable.Range(0, selectionCount)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            Assert.Equal(expected, actions.Object.Void.Enabled);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void HandleOrderSelectionChanged_DisablesReturn_WhenLoadedShipmentIsProcessed(bool isProcessed, bool isReturned, bool expected)
        {
            actions.Object.Return.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = isProcessed;
                x.Voided = isReturned;
            }));

            Assert.Equal(expected, actions.Object.Return.Enabled);
        }

        [Fact]
        public void HandleOrderSelectionChanged_DisablesReturn_WhenLoadedOrderHasNoShipments()
        {
            actions.Object.Return.Enabled = true;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection();

            Assert.False(actions.Object.Return.Enabled);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(2, false)]
        public void HandleOrderSelectionChanged_SetsEnabledOnReturn_WhenMessageHasNoLoadedOrders(int selectionCount, bool expected)
        {
            actions.Object.Return.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            var orderSelections = Enumerable.Range(0, selectionCount)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            Assert.Equal(expected, actions.Object.Return.Enabled);
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void HandleOrderSelectionChanged_DisablesReprint_WhenLoadedShipmentIsProcessed(bool isProcessed, bool isReprinted, bool expected)
        {
            actions.Object.Reprint.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = isProcessed;
                x.Voided = isReprinted;
            }));

            Assert.Equal(expected, actions.Object.Reprint.Enabled);
        }

        [Fact]
        public void HandleOrderSelectionChanged_DisablesReprint_WhenLoadedOrderHasNoShipments()
        {
            actions.Object.Reprint.Enabled = true;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection();

            Assert.False(actions.Object.Reprint.Enabled);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(2, false)]
        public void HandleOrderSelectionChanged_SetsEnabledOnReprint_WhenMessageHasNoLoadedOrders(int selectionCount, bool expected)
        {
            actions.Object.Reprint.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            var orderSelections = Enumerable.Range(0, selectionCount)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            Assert.Equal(expected, actions.Object.Reprint.Enabled);
        }

        private void SendOrderSelectionChangedMessageWithLoadedOrderSelection(params ICarrierShipmentAdapter[] shipments)
        {
            messenger.Send(new OrderSelectionChangedMessage(this, new List<IOrderSelection> {
                new LoadedOrderSelection(new OrderEntity(),
                    shipments ?? Enumerable.Empty<ICarrierShipmentAdapter>(),
                    ShippingAddressEditStateType.Editable)
            }));
        }

        private ICarrierShipmentAdapter CreateShipmentAdapter(Action<ShipmentEntity> configure)
        {
            var shipment = new ShipmentEntity();
            configure(shipment);
            return mock.CreateMock<ICarrierShipmentAdapter>(x => x.Setup(c => c.Shipment).Returns(shipment)).Object;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
