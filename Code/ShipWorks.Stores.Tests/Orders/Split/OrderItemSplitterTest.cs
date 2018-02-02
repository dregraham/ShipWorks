using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    public class OrderItemSplitterTest
    {
        [Fact]
        public void Split_SplitOrder_HasNoOrderItemQuantities_WhenNoOrderItemsRequested()
        {
            OrderEntity originalOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            originalOrder.OrderItems.Add(new OrderItemEntity(1) { Quantity = 1 });
            originalOrder.OrderItems.Add(new OrderItemEntity(2) { Quantity = 3 });

            OrderEntity splitOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            splitOrder.OrderItems.Add(new OrderItemEntity(1) { Quantity = 1 });
            splitOrder.OrderItems.Add(new OrderItemEntity(2) { Quantity = 3 });

            Dictionary<long, decimal> newOrderItemQuantities = new Dictionary<long, decimal>();
            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(originalOrder, newOrderItemQuantities, new Dictionary<long, decimal>(), "");

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Equal(0, splitOrder.OrderItems.Sum(oi => oi.Quantity));
            Assert.Equal(4, originalOrder.OrderItems.Sum(oi => oi.Quantity));

            Assert.Equal(1, originalOrder.OrderItems.First(oc => oc.OrderItemID == 1).Quantity);
            Assert.Equal(3, originalOrder.OrderItems.First(oc => oc.OrderItemID == 2).Quantity);
        }

        [Fact]
        public void Split_SplitOrder_HasAllOrderItemQuantities_WhenAllOrderItemsRequested()
        {
            OrderEntity originalOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            originalOrder.OrderItems.Add(new OrderItemEntity(1) { Quantity = 1 });
            originalOrder.OrderItems.Add(new OrderItemEntity(2) { Quantity = 3 });

            OrderEntity splitOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            splitOrder.OrderItems.Add(new OrderItemEntity(1) { Quantity = 1 });
            splitOrder.OrderItems.Add(new OrderItemEntity(2) { Quantity = 3 });

            Dictionary<long, decimal> newOrderItemQuantities = new Dictionary<long, decimal>
            {
                { 1, 1 },
                { 2, 3 }
            };
            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(originalOrder, newOrderItemQuantities, new Dictionary<long, decimal>(), "");

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Equal(4, splitOrder.OrderItems.Sum(oi => oi.Quantity));
            Assert.Equal(0, originalOrder.OrderItems.Sum(oi => oi.Quantity));

            Assert.Equal(1, splitOrder.OrderItems.First(oc => oc.OrderItemID == 1).Quantity);
            Assert.Equal(3, splitOrder.OrderItems.First(oc => oc.OrderItemID == 2).Quantity);
        }

        [Fact]
        public void Split_SplitOrder_HasCorrectOrderItemQuantities_ForChangedQuantities()
        {
            OrderEntity originalOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            originalOrder.OrderItems.Add(new OrderItemEntity(1) { Quantity = 1 });
            originalOrder.OrderItems.Add(new OrderItemEntity(2) { Quantity = 3 });

            OrderEntity splitOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            splitOrder.OrderItems.Add(new OrderItemEntity(1) { Quantity = 1 });
            splitOrder.OrderItems.Add(new OrderItemEntity(2) { Quantity = 3 });

            Dictionary<long, decimal> newOrderItemQuantities = new Dictionary<long, decimal>();
            newOrderItemQuantities.Add(1, 0.5M);
            newOrderItemQuantities.Add(2, 1.5M);
            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(originalOrder, newOrderItemQuantities, new Dictionary<long, decimal>(), "");

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Equal(2, splitOrder.OrderItems.Sum(oi => oi.Quantity));
            Assert.Equal(2, originalOrder.OrderItems.Sum(oi => oi.Quantity));

            Assert.Equal(0.5, splitOrder.OrderItems.First(oc => oc.OrderItemID == 1).Quantity);
            Assert.Equal(1.5, splitOrder.OrderItems.First(oc => oc.OrderItemID == 2).Quantity);

            Assert.Equal(0.5, originalOrder.OrderItems.First(oc => oc.OrderItemID == 1).Quantity);
            Assert.Equal(1.5, originalOrder.OrderItems.First(oc => oc.OrderItemID == 2).Quantity);
        }

        [Fact]
        public void Split_SplitOrder_HasCorrectOrderItemQuantities_ForZeroQuantities()
        {
            OrderEntity originalOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            originalOrder.OrderItems.Add(new OrderItemEntity(1) { Quantity = 1 });
            originalOrder.OrderItems.Add(new OrderItemEntity(2) { Quantity = 3 });

            OrderEntity splitOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            splitOrder.OrderItems.Add(new OrderItemEntity(1) { Quantity = 1 });
            splitOrder.OrderItems.Add(new OrderItemEntity(2) { Quantity = 3 });

            Dictionary<long, decimal> newOrderItemQuantities = new Dictionary<long, decimal>();
            newOrderItemQuantities.Add(1, 0);
            newOrderItemQuantities.Add(2, 0);
            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(originalOrder, newOrderItemQuantities, new Dictionary<long, decimal>(), "");

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Equal(0, splitOrder.OrderItems.Sum(oi => oi.Quantity));
            Assert.Equal(4, originalOrder.OrderItems.Sum(oi => oi.Quantity));

            Assert.Equal(1, originalOrder.OrderItems.First(oc => oc.OrderItemID == 1).Quantity);
            Assert.Equal(3, originalOrder.OrderItems.First(oc => oc.OrderItemID == 2).Quantity);
        }

        [Fact]
        public void Split_RemovesItemsWithZeroQuantityFromOriginalOrder_WhenSplitOrderHasItemWithNonZeroQuantity()
        {
            var originalOrder = new OrderEntity { OrderNumber = 1 };
            originalOrder.OrderItems.Add(new OrderItemEntity(1) { Description = "Foo", Quantity = 5 });

            OrderEntity splitOrder = new OrderEntity { OrderNumber = 1 };
            splitOrder.OrderItems.Add(new OrderItemEntity(1) { Description = "Foo" });

            Dictionary<long, decimal> newOrderItemQuantities = new Dictionary<long, decimal> { { 1, 5 } };

            var orderSplitDefinition = new OrderSplitDefinition(originalOrder, newOrderItemQuantities, new Dictionary<long, decimal>(), "");

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Empty(originalOrder.OrderItems);
        }

        [Fact]
        public void Split_RemovesItemsWithZeroQuantityFromSplitOrder_WhenOriginalOrderHasItemWithNonZeroQuantity()
        {
            var originalOrder = new OrderEntity { OrderNumber = 1 };
            originalOrder.OrderItems.Add(new OrderItemEntity(1) { Description = "Foo", Quantity = 5 });

            OrderEntity splitOrder = new OrderEntity { OrderNumber = 1 };
            splitOrder.OrderItems.Add(new OrderItemEntity(1) { Description = "Foo" });

            Dictionary<long, decimal> newOrderItemQuantities = new Dictionary<long, decimal> { { 1, 0 } };

            var orderSplitDefinition = new OrderSplitDefinition(originalOrder, newOrderItemQuantities, new Dictionary<long, decimal>(), "");

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Empty(splitOrder.OrderItems);
        }

        [Fact]
        public void Split_DoesNotRemoveItemsWithZeroQuantityFromOriginalOrder_WhenSplitOrderHasItemWithNonZeroQuantity()
        {
            var originalOrder = new OrderEntity { OrderNumber = 1 };
            originalOrder.OrderItems.Add(new OrderItemEntity(1) { Description = "Foo", Quantity = 0 });

            OrderEntity splitOrder = new OrderEntity { OrderNumber = 1 };
            splitOrder.OrderItems.Add(new OrderItemEntity(1) { Description = "Foo" });

            Dictionary<long, decimal> newOrderItemQuantities = new Dictionary<long, decimal> { { 1, 0 } };

            var orderSplitDefinition = new OrderSplitDefinition(originalOrder, newOrderItemQuantities, new Dictionary<long, decimal>(), "");

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.NotEmpty(originalOrder.OrderItems);
            Assert.Empty(splitOrder.OrderItems);
        }

        [Fact]
        public void Split_DoesNotRemoveItemsWithZeroAmountFromOriginalOrder_WhenNotIncludedInSplitAmounts()
        {
            var originalOrder = new OrderEntity { OrderNumber = 1 };
            originalOrder.OrderItems.Add(new OrderItemEntity(1) { Description = "Foo", Quantity = 0 });

            OrderEntity splitOrder = new OrderEntity { OrderNumber = 1 };
            splitOrder.OrderItems.Add(new OrderItemEntity(1) { Description = "Foo" });

            var orderSplitDefinition = new OrderSplitDefinition(originalOrder, new Dictionary<long, decimal>(), new Dictionary<long, decimal>(), "");

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.NotEmpty(originalOrder.OrderItems);
            Assert.Empty(splitOrder.OrderItems);
        }
    }
}
