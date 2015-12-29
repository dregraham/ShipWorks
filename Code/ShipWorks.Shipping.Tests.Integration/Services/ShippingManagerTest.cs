using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
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
            dbContext = db.CreateDataContext(mock);

            using (SqlAdapter sqlAdapter = dbContext.CreateSqlAdapter())
            {
                var store = Create.Entity<GenericModuleStoreEntity>().Save(sqlAdapter);
                var customer = Create.Entity<CustomerEntity>().Save(sqlAdapter);

                order = Create.Order(store, customer).WithOrderNumber(12345).Save(sqlAdapter);
            }

            mock.Mock<IStoreManager>()
                .Setup(x => x.GetStore(It.IsAny<long>()))
                .Returns(new StoreEntity());

            // Reset the static fields before each test
            ShippingManager.InitializeForCurrentDatabase();
        }

        [Fact]
        public void CreateShipment_ThrowsPermissionException_WhenUserDoesNotHavePermission()
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.DemandPermission(PermissionType.ShipmentsCreateEditProcess, order.OrderID))
                .Throws<PermissionException>();

            Assert.Throws<PermissionException>(() => ShippingManager.CreateShipment(order, mock.Container));
        }

        [Fact]
        public void CreateShipment_SetsShipDateToNoonToday()
        {
            mock.Mock<IDateTimeProvider>()
                .Setup(x => x.Now)
                .Returns(new DateTime(2015, 12, 28, 15, 30, 12));

            ShipmentEntity shipment = ShippingManager.CreateShipment(order, mock.Container);
            Assert.Equal(new DateTime(2015, 12, 28, 12, 00, 00), shipment.ShipDate);
        }

        public void Dispose()
        {
            dbContext.Dispose();
            mock.Dispose();
        }
    }
}
