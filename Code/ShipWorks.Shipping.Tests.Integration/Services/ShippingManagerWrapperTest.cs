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
                GenericModuleStoreEntity store = dbContext.CreateEntityWithDefaults<GenericModuleStoreEntity>();
                sqlAdapter.SaveAndRefetch(store);

                CustomerEntity customer = dbContext.CreateEntityWithDefaults<CustomerEntity>();
                sqlAdapter.SaveAndRefetch(customer);

                OrderEntity order = dbContext.CreateEntityWithDefaults<OrderEntity>();
                order.OrderNumber = 123999;
                order.Store = store;
                order.Customer = customer;

                sqlAdapter.SaveAndRefetch(order);

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
