using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared.Database;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Helpers
{
    public class CombineSplitHelpers
    {
        private readonly DataContext context;
        private Mock<IOrderSplitUserInteraction> splitInteraction;
        private Mock<IOrderCombinationUserInteraction> combineInteraction;

        public CombineSplitHelpers(DataContext context, Mock<IOrderSplitUserInteraction> splitInteraction, 
            Mock<IOrderCombinationUserInteraction> combineInteraction)
        {
            this.context = context;
            this.splitInteraction = splitInteraction;
            this.combineInteraction = combineInteraction;
        }

        /// <summary>
        /// Perform a split of the given order
        /// </summary>
        public async Task<(OrderEntity original, OrderEntity split)> PerformSplit(OrderEntity order)
        {
            var dataProvider = context.Mock.Container.Resolve<IDataProvider>();
            var orchestrator = context.Mock.Container.Resolve<IOrderSplitOrchestrator>();

            splitInteraction.Setup(x => x.GetSplitDetailsFromUser(AnyOrder, AnyString))
                .ReturnsAsync(new OrderSplitDefinition(order, new Dictionary<long, decimal>(),
                    new Dictionary<long, decimal>(),
                    order.OrderNumberComplete + "-1"));

            var result = await orchestrator.Split(order.OrderID);
            var original = await dataProvider.GetEntityAsync<OrderEntity>(result.First());
            var split = await dataProvider.GetEntityAsync<OrderEntity>(result.Last());

            return (original, split);
        }

        /// <summary>
        /// Combine the given orders, using the first order in the params as the surviving order
        /// </summary>
        public async Task<OrderEntity> PerformCombine(string orderNumber, params OrderEntity[] ordersToCombine)
        {
            var dataProvider = context.Mock.Container.Resolve<IDataProvider>();
            var combineOrchestrator = context.Mock.Container.Resolve<ICombineOrderOrchestrator>();

            combineInteraction.Setup(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()))
                .Returns(Tuple.Create(ordersToCombine.First().OrderID, orderNumber));

            var result = await combineOrchestrator.Combine(ordersToCombine.Select(x => x.OrderID));
            return await dataProvider.GetEntityAsync<OrderEntity>(result.Value);
        }
    }
}
