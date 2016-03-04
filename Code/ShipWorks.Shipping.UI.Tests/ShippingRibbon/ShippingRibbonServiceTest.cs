using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
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

        public ShippingRibbonServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);
        }

        [Fact]
        public void Register_DisablesAllActions()
        {
            var actions = mock.CreateMock<IShippingRibbonActions>();
            var testObject = mock.Create<ShippingRibbonService>();

            testObject.Register(actions.Object);

            actions.VerifySet(x => x.CreateLabel.Enabled = false);
            actions.VerifySet(x => x.Void.Enabled = false);
            actions.VerifySet(x => x.Return.Enabled = false);
            actions.VerifySet(x => x.Reprint.Enabled = false);
            actions.VerifySet(x => x.ShipAgain.Enabled = false);
        }

        [Fact]
        public void HandleOrderSelectionChanged_DisablesCreateLabel_WhenLoadedShipmentIsProcessed()
        {
            var actions = mock.CreateMock<IShippingRibbonActions>();
            actions.SetupAllProperties();
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            messenger.Send(new OrderSelectionChangedMessage(this, new List<IOrderSelection> {
                new LoadedOrderSelection(new OrderEntity(), new [] {
                    CreateShipmentAdapter(x => x.Processed = true)
                }, ShippingAddressEditStateType.Editable)
            }));

            actions.VerifySet(x => x.CreateLabel.Enabled = false);
        }

        [Fact]
        public void HandleOrderSelectionChanged_EnablesCreateLabel_WhenLoadedShipmentIsNotProcessed()
        {
            var actions = mock.CreateMock<IShippingRibbonActions>();
            actions.SetupAllProperties();
            var testObject = mock.Create<ShippingRibbonService>();
            testObject.Register(actions.Object);

            messenger.Send(new OrderSelectionChangedMessage(this, new List<IOrderSelection> {
                new LoadedOrderSelection(new OrderEntity(), new [] {
                    CreateShipmentAdapter(x => x.Processed = false)
                }, ShippingAddressEditStateType.Editable)
            }));

            //Assert.False(actions.Create)
            actions.VerifySet(x => x.CreateLabel.Enabled = true);
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
