using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Content
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OrderCombinationUserInteraction : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private Mock<IOrderCombinationUserInteraction> interaction;
        private readonly GenericModuleStoreEntity store;
        private readonly OrderEntity order1;
        private readonly OrderEntity order2;
        private readonly OrderEntity order3;
        private readonly Dictionary<long, OrderEntity> orders;
        private Mock<IBigCommerceWebClient> webClient;

        public OrderCombinationUserInteraction(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                interaction = mock.Override<IOrderCombinationUserInteraction>();
                mock.Override<IMessageHelper>();
            });

            store = Create.Store<GenericModuleStoreEntity>(StoreTypeCode.GenericModule)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            Create.Order(store, context.Customer).Save();

            order1 = Create.Order(store, context.Customer)
                .Set(x => x.OrderNumber, 10)
                .Save();
            order2 = Create.Order(store, context.Customer)
                .Set(x => x.OrderNumber, 20)
                .Save();
            order3 = Create.Order(store, context.Customer)
                .Set(x => x.OrderNumber, 30)
                .Save();

            orders = new Dictionary<long, OrderEntity> { { 1, order1 }, { 2, order2 }, { 3, order3 } };
        }

        [Theory]
        [InlineData(new[] { false, false, false }, 1, 3, new[] { 10L, 20L, 30L })]
        [InlineData(new[] { true, true, false }, 1, 3, new[] { 30L })]
        [InlineData(new[] { false, true, false }, 2, 3, new[] { 10L, 30L })]
        [InlineData(new[] { true, false, false }, 2, 3, new[] { 20L, 30L })]
        [InlineData(new[] { false, false, false }, 1, 4, new[] { 10L, 20L, 30L })]
        [InlineData(new[] { true, true, false }, 1, 4, new[] { 30L })]
        [InlineData(new[] { false, true, false }, 2, 4, new[] { 10L, 30L })]
        [InlineData(new[] { true, false, false }, 2, 4, new[] { 20L, 30L })]
        [InlineData(new[] { false, false, true }, 1, 3, new[] { 10L, 20L })]
        [InlineData(new[] { true, true, true }, 1, 3, new long[] { })]
        [InlineData(new[] { false, true, true }, 2, 3, new[] { 10L })]
        [InlineData(new[] { true, false, true }, 2, 3, new[] { 20L })]
        [InlineData(new[] { false, false, true }, 1, 4, new[] { 10L, 20L })]
        [InlineData(new[] { true, true, true }, 1, 4, new long[] { })]
        [InlineData(new[] { false, true, true }, 2, 4, new[] { 10L })]
        [InlineData(new[] { true, false, true }, 2, 4, new[] { 20L })]
        public async Task VariousOrderCombinations(bool[] manualOrders, int firstSurviving, int secondSurviving, long[] expected)
        {
            for (int i = 0; i < manualOrders.Length; i++)
            {
                Modify.Order(orders[i + 1]).Set(x => x.IsManual, manualOrders[i]).Save();
            }

            // Perform combines
            OrderEntity initialCombinedOrder = await PerformCombine(firstSurviving, order1, order2);
            orders.Add(4, initialCombinedOrder);

            OrderEntity finalOrder = await PerformCombine(secondSurviving, order3, initialCombinedOrder);

            QueryFactory factory = new QueryFactory();
            var query = factory.OrderSearch.Where(OrderSearchFields.OrderID == finalOrder.OrderID);

            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var results = await sqlAdapter.FetchQueryAsync(query);
                var orderTests = new[] { order1, order2, order3 }
                    .LeftJoin(results.OfType<OrderSearchEntity>(), x => x.OrderID, x => x.OriginalOrderID)
                    .ToArray();

                for (int i = 0; i < orderTests.Length; i++)
                {
                    var test = orderTests[i];

                    Assert.NotNull(test.Item2);
                    Assert.Equal(test.Item1.OrderNumber, test.Item2.OrderNumber);
                    Assert.Equal(test.Item1.OrderNumberComplete, test.Item2.OrderNumberComplete);
                    Assert.Equal(manualOrders[i], test.Item2.IsManual);
                }
            }
        }

        private async Task<OrderEntity> PerformCombine(int surviving, params IOrderEntity[] ordersToCombine)
        {
            var dataProvider = context.Mock.Container.Resolve<IDataProvider>();
            var combineOrchestrator = context.Mock.Container.Resolve<ICombineOrderOrchestrator>();

            interaction.Setup(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()))
                .Returns(GenericResult.FromSuccess(Tuple.Create(orders[surviving].OrderID, "1-C")));

            var result = await combineOrchestrator.Combine(ordersToCombine.Select(x => x.OrderID));
            return await dataProvider.GetEntityAsync<OrderEntity>(result.Value);
        }

        public void Dispose() => context.Dispose();
    }
}