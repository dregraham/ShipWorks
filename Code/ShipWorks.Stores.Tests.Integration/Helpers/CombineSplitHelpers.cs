using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Moq;
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
        private readonly Mock<IOrderSplitUserInteraction> splitInteraction;
        private readonly Mock<IOrderCombinationUserInteraction> combineInteraction;
        private readonly IOrderSplitGateway orderSplitGateway;

        public CombineSplitHelpers(DataContext context, Mock<IOrderSplitUserInteraction> splitInteraction,
            Mock<IOrderCombinationUserInteraction> combineInteraction)
        {
            this.context = context;
            this.splitInteraction = splitInteraction;
            this.combineInteraction = combineInteraction;
            orderSplitGateway = context.Mock.Container.Resolve<IOrderSplitGateway>();
        }

        /// <summary>
        /// Perform a split of the given order
        /// </summary>
        public Task<(OrderEntity original, OrderEntity split)> PerformSplit(OrderEntity order) =>
            PerformSplit(order, new Dictionary<long, decimal>());

        /// <summary>
        /// Perform a split of the given order
        /// </summary>
        public async Task<(OrderEntity original, OrderEntity split)> PerformSplit(OrderEntity order, Dictionary<long, decimal> itemQuantities)
        {
            var orchestrator = context.Mock.Container.Resolve<IOrderSplitOrchestrator>();

            splitInteraction.Setup(x => x.GetSplitDetailsFromUser(AnyOrder, AnyString))
                .ReturnsAsync(new OrderSplitDefinition(order, itemQuantities,
                    new Dictionary<long, decimal>(),
                    order.OrderNumberComplete + "-1"));

            var result = await orchestrator.Split(order.OrderID);
            var original = await orderSplitGateway.LoadOrder(result.First());
            var split = await orderSplitGateway.LoadOrder(result.Last());

            return (original, split);
        }

        /// <summary>
        /// Perform a split of the given order
        /// </summary>
        public async Task<(OrderEntity original, OrderEntity split)> PerformSplit(OrderEntity order, int itemsToMove, double itemQuantityToMove)
        {
            var orchestrator = context.Mock.Container.Resolve<IOrderSplitOrchestrator>();

            Dictionary<long, decimal> items = new Dictionary<long, decimal>();
            foreach (var item in order.OrderItems.Take(itemsToMove))
            {
                items.Add(item.OrderItemID, (decimal) itemQuantityToMove);
            }

            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(order,
                items,
                new Dictionary<long, decimal>(),
                order.OrderNumberComplete + "-1");

            splitInteraction.Setup(x => x.GetSplitDetailsFromUser(AnyOrder, AnyString))
                .ReturnsAsync(orderSplitDefinition);

            var result = await orchestrator.Split(order.OrderID);

            var original = await orderSplitGateway.LoadOrder(result.First());
            var split = await orderSplitGateway.LoadOrder(result.Last());

            return (original, split);
        }

        /// <summary>
        /// Combine the given orders, using the first order in the params as the surviving order
        /// </summary>
        public async Task<OrderEntity> PerformCombine(string orderNumber, params OrderEntity[] ordersToCombine)
        {
            var combineOrchestrator = context.Mock.Container.Resolve<ICombineOrderOrchestrator>();

            combineInteraction.Setup(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()))
                .Returns(Tuple.Create(ordersToCombine.First().OrderID, orderNumber));

            var result = await combineOrchestrator.Combine(ordersToCombine.Select(x => x.OrderID));
            return await orderSplitGateway.LoadOrder(result.Value);
        }
    }
}
