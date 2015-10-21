using System;
using System.Collections.Generic;
using System.Windows;
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
using ShipWorks.Tests.Shared;
using Xunit;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.Shipping;
using System.Reactive.Subjects;
using Autofac.Core.Activators.Reflection;
using ShipWorks.Shipping.UI.ShippingPanel.AddressControl;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel
{
    public class ShippingPanelViewModelTest
    {
        private readonly OrderEntity orderEntity;
        private readonly StoreEntity storeEntity;
        private readonly ShipmentEntity shipmentEntity;
        private OrderSelectionLoaded orderSelectionLoaded;

        private Mock<ICarrierShipmentAdapterFactory> shipmentAdapterFactory;
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;

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
                RequestedShipping = "Ground",
                ShipCity = "Saint Louis",
                ShipCompany = "Ship Company",
                ShipCountryCode = "US",
                ShipEmail = "ShipEmail@test.com",
                ShipFax = "",
                ShipFirstName = "ShipFirstName",
                ShipLastName = "ShipLastName",
                ShipMiddleName = "ShipMiddleName",
                ShipMilitaryAddress = 0,
                ShipPOBox = 0,
                ShipPhone = "8885551212",
                ShipPostalCode = "63109",
                ShipStateProvCode = "MO",
                ShipStreet1 = "1 Memorial Drive",
                ShipStreet2 = "Suite 2000",
                ShipStreet3 = "",
                ShipWebsite = "www.shipworks.com"
            };

            shipmentEntity = new ShipmentEntity(1031)
            {
                ShipmentTypeCode = ShipmentTypeCode.Other,
                OriginOriginID = (int)ShipmentOriginSource.Store,
                OriginCity = "Saint Louis",
                OriginCompany = "Origin Company",
                OriginCountryCode = "US",
                OriginEmail = "OriginEmail@test.com",
                OriginFax = "",
                OriginFirstName = "OriginFirstName",
                OriginLastName = "OriginLastName",
                OriginMiddleName = "OriginMiddleName",
                OriginPhone = "8885551212",
                OriginPostalCode = "63109",
                OriginStateProvCode = "MO",
                OriginStreet1 = "1 Memorial Drive",
                OriginStreet2 = "Suite 2000",
                OriginStreet3 = "",
                OriginWebsite = "www.Originworks.com",
                ShipCity = "Saint Louis",
                ShipCompany = "Ship Company",
                ShipCountryCode = "US",
                ShipEmail = "ShipEmail@test.com",
                ShipFirstName = "ShipFirstName",
                ShipLastName = "ShipLastName",
                ShipMiddleName = "ShipMiddleName",
                ShipMilitaryAddress = 0,
                ShipPOBox = 0,
                ShipPhone = "8885551212",
                ShipPostalCode = "63109",
                ShipStateProvCode = "MO",
                ShipStreet1 = "1 Memorial Drive",
                ShipStreet2 = "Suite 2000",
                ShipStreet3 = ""
            };

            shipmentEntity.Order = orderEntity;

            shipmentAdapter = new Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(s => s.Shipment).Returns(shipmentEntity);

            shipmentAdapterFactory = new Mock<ICarrierShipmentAdapterFactory>();
            shipmentAdapterFactory.Setup(s => s.Get(It.IsAny<ShipmentEntity>())).Returns(shipmentAdapter.Object);

            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );
        }

        private ShippingPanelViewModel GetViewModelWithLoadedShipment(AutoMock mock)
        {
            OrderSelectionChangedMessage message = new OrderSelectionChangedMessage(null, new List<OrderSelectionLoaded> {orderSelectionLoaded});
            
            ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
            testObject.LoadOrder(message);

            // Reset mocks so that tests don't have to worry about calls made during loading
            mock.Mock<IShipmentTypeFactory>().ResetCalls();
            mock.Mock<IShippingManager>().ResetCalls();

            return testObject;
        }

        [Fact]
        public void Save_DelegatesToDestination_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Mock<AddressViewModel> destinationAddress = new Mock<AddressViewModel>();

                mock.Mock<IShippingViewModelFactory>()
                    .SetupSequence(s => s.GetAddressViewModel())
                    .Returns(new Mock<AddressViewModel>().Object)
                    .Returns(destinationAddress.Object);

                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                
                testObject.Save();

                destinationAddress.Verify(x => x.SaveToEntity(shipmentEntity.ShipPerson));
            }
        }

        [Fact]
        public void Save_DelegatesToOrigin_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Mock<AddressViewModel> originAddress = new Mock<AddressViewModel>();

                mock.Mock<IShippingViewModelFactory>()
                    .SetupSequence(s => s.GetAddressViewModel())
                    .Returns(originAddress.Object)
                    .Returns(new Mock<AddressViewModel>().Object);

                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

                testObject.Save();

                originAddress.Verify(x => x.SaveToEntity(shipmentEntity.OriginPerson));
            }
        }

        [Fact]
        public void Save_DoesNotSendShipmentChangedMessage_WhenLoadingOrderTest()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                OrderSelectionChangedMessage message = new OrderSelectionChangedMessage(null, new List<OrderSelectionLoaded> { orderSelectionLoaded });
                
                ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
                testObject.LoadOrder(message);

                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
            }
        }

        [Fact]
        public void Save_DoesNotSendShipmentChangedMessage_WhenNothingChanged_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

                testObject.Save();

                mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
            }
        }

        [Fact]
        public void StoreChangedMessage_ChangesShipmentOriginAddress_WhenOriginTypeIsStore_Test()
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
                
                Mock<AddressViewModel> addressViewModel = new Mock<AddressViewModel>();

                mock.Mock<IShippingViewModelFactory>().Setup(s => s.GetAddressViewModel()).Returns(addressViewModel.Object);

                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                
                messenger.Send(new StoreChangedMessage(null, storeEntity));

                addressViewModel.Verify(s => s.SetAddressFromOrigin(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()), Times.AtLeastOnce);
            }
        }

        [Fact]
        public void StoreChangedMessage_DoesNotChangeShipmentOriginAddress_WhenOriginTypeIsOther_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Mock<AddressViewModel> originAddress = mock.WithOriginAddressViewModel();

                IMessenger messenger = mock.Provide<IMessenger>(new TestMessenger());

                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.OriginAddressType = (long)ShipmentOriginSource.Other;

                originAddress.ResetCalls();
                messenger.Send(new StoreChangedMessage(null, storeEntity));

                originAddress.Verify(x => x.SetAddressFromOrigin(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()), Times.Never);
            }
        }

        [Fact]
        public void StoreChangedMessage_DoesNotChangeShipmentOriginAddress_WhenOriginTypeIsAccount_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Mock<AddressViewModel> originAddress = mock.WithOriginAddressViewModel();

                IMessenger messenger = mock.Provide<IMessenger>(new TestMessenger());
                
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.OriginAddressType = (long)ShipmentOriginSource.Account;

                originAddress.ResetCalls();
                messenger.Send(new StoreChangedMessage(null, storeEntity));

                originAddress.Verify(x => x.SetAddressFromOrigin(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()), Times.Never);
            }
        }

#pragma warning disable S125 // Sections of code should not be "commented out"
        //[Fact]
        //public void Save_DoesNotSendShipmentChangedMessage_WhenTotalWeightSetToSameValue_Test()
        //{
        //    using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
        //    {
        //        shipmentEntity.TotalWeight = 2.93;

        //        ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

        //        testObject.ShipmentViewModel.TotalWeight = 2.93;
        //        testObject.Save();

        //        mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
        //    }
        //}

        //[Fact]
        //public void Save_SendsShipmentChangedMessage_WhenOriginRatingFieldsChanged_Test()
        //{
        //    using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
        //    {
        //        mock.WithShipmentTypeFromFactory(type => type.SetupGet(x => x.RatingFields).CallBase());

        //        ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

        //        testObject.Origin.CountryCode = "XX";
        //        testObject.Origin.Street = "XX";
        //        testObject.Origin.StateProvCode = "XX";
        //        testObject.Origin.PostalCode = "XX";
        //        testObject.Origin.City = "XX";
        //        mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Exactly(5));
        //    }
        //}

        //[Fact]
        //public void Save_SendsShipmentChangedMessage_WhenDestinationRatingFieldsChanged_Test()
        //{
        //    using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
        //    {
        //        mock.WithShipmentTypeFromFactory(type => type.SetupGet(x => x.RatingFields).CallBase());

        //        ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

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
        public void SetShipmentType_UpdatesShipmentType_WhenChanged()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.OnTrac;
                Assert.Equal(ShipmentTypeCode.OnTrac, testObject.ShipmentType);
            }
        }

        [Fact]
        public void SetShipmentType_CallsEnsureShipmentLoaded_WhenChanged()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.OnTrac;

                mock.Mock<IShippingManager>()
                    .Verify(x => x.EnsureShipmentLoaded(shipmentEntity));
            }
        }

        [Fact]
        public void SetShipmentType_DoesNotCallEnsureShipmentLoaded_WhenNotChanged()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = shipmentEntity.ShipmentTypeCode;

                mock.Mock<IShippingManager>()
                    .Verify(x => x.EnsureShipmentLoaded(It.IsAny<ShipmentEntity>()), Times.Never);
            }
        }

        [Fact]
        public void SetShipmentType_GetsShipmentType_WhenChanged()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.FedEx;

                mock.Mock<IShipmentTypeFactory>()
                    .Verify(x => x.Get(ShipmentTypeCode.FedEx));
            }
        }

        [Fact]
        public void SetShipmentType_DoesNotGetShipmentType_WhenNotChanged()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = shipmentEntity.ShipmentTypeCode;

                mock.Mock<IShipmentTypeFactory>()
                    .Verify(x => x.Get(It.IsAny<ShipmentTypeCode>()), Times.Never);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetShipmentType_UpdatesSupportsMultiplePackagesWithValueFromShipmentType_WhenChanged(bool supportsMultiplePackages)
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.WithShipmentTypeFromFactory(type =>
                {
                    type.SetupGet(x => x.SupportsMultiplePackages).Returns(supportsMultiplePackages);
                });

                shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Usps;
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.FedEx;

                Assert.Equal(supportsMultiplePackages, testObject.SupportsMultiplePackages);
            }
        }

        [Fact]
        public void Load_LoadedShipmentResult_IsSuccess_WhenMultipleShipmentsAreLoaded()
        {
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

                Assert.Equal(ShippingPanelLoadedShipmentResult.Success, testObject.LoadedShipmentResult);
            }
        }

        [Fact]
        public void Load_LoadedShipmentResult_IsMultiple_WhenMultipleShipmentsAreLoaded()
        {
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity), shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

                Assert.Equal(ShippingPanelLoadedShipmentResult.Multiple, testObject.LoadedShipmentResult);
            }
        }

        [Fact]
        public void Load_LoadedShipmentResult_IsNotCreated_WhenNoShipmentsAreLoaded()
        {
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity,
                new List<ICarrierShipmentAdapter>() { },
                ShippingAddressEditStateType.Editable
                );

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

                Assert.Equal(ShippingPanelLoadedShipmentResult.NotCreated, testObject.LoadedShipmentResult);
            }
        }

        [Fact]
        public void Load_LoadedShipmentResult_IsError_WhenNullList()
        {
            orderSelectionLoaded = new OrderSelectionLoaded(new Exception());

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
                testObject.LoadOrder(new OrderSelectionChangedMessage(this, new[] { new OrderSelectionLoaded(new Exception()) }));
                
                Assert.Equal(ShippingPanelLoadedShipmentResult.Error, testObject.LoadedShipmentResult);
            }
        }

        [Fact]
        public void Save_DoesNotCallSaveToDatabase_WhenShipmentIsProcessed()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                shipmentEntity.Processed = true;
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

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
        public void Save_DoesNotCallSaveToDatabase_WhenShipmentIsNull()
        {
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity, null, ShippingAddressEditStateType.Editable);

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

                testObject.SaveToDatabase();

                mock.Mock<IShippingManager>()
                    .Verify(x => x.SaveShipmentToDatabase(It.IsAny<ShipmentEntity>(), It.IsAny<ValidatedAddressScope>(), It.IsAny<bool>()), Times.Never);
            }
        }

        [Fact]
        public void Save_DoesNotUpdateShipment_WhenShipmentIsProcessed()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Usps;
                shipmentEntity.Processed = true;

                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.OnTrac;

                testObject.SaveToDatabase();

                Assert.Equal(ShipmentTypeCode.Usps, shipmentEntity.ShipmentTypeCode);
            }
        }

        [Fact]
        public void Save_UpdatesShipment_WhenShipmentIsNotProcessed()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Usps;

                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.OnTrac;

                testObject.SaveToDatabase();

                Assert.Equal(ShipmentTypeCode.OnTrac, shipmentEntity.ShipmentTypeCode);
            }
        }

        [Fact]
        public void Save_CallsSaveToDatabase_WhenShipmentIsNotProcessed()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                
                testObject.SaveToDatabase();

                mock.Mock<IShippingManager>()
                    .Verify(x => x.SaveShipmentToDatabase(shipmentEntity, It.IsAny<ValidatedAddressScope>(), false));
            }
        }

        [Fact]
        public void Load_AccountVisibility_IsVisible_WhenShipmentType_IsUsps()
        {
            shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Usps;
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

                Assert.Equal(Visibility.Visible, testObject.AccountVisibility);
            }
        }

        [Fact]
        public void Load_AccountVisibility_IsCollapsed_WhenShipmentType_IsPostalWebTools()
        {
            shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.PostalWebTools;
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

                Assert.Equal(Visibility.Collapsed, testObject.AccountVisibility);
            }
        }

        [Fact]
        public void ShipmentTypeChanged_AccountVisibility_IsVisible_WhenShipmentType_IsUsps()
        {
            shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Usps;
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.BestRate;
                testObject.ShipmentType = ShipmentTypeCode.Usps;

                Assert.Equal(Visibility.Visible, testObject.AccountVisibility);
            }
        }

        [Fact]
        public void ShipmentTypeChanged_AccountVisibility_IsCollapsed_WhenShipmentType_IsPostalWebTools()
        {
            shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.PostalWebTools;
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                testObject.ShipmentType = ShipmentTypeCode.Usps;
                testObject.ShipmentType = ShipmentTypeCode.BestRate;

                Assert.Equal(Visibility.Collapsed, testObject.AccountVisibility);
            }
        }

        [Fact]
        public void ShipmentDeletedMessage_WhenCurrentShipmentIsDeleted_UpdatesStatus()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                TestMessenger messenger = new TestMessenger();
                mock.Provide<IMessenger>(messenger);

                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                messenger.Send(new ShipmentDeletedMessage(this, shipmentEntity.ShipmentID));

                Assert.Equal(ShippingPanelLoadedShipmentResult.Deleted, testObject.LoadedShipmentResult);
            }
        }

        [Fact]
        public void ShipmentDeletedMessage_WhenOtherShipmentIsDeleted_DoesNotUpdateStatus()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                TestMessenger messenger = new TestMessenger();
                mock.Provide<IMessenger>(messenger);

                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                messenger.Send(new ShipmentDeletedMessage(this, shipmentEntity.ShipmentID + 1));

                Assert.NotEqual(ShippingPanelLoadedShipmentResult.Error, testObject.LoadedShipmentResult);
            }
        }

        [Fact]
        public void DestinationAddressEditableState_IsSet_AfterLoad()
        {
            orderSelectionLoaded = new OrderSelectionLoaded(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Processed
                );

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                
                Assert.Equal(ShippingAddressEditStateType.Processed, testObject.DestinationAddressEditableState);
            }
        }
    }
}
