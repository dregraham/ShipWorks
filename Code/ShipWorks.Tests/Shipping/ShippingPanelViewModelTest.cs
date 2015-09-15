using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Messaging;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using Xunit;

namespace ShipWorks.Tests.Shipping
{
    public class ShippingPanelViewModelTest
    {
        private ShippingPanelViewModel testObject;
        private OrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;
        private Mock<IShipmentLoader> shipmentLoader;
        private Mock<IValidator<ShipmentEntity>> validator;
        private Mock<ILoader<ShippingPanelLoadedShipment>> shippingPanelShipmentLoader;
        private Mock<IMessenger> messenger;

        public ShippingPanelViewModelTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;

            ShippingPanelLoadedShipment shippingPanelLoadedShipment = new ShippingPanelLoadedShipment()
            {
                Shipment = shipmentEntity,
                Result = ShippingPanelLoadedShipmentResult.Success,
                Exception = null
            };

            shipmentLoader = new Mock<IShipmentLoader>();
            shipmentLoader.Setup(s => s.LoadAsync(It.IsAny<long>())).Returns(Task.FromResult(shippingPanelLoadedShipment));

            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);

            validator = new Mock<IValidator<ShipmentEntity>>();
            validator.Setup(s => s.ValidateAsync(It.IsAny<ShipmentEntity>())).Returns(tcs.Task);

            var tcsShippingPanelShipmentLoader = new TaskCompletionSource<ShippingPanelLoadedShipment>();
            tcsShippingPanelShipmentLoader.SetResult(shippingPanelLoadedShipment);

            shippingPanelShipmentLoader = new Mock<ILoader<ShippingPanelLoadedShipment>>();
            shippingPanelShipmentLoader.Setup(s => s.LoadAsync(It.IsAny<long>())).Returns(tcsShippingPanelShipmentLoader.Task);

            messenger = new Mock<IMessenger>();
            messenger.Setup(s => s.Send(It.IsAny<IShipWorksMessage>())).Verifiable();
        }

        [Fact]
        public async void Save_UpdatesShipmentEntity_WhenTotalWeightChanged_Test()
        {
            testObject = new ShippingPanelViewModel(shippingPanelShipmentLoader.Object, messenger.Object);
            await testObject.LoadOrder(orderEntity.OrderID);

            testObject.Shipment.TotalWeight = 2.93;
            testObject.Save();

            Assert.Equal(2.93, shipmentEntity.TotalWeight);
        }

        [Fact]
        public async void Save_UpdatesShipmentEntity_WhenDestinationCountryCodeChanged_Test()
        {
            testObject = new ShippingPanelViewModel(shippingPanelShipmentLoader.Object, messenger.Object);
            await testObject.LoadOrder(orderEntity.OrderID);

            testObject.Destination.CountryCode = "XX";
            testObject.Save();

            Assert.Equal("XX", shipmentEntity.ShipCountryCode);
        }

        [Fact]
        public async void Save_UpdatesShipmentEntity_WhenOriginCountryCodeChanged_Test()
        {
            testObject = new ShippingPanelViewModel(shippingPanelShipmentLoader.Object, messenger.Object);
            await testObject.LoadOrder(orderEntity.OrderID);

            testObject.Origin.CountryCode = "XX";
            testObject.Save();

            Assert.Equal("XX", shipmentEntity.OriginCountryCode);
        }

        [Fact]
        public async void Save_SendsShipmentChangedMessage_WhenTotalWeightChanged_Test()
        {
            testObject = new ShippingPanelViewModel(shippingPanelShipmentLoader.Object, messenger.Object);
            await testObject.LoadOrder(orderEntity.OrderID);

            testObject.Shipment.TotalWeight = 2.93;
            testObject.Save();

            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Once);
        }

        [Fact]
        public async void Save_DoesNotSendShipmentChangedMessage_WhenLoadingOrderTest()
        {
            testObject = new ShippingPanelViewModel(shippingPanelShipmentLoader.Object, messenger.Object);
            await testObject.LoadOrder(orderEntity.OrderID);

            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
        }

        [Fact]
        public async void Save_DoesNotSendShipmentChangedMessage_WhenNothingChanged_Test()
        {
            testObject = new ShippingPanelViewModel(shippingPanelShipmentLoader.Object, messenger.Object);
            await testObject.LoadOrder(orderEntity.OrderID);

            testObject.Save();

            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
        }

        [Fact]
        public async void Save_DoesNotSendShipmentChangedMessage_WhenTotalWeightSetToSameValue_Test()
        {
            testObject = new ShippingPanelViewModel(shippingPanelShipmentLoader.Object, messenger.Object);
            await testObject.LoadOrder(orderEntity.OrderID);

            testObject.Shipment.TotalWeight = testObject.Shipment.TotalWeight;
            testObject.Save();

            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
        }

        [Fact]
        public async void Save_SendsShipmentChangedMessage_WhenOriginRatingFieldsChanged_Test()
        {
            testObject = new ShippingPanelViewModel(shippingPanelShipmentLoader.Object, messenger.Object);
            await testObject.LoadOrder(orderEntity.OrderID);

            int calls = 1;
            testObject.Origin.CountryCode = "XX";
            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(calls));

            calls++;
            testObject.Origin.Street = "XX";
            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(calls));

            calls++;
            testObject.Origin.StateProvCode = "XX";
            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(calls));

            calls++;
            testObject.Origin.PostalCode = "XX";
            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(calls));

            calls++;
            testObject.Origin.City = "XX";
            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(calls));
        }

        [Fact]
        public async void Save_SendsShipmentChangedMessage_WhenDestinationRatingFieldsChanged_Test()
        {
            testObject = new ShippingPanelViewModel(shippingPanelShipmentLoader.Object, messenger.Object);
            await testObject.LoadOrder(orderEntity.OrderID);

            int calls = 1;
            testObject.Destination.CountryCode = "XX";
            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(calls));

            calls++;
            testObject.Destination.Street = "XX";
            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(calls));

            calls++;
            testObject.Destination.StateProvCode = "XX";
            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(calls));

            calls++;
            testObject.Destination.PostalCode = "XX";
            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(calls));

            calls++;
            testObject.Destination.City = "XX";
            messenger.Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(calls));
        }
    }
}
