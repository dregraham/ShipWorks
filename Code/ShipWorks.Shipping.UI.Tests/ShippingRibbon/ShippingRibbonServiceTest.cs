using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Core.UI.SandRibbon;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingRibbon;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingRibbon
{
    public class ShippingRibbonServiceTest : IDisposable
    {
        readonly AutoMock mock;
        readonly TestMessenger messenger;
        Mock<IShippingRibbonActions> actions;
        Mock<Func<ISecurityContext>> getSecurityContext;
        Mock<ISecurityContext> securityContext;

        public ShippingRibbonServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            securityContext = mock.Mock<ISecurityContext>();
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(true);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(true);
            
            getSecurityContext = mock.MockRepository.Create<Func<ISecurityContext>>();
            getSecurityContext.Setup(sc => sc()).Returns(securityContext.Object);
            mock.Provide<Func<ISecurityContext>>(getSecurityContext.Object);

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
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        public void HandleOrderSelectionChanged_DisablesCreateLabel_WhenLoadedShipmentIsProcessed(bool isProcessed, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.CreateLabel.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x => x.Processed = isProcessed));

            Assert.Equal(expected, actions.Object.CreateLabel.Enabled);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public void HandleOrderSelectionChanged_DisablesCreateLabel_WhenLoadedOrderHasNoShipments(bool hasPermission, bool expected)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.CreateLabel.Enabled = true;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection();

            Assert.Equal(expected, actions.Object.CreateLabel.Enabled);
        }

        [Theory]
        [InlineData(0, false, true)]
        [InlineData(1, false, true)]
        [InlineData(2, false, true)]
        [InlineData(0, false, false)]
        [InlineData(1, false, false)]
        [InlineData(2, false, false)]
        public void HandleOrderSelectionChanged_SetsEnabledOnCreateLabel_WhenMessageHasNoLoadedOrders(int selectionCount, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.CreateLabel.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            var orderSelections = Enumerable.Range(0, selectionCount)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            Assert.Equal(expected, actions.Object.CreateLabel.Enabled);
        }

        [Theory]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        public void HandleShipmentsProcessedMessage_SetsEnabledOnCreateLabel_WhenSingleUnprocessedShipmentIsLoaded(bool isProcessed, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

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
        public void CreateLabelClick_SendsOpenShippingDialogWithOrdersMessage_WhenMultipleShipmentsAreLoaded()
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

        #region Void Label
        [Theory]
        [InlineData(true, true, false, true)]
        [InlineData(true, false, true, true)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, true)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, false, false)]
        [InlineData(false, true, false, false)]
        [InlineData(false, false, false, false)]
        public void HandleOrderSelectionChanged_DisablesVoid_WhenLoadedShipmentIsProcessed(bool isProcessed, bool isVoided, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

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

        [Theory]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void HandleOrderSelectionChanged_DisablesVoid_WhenLoadedOrderHasNoShipments(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.Void.Enabled = true;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection();

            Assert.Equal(expected, actions.Object.Void.Enabled);
        }

        [Theory]
        [InlineData(0, false, true)]
        [InlineData(1, false, true)]
        [InlineData(2, false, true)]
        [InlineData(0, false, false)]
        [InlineData(1, false, false)]
        [InlineData(2, false, false)]
        public void HandleOrderSelectionChanged_SetsEnabledOnVoid_WhenMessageHasNoLoadedOrders(int selectionCount, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.Void.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            var orderSelections = Enumerable.Range(0, selectionCount)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            Assert.Equal(expected, actions.Object.Void.Enabled);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public void VoidLabelClick_SendsVoidLabelMessage_WhenSingleInvalidShipmentIsLoaded(bool isProcessed, bool isVoided)
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = isProcessed;
                x.Voided = isVoided;
                x.ShipmentID = 1234;
            }));

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.Void).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }

        [Fact]
        public void VoidLabelClick_SendsVoidLabelMessage_WhenSingleValidShipmentIsLoaded()
        {
            long shipmentID = 0;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = true;
                x.ShipmentID = 1234;
            }));

            messenger.OfType<VoidLabelMessage>().Subscribe(x => shipmentID = x.ShipmentID);

            Mock.Get(actions.Object.Void).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Equal(1234, shipmentID);
        }

        [Fact]
        public void VoidLabelClick_SendsOpenShippingDialogWithOrdersMessage_WhenMultipleShipmentsAreLoaded()
        {
            IEnumerable<long> orderIDs = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            var orderSelections = Enumerable.Range(0, 3)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            messenger.OfType<OpenShippingDialogWithOrdersMessage>().Subscribe(x => orderIDs = x.OrderIDs);

            Mock.Get(actions.Object.Void).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Contains(0, orderIDs);
            Assert.Contains(1, orderIDs);
            Assert.Contains(2, orderIDs);
        }

        [Fact]
        public void VoidLabelClick_DoesNotSendAMessage_WhenCurrentSelectionHasNotBeenSet()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.Void).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }

        [Theory]
        [InlineData(true, true, false, true)]
        [InlineData(true, false, true, true)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, true)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, false, false)]
        [InlineData(false, true, false, false)]
        [InlineData(false, false, false, false)]
        public void HandleLabelVoidedMessage_SetsEnabledOnVoid_WhenSingleProcessedShipmentIsLoaded(bool isProcessed, bool voided, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.Void.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = isProcessed;
                x.Voided = voided;
                x.ShipmentID = 123;
            }));

            messenger.Send(new ShipmentsVoidedMessage(this, new[]
            {
                new VoidShipmentResult(new ShipmentEntity { ShipmentID = 123, Processed = isProcessed, Voided = voided})
            }));

            Assert.Equal(expected, actions.Object.Void.Enabled);
        }
        #endregion

        #region Create return
        [Theory]
        [InlineData(true, true, false, true)]
        [InlineData(true, false, true, true)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, true)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, false, false)]
        [InlineData(false, true, false, false)]
        [InlineData(false, false, false, false)]
        public void HandleOrderSelectionChanged_DisablesReturn_WhenLoadedShipmentIsProcessed(bool isProcessed, bool isReturned, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

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

        [Theory]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void HandleOrderSelectionChanged_DisablesReturn_WhenLoadedOrderHasNoShipments(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.Return.Enabled = true;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection();

            Assert.Equal(expected, actions.Object.Return.Enabled);
        }

        [Theory]
        [InlineData(0, false, true)]
        [InlineData(1, false, true)]
        [InlineData(2, false, true)]
        [InlineData(0, false, false)]
        [InlineData(1, false, false)]
        [InlineData(2, false, false)]
        public void HandleOrderSelectionChanged_SetsEnabledOnReturn_WhenMessageHasNoLoadedOrders(int selectionCount, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.Return.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            var orderSelections = Enumerable.Range(0, selectionCount)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            Assert.Equal(expected, actions.Object.Return.Enabled);
        }

        [Fact]
        public void CreateReturnShipmentClick_SendsCreateReturnShipmentMessage_WhenSingleValidShipmentIsLoaded()
        {
            long shipmentID = 0;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = true;
                x.Voided = false;
                x.ShipmentID = 1234;
            }));

            messenger.OfType<CreateReturnShipmentMessage>().Subscribe(x => shipmentID = x.Shipment.ShipmentID);

            Mock.Get(actions.Object.Return).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Equal(1234, shipmentID);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void CreateReturnShipmentClick_DoesNotSendAMessage_WhenSingleInvalidShipmentIsLoaded(bool isProcessed, bool isVoided)
        {
            DoesNotSendMessageWhenSingleInvalidShipmentIsLoaded(actions.Object.Return, isProcessed, isVoided);
        }

        [Fact]
        public void CreateReturnShipmentClick_DoesNotSendAMessage_WhenMultipleShipmentsAreLoaded()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            var orderSelections = Enumerable.Range(0, 3)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.Return).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }

        [Fact]
        public void CreateReturnShipmentClick_DoesNotSendAMessage_WhenCurrentSelectionHasNotBeenSet()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.Return).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }
        #endregion

        #region Reprint label
        [Theory]
        [InlineData(true, true, false, true)]
        [InlineData(true, false, true, true)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, true)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, false)]
        [InlineData(false, true, false, false)]
        [InlineData(false, false, false, false)]
        public void HandleOrderSelectionChanged_DisablesReprint_WhenLoadedShipmentIsProcessed(bool isProcessed, bool isReprinted, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

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

        [Theory]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void HandleOrderSelectionChanged_DisablesReprint_WhenLoadedOrderHasNoShipments(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.Reprint.Enabled = true;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection();

            Assert.Equal(expected, actions.Object.Reprint.Enabled);
        }

        [Theory]
        [InlineData(0, false, true)]
        [InlineData(1, false, true)]
        [InlineData(2, false, true)]
        [InlineData(0, false, false)]
        [InlineData(1, false, false)]
        [InlineData(2, false, false)]
        public void HandleOrderSelectionChanged_SetsEnabledOnReprint_WhenMessageHasNoLoadedOrders(int selectionCount, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.Reprint.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            var orderSelections = Enumerable.Range(0, selectionCount)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            Assert.Equal(expected, actions.Object.Reprint.Enabled);
        }

        [Fact]
        public void ReprintLabelClick_SendsReprintLabelMessage_WhenSingleProcessedShipmentIsLoaded()
        {
            long shipmentID = 0;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = true;
                x.ShipmentID = 1234;
            }));

            messenger.OfType<ReprintLabelsMessage>().Subscribe(x => shipmentID = x.Shipments.First().ShipmentID);

            Mock.Get(actions.Object.Reprint).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Equal(1234, shipmentID);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void ReprintLabelClick_DoesNotSendAMessage_WhenSingleInvalidShipmentIsLoaded(bool isProcessed, bool isVoided)
        {
            DoesNotSendMessageWhenSingleInvalidShipmentIsLoaded(actions.Object.Reprint, isProcessed, isVoided);
        }

        [Fact]
        public void ReprintLabelClick_DoesNotSendAMessage_WhenMultipleShipmentsAreLoaded()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            var orderSelections = Enumerable.Range(0, 3)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.Reprint).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }

        [Fact]
        public void ReprintLabelClick_DoesNotSendAMessage_WhenCurrentSelectionHasNotBeenSet()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.Reprint).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }
        #endregion

        #region Ship again
        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, false, true, true)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, true)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, false, false)]
        [InlineData(false, true, false, false)]
        [InlineData(false, false, false, false)]
        public void HandleOrderSelectionChanged_DisablesShipAgain_WhenLoadedShipmentIsProcessed(bool isProcessed, bool isVoided, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.ShipAgain.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = isProcessed;
                x.Voided = isVoided;
            }));

            Assert.Equal(expected, actions.Object.ShipAgain.Enabled);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void HandleOrderSelectionChanged_DisablesShipAgain_WhenLoadedOrderHasNoShipments(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.ShipAgain.Enabled = true;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection();

            Assert.Equal(expected, actions.Object.ShipAgain.Enabled);
        }

        [Theory]
        [InlineData(0, false, true)]
        [InlineData(1, false, true)]
        [InlineData(2, false, true)]
        [InlineData(0, false, false)]
        [InlineData(1, false, false)]
        [InlineData(2, false, false)]
        public void HandleOrderSelectionChanged_SetsEnabledOnShipAgain_WhenMessageHasNoLoadedOrders(int selectionCount, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.ShipAgain.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            var orderSelections = Enumerable.Range(0, selectionCount)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            Assert.Equal(expected, actions.Object.ShipAgain.Enabled);
        }

        [Fact]
        public void ShipAgainClick_SendsShipAgainMessage_WhenSingleProcessedShipmentIsLoaded()
        {
            long shipmentID = 0;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = true;
                x.ShipmentID = 1234;
            }));

            messenger.OfType<ShipAgainMessage>().Subscribe(x => shipmentID = x.Shipment.ShipmentID);

            Mock.Get(actions.Object.ShipAgain).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Equal(1234, shipmentID);
        }

        [Fact]
        public void ShipAgainClick_DoesNotSendAMessage_WhenSingleUnprocessedShipmentIsLoaded()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = false;
                x.ShipmentID = 1234;
            }));

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.ShipAgain).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }

        [Fact]
        public void ShipAgainClick_DoesNotSendAMessage_WhenMultipleShipmentsAreLoaded()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            var orderSelections = Enumerable.Range(0, 3)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.ShipAgain).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }

        [Fact]
        public void ShipAgainClick_DoesNotSendAMessage_WhenCurrentSelectionHasNotBeenSet()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.ShipAgain).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }
        #endregion

        #region Create Label
        [Theory]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        public void HandleOrderSelectionChanged_DisablesApplyProfile_WhenLoadedShipmentIsProcessed(bool isProcessed, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.ApplyProfile.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x => x.Processed = isProcessed));

            Assert.Equal(expected, actions.Object.ApplyProfile.Enabled);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void HandleOrderSelectionChanged_DisablesApplyProfile_WhenLoadedOrderHasNoShipments(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.ApplyProfile.Enabled = true;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            SendOrderSelectionChangedMessageWithLoadedOrderSelection();

            Assert.Equal(expected, actions.Object.ApplyProfile.Enabled);
        }

        [Theory]
        [InlineData(0, false, true)]
        [InlineData(1, false, true)]
        [InlineData(2, false, true)]
        [InlineData(0, false, false)]
        [InlineData(1, false, false)]
        [InlineData(2, false, false)]
        public void HandleOrderSelectionChanged_SetsEnabledOnApplyProfile_WhenMessageHasNoLoadedOrders(int selectionCount, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.ApplyProfile.Enabled = !expected;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            var orderSelections = Enumerable.Range(0, selectionCount)
                .Select(x => new BasicOrderSelection(x)).OfType<IOrderSelection>();
            messenger.Send(new OrderSelectionChangedMessage(this, orderSelections));

            Assert.Equal(expected, actions.Object.ApplyProfile.Enabled);
        }

        [Theory]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        public void HandleShipmentsProcessedMessage_SetsEnabledOnApplyProfile_WhenSingleUnprocessedShipmentIsLoaded(bool isProcessed, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>())).Returns(hasPermission);
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            actions.Object.ApplyProfile.Enabled = !expected;
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

            Assert.Equal(expected, actions.Object.ApplyProfile.Enabled);
        }

        [Fact]
        public void ApplyProfileClick_SendsApplyProfileMessage_WhenSingleUnprocessedShipmentIsLoaded()
        {
            long shipmentID = 0;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = false;
                x.ShipmentID = 1234;
            }));

            messenger.OfType<ApplyProfileMessage>().Subscribe(x => shipmentID = x.ShipmentID);

            Mock<IRibbonButton> applyProfileButton = Mock.Get(actions.Object.ApplyProfile);
            applyProfileButton.SetupGet(x => x.Tag).Returns(new ShippingProfileEntity());
            applyProfileButton.Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Equal(1234, shipmentID);
        }

        [Fact]
        public void ApplyProfileClick_DoesNotSendAMessage_WhenSingleProcessedShipmentIsLoaded()
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

            Mock.Get(actions.Object.ApplyProfile).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }

        [Fact]
        public void ApplyProfileClick_DoesNotSendAMessage_WhenSingleUnProcessedShipmentIsLoadedAndButtonHasNoTag()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = false;
                x.ShipmentID = 1234;
            }));

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.ApplyProfile).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }

        [Fact]
        public void ApplyProfileClick_DoesNotSendAMessage_WhenCurrentSelectionHasNotBeenSet()
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            messenger.Subscribe(x => message = x);

            Mock.Get(actions.Object.ApplyProfile).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
        }
        #endregion

        private void DoesNotSendMessageWhenSingleInvalidShipmentIsLoaded(IRibbonButton button, bool isProcessed, bool isVoided)
        {
            IShipWorksMessage message = null;
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);
            SendOrderSelectionChangedMessageWithLoadedOrderSelection(CreateShipmentAdapter(x =>
            {
                x.Processed = isProcessed;
                x.Voided = isVoided;
                x.ShipmentID = 1234;
            }));

            messenger.Subscribe(x => message = x);

            Mock.Get(button).Raise(x => x.Activate += null, EventArgs.Empty);

            Assert.Null(message);
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
