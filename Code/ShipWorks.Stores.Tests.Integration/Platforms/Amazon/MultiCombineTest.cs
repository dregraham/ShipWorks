using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Stores.Platforms.Amazon.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Amazon
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class MultiCombineTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private Mock<IOrderCombinationUserInteraction> interaction;
        private readonly AmazonStoreEntity store;
        private readonly OrderEntity order1;
        private readonly OrderEntity order2;
        private readonly OrderEntity order3;
        private readonly Dictionary<long, OrderEntity> orders;

        public MultiCombineTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                interaction = mock.Override<IOrderCombinationUserInteraction>();
                mock.Override<IMessageHelper>();
            });

            store = Create.Store<AmazonStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.Amazon)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            Create.Order(store, context.Customer).Save();

            order1 = Create.Order<AmazonOrderEntity>(store, context.Customer)
                .Set(x => x.AmazonOrderID, "10")
                .Save();
            order2 = Create.Order<AmazonOrderEntity>(store, context.Customer)
                .Set(x => x.AmazonOrderID, "20")
                .Save();
            order3 = Create.Order<AmazonOrderEntity>(store, context.Customer)
                .Set(x => x.AmazonOrderID, "30")
                .Save();

            orders = new Dictionary<long, OrderEntity> { { 1, order1 }, { 2, order2 }, { 3, order3 } };
        }

        [Theory]
        [InlineData(new[] { false, false, false }, 1, 3, new[] { "10", "20", "30" })]
        [InlineData(new[] { true, true, false }, 1, 3, new[] { "30" })]
        [InlineData(new[] { false, true, false }, 2, 3, new[] { "10", "30" })]
        [InlineData(new[] { true, false, false }, 2, 3, new[] { "20", "30" })]
        [InlineData(new[] { false, false, false }, 1, 4, new[] { "10", "20", "30" })]
        [InlineData(new[] { true, true, false }, 1, 4, new[] { "30" })]
        [InlineData(new[] { false, true, false }, 2, 4, new[] { "10", "30" })]
        [InlineData(new[] { true, false, false }, 2, 4, new[] { "20", "30" })]
        [InlineData(new[] { false, false, true }, 1, 3, new[] { "10", "20" })]
        [InlineData(new[] { true, true, true }, 1, 3, new string[] { })]
        [InlineData(new[] { false, true, true }, 2, 3, new[] { "10" })]
        [InlineData(new[] { true, false, true }, 2, 3, new[] { "20" })]
        [InlineData(new[] { false, false, true }, 1, 4, new[] { "10", "20" })]
        [InlineData(new[] { true, true, true }, 1, 4, new string[] { })]
        [InlineData(new[] { false, true, true }, 2, 4, new[] { "10" })]
        [InlineData(new[] { true, false, true }, 2, 4, new[] { "20" })]
        public async Task VariousOrderCombinations(bool[] manualOrders, int firstSurviving, int secondSurviving, string[] expected)
        {
            for (int i = 0; i < manualOrders.Length; i++)
            {
                Modify.Order(orders[i + 1]).Set(x => x.IsManual, manualOrders[i]).Save();
            }

            // Perform combines
            OrderEntity initialCombinedOrder = await PerformCombine(firstSurviving, order1, order2);
            orders.Add(4, initialCombinedOrder);

            OrderEntity finalOrder = await PerformCombine(secondSurviving, order3, initialCombinedOrder);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<IAmazonOrderSearchProvider>();
            var identities = await identityProvider.GetOrderIdentifiers(finalOrder);

            Assert.Equal(expected, identities);
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