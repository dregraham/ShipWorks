using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Data;
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
        private readonly DataContext dbContext;
        private readonly OrderEntity order;

        public ShippingManagerTest(DatabaseFixture db)
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            ContainerInitializer.Initialize(mock.Container);

            dbContext = db.CreateDataContext(mock);

            try
            {
                using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
                {
                    var store = Create.Store<GenericModuleStoreEntity>()
                        .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                        .SetField(x => x.StoreName, "A Test Store")
                        .Save(sqlAdapter);

                    var customer = Create.Entity<CustomerEntity>().Save(sqlAdapter);

                    order = Create.Order(store, customer)
                        .WithOrderNumber(12345)
                        .WithShipAddress("1 Memorial Dr.", "Suite 2000", "St. Louis", "MO", "63102", "US")
                        .Save(sqlAdapter);
                }

                // Reset the static fields before each test
                StoreManager.CheckForChanges();
            }
            catch (Exception)
            {
                dbContext?.Dispose();

                throw;
            }
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
            mock.Override<IDataProvider>()
                .Setup(x => x.GetRelatedEntities(order.OrderID, Data.Model.EntityType.OrderItemEntity))
                .Returns(new[] {
                    new OrderItemEntity { Quantity = 2, Weight = 2.5 },
                    new OrderItemEntity { Quantity = 1, Weight = 1.25 }
                });

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
            OrderEntity otherOrder;

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                otherOrder = Create.Order(order.Store, order.Customer)
                    .WithOrderNumber(6789)
                    .WithShipAddress("1 Memorial Dr.", "Suite 2000", "London", string.Empty, "63102", "UK")
                    .WithItem(i => i.SetField(x => x.Weight, 2)
                        .SetField(x => x.Quantity, 1)
                        .SetField(x => x.Name, "Foo"))
                    .WithItem(i => i.SetField(x => x.Weight, 3)
                        .SetField(x => x.Quantity, 4)
                        .SetField(x => x.Name, "Bar"))
                    .Save(sqlAdapter);
            }

            ShipmentEntity shipment = ShippingManager.CreateShipment(otherOrder, mock.Container);

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
            OrderEntity otherOrder;

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                otherOrder = Create.Order(order.Store, order.Customer)
                    .WithOrderNumber(6789)
                    .WithShipAddress("1 Memorial Dr.", "Suite 2000", "St. Louis", "MO", "63102", "US")
                    .WithItem(i => i.SetField(x => x.Weight, 2)
                        .SetField(x => x.Quantity, 1)
                        .SetField(x => x.Name, "Foo"))
                    .WithItem(i => i.SetField(x => x.Weight, 3)
                        .SetField(x => x.Quantity, 4)
                        .SetField(x => x.Name, "Bar"))
                    .Save(sqlAdapter);

            }

            ShipmentEntity shipment = ShippingManager.CreateShipment(otherOrder, mock.Container);

            Assert.Empty(shipment.CustomsItems);
        }

        public void Dispose()
        {
            dbContext.Dispose();
            mock.Dispose();
        }
    }
}
