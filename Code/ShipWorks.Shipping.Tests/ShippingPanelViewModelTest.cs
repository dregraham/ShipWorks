using System.Threading.Tasks;
using Interapptive.Shared.Messaging;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using Xunit;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Tests;
using ShipWorks.Tests.Shared;
using System.Collections.Generic;
using ShipWorks.AddressValidation;

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
            shipmentEntity = new ShipmentEntity(1031)
            {
                ShipmentTypeCode = ShipmentTypeCode.Other
            };

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

            // Reset mocks so that tests don't have to worry about calls made during loading
            mock.Mock<IShipmentTypeFactory>().ResetCalls();
            mock.Mock<IShippingManager>().ResetCalls();

            return testObject;
        }
#pragma warning disable S125 // Sections of code should not be "commented out"

        //[Fact]
        //public async void Save_UpdatesShipmentEntity_WhenDestinationCountryCodeChanged_Test()
        //{
        //    using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
        //    {
        //        ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

        //        testObject.Destination.CountryCode = "XX";
        //        testObject.Save();

        //        Assert.Equal("XX", shipmentEntity.ShipCountryCode);
        //    }
        //}

        //[Fact]
        //public async void Save_UpdatesShipmentEntity_WhenOriginCountryCodeChanged_Test()
        //{
        //    using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
        //    {
        //        ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

        //        testObject.Origin.CountryCode = "XX";
        //        testObject.Save();

        //        Assert.Equal("XX", shipmentEntity.OriginCountryCode);
        //    }
        //}

        [Fact]
#pragma warning restore S125 // Sections of code should not be "commented out"
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

#pragma warning disable S125 // Sections of code should not be "commented out"
        //[Fact]
        //public async void Save_DoesNotSendShipmentChangedMessage_WhenTotalWeightSetToSameValue_Test()
        //{
        //    using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
        //    {
        //        shipmentEntity.TotalWeight = 2.93;

        //        ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

        //        testObject.ShipmentViewModel.TotalWeight = 2.93;
        //        testObject.Save();

        //        mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
        //    }
        //}

        //[Fact]
        //public async void Save_SendsShipmentChangedMessage_WhenOriginRatingFieldsChanged_Test()
        //{
        //    using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
        //    {
        //        mock.WithShipmentTypeFromFactory(type => type.SetupGet(x => x.RatingFields).CallBase());

        //        ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

        //        testObject.Origin.CountryCode = "XX";
        //        testObject.Origin.Street = "XX";
        //        testObject.Origin.StateProvCode = "XX";
        //        testObject.Origin.PostalCode = "XX";
        //        testObject.Origin.City = "XX";
        //        mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(5));
        //    }
        //}

        //[Fact]
        //public async void Save_SendsShipmentChangedMessage_WhenDestinationRatingFieldsChanged_Test()
        //{
        //    using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
        //    {
        //        mock.WithShipmentTypeFromFactory(type => type.SetupGet(x => x.RatingFields).CallBase());

        //        ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

        //        testObject.Destination.CountryCode = "XX";
        //        testObject.Destination.Street = "XX";
        //        testObject.Destination.StateProvCode = "XX";
        //        testObject.Destination.PostalCode = "XX";
        //        testObject.Destination.City = "XX";
        //        mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<ShipmentChangedMessage>()), Times.Exactly(5));
        //    }
        //}

#pragma warning restore S125 // Sections of code should not be "commented out"
        
        [Fact]
        public async void SetShipmentType_UpdatesShipmentType_WhenChanged()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.OnTrac;
                Assert.Equal(ShipmentTypeCode.OnTrac, testObject.ShipmentType);
            }
        }

        [Fact]
        public async void SetShipmentType_CallsEnsureShipmentLoaded_WhenChanged()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.OnTrac;

                mock.Mock<IShippingManager>()
                    .Verify(x => x.EnsureShipmentLoaded(shipmentEntity));
            }
        }

        [Fact]
        public async void SetShipmentType_DoesNotCallEnsureShipmentLoaded_WhenNotChanged()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = shipmentEntity.ShipmentTypeCode;

                mock.Mock<IShippingManager>()
                    .Verify(x => x.EnsureShipmentLoaded(It.IsAny<ShipmentEntity>()), Times.Never);
            }
        }

        [Fact]
        public async void SetShipmentType_GetsShipmentType_WhenChanged()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.FedEx;

                mock.Mock<IShipmentTypeFactory>()
                    .Verify(x => x.Get(ShipmentTypeCode.FedEx));
            }
        }

        [Fact]
        public async void SetShipmentType_DoesNotGetShipmentType_WhenNotChanged()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = shipmentEntity.ShipmentTypeCode;

                mock.Mock<IShipmentTypeFactory>()
                    .Verify(x => x.Get(It.IsAny<ShipmentTypeCode>()), Times.Never);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void SetShipmentType_UpdatesSupportsMultiplePackagesWithValueFromShipmentType_WhenChanged(bool supportsMultiplePackages)
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentTypeFromFactory(type =>
                {
                    type.SetupGet(x => x.SupportsMultiplePackages).Returns(supportsMultiplePackages);
                });

                shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Usps;
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.FedEx;

                Assert.Equal(supportsMultiplePackages, testObject.SupportsMultiplePackages);
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

                Assert.Equal(ShippingPanelLoadedShipmentResult.Multiple, testObject.LoadedShipmentResult);
            }
        }

        [Fact]
        public async void Save_DoesNotCallSaveToDatabase_WhenShipmentIsProcessed()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                shipmentEntity.Processed = true;
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

                testObject.SaveToDatabase();

                mock.Mock<IShippingManager>()
                    .Verify(x => x.SaveShipmentsToDatabase(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<ValidatedAddressScope>(), It.IsAny<bool>()), Times.Never);
            }
        }

        [Fact]
        public void Save_DoesNotCallSaveToDatabase_WhenLoadedShipmentIsNull()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();

                testObject.SaveToDatabase();

                mock.Mock<IShippingManager>()
                    .Verify(x => x.SaveShipmentsToDatabase(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<ValidatedAddressScope>(), It.IsAny<bool>()), Times.Never);
            }
        }

        [Fact]
        public async void Save_DoesNotCallSaveToDatabase_WhenShipmentIsNull()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                shippingPanelLoadedShipment.Shipment = null;
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

                testObject.SaveToDatabase();

                mock.Mock<IShippingManager>()
                    .Verify(x => x.SaveShipmentToDatabase(It.IsAny<ShipmentEntity>(), It.IsAny<ValidatedAddressScope>(), It.IsAny<bool>()), Times.Never);
            }
        }

        [Fact]
        public async void Save_DoesNotUpdateShipment_WhenShipmentIsProcessed()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Usps;
                shipmentEntity.Processed = true;

                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.OnTrac;

                testObject.SaveToDatabase();

                Assert.Equal(ShipmentTypeCode.Usps, shipmentEntity.ShipmentTypeCode);
            }
        }

        [Fact]
        public async void Save_UpdatesShipment_WhenShipmentIsNotProcessed()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Usps;

                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.OnTrac;

                testObject.SaveToDatabase();

                Assert.Equal(ShipmentTypeCode.OnTrac, shipmentEntity.ShipmentTypeCode);
            }
        }

        [Fact]
        public async void Save_CallsSaveToDatabase_WhenShipmentIsNotProcessed()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                
                testObject.SaveToDatabase();

                mock.Mock<IShippingManager>()
                    .Verify(x => x.SaveShipmentToDatabase(shipmentEntity, It.IsAny<ValidatedAddressScope>(), false));
            }
        }
    }
}
