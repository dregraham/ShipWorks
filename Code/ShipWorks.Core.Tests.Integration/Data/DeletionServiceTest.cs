using System;
using System.Reactive.Linq;
using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class DeletionServiceTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ILifetimeScope lifetimeScope;
        private IDisposable subscriptions;

        public DeletionServiceTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            lifetimeScope = IoC.BeginLifetimeScope();
        }

        [Fact]
        public void DeleteStore_SendsStoreDeletedMessage_WithStoreId()
        {
            long storeId = context.Store.StoreID;
            long? deletedStoreId = null;

            subscriptions = lifetimeScope.Resolve<IObservable<IShipWorksMessage>>()
                .OfType<StoreDeletedMessage>()
                .Subscribe(x => deletedStoreId = x.DeletedEntityID);

            DeletionService.DeleteStore(context.Store, lifetimeScope.Resolve<ISecurityContext>());

            Assert.True(deletedStoreId.HasValue);
            Assert.Equal(storeId, deletedStoreId.Value);
        }

        [Fact]
        public void DeleteOrder_SendsOrderDeletedMessage_WithOrderId()
        {
            long orderId = context.Order.OrderID;
            long? deletedOrderId = null;

            subscriptions = lifetimeScope.Resolve<IObservable<IShipWorksMessage>>()
                .OfType<OrderDeletedMessage>()
                .Subscribe(x => deletedOrderId = x.DeletedEntityID);

            DeletionService.DeleteOrder(orderId);

            Assert.True(deletedOrderId.HasValue);
            Assert.Equal(orderId, deletedOrderId.Value);
        }

        [Fact]
        public void DeleteCustomer_DeletesAssociatedOrders()
        {
            long orderId = context.Order.OrderID;

            DeletionService.DeleteCustomer(context.Customer.CustomerID);

            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var orders = new EntityCollection<OrderEntity>();
                sqlAdapter.FetchEntityCollection(orders, new RelationPredicateBucket(OrderFields.OrderID == orderId));
                Assert.Empty(orders);
            }
        }

        [Fact]
        public void DeleteCustomer_SendsCustomerDeletedMessage_WithCustomerId()
        {
            long CustomerId = context.Customer.CustomerID;
            long? deletedCustomerId = null;

            subscriptions = lifetimeScope.Resolve<IObservable<IShipWorksMessage>>()
                .OfType<CustomerDeletedMessage>()
                .Subscribe(x => deletedCustomerId = x.DeletedEntityID);

            DeletionService.DeleteCustomer(CustomerId);

            Assert.True(deletedCustomerId.HasValue);
            Assert.Equal(CustomerId, deletedCustomerId.Value);
        }

        public void Dispose()
        {
            subscriptions?.Dispose();
            lifetimeScope.Dispose();
            context.Dispose();
        }
    }
}
