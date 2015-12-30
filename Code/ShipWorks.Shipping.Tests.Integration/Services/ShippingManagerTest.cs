using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
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
                mock.Override<ISecurityContext>()
                    .Setup(x => x.DemandPermission(It.IsAny<PermissionType>(), It.IsAny<long>()));

                TestExecutionMode executionMode = new TestExecutionMode();

                foreach (IInitializeForCurrentDatabase service in IoC.UnsafeGlobalLifetimeScope.Resolve<IEnumerable<IInitializeForCurrentDatabase>>())
                {
                    service.InitializeForCurrentDatabase(executionMode);
                }

                foreach (IInitializeForCurrentSession service in mock.Container.Resolve<IEnumerable<IInitializeForCurrentSession>>())
                {
                    service.InitializeForCurrentSession();
                }

                using (SqlAdapter sqlAdapter = dbContext.CreateSqlAdapter())
                {
                    var store = Create.Entity<GenericModuleStoreEntity>().Save(sqlAdapter);
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
            mock.Override<IStoreManager>()
                .Setup(x => x.GetStore(order.StoreID))
                .Returns(new StoreEntity
                {
                    StoreName = "A Test Store",
                    Street1 = "123 Main St.",
                    Street2 = "Suite 456",
                    City = "St. Louis",
                    StateProvCode = "MO",
                    PostalCode = "63123",
                    CountryCode = "US"
                });

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

        public void Dispose()
        {
            dbContext.Dispose();
            mock.Dispose();
        }
    }
}
