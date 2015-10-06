﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using ShipWorks.Core.Messaging;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.Loading;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;
using ShipWorks.Core.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel
{
    public class ShippingPanelViewModelTest
    {
        private readonly OrderEntity orderEntity;
        private readonly StoreEntity storeEntity;
        private readonly ShipmentEntity shipmentEntity;
        private OrderSelectionLoaded orderSelectionLoaded;

        public ShippingPanelViewModelTest()
        {

            storeEntity = new StoreEntity(1005)
            {
                City = "Saint Louis",
                Company = "ShipWorks",
                CountryCode = "US",
                Email = "mirza@shipworks.com",
                Phone = "8885551212",
                PostalCode = "63102",
                StateProvCode = "MO",
                StoreName = "A Store",
                Street1 = "1 memorial drive",
                Street2 = "Suite 2000",
                Street3 = "C/O Mirza",
                Website = "www.shipworks.com"
            };

            orderEntity = new OrderEntity(1006)
            {
                Store = storeEntity,
                StoreID = storeEntity.StoreID,
                RequestedShipping = "Ground"
            };

            shipmentEntity = new ShipmentEntity(1031)
            {
                ShipmentTypeCode = ShipmentTypeCode.Other,
                OriginOriginID = (int)ShipmentOriginSource.Store
            };

            shipmentEntity.Order = orderEntity;
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity, new List<ShipmentEntity>() {shipmentEntity});
        }

        private async Task<ShippingPanelViewModel> GetViewModelWithLoadedShipment(AutoMock mock)
        {
            OrderSelectionChangedMessage message = new OrderSelectionChangedMessage(null, new List<OrderSelectionLoaded> {orderSelectionLoaded});
            
            ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
            await testObject.LoadOrder(message);

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
                OrderSelectionChangedMessage message = new OrderSelectionChangedMessage(null, new List<OrderSelectionLoaded> { orderSelectionLoaded });

                ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
                await testObject.LoadOrder(message);

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
        public async void StoreChangedMessage_ChangesShipmentOriginAddress_WhenOriginTypeIsStore_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                shipmentEntity.OriginOriginID = (int) ShipmentOriginSource.Store;

                IMessenger messenger = new TestMessenger();
                mock.Provide<IMessenger>(messenger);

                PersonAdapter newStoreAddress = new PersonAdapter(storeEntity, null)
                {
                    Street1 = "It changed!"
                };

                mock.Mock<IShippingOriginManager>().Setup(s => s.GetOriginAddress(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()))
                    .Returns(newStoreAddress);

                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                
                messenger.Send(new StoreChangedMessage(null, storeEntity));

                Assert.Equal(newStoreAddress.StreetAll, testObject.Origin.Street);
            }
        }

        [Fact]
        public async void StoreChangedMessage_DoesNotChangeShipmentOriginAddress_WhenOriginTypeIsOther_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                IMessenger messenger = mock.Provide<IMessenger>(new TestMessenger());

                PersonAdapter newStoreAddress = new PersonAdapter(storeEntity, null)
                {
                    Street1 = "It changed!"
                };

                mock.Mock<IShippingOriginManager>().Setup(s => s.GetOriginAddress(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()))
                    .Returns(newStoreAddress);

                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                testObject.OriginAddressType = (long)ShipmentOriginSource.Other;
                testObject.Origin.Street = "Foo";

                messenger.Send(new StoreChangedMessage(null, storeEntity));

                Assert.Equal("Foo", testObject.Origin.Street);
            }
        }

        [Fact]
        public async void StoreChangedMessage_DoesNotChangeShipmentOriginAddress_WhenOriginTypeIsAccount_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                IMessenger messenger = mock.Provide<IMessenger>(new TestMessenger());

                PersonAdapter newStoreAddress = new PersonAdapter(storeEntity, null)
                {
                    Street1 = "It changed!"
                };

                mock.Mock<IShippingOriginManager>()
                    .Setup(s => s.GetOriginAddress(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()))
                    .Returns(newStoreAddress);

                ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);
                testObject.OriginAddressType = (long)ShipmentOriginSource.Account;
                testObject.Origin.Street = "Foo";

                messenger.Send(new StoreChangedMessage(null, storeEntity));
                
                Assert.Equal("Foo", testObject.Origin.Street);
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
        public async void Load_ShowsMessage_WhenMultipleShipmentsAreLoaded()
        {
            Assert.False(true, "Fix this test to test show message.");

            //orderSelectionLoaded.Result = ShippingPanelLoadedShipmentResult.Multiple;

            //using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            //{
            //    ShippingPanelViewModel testObject = await GetViewModelWithLoadedShipment(mock);

            //    Assert.Equal(ShippingPanelLoadedShipmentResult.Multiple, testObject.LoadedShipmentResult);
            //}
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
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity, null);

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
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
