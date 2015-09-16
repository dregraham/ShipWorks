﻿using System.Threading.Tasks;
using Interapptive.Shared.Messaging;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using Xunit;
using Autofac.Extras.Moq;

namespace ShipWorks.Tests.Shipping
{
    public class ShippingPanelViewModelTest
    {
        private OrderEntity orderEntity;
        private ShipmentEntity shipmentEntity;

        public ShippingPanelViewModelTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;
        }

        ShippingPanelLoadedShipment ShippingPanelLoadedShipment => new ShippingPanelLoadedShipment()
        {
            Shipment = shipmentEntity,
            Result = ShippingPanelLoadedShipmentResult.Success,
            Exception = null
        };

        private async Task<ShippingPanelViewModel> GetViewModelWithLoadedShipment(AutoMock mock)
        {
            mock.Mock<ILoader<ShippingPanelLoadedShipment>>()
                .Setup(s => s.LoadAsync(It.IsAny<long>()))
                .ReturnsAsync(ShippingPanelLoadedShipment);

            ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
            testObject.SelectedShipmentType = mock.Mock<ShipmentType>().Object;

            await testObject.LoadOrder(orderEntity.OrderID);

            return testObject;
        }

        [Fact]
        public async void Save_UpdatesShipmentEntity_WhenTotalWeightChanged_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                
                testObject.Shipment.TotalWeight = 2.93;
                testObject.Save();

                Assert.Equal(2.93, shipmentEntity.TotalWeight);
            }
        }

        [Fact]
        public async void Save_UpdatesShipmentEntity_WhenDestinationCountryCodeChanged_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

                testObject.Destination.CountryCode = "XX";
                testObject.Save();

                Assert.Equal("XX", shipmentEntity.ShipCountryCode);
            }
        }

        [Fact]
        public async void Save_UpdatesShipmentEntity_WhenOriginCountryCodeChanged_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

                testObject.Origin.CountryCode = "XX";
                testObject.Save();

                Assert.Equal("XX", shipmentEntity.OriginCountryCode);
            }
        }

        [Fact]
        public async void Save_SendsShipmentChangedMessage_WhenTotalWeightChanged_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                
                testObject.Shipment.TotalWeight = 2.93;
                testObject.Save();

                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Once);
            }
        }

        [Fact]
        public async void Save_DoesNotSendShipmentChangedMessage_WhenLoadingOrderTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ILoader<ShippingPanelLoadedShipment>>()
                    .Setup(s => s.LoadAsync(It.IsAny<long>()))
                    .ReturnsAsync(ShippingPanelLoadedShipment);

                ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
                await testObject.LoadOrder(orderEntity.OrderID);

                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
            }
        }

        [Fact]
        public async void Save_DoesNotSendShipmentChangedMessage_WhenNothingChanged_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                
                testObject.Save();

                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
            }
        }

        [Fact]
        public async void Save_DoesNotSendShipmentChangedMessage_WhenTotalWeightSetToSameValue_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

                testObject.Shipment.TotalWeight = testObject.Shipment.TotalWeight;
                testObject.Save();

                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
            }
        }

        [Fact]
        public async void Save_SendsShipmentChangedMessage_WhenOriginRatingFieldsChanged_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                
                testObject.Origin.CountryCode = "XX";
                testObject.Origin.Street = "XX";
                testObject.Origin.StateProvCode = "XX";
                testObject.Origin.PostalCode = "XX";
                testObject.Origin.City = "XX";
                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(5));
            }
        }

        [Fact]
        public async void Save_SendsShipmentChangedMessage_WhenDestinationRatingFieldsChanged_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                
                testObject.Destination.CountryCode = "XX";
                testObject.Destination.Street = "XX";
                testObject.Destination.StateProvCode = "XX";
                testObject.Destination.PostalCode = "XX";
                testObject.Destination.City = "XX";
                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(5));
            }
        }
    }
}
