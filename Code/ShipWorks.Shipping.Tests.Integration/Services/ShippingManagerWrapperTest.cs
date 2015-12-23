using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    public class ShippingManagerWrapperTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext dbContext;
        private readonly long createdOrderId;

        public ShippingManagerWrapperTest(DatabaseFixture db)
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            dbContext = db.CreateDataContext(mock);

            using (SqlAdapter sqlAdapter = dbContext.CreateSqlAdapter())
            {
                var store = EntityBuilder.Create<GenericModuleStoreEntity>().WithDefaults().Save(sqlAdapter);
                var customer = EntityBuilder.Create<CustomerEntity>().WithDefaults().Save(sqlAdapter);
                var order = EntityBuilder.Create<OrderEntity>().WithDefaults()
                    .Configure(x =>
                    {
                        x.OrderNumber = 123999;
                        x.Store = store;
                        x.Customer = customer;
                    })
                    .Save(sqlAdapter);

                createdOrderId = order.OrderID;
            }
        }

        [Fact]
        public void Foo()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(createdOrderId);

            Assert.Equal(123999, loadedOrder.OrderNumber);
        }

        public void Dispose()
        {
            dbContext.Dispose();
            mock.Dispose();
        }
    }
}
