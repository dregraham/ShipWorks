using System.Threading.Tasks;
using Interapptive.Shared.Messaging;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using Xunit;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Tests;

namespace ShipWorks.Tests.Shipping
{
    public class ShippingPanelViewModelTest
    {
        private readonly OrderEntity orderEntity;
        private readonly ShipmentEntity shipmentEntity;
        private readonly ShippingPanelLoadedShipment shippingPanelLoadedShipment;

        public ShippingPanelViewModelTest()
        {
            orderEntity = new OrderEntity(1006);
            shipmentEntity = new ShipmentEntity(1031);
            shipmentEntity.Order = orderEntity;
            shippingPanelLoadedShipment = new ShippingPanelLoadedShipment()
            {
                Shipment = shipmentEntity,
                Result = ShippingPanelLoadedShipmentResult.Success,
                Exception = null
            };
        }

        private async Task<ShippingPanelViewModel> GetViewModelWithLoadedShipment(AutoMock mock)
        {
            mock.Mock<ILoader<ShippingPanelLoadedShipment>>()
                .Setup(s => s.LoadAsync(It.IsAny<long>()))
                .ReturnsAsync(shippingPanelLoadedShipment);
            
            ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
            await testObject.LoadOrder(orderEntity.OrderID);
            testObject.SelectedShipmentType = ShipmentTypeCode.Other;
            
            return testObject;
        }

        [Fact]
        public async void Save_UpdatesShipmentEntity_WhenDestinationCountryCodeChanged_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
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
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

                testObject.Origin.CountryCode = "XX";
                testObject.Save();

                Assert.Equal("XX", shipmentEntity.OriginCountryCode);
            }
        }

        [Fact]
        public async void Save_DoesNotSendShipmentChangedMessage_WhenLoadingOrderTest()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.Mock<ILoader<ShippingPanelLoadedShipment>>()
                    .Setup(s => s.LoadAsync(It.IsAny<long>()))
                    .ReturnsAsync(shippingPanelLoadedShipment);

                ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
                await testObject.LoadOrder(orderEntity.OrderID);

                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
            }
        }

        [Fact]
        public async void Save_DoesNotSendShipmentChangedMessage_WhenNothingChanged_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                
                testObject.Save();

                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
            }
        }

        [Fact]
        public async void Save_DoesNotSendShipmentChangedMessage_WhenTotalWeightSetToSameValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                shipmentEntity.TotalWeight = 2.93;

                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

                testObject.ShipmentViewModel.TotalWeight = 2.93;
                testObject.Save();

                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
            }
        }

        [Fact]
        public async void Save_SendsShipmentChangedMessage_WhenOriginRatingFieldsChanged_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentTypeFromFactory(type => type.SetupGet(x => x.RatingFields).CallBase());

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
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentTypeFromFactory(type => type.SetupGet(x => x.RatingFields).CallBase());

                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

                testObject.Destination.CountryCode = "XX";
                testObject.Destination.Street = "XX";
                testObject.Destination.StateProvCode = "XX";
                testObject.Destination.PostalCode = "XX";
                testObject.Destination.City = "XX";
                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<ShipmentChangedMessage>()), Times.Exactly(5));
            }
        }

        [Fact]
        public async void Load_DelegatesToShipmentLoader()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                await testObject.LoadOrder(3);

                mock.Mock<ILoader<ShippingPanelLoadedShipment>>()
                    .Verify(x => x.LoadAsync((long)3));
            }
        }

        [Fact]
        public async void Load_ShowsMessage_WhenMultipleShipmentsAreLoaded()
        {
            shippingPanelLoadedShipment.Result = ShippingPanelLoadedShipmentResult.Multiple;

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

                Assert.Equal(ShippingPanelLoadedShipmentResult.Multiple, testObject.LoadResult);
            }
        }
    }
}
