using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    public class ShippingManagerTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OrderEntity order;

        public ShippingManagerTest(DatabaseFixture db)
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            ContainerInitializer.Initialize(mock.Container);

            db.CreateDataContext(mock);

            var store = Create.Store<GenericModuleStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "A Test Store")
                .Save();

            var customer = Create.Entity<CustomerEntity>().Save();

            order = Create.Order(store, customer)
                .WithOrderNumber(12345)
                .WithShipAddress("1 Memorial Dr.", "Suite 2000", "St. Louis", "MO", "63102", "US")
                .Save();

            // Reset the static fields before each test
            StoreManager.CheckForChanges();
        }

        [Fact]
        public void CreateShipment_ThrowsPermissionException_WhenUserDoesNotHavePermission()
        {
            mock.Override<ISecurityContext>()
                .Setup(x => x.DemandPermission(PermissionType.ShipmentsCreateEditProcess, order.OrderID))
                .Throws<PermissionException>();

            Assert.Throws<PermissionException>(() => ShippingManager.CreateShipment(order, mock.Container));
        }

        [Fact]
        public void CreateShipment_SetsShipDateToNoonToday()
        {
            mock.Override<IDateTimeProvider>()
                .Setup(x => x.Now)
                .Returns(new DateTime(2015, 12, 28, 15, 30, 12));

            ShipmentEntity shipment = ShippingManager.CreateShipment(order, mock.Container);

            Assert.Equal(new DateTime(2015, 12, 28, 12, 00, 00), shipment.ShipDate);
        }

        [Fact]
        public void CreateShipment_SetsWeightToSumOfItems_WhenOrderHasItems()
        {
            Modify.Order(order)
                .WithItem(i => i.Set(x => x.Weight, 2.5).Set(x => x.Quantity, 2))
                .WithItem(i => i.Set(x => x.Weight, 1.25).Set(x => x.Quantity, 1))
                .Save();

            ShipmentEntity shipment = ShippingManager.CreateShipment(order, mock.Container);

            Assert.Equal(6.25, shipment.ContentWeight);
            Assert.Equal(6.25, shipment.TotalWeight);
        }

        [Fact]
        public void CreateShipment_SetsShipAddress_ToOrderShipAddress()
        {
            ShipmentEntity shipment = ShippingManager.CreateShipment(order, mock.Container);

            Assert.Equal("1 Memorial Dr.", shipment.ShipStreet1);
            Assert.Equal("Suite 2000", shipment.ShipStreet2);
            Assert.Equal("St. Louis", shipment.ShipCity);
            Assert.Equal("MO", shipment.ShipStateProvCode);
            Assert.Equal("63102", shipment.ShipPostalCode);
            Assert.Equal("US", shipment.ShipCountryCode);
        }

        [Fact]
        public void CreateShipment_SetsOriginAddress_ToStoreAddress()
        {
            ShipmentEntity shipment = ShippingManager.CreateShipment(order, mock.Container);

            Assert.Equal("A Test Store", shipment.OriginUnparsedName);
            Assert.Equal("123 Main St.", shipment.OriginStreet1);
            Assert.Equal("Suite 456", shipment.OriginStreet2);
            Assert.Equal("St. Louis", shipment.OriginCity);
            Assert.Equal("MO", shipment.OriginStateProvCode);
            Assert.Equal("63123", shipment.OriginPostalCode);
            Assert.Equal("US", shipment.OriginCountryCode);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Usps)]
        [InlineData(ShipmentTypeCode.FedEx)]
        public void CreateShipment_SetsShipmentType_BasedOnShipmentTypeManager(ShipmentTypeCode shipmentTypeCode)
        {
            var shipmentType = mock.CreateMock<ShipmentType>();
            shipmentType.Setup(x => x.ShipmentTypeCode).Returns(shipmentTypeCode);

            mock.Override<IShipmentTypeManager>()
                .Setup(x => x.InitialShipmentType(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentType.Object);

            ShipmentEntity shipment = ShippingManager.CreateShipment(order, mock.Container);

            Assert.Equal(shipmentTypeCode, shipment.ShipmentTypeCode);
        }

        [Fact]
        public void CreateShipment_DelegatesToValidatedAddressManager_ToCopyValidatedAddresses()
        {
            mock.Override<IValidatedAddressManager>();

            ShipmentEntity shipment = ShippingManager.CreateShipment(order, mock.Container);

            mock.Mock<IValidatedAddressManager>()
                .Verify(x => x.CopyValidatedAddresses(It.IsAny<SqlAdapter>(), order.OrderID, "Ship", shipment.ShipmentID, "Ship"));
        }

        [Fact]
        public void CreateShipment_CreatesCustomsItems_WhenShipmentIsInternational()
        {
            Modify.Order(order)
                .WithShipAddress("1 Memorial Dr.", "Suite 2000", "London", string.Empty, "63102", "UK")
                .WithItem(i => i.Set(x => x.Weight, 2).Set(x => x.Quantity, 1).Set(x => x.Name, "Foo"))
                .WithItem(i => i.Set(x => x.Weight, 3).Set(x => x.Quantity, 4).Set(x => x.Name, "Bar"))
                .Save();

            ShipmentEntity shipment = ShippingManager.CreateShipment(order, mock.Container);

            Assert.Equal(2, shipment.CustomsItems.Count);

            Assert.Equal(1, shipment.CustomsItems[0].Quantity);
            Assert.Equal("Foo", shipment.CustomsItems[0].Description);
            Assert.Equal(2, shipment.CustomsItems[0].Weight);

            Assert.Equal(4, shipment.CustomsItems[1].Quantity);
            Assert.Equal("Bar", shipment.CustomsItems[1].Description);
            Assert.Equal(3, shipment.CustomsItems[1].Weight);
        }

        [Fact]
        public void CreateShipment_CreatesCustomsItems_WhenShipmentIsDomestic()
        {
            Modify.Order(order)
                .WithItem(i => i.Set(x => x.Weight, 2).Set(x => x.Quantity, 1).Set(x => x.Name, "Foo"))
                .WithItem(i => i.Set(x => x.Weight, 3).Set(x => x.Quantity, 4).Set(x => x.Name, "Bar"))
                .Save();

            ShipmentEntity shipment = ShippingManager.CreateShipment(order, mock.Container);

            Assert.Empty(shipment.CustomsItems);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
