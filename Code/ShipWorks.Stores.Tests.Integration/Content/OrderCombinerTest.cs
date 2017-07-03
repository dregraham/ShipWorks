using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Content
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OrderCombinerTest : IDisposable
    {
        private readonly DataContext context;
        private OrderEntity secondOrder;

        public OrderCombinerTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            Modify.Order(context.Order)
                .WithShipAddress("Foo", string.Empty, "St. Louis", "MO", "63123", "US")
                .Save();

            secondOrder = Create.Order(context.Store, context.Customer)
                .WithShipAddress("Bar", string.Empty, "Chicago", "IL", "12345", "US")
                .Save();
        }

        [Fact]
        public async Task Combine_UsesGivenID_AsBasisForNewOrder()
        {
            var testObject = context.Mock.Create<OrderCombiner>();

            var result = await testObject.Combine(context.Order.OrderID, new IOrderEntity[] { secondOrder, context.Order }, "1234-C");

            Assert.True(result.Success);

            var newOrder = IoC.UnsafeGlobalLifetimeScope.Resolve<IOrderManager>().FetchOrder(result.Value);

            var shipAddress = newOrder.ShipPerson;
            Assert.Equal("Foo", shipAddress.Street1);
            Assert.Equal(string.Empty, shipAddress.Street2);
            Assert.Equal("St. Louis", shipAddress.City);
            Assert.Equal("MO", shipAddress.StateProvCode);
            Assert.Equal("63123", shipAddress.PostalCode);
            Assert.Equal("US", shipAddress.CountryCode);
        }

        [Fact]
        public async Task Combine_MovesItemsToNewOrder()
        {
            Modify.Order(context.Order)
                .WithItem(i => i.Set(x => x.Name, "Foo"))
                .Save();

            Modify.Order(secondOrder)
                .WithItem(i => i.Set(x => x.Name, "Bar"))
                .Save();

            var testObject = context.Mock.Create<OrderCombiner>();

            var result = await testObject.Combine(context.Order.OrderID, new IOrderEntity[] { secondOrder, context.Order }, "1234-C");

            var newOrder = await GetOrder(result.Value, OrderEntity.PrefetchPathOrderItems);

            Assert.Equal(2, newOrder.OrderItems.Count());
            Assert.Equal(2, newOrder.RollupItemCount);

            Assert.Contains("Foo", newOrder.OrderItems.Select(x => x.Name));
            Assert.Contains("Bar", newOrder.OrderItems.Select(x => x.Name));
        }

        [Fact]
        public async Task Combine_MovesNotesToNewOrder()
        {
            Modify.Order(context.Order)
                .WithNote(i => i.Set(x => x.Text, "Foo"))
                .Save();

            Modify.Order(secondOrder)
                .WithNote(i => i.Set(x => x.Text, "Bar"))
                .Save();

            var testObject = context.Mock.Create<OrderCombiner>();

            var result = await testObject.Combine(context.Order.OrderID, new IOrderEntity[] { secondOrder, context.Order }, "1234-C");

            var newOrder = await GetOrder(result.Value, OrderEntity.PrefetchPathNotes);

            Assert.Equal(2, newOrder.Notes.Count());
            Assert.Equal(2, newOrder.RollupNoteCount);

            Assert.Contains("Foo", newOrder.Notes.Select(x => x.Text));
            Assert.Contains("Bar", newOrder.Notes.Select(x => x.Text));
        }

        [Fact]
        public async Task Combine_MovesOrderPaymentsToNewOrder()
        {
            Modify.Order(context.Order)
                .WithPaymentDetail(i => i.Set(x => x.Label, "Foo"))
                .Save();

            Modify.Order(secondOrder)
                .WithPaymentDetail(i => i.Set(x => x.Label, "Bar"))
                .Save();

            var testObject = context.Mock.Create<OrderCombiner>();

            var result = await testObject.Combine(context.Order.OrderID, new IOrderEntity[] { secondOrder, context.Order }, "1234-C");

            var newOrder = await GetOrder(result.Value, OrderEntity.PrefetchPathOrderPaymentDetails);

            Assert.Equal(2, newOrder.OrderPaymentDetails.Count());

            Assert.Contains("Foo", newOrder.OrderPaymentDetails.Select(x => x.Label));
            Assert.Contains("Bar", newOrder.OrderPaymentDetails.Select(x => x.Label));
        }

        [Fact]
        public async Task Combine_AddsOrderSearchEntries()
        {
            Modify.Order(context.Order)
                .WithPaymentDetail(i => i.Set(x => x.Label, "Foo"))
                .Save();

            Modify.Order(secondOrder)
                .WithPaymentDetail(i => i.Set(x => x.Label, "Bar"))
                .Save();

            var testObject = context.Mock.Create<OrderCombiner>();

            var result = await testObject.Combine(context.Order.OrderID, new IOrderEntity[] { secondOrder, context.Order }, "1234-C");

            var query = new QueryFactory().OrderSearch.Where(OrderSearchFields.OrderID == result.Value);

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ISqlAdapter sqlAdapter = lifetimeScope.Resolve<ISqlAdapterFactory>().Create();

                var searchEntities = await sqlAdapter.FetchQueryAsync(query);

                Assert.Equal(2, searchEntities.Count);
                Assert.True(searchEntities.OfType<IOrderSearchEntity>().All(x => x.StoreID == context.Order.StoreID));
                Assert.Equal(
                    new[] { context.Order.OrderNumber, secondOrder.OrderNumber }.OrderBy(x => x),
                    searchEntities.OfType<IOrderSearchEntity>().Select(x => x.OrderNumber).OrderBy(x => x));
                Assert.Equal(
                    new[] { context.Order.OrderNumberComplete, secondOrder.OrderNumberComplete }.OrderBy(x => x),
                    searchEntities.OfType<IOrderSearchEntity>().Select(x => x.OrderNumberComplete).OrderBy(x => x));
            }
        }

        [Fact]
        public async Task Combine_DeletesOriginalOrders()
        {
            var testObject = context.Mock.Create<OrderCombiner>();

            await testObject.Combine(context.Order.OrderID, new IOrderEntity[] { secondOrder, context.Order }, "1234-C");

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                using (ISqlAdapter sqlAdapter = lifetimeScope.Resolve<ISqlAdapterFactory>().Create())
                {
                    var queryFactory = new QueryFactory();
                    var query = queryFactory.Order
                        .Select(OrderFields.OrderID.Count())
                        .Where(OrderFields.OrderID == context.Order.OrderID | OrderFields.OrderID == secondOrder.OrderID);

                    var count = await sqlAdapter.FetchScalarAsync<int>(query);
                    Assert.Equal(0, count);
                }
            }
        }

        /// <summary>
        /// Get a fully loaded order with all its children
        /// </summary>
        private async Task<IOrderEntity> GetOrder(long orderID, params IPrefetchPathElementCore[] prefetchPaths)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                var sqlAdapter = lifetimeScope.Resolve<ISqlAdapterFactory>().Create();
                var queryFactory = new QueryFactory();
                var query = queryFactory.Order
                    .Where(OrderFields.OrderID == orderID);

                if (prefetchPaths?.Any() == true)
                {
                    query = query.WithPath(prefetchPaths.First(), prefetchPaths.Skip(1).ToArray());
                }

                return await sqlAdapter.FetchFirstAsync(query);
            }
        }

        public void Dispose() => context.Dispose();
    }
}