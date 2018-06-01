﻿using System;
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
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Overstock
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OverstockCombineTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private Mock<IOrderCombinationUserInteraction> combineInteraction;
        private readonly OverstockStoreEntity store;
        private readonly Dictionary<long, OrderEntity> orders;

        public OverstockCombineTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                combineInteraction = mock.Override<IOrderCombinationUserInteraction>();
                mock.Override<IMessageHelper>();
            });

            store = Create.Store<OverstockStoreEntity>(StoreTypeCode.Overstock).Save();

            // Create a dummy order
            Create.Order<OverstockOrderEntity>(store, context.Customer).Save();

            var orderA = Create.Order<OverstockOrderEntity>(store, context.Customer)
                .Set(x => x.SalesChannelName, "Channel1")
                .Set(x => x.WarehouseCode, "Code1")
                .Set(x => x.OrderNumberComplete, "1000")
                .Save();
            var orderB = Create.Order<OverstockOrderEntity>(store, context.Customer)
                .Set(x => x.SalesChannelName, "Channel2")
                .Set(x => x.WarehouseCode, "Code2")
                .Set(x => x.OrderNumberComplete, "2000")
                .Save();
            var orderC = Create.Order<OverstockOrderEntity>(store, context.Customer)
                .Set(x => x.SalesChannelName, "Channel3")
                .Set(x => x.WarehouseCode, "Code3")
                .Set(x => x.OrderNumberComplete, "3000")
                .Save();

            orders = new Dictionary<long, OrderEntity> {{1, orderA}, {2, orderB}, {3, orderC}};
        }

        [Theory]
        [InlineData(new[] { false, false, false }, 1, 3)]
        [InlineData(new[] { true, true, false }, 1, 3)]
        [InlineData(new[] { false, true, false }, 2, 3)]
        [InlineData(new[] { true, false, false }, 2, 3)]
        [InlineData(new[] { false, false, false }, 1, 4)]
        [InlineData(new[] { true, true, false }, 1, 4)]
        [InlineData(new[] { false, true, false }, 2, 4)]
        [InlineData(new[] { true, false, false }, 2, 4)]
        [InlineData(new[] { false, false, true }, 1, 3)]
        [InlineData(new[] { true, true, true }, 1, 3)]
        [InlineData(new[] { false, true, true }, 2, 3)]
        [InlineData(new[] { true, false, true }, 2, 3)]
        [InlineData(new[] { false, false, true }, 1, 4)]
        [InlineData(new[] { true, true, true }, 1, 4)]
        [InlineData(new[] { false, true, true }, 2, 4)]
        [InlineData(new[] { true, false, true }, 2, 4)]
        public async Task VariousOrderCombinations(bool[] manualOrders, int firstSurviving, int secondSurviving)
        {
            for (int i = 0; i < manualOrders.Length; i++)
            {
                Modify.Order(orders[i + 1]).Set(x => x.IsManual, manualOrders[i]).Save();
            }
            // Perform combines
            OrderEntity initialCombinedOrder = await PerformCombine(firstSurviving, orders[1], orders[2]);
            orders.Add(4, initialCombinedOrder);

            OrderEntity finalOrder = await PerformCombine(secondSurviving, orders[3], initialCombinedOrder);

            QueryFactory factory = new QueryFactory();
            var query = factory.OrderSearch.Where(OrderSearchFields.OrderID == finalOrder.OrderID);
            var storeQuery = factory.OverstockOrderSearch.Where(OverstockOrderSearchFields.OrderID == finalOrder.OrderID);

            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var results = await sqlAdapter.FetchQueryAsync(query);
                var storeResults = await sqlAdapter.FetchQueryAsync(storeQuery);

                var orderTests = orders.Take(3).Select(x => x.Value)
                    .LeftJoin(results.OfType<IOrderSearchEntity>(), x => x.OrderID, x => x.OriginalOrderID)
                    .LeftJoin(storeResults.OfType<IOverstockOrderSearchEntity>(), x => x.Item1.OrderID, x => x.OriginalOrderID)
                    .Select(x => new { Order = x.Item1.Item1, BasicSearch = x.Item1.Item2, StoreSearch = x.Item2 })
                    .ToArray();

                for (int i = 0; i < orderTests.Length; i++)
                {
                    var test = orderTests[i];

                    Assert.NotNull(test.BasicSearch);
                    Assert.Equal(test.Order.OrderNumber, test.BasicSearch.OrderNumber);
                    Assert.Equal(test.Order.OrderNumberComplete, test.BasicSearch.OrderNumberComplete);
                    Assert.Equal(manualOrders[i], test.BasicSearch.IsManual);

                    if (manualOrders[i])
                    {
                        Assert.Null(test.StoreSearch);
                    }
                    else
                    {
                        Assert.Equal((test.Order as IOverstockOrderEntity).WarehouseCode,
                            test.StoreSearch.WarehouseCode);
                    }
                }
            }
        }

        private async Task<OrderEntity> PerformCombine(int surviving, params IOrderEntity[] ordersToCombine)
        {
            var dataProvider = context.Mock.Container.Resolve<IDataProvider>();
            var combineOrchestrator = context.Mock.Container.Resolve<ICombineOrderOrchestrator>();

            combineInteraction.Setup(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()))
                .Returns(GenericResult.FromSuccess(Tuple.Create(orders[surviving].OrderID, "1-C")));

            var result = await combineOrchestrator.Combine(ordersToCombine.Select(x => x.OrderID));
            return await dataProvider.GetEntityAsync<OrderEntity>(result.Value);
        }

        public void Dispose() => context.Dispose();
    }
}