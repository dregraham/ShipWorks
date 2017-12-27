﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    public class OrderItemSplitterTest
    {
        [Fact]
        public void Split_SplitOrder_HasNoOrderItems_WhenNoOrderItemsRequested()
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

            Dictionary<long, double> newOrderItemQuantities = new Dictionary<long, double>();

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(newOrderItemQuantities, originalOrder, splitOrder);

            Assert.Equal(0, splitOrder.OrderItems.Count);
            Assert.Equal(2, originalOrder.OrderItems.Count);

            Assert.Equal(1, originalOrder.OrderItems.First(oc => oc.OrderItemID == 1).Quantity);
            Assert.Equal(3, originalOrder.OrderItems.First(oc => oc.OrderItemID == 2).Quantity);
        }

        [Fact]
        public void Split_SplitOrder_HasAllOrderItems_WhenAllOrderItemsRequested()
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

            Dictionary<long, double> newOrderItemQuantities = new Dictionary<long, double>();
            newOrderItemQuantities.Add(1, 1);
            newOrderItemQuantities.Add(2, 3);

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(newOrderItemQuantities, originalOrder, splitOrder);

            Assert.Equal(2, splitOrder.OrderItems.Count);
            Assert.Equal(0, originalOrder.OrderItems.Count);

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

            Dictionary<long, double> newOrderItemQuantities = new Dictionary<long, double>();
            newOrderItemQuantities.Add(1, 0.5);
            newOrderItemQuantities.Add(2, 1.5);

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(newOrderItemQuantities, originalOrder, splitOrder);

            Assert.Equal(2, splitOrder.OrderItems.Count);
            Assert.Equal(2, originalOrder.OrderItems.Count);

            Assert.Equal(0.5, splitOrder.OrderItems.First(oc => oc.OrderItemID == 1).Quantity);
            Assert.Equal(1.5, splitOrder.OrderItems.First(oc => oc.OrderItemID == 2).Quantity);

            Assert.Equal(0.5, originalOrder.OrderItems.First(oc => oc.OrderItemID == 1).Quantity);
            Assert.Equal(1.5, originalOrder.OrderItems.First(oc => oc.OrderItemID == 2).Quantity);
        }

        [Fact]
        public void Split_SplitOrder_HasCorrectOrderItemCounts_ForZeroQuantities()
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

            Dictionary<long, double> newOrderItemQuantities = new Dictionary<long, double>();
            newOrderItemQuantities.Add(1, 0);
            newOrderItemQuantities.Add(2, 0);

            OrderItemSplitter testObject = new OrderItemSplitter();
            testObject.Split(newOrderItemQuantities, originalOrder, splitOrder);

            Assert.Equal(0, splitOrder.OrderItems.Count);
            Assert.Equal(2, originalOrder.OrderItems.Count);
            
            Assert.Equal(1, originalOrder.OrderItems.First(oc => oc.OrderItemID == 1).Quantity);
            Assert.Equal(3, originalOrder.OrderItems.First(oc => oc.OrderItemID == 2).Quantity);
        }
    }
}
