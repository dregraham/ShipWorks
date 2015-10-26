using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Shipping.Tests.Loading
{
    public class ShipmentLoaderTest
    {
        private ShipmentLoader testObject;
        private readonly OrderEntity orderEntity;
        private readonly ShipmentEntity shipmentEntity;

        private Mock<IShippingConfiguration> shippingConfigurator;
        private Mock<IShippingManager> shippingManager;
        private Mock<IFilterHelper> filterHelper;
        private Mock<IValidator<ShipmentEntity>> addressValidator;
        private Mock<TestStoreType> storeType;

        public ShipmentLoaderTest()
        {
            orderEntity = new OrderEntity(1006);
            orderEntity.StoreID = 1;
            orderEntity.Store = new StoreEntity(1);
            shipmentEntity = new ShipmentEntity(1031) {Processed = false};
            shipmentEntity.Order = orderEntity;
        }

        [Fact]
        public void Shipment_WhenOrderHasOneShipment_ReturnsThatShipment_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();

                OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

                Assert.Equal(shipmentEntity.ShipmentID, orderSelectionLoaded.ShipmentAdapters.FirstOrDefault().Shipment.ShipmentID);
                Assert.Equal(1, orderSelectionLoaded.ShipmentAdapters.Count());
                Assert.Equal(orderEntity, orderSelectionLoaded.Order);
            }
        }

        [Fact]
        public void ShipmentsReturned_Correct_WhenOrderHasMultipleShipments_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();
                shippingManager.Setup(s => s.GetShipments(It.IsAny<long>(), It.IsAny<bool>()))
                    .Returns(new List<ICarrierShipmentAdapter> {
                        mock.Create<ICarrierShipmentAdapter>(),
                        mock.Create<ICarrierShipmentAdapter>()
                    });

                OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

                Assert.Equal(2, orderSelectionLoaded.ShipmentAdapters.Count());
                Assert.Equal(orderEntity, orderSelectionLoaded.Order);
            }
        }

        [Fact]
        public void NoShipmentsReturned_WhenAutoCreateIsFalse_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();
                shippingConfigurator.Setup(s => s.AutoCreateShipments).Returns(false);

                OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

                Assert.Equal(0, orderSelectionLoaded.ShipmentAdapters.Count());
                Assert.Equal(null, orderSelectionLoaded.Order);
            }
        }

        [Fact]
        public void ShipmentsReturned_WhenAutoCreateIsTrue_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();
                shippingConfigurator.Setup(s => s.AutoCreateShipments).Returns(true);

                OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

                Assert.Equal(1, orderSelectionLoaded.ShipmentAdapters.Count());
                Assert.Equal(orderEntity, orderSelectionLoaded.Order);
            }
        }

        [Fact]
        public void NoShipmentsReturned_WhenCreateShipmentPermissionNotAllowed_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();
                shippingConfigurator.Setup(s => s.UserHasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(false);

                OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

                Assert.Equal(0, orderSelectionLoaded.ShipmentAdapters.Count());
                Assert.Equal(null, orderSelectionLoaded.Order);
            }
        }

        [Fact]
        public void ShipmentsReturned_WhenCreateShipmentPermissionIsAllowed_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();
                shippingConfigurator.Setup(s => s.UserHasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(true);

                OrderSelectionLoaded orderSelectionLoaded = testObject.Load(orderEntity.OrderID);

                Assert.Equal(1, orderSelectionLoaded.ShipmentAdapters.Count());
                Assert.Equal(orderEntity, orderSelectionLoaded.Order);
            }
        }

        [Fact]
        public void AddressValidation_NotPerformed_WhenAddressValidationNotAllowed_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();
                shippingConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(false);

                testObject.Load(orderEntity.OrderID);

                addressValidator.Verify(av => av.ValidateAsync(It.IsAny<ShipmentEntity>()), Times.Never);
            }
        }

        [Fact]
        public void AddressValidation_Performed_WhenAddressValidationAllowed_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();
                shippingConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(true);

                testObject.Load(orderEntity.OrderID);

                addressValidator.Verify(av => av.ValidateAsync(It.IsAny<ShipmentEntity>()), Times.Once);
            }
        }

        [Fact]
        public void AddressValidation_NotPerformed_WhenNoShipmentsAndAddressValidationAllowed_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();
                shippingConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(true);
                shippingConfigurator.Setup(s => s.AutoCreateShipments).Returns(false);

                testObject.Load(orderEntity.OrderID);

                addressValidator.Verify(av => av.ValidateAsync(It.IsAny<ShipmentEntity>()), Times.Never);
            }
        }

        [Fact]
        public void EnsureFiltersUpToDate_Performed_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();
                shippingConfigurator.Setup(s => s.GetAddressValidation(It.IsAny<ShipmentEntity>())).Returns(true);
                shippingConfigurator.Setup(s => s.AutoCreateShipments).Returns(false);

                testObject.Load(orderEntity.OrderID);

                filterHelper.Verify(s => s.EnsureFiltersUpToDate(It.IsAny<TimeSpan>()), Times.Once);
            }
        }

        [Fact]
        public void OrderSelectionLoaded_HasException_WhenInvalidOrderID_Test()
        {
            using (var mock = GetDefaultMocks())
            {
                testObject = mock.Create<ShipmentLoader>();
                OrderSelectionLoaded orderSelectionLoaded = testObject.Load(0);

                Assert.NotNull(orderSelectionLoaded.Exception);
            }
        }

        private AutoMock GetDefaultMocks()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shippingConfigurator = mock.WithShippingConfiguration();
            filterHelper = mock.WithFilterHelper();
            addressValidator = mock.WithAddressValidator();
            shippingManager = mock.WithShippingManager(orderEntity.OrderID, 
                new List<ICarrierShipmentAdapter> { mock.Create<ICarrierShipmentAdapter>() }, 
                Enumerable.Empty<ICarrierShipmentAdapter>());

            storeType = mock.WithTestStoreType();

            mock.WithStoreTypeManager(storeType.Object); 

            Mock<ICarrierShipmentAdapter> shipmentAdapter = mock.WithCarrierShipmentAdapter(shipmentEntity, true);
            mock.WithCarrierShipmentAdapterFactory(shipmentAdapter.Object);
            mock.WithStoreManager(orderEntity.Store);

            return mock;
        }
    }
}
