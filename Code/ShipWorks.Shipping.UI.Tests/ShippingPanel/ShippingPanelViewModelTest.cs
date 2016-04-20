using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.Tests.Shared;
using ShipWorks.UI.Controls.AddressControl;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel
{
    public class ShippingPanelViewModelTest : IDisposable
    {
        private readonly OrderEntity orderEntity;
        private readonly StoreEntity storeEntity;
        private readonly ShipmentEntity shipmentEntity;
        private LoadedOrderSelection orderSelectionLoaded;
        private readonly AutoMock mock;
        Mock<Func<ISecurityContext>> getSecurityContext;
        Mock<ISecurityContext> securityContext;

        private readonly Mock<ICarrierShipmentAdapterFactory> shipmentAdapterFactory;
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private Mock<IShippingErrorManager> shippingErrorManager;

        public ShippingPanelViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            securityContext = mock.Mock<ISecurityContext>();
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(false);

            getSecurityContext = mock.MockRepository.Create<Func<ISecurityContext>>();
            getSecurityContext.Setup(sc => sc()).Returns(securityContext.Object);
            mock.Provide<Func<ISecurityContext>>(getSecurityContext.Object);

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
                OriginOriginID = (int) ShipmentOriginSource.Store,
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

            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(s => s.Shipment).Returns(() => shipmentEntity);
            shipmentAdapter.Setup(s => s.ShipmentTypeCode).Returns(() => shipmentEntity.ShipmentTypeCode);
            shipmentAdapter.Setup(s => s.SupportsAccounts).Returns(() =>
            {
                return !(shipmentAdapter.Object.ShipmentTypeCode == ShipmentTypeCode.PostalWebTools ||
                         shipmentAdapter.Object.ShipmentTypeCode == ShipmentTypeCode.BestRate ||
                         shipmentAdapter.Object.ShipmentTypeCode == ShipmentTypeCode.Other ||
                         shipmentAdapter.Object.ShipmentTypeCode == ShipmentTypeCode.None);
            });
            shipmentAdapter.Setup(s => s.UpdateDynamicData()).Returns(new Dictionary<ShipmentEntity, Exception>());

            shipmentAdapterFactory = new Mock<ICarrierShipmentAdapterFactory>();
            shipmentAdapterFactory.Setup(s => s.Get(It.IsAny<ShipmentEntity>())).Returns(() => shipmentAdapter.Object);

            orderSelectionLoaded = new LoadedOrderSelection(orderEntity,
                new List<ICarrierShipmentAdapter> { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );


            shippingErrorManager = mock.Mock<IShippingErrorManager>();
        }

        private ShippingPanelViewModel GetViewModelWithLoadedShipment(AutoMock autoMock)
        {
            OrderSelectionChangedMessage message = new OrderSelectionChangedMessage(null, new IOrderSelection[] { orderSelectionLoaded });

            ShippingPanelViewModel testObject = autoMock.Create<ShippingPanelViewModel>();
            testObject.LoadOrder(message);

            // Reset mocks so that tests don't have to worry about calls made during loading
            autoMock.Mock<IShippingManager>().ResetCalls();

            return testObject;
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Save_DelegatesToDestination_Test(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            Mock<AddressViewModel> destinationAddress = new Mock<AddressViewModel>();

            mock.Mock<IShippingViewModelFactory>()
                .SetupSequence(s => s.GetAddressViewModel())
                .Returns(new Mock<AddressViewModel>().Object)
                .Returns(destinationAddress.Object);

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            testObject.Save();

            destinationAddress.Verify(x => x.SaveToEntity(shipmentEntity.ShipPerson), hasPermission ? Times.Once() : Times.Never());
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Save_DelegatesToOrigin_Test(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            Mock<AddressViewModel> originAddress = new Mock<AddressViewModel>();

            mock.Mock<IShippingViewModelFactory>()
                .SetupSequence(s => s.GetAddressViewModel())
                .Returns(originAddress.Object)
                .Returns(new Mock<AddressViewModel>().Object);

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            testObject.Save();

            originAddress.Verify(x => x.SaveToEntity(shipmentEntity.OriginPerson), hasPermission ? Times.Once() : Times.Never());
        }

        [Fact]
        public void Save_SendsOneShipmentChangedMessage_WhenLoadingOrderTest()
        {
            OrderSelectionChangedMessage message = new OrderSelectionChangedMessage(null, new IOrderSelection[] { orderSelectionLoaded });

            ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
            testObject.LoadOrder(message);

            mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Once);
        }

        [Fact]
        public void Save_DoesNotSendShipmentChangedMessage_WhenNothingChanged_Test()
        {
            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
            mock.Mock<IMessenger>().ResetCalls();

            testObject.Save();

            mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<IShipWorksMessage>()), Times.Never);
        }

        [Theory]
        [InlineData(ShippingPanelLoadedShipmentResult.Deleted)]
        [InlineData(ShippingPanelLoadedShipmentResult.Error)]
        [InlineData(ShippingPanelLoadedShipmentResult.Multiple)]
        [InlineData(ShippingPanelLoadedShipmentResult.NotCreated)]
        [InlineData(ShippingPanelLoadedShipmentResult.UnsupportedShipmentType)]
        public void Save_DoesNotDelegate_WhenLoadedShipmentResult_IsNotSuccess_Test(ShippingPanelLoadedShipmentResult shippingPanelLoadedShipmentResult)
        {
            Mock<ShipmentViewModel> shipmentViewModel = mock.CreateMock<ShipmentViewModel>();

            mock.Mock<IShippingViewModelFactory>()
                .SetupSequence(s => s.GetShipmentViewModel(It.IsAny<ShipmentTypeCode>()))
                .Returns(shipmentViewModel.Object)
                .Returns(new Mock<ShipmentViewModel>().Object);

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
            testObject.LoadedShipmentResult = shippingPanelLoadedShipmentResult;

            mock.Mock<IMessenger>().ResetCalls();

            testObject.Save();

            shipmentViewModel.Verify(x => x.Save(), Times.Never);
        }

        [Theory]
        [InlineData(ShippingPanelLoadedShipmentResult.Success, true, true)]
        [InlineData(ShippingPanelLoadedShipmentResult.Success, false, false)]
        public void Save_DoesDelegate_WhenLoadedShipmentResult_IsSuccess_Test(ShippingPanelLoadedShipmentResult shippingPanelLoadedShipmentResult, bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            Mock<ShipmentViewModel> shipmentViewModel = mock.CreateMock<ShipmentViewModel>();

            mock.Mock<IShippingViewModelFactory>()
                .SetupSequence(s => s.GetShipmentViewModel(It.IsAny<ShipmentTypeCode>()))
                .Returns(shipmentViewModel.Object)
                .Returns(new Mock<ShipmentViewModel>().Object);

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
            testObject.LoadedShipmentResult = shippingPanelLoadedShipmentResult;

            mock.Mock<IMessenger>().ResetCalls();

            testObject.Save();

            shipmentViewModel.Verify(x => x.Save(), hasPermission ? Times.Once() : Times.Never());
        }

        [Fact]
        public void StoreChangedMessage_ChangesShipmentOriginAddress_WhenOriginTypeIsStore_Test()
        {
            shipmentEntity.OriginOriginID = (int) ShipmentOriginSource.Store;

            IMessenger messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            Mock<AddressViewModel> addressViewModel = new Mock<AddressViewModel>();

            mock.Mock<IShippingViewModelFactory>().Setup(s => s.GetAddressViewModel()).Returns(addressViewModel.Object);

            GetViewModelWithLoadedShipment(mock);

            messenger.Send(new StoreChangedMessage(null, storeEntity));

            addressViewModel.Verify(s => s.SetAddressFromOrigin(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()), Times.AtLeastOnce);
        }

        [Fact]
        public void StoreChangedMessage_DoesNotChangeShipmentOriginAddress_WhenOriginTypeIsOther_Test()
        {
            Mock<AddressViewModel> originAddress = mock.WithOriginAddressViewModel();

            IMessenger messenger = mock.Provide<IMessenger>(new TestMessenger());

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
            testObject.OriginAddressType = (long) ShipmentOriginSource.Other;

            originAddress.ResetCalls();
            messenger.Send(new StoreChangedMessage(null, storeEntity));

            originAddress.Verify(x => x.SetAddressFromOrigin(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()), Times.Never);
        }

        [Fact]
        public void StoreChangedMessage_DoesNotChangeShipmentOriginAddress_WhenOriginTypeIsAccount_Test()
        {
            Mock<AddressViewModel> originAddress = mock.WithOriginAddressViewModel();

            IMessenger messenger = mock.Provide<IMessenger>(new TestMessenger());

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
            testObject.OriginAddressType = (long) ShipmentOriginSource.Account;

            originAddress.ResetCalls();
            messenger.Send(new StoreChangedMessage(null, storeEntity));

            originAddress.Verify(x => x.SetAddressFromOrigin(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()), Times.Never);
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
            mock.WithCarrierShipmentAdapterFromChangeShipment(x =>
                x.SetupGet(a => a.ShipmentTypeCode).Returns(ShipmentTypeCode.OnTrac));

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
            testObject.ShipmentType = ShipmentTypeCode.OnTrac;

            Assert.Equal(ShipmentTypeCode.OnTrac, testObject.ShipmentType);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Populate_SetsSupportsMultiplePackages_WithValueFromShipmentType(bool supportsMultiplePackages)
        {
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(supportsMultiplePackages);

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
            testObject.LoadShipment(shipmentAdapter.Object);

            Assert.Equal(supportsMultiplePackages, testObject.SupportsMultiplePackages);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Populate_SetsSupportsAccounts_WhenCalled(bool supportsAccounts)
        {
            shipmentAdapter.SetupGet(a => a.SupportsAccounts).Returns(supportsAccounts);

            var testObject = GetViewModelWithLoadedShipment(mock);
            testObject.LoadShipment(shipmentAdapter.Object);

            Assert.Equal(supportsAccounts, testObject.SupportsAccounts);
        }

        [Fact]
        public void Load_LoadedShipmentResult_IsSuccess_WhenMultipleShipmentsAreLoaded()
        {
            orderSelectionLoaded = new LoadedOrderSelection(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            Assert.Equal(ShippingPanelLoadedShipmentResult.Success, testObject.LoadedShipmentResult);
        }

        [Fact]
        public void Load_LoadedShipmentResult_IsMultiple_WhenMultipleShipmentsAreLoaded()
        {
            orderSelectionLoaded = new LoadedOrderSelection(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity), shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            Assert.Equal(ShippingPanelLoadedShipmentResult.Multiple, testObject.LoadedShipmentResult);
        }

        [Fact]
        public void Load_LoadedShipmentResult_IsNotCreated_WhenNoShipmentsAreLoaded()
        {
            orderSelectionLoaded = new LoadedOrderSelection(orderEntity,
                new List<ICarrierShipmentAdapter>() { },
                ShippingAddressEditStateType.Editable
                );

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            Assert.Equal(ShippingPanelLoadedShipmentResult.NotCreated, testObject.LoadedShipmentResult);
        }

        [Fact]
        public void Load_LoadedShipmentResult_IsError_WhenNullList()
        {
            orderSelectionLoaded = new LoadedOrderSelection(new Exception(), null, null, ShippingAddressEditStateType.Editable);

            ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
            testObject.LoadOrder(new OrderSelectionChangedMessage(this, new IOrderSelection[] { new LoadedOrderSelection(new Exception(), null, null, ShippingAddressEditStateType.Editable) }));

            Assert.Equal(ShippingPanelLoadedShipmentResult.Error, testObject.LoadedShipmentResult);
        }

        [Fact]
        public void Load_LoadedShipmentResult_IsUnsupportedShipmentType_WhenAmazonShipmentType()
        {
            shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Amazon;
            orderSelectionLoaded = new LoadedOrderSelection(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            Assert.Equal(ShippingPanelLoadedShipmentResult.UnsupportedShipmentType, testObject.LoadedShipmentResult);
        }

        [Fact]
        public void Load_LoadedShipmentResult_IsSuccessShipmentType_WhenBestRateShipmentType()
        {
            shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.BestRate;
            orderSelectionLoaded = new LoadedOrderSelection(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            Assert.Equal(ShippingPanelLoadedShipmentResult.Success, testObject.LoadedShipmentResult);
        }

        [Fact]
        public void Save_DoesNotCallSaveToDatabase_WhenShipmentIsProcessed()
        {
            shipmentEntity.Processed = true;
            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            testObject.SaveToDatabase();

            mock.Mock<IShippingManager>()
                .Verify(x => x.SaveShipmentsToDatabase(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void Save_DoesNotCallSaveToDatabase_WhenLoadedShipmentIsNull()
        {
            shipmentAdapter = null;
            ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();

            testObject.SaveToDatabase();

            mock.Mock<IShippingManager>()
                .Verify(x => x.SaveShipmentsToDatabase(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void Save_DoesNotCallSaveToDatabase_WhenShipmentIsNull()
        {
            orderSelectionLoaded = new LoadedOrderSelection(orderEntity, null, ShippingAddressEditStateType.Editable);

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            testObject.SaveToDatabase();

            mock.Mock<IShippingManager>()
                .Verify(x => x.SaveShipmentToDatabase(It.IsAny<ShipmentEntity>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void Save_DoesNotUpdateShipment_WhenShipmentIsProcessed()
        {
            shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Usps;
            shipmentEntity.Processed = true;

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
            testObject.ShipmentType = ShipmentTypeCode.OnTrac;

            testObject.SaveToDatabase();

            Assert.Equal(ShipmentTypeCode.Usps, shipmentEntity.ShipmentTypeCode);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Save_CallsSaveToDatabase_WhenShipmentIsNotProcessed(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            testObject.SaveToDatabase();

            mock.Mock<IShippingManager>()
                .Verify(x => x.SaveShipmentToDatabase(shipmentEntity, false), hasPermission ? Times.Once() : Times.Never());
        }

        [Fact]
        public void Load_AccountVisibility_IsVisible_WhenShipmentType_IsUsps()
        {
            shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.Usps;
            orderSelectionLoaded = new LoadedOrderSelection(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void Load_AccountVisibility_IsCollapsed_WhenShipmentType_IsPostalWebTools()
        {
            shipmentEntity.ShipmentTypeCode = ShipmentTypeCode.PostalWebTools;
            orderSelectionLoaded = new LoadedOrderSelection(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Editable
                );

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            Assert.False(testObject.SupportsAccounts);
        }

        [Fact]
        public void Unload_UpdatesDetails_WhenCalled()
        {
            var testObject = GetViewModelWithLoadedShipment(mock);

            testObject.UnloadShipment();

            Assert.Null(testObject.ShipmentAdapter);
        }

        [Fact]
        public void ShipmentDeletedMessage_WhenOtherShipmentIsDeleted_DoesNotUpdateStatus()
        {
            using (TestMessenger messenger = new TestMessenger())
            {
                mock.Provide<IMessenger>(messenger);

                ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);
                messenger.Send(new ShipmentDeletedMessage(this, shipmentEntity.ShipmentID + 1));

                Assert.NotEqual(ShippingPanelLoadedShipmentResult.Error, testObject.LoadedShipmentResult);
            }
        }

        [Fact]
        public void DestinationAddressEditableState_IsSet_AfterLoad()
        {
            orderSelectionLoaded = new LoadedOrderSelection(orderEntity,
                new List<ICarrierShipmentAdapter>() { shipmentAdapterFactory.Object.Get(shipmentEntity) },
                ShippingAddressEditStateType.Processed
                );

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            Assert.Equal(ShippingAddressEditStateType.Processed, testObject.DestinationAddressEditableState);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Save_DelegatesToShipmentViewModelFactory_Test(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            Mock<ShipmentViewModel> shipmentViewModel = mock.CreateMock<ShipmentViewModel>();

            mock.Mock<IShippingViewModelFactory>()
                .SetupSequence(s => s.GetShipmentViewModel(It.IsAny<ShipmentTypeCode>()))
                .Returns(shipmentViewModel.Object)
                .Returns(new Mock<ShipmentViewModel>().Object);

            ShippingPanelViewModel testObject = GetViewModelWithLoadedShipment(mock);

            testObject.Save();

            shipmentViewModel.Verify(x => x.Save(), hasPermission ? Times.Once() : Times.Never());
        }

        [Fact]
        public void SaveToDatabase_DoesNotSave_WhenAllowEditingIsFalse()
        {
            var testShipmentAdapter = mock.Create<ICarrierShipmentAdapter>();

            List<ICarrierShipmentAdapter> shipmentAdapters = new List<ICarrierShipmentAdapter>()
            {
                testShipmentAdapter,
                mock.Create<ICarrierShipmentAdapter>()
            };

            var testObject = GetShippingPanelViewModelWithLoadedOrder(shipmentAdapters);

            testObject.LoadShipment(testShipmentAdapter);
            testObject.AllowEditing = false;

            testObject.SaveToDatabase();

            mock.Mock<IShippingManager>()
                .Verify(x => x.SaveShipmentToDatabase(It.IsAny<ShipmentEntity>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void SaveToDatabase_DoesNotSave_WhenShipmentAdapterIsNull()
        {
            var testObject = GetShippingPanelViewModelWithLoadedOrder();
            testObject.AllowEditing = true;

            testObject.SaveToDatabase();

            mock.Mock<IShippingManager>()
                .Verify(x => x.SaveShipmentToDatabase(It.IsAny<ShipmentEntity>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void SaveToDatabase_DoesNotSave_WhenShipmentIsProcessed()
        {
            var testObject = GetShippingPanelViewModelWithLoadedOrder();
            testObject.LoadShipment(mock.CreateMock<ICarrierShipmentAdapter>(s => s.Setup(x => x.Shipment).Returns(new ShipmentEntity())).Object);
            testObject.Shipment.Processed = true;
            testObject.AllowEditing = true;

            testObject.SaveToDatabase();

            mock.Mock<IShippingManager>()
                .Verify(x => x.SaveShipmentToDatabase(It.IsAny<ShipmentEntity>(), It.IsAny<bool>()), Times.Never);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void SaveToDatabase_CallsCommitBindings_WhenViewModelCanSave(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            var called = false;
            var testObject = GetShippingPanelViewModelWithLoadedOrder();
            testObject.LoadShipment(mock.CreateMock<ICarrierShipmentAdapter>().Object);
            testObject.CommitBindings = () => called = true;

            testObject.SaveToDatabase();

            Assert.Equal(expected, called);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void SaveToDatabase_DelegatesToShippingManager_WhenViewModelCanSave(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            var testObject = GetShippingPanelViewModelWithLoadedOrder();
            testObject.LoadShipment(mock.CreateMock<ICarrierShipmentAdapter>().Object);

            testObject.SaveToDatabase();

            mock.Mock<IShippingManager>()
                .Verify(x => x.SaveShipmentToDatabase(testObject.ShipmentAdapter.Shipment, false), hasPermission ? Times.Once() : Times.Never());
        }

        [Fact]
        public void SaveToDatabase_DoesNotDelegateToMessageHelper_WhenShippingManagerReturnsNoErrors()
        {
            var testObject = GetShippingPanelViewModelWithLoadedOrder();
            testObject.LoadShipment(mock.CreateMock<ICarrierShipmentAdapter>().Object);

            testObject.SaveToDatabase();

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void SaveToDatabase_DoesNotSendOrderSelectionChangedMessage_WhenShippingManagerReturnsNoErrors()
        {
            var testObject = GetShippingPanelViewModelWithLoadedOrder();
            testObject.LoadShipment(mock.CreateMock<ICarrierShipmentAdapter>().Object);

            testObject.SaveToDatabase();

            mock.Mock<IMessenger>()
                .Verify(x => x.Send(It.IsAny<OrderSelectionChangingMessage>()), Times.Never);
        }

        [Fact]
        public void SaveToDatabase_DoesNotDelegateToMessageHelper_WhenShippingManagerReturnsErrorsThatDoNotApply()
        {
            var testObject = GetShippingPanelViewModelWithLoadedOrder();
            testObject.LoadShipment(mock.CreateMock<ICarrierShipmentAdapter>().Object);
            mock.Mock<IShippingManager>()
                .Setup(x => x.SaveShipmentToDatabase(It.IsAny<ShipmentEntity>(), It.IsAny<bool>()))
                .Returns(new Dictionary<ShipmentEntity, Exception> { { new ShipmentEntity(), new Exception() } });

            testObject.SaveToDatabase();

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void SaveToDatabase_DoesNotSendOrderSelectionChangedMessage_WhenShippingManagerReturnsErrorsThatDoNotApply()
        {
            var testObject = GetShippingPanelViewModelWithLoadedOrder();
            testObject.LoadShipment(mock.CreateMock<ICarrierShipmentAdapter>().Object);
            mock.Mock<IShippingManager>()
                .Setup(x => x.SaveShipmentToDatabase(It.IsAny<ShipmentEntity>(), It.IsAny<bool>()))
                .Returns(new Dictionary<ShipmentEntity, Exception> { { new ShipmentEntity(), new Exception() } });

            testObject.SaveToDatabase();

            mock.Mock<IMessenger>()
                .Verify(x => x.Send(It.IsAny<OrderSelectionChangingMessage>()), Times.Never);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void SaveToDatabase_DelegatesToMessageHelper_WhenShippingManagerReturnsErrors(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            var testObject = GetShippingPanelViewModelWithLoadedOrder();
            testObject.LoadShipment(mock.CreateMock<ICarrierShipmentAdapter>(s => s.Setup(x => x.Shipment).Returns(new ShipmentEntity())).Object);
            mock.Mock<IShippingManager>()
                .Setup(x => x.SaveShipmentToDatabase(It.IsAny<ShipmentEntity>(), It.IsAny<bool>()))
                .Returns(new Dictionary<ShipmentEntity, Exception> { { testObject.Shipment, new Exception() } });

            testObject.SaveToDatabase();

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(It.IsAny<string>()), hasPermission ? Times.Once() : Times.Never());
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void SaveToDatabase_SendOrderSelectionChangedMessage_WhenShippingManagerReturnsErrors(bool expected, bool hasPermission)
        {
            securityContext.Setup(sc => sc.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>())).Returns(hasPermission);

            var testObject = GetShippingPanelViewModelWithLoadedOrder();
            testObject.LoadShipment(mock.CreateMock<ICarrierShipmentAdapter>(s => s.Setup(x => x.Shipment).Returns(new ShipmentEntity())).Object);
            mock.Mock<IShippingManager>()
                .Setup(x => x.SaveShipmentToDatabase(It.IsAny<ShipmentEntity>(), It.IsAny<bool>()))
                .Returns(new Dictionary<ShipmentEntity, Exception> { { testObject.Shipment, new Exception() } });

            testObject.SaveToDatabase();

            mock.Mock<IMessenger>()
                .Verify(x => x.Send(It.IsAny<OrderSelectionChangingMessage>()), hasPermission ? Times.Once() : Times.Never());
        }

        [Fact]
        public void OpenShippingDialogCommand_SendsOpenDialogMessage_WhenSingleOrderWithMultipleShipmentsLoaded()
        {
            var shipment = new ShipmentEntity();
            var testObject = mock.Create<ShippingPanelViewModel>();
            var orderSelection = new LoadedOrderSelection(new OrderEntity(), new[]
            {
                mock.CreateMock<ICarrierShipmentAdapter>(csa => csa.Setup(x => x.Shipment).Returns(shipment)).Object
            }, ShippingAddressEditStateType.Editable);
            testObject.LoadOrder(new OrderSelectionChangedMessage(this, new IOrderSelection[] { orderSelection }));

            testObject.OpenShippingDialogCommand.Execute(null);

            mock.Mock<IMessenger>()
                .Verify(x => x.Send(It.IsAny<OpenShippingDialogMessage>()));
        }

        [Fact]
        public void OpenShippingDialogCommand_SendsOpenShippingDialogWithOrdersMessage_WhenSingleOrderWithMultipleShipmentsLoaded()
        {
            var testObject = mock.Create<ShippingPanelViewModel>();
            var orderSelection = new LoadedOrderSelection(new OrderEntity(), new[]
            {
                mock.Create<ICarrierShipmentAdapter>(),
                mock.Create<ICarrierShipmentAdapter>()
            }, ShippingAddressEditStateType.Editable);
            testObject.LoadOrder(new OrderSelectionChangedMessage(this, new IOrderSelection[] { orderSelection }));

            testObject.OpenShippingDialogCommand.Execute(null);

            mock.Mock<IMessenger>()
                .Verify(x => x.Send(It.IsAny<OpenShippingDialogWithOrdersMessage>()));
        }

        [Fact]
        public void OpenShippingDialogCommand_SendsOpenShippingDialogWithOrdersMessage_WhenMultipleOrdersLoaded()
        {
            var testObject = mock.Create<ShippingPanelViewModel>();
            testObject.LoadOrder(new OrderSelectionChangedMessage(this, new[] {
                mock.Create<IOrderSelection>(),
                mock.Create<IOrderSelection>()
            }));

            testObject.OpenShippingDialogCommand.Execute(null);

            mock.Mock<IMessenger>()
                .Verify(x => x.Send(It.IsAny<OpenShippingDialogWithOrdersMessage>()));
        }

        [Fact]
        public void LoadOrder_SetsShipmentStatusToNone_WhenMultipleOrdersAreLoaded()
        {
            var testObject = mock.Create<ShippingPanelViewModel>();

            testObject.LoadOrder(new OrderSelectionChangedMessage(this, new[] {
                mock.Create<IOrderSelection>(),
                mock.Create<IOrderSelection>()
            }));

            Assert.Equal(ShipmentStatus.None, testObject.ShipmentStatus);
        }

        [Fact]
        public void LoadOrder_SetsShipmentStatusToNone_WhenSingleOrderWithMultipleShipmentsLoaded()
        {
            var testObject = mock.Create<ShippingPanelViewModel>();

            testObject.LoadOrder(new OrderSelectionChangedMessage(this, new IOrderSelection[] {
                new LoadedOrderSelection(new OrderEntity(), new [] {
                    mock.Create<ICarrierShipmentAdapter>(),
                    mock.Create<ICarrierShipmentAdapter>()
                }, ShippingAddressEditStateType.Editable)
            }));

            Assert.Equal(ShipmentStatus.None, testObject.ShipmentStatus);
        }

        [Theory]
        [InlineData(false, false, ShipmentStatus.Unprocessed)]
        [InlineData(true, false, ShipmentStatus.Processed)]
        [InlineData(true, true, ShipmentStatus.Voided)]
        public void LoadOrder_SetsShipmentStatusToUnprocessed_WhenSingleUnprocessShipmentLoaded(
            bool processed, bool voided, ShipmentStatus expected)
        {
            var shipment = new ShipmentEntity { Processed = processed, Voided = voided };

            var testObject = mock.Create<ShippingPanelViewModel>();

            testObject.LoadOrder(new OrderSelectionChangedMessage(this, new IOrderSelection[] {
                new LoadedOrderSelection(new OrderEntity(), new [] {
                    mock.CreateMock<ICarrierShipmentAdapter>(c => c.Setup(x => x.Shipment).Returns(shipment)).Object
                }, ShippingAddressEditStateType.Editable)
            }));

            Assert.Equal(expected, testObject.ShipmentStatus);
        }
        [Fact]
        public void LoadOrder_SendsRatesNotSupportedMessage_WhenMultipleShipmentAdapters()
        {
            shipmentEntity.Order.Shipments.Add(new ShipmentEntity(59945));
            shipmentEntity.Order.Shipments.Add(new ShipmentEntity(59946));
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipmentEntity);

            orderSelectionLoaded = new LoadedOrderSelection(orderEntity, new[] { shipmentAdapter.Object, shipmentAdapter.Object.Clone() }, ShippingAddressEditStateType.Editable);

            ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
            testObject.LoadOrder(new OrderSelectionChangedMessage(this, new IOrderSelection[] { orderSelectionLoaded }));

            mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<RatesNotSupportedMessage>()), Times.Once);
        }

        [Fact]
        public void LoadOrder_SendsRatesNotSupportedMessage_WhenOrderHasMultipleShipments()
        {
            shipmentEntity.Order.Shipments.Add(new ShipmentEntity(59945));
            shipmentEntity.Order.Shipments.Add(new ShipmentEntity(59946));
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipmentEntity);

            orderSelectionLoaded = new LoadedOrderSelection(orderEntity, new[] { shipmentAdapter.Object }, ShippingAddressEditStateType.Editable);

            ShippingPanelViewModel testObject = mock.Create<ShippingPanelViewModel>();
            testObject.LoadOrder(new OrderSelectionChangedMessage(this, new IOrderSelection[] { orderSelectionLoaded }));

            mock.Mock<IMessenger>().Verify(s => s.Send(It.IsAny<RatesNotSupportedMessage>()), Times.Once);
        }

        private ShippingPanelViewModel GetShippingPanelViewModelWithLoadedOrder(List<ICarrierShipmentAdapter> shipmentAdapters)
        {
            ShippingPanelViewModel shippingPanelViewModel = mock.Create<ShippingPanelViewModel>();

            var orderSelection = new LoadedOrderSelection(new OrderEntity(), shipmentAdapters, ShippingAddressEditStateType.Editable);
            shippingPanelViewModel.LoadOrder(new OrderSelectionChangedMessage(this, new IOrderSelection[] { orderSelection }));

            return shippingPanelViewModel;
        }
        private ShippingPanelViewModel GetShippingPanelViewModelWithLoadedOrder()
        {
            List<ICarrierShipmentAdapter> shipmentAdapters = new List<ICarrierShipmentAdapter>()
            {
                mock.Create<ICarrierShipmentAdapter>(),
                mock.Create<ICarrierShipmentAdapter>()
            };

            return GetShippingPanelViewModelWithLoadedOrder(shipmentAdapters);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
