using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    public class OrderChargeSplitterTest
    {
        [Fact]
        public void Split_SplitOrder_HasNoChargeAmounts_WhenNoChargesRequested()
        {
            OrderEntity originalOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            originalOrder.OrderCharges.Add(new OrderChargeEntity(1) { Amount = 1 });
            originalOrder.OrderCharges.Add(new OrderChargeEntity(2) { Amount = 3 });

            OrderEntity splitOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            splitOrder.OrderCharges.Add(new OrderChargeEntity(1) { Amount = 1 });
            splitOrder.OrderCharges.Add(new OrderChargeEntity(2) { Amount = 3 });

            Dictionary<long, decimal> newOrderChargeAmounts = new Dictionary<long, decimal>();

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(new OrderSplitDefinition(originalOrder, new Dictionary<long, decimal>(), newOrderChargeAmounts, ""), originalOrder, splitOrder);

            Assert.Equal(0, splitOrder.OrderCharges.Sum(oi => oi.Amount));
            Assert.Equal(4, originalOrder.OrderCharges.Sum(oi => oi.Amount));

            Assert.Equal(1M, originalOrder.OrderCharges.First(oc => oc.OrderChargeID == 1).Amount);
            Assert.Equal(3M, originalOrder.OrderCharges.First(oc => oc.OrderChargeID == 2).Amount);
        }

        [Fact]
        public void Split_SplitOrder_HasAllChargeAmounts_WhenAllChargesRequested()
        {
            OrderEntity originalOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            originalOrder.OrderCharges.Add(new OrderChargeEntity(1) { Amount = 1 });
            originalOrder.OrderCharges.Add(new OrderChargeEntity(2) { Amount = 3 });

            OrderEntity splitOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            splitOrder.OrderCharges.Add(new OrderChargeEntity(1) { Amount = 1 });
            splitOrder.OrderCharges.Add(new OrderChargeEntity(2) { Amount = 3 });

            Dictionary<long, decimal> newOrderChargeAmounts = new Dictionary<long, decimal>();
            newOrderChargeAmounts.Add(1, 1);
            newOrderChargeAmounts.Add(2, 3);

            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(originalOrder, new Dictionary<long, decimal>(), newOrderChargeAmounts, "");

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Equal(4, splitOrder.OrderCharges.Sum(oi => oi.Amount));
            Assert.Equal(0, originalOrder.OrderCharges.Sum(oi => oi.Amount));

            Assert.Equal(1M, splitOrder.OrderCharges.First(oc => oc.OrderChargeID == 1).Amount);
            Assert.Equal(3M, splitOrder.OrderCharges.First(oc => oc.OrderChargeID == 2).Amount);
        }

        [Fact]
        public void Split_SplitOrder_HasCorrectChargeAmounts_ForChangedAmounts()
        {
            OrderEntity originalOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            originalOrder.OrderCharges.Add(new OrderChargeEntity(1) { Amount = 1 });
            originalOrder.OrderCharges.Add(new OrderChargeEntity(2) { Amount = 3 });

            OrderEntity splitOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            splitOrder.OrderCharges.Add(new OrderChargeEntity(1) { Amount = 1 });
            splitOrder.OrderCharges.Add(new OrderChargeEntity(2) { Amount = 3 });

            Dictionary<long, decimal> newOrderChargeAmounts = new Dictionary<long, decimal>();
            newOrderChargeAmounts.Add(1, 0.5M);
            newOrderChargeAmounts.Add(2, 1.5M);

            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(originalOrder, new Dictionary<long, decimal>(), newOrderChargeAmounts, "");

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Equal(2, splitOrder.OrderCharges.Count);
            Assert.Equal(2, originalOrder.OrderCharges.Count);

            Assert.Equal(0.5M, splitOrder.OrderCharges.First(oc => oc.OrderChargeID == 1).Amount);
            Assert.Equal(1.5M, splitOrder.OrderCharges.First(oc => oc.OrderChargeID == 2).Amount);

            Assert.Equal(0.5M, originalOrder.OrderCharges.First(oc => oc.OrderChargeID == 1).Amount);
            Assert.Equal(1.5M, originalOrder.OrderCharges.First(oc => oc.OrderChargeID == 2).Amount);
        }

        [Fact]
        public void Split_SplitOrder_HasCorrectChargeAmounts_ForZeroAmounts()
        {
            OrderEntity originalOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            originalOrder.OrderCharges.Add(new OrderChargeEntity(1) { Amount = 1 });
            originalOrder.OrderCharges.Add(new OrderChargeEntity(2) { Amount = 3 });

            OrderEntity splitOrder = new OrderEntity()
            {
                OrderNumber = 1
            };

            splitOrder.OrderCharges.Add(new OrderChargeEntity(1) { Amount = 1 });
            splitOrder.OrderCharges.Add(new OrderChargeEntity(2) { Amount = 3 });

            Dictionary<long, decimal> newOrderChargeAmounts = new Dictionary<long, decimal>();
            newOrderChargeAmounts.Add(1, 0M);
            newOrderChargeAmounts.Add(2, 0M);

            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(originalOrder, new Dictionary<long, decimal>(), newOrderChargeAmounts, "");

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Equal(0, splitOrder.OrderCharges.Sum(oi => oi.Amount));
            Assert.Equal(4, originalOrder.OrderCharges.Sum(oi => oi.Amount));

            Assert.Equal(1M, originalOrder.OrderCharges.First(oc => oc.OrderChargeID == 1).Amount);
            Assert.Equal(3M, originalOrder.OrderCharges.First(oc => oc.OrderChargeID == 2).Amount);
        }

        [Fact]
        public void Split_RemovesChargesWithZeroAmountFromOriginalOrder_WhenSplitOrderHasChargeWithNonZeroAmount()
        {
            var originalOrder = new OrderEntity { OrderNumber = 1 };
            originalOrder.OrderCharges.Add(new OrderChargeEntity(1) { Description = "Foo", Amount = 5 });

            OrderEntity splitOrder = new OrderEntity { OrderNumber = 1 };
            splitOrder.OrderCharges.Add(new OrderChargeEntity(1) { Description = "Foo" });

            Dictionary<long, decimal> newOrderChargeAmounts = new Dictionary<long, decimal> { { 1, 5 } };

            var orderSplitDefinition = new OrderSplitDefinition(originalOrder, new Dictionary<long, decimal>(), newOrderChargeAmounts, "");

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Empty(originalOrder.OrderCharges);
        }

        [Fact]
        public void Split_RemovesChargesWithZeroAmountFromSplitOrder_WhenOriginalOrderHasChargeWithNonZeroAmount()
        {
            var originalOrder = new OrderEntity { OrderNumber = 1 };
            originalOrder.OrderCharges.Add(new OrderChargeEntity(1) { Description = "Foo", Amount = 5 });

            OrderEntity splitOrder = new OrderEntity { OrderNumber = 1 };
            splitOrder.OrderCharges.Add(new OrderChargeEntity(1) { Description = "Foo" });

            Dictionary<long, decimal> newOrderChargeAmounts = new Dictionary<long, decimal> { { 1, 0 } };

            var orderSplitDefinition = new OrderSplitDefinition(originalOrder, new Dictionary<long, decimal>(), newOrderChargeAmounts, "");

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.Empty(splitOrder.OrderCharges);
        }

        [Fact]
        public void Split_DoesNotRemoveChargesWithZeroAmountFromOriginalOrder_WhenSplitOrderHasChargeWithNonZeroAmount()
        {
            var originalOrder = new OrderEntity { OrderNumber = 1 };
            originalOrder.OrderCharges.Add(new OrderChargeEntity(1) { Description = "Foo", Amount = 0 });

            OrderEntity splitOrder = new OrderEntity { OrderNumber = 1 };
            splitOrder.OrderCharges.Add(new OrderChargeEntity(1) { Description = "Foo" });

            Dictionary<long, decimal> newOrderChargeAmounts = new Dictionary<long, decimal> { { 1, 0 } };

            var orderSplitDefinition = new OrderSplitDefinition(originalOrder, new Dictionary<long, decimal>(), newOrderChargeAmounts, "");

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.NotEmpty(originalOrder.OrderCharges);
            Assert.Empty(splitOrder.OrderCharges);
        }

        [Fact]
        public void Split_DoesNotRemoveChargesWithZeroAmountFromOriginalOrder_WhenNotIncludedInSplitAmounts()
        {
            var originalOrder = new OrderEntity { OrderNumber = 1 };
            originalOrder.OrderCharges.Add(new OrderChargeEntity(1) { Description = "Foo", Amount = 0 });

            OrderEntity splitOrder = new OrderEntity { OrderNumber = 1 };
            splitOrder.OrderCharges.Add(new OrderChargeEntity(1) { Description = "Foo" });

            var orderSplitDefinition = new OrderSplitDefinition(originalOrder, new Dictionary<long, decimal>(), new Dictionary<long, decimal>(), "");

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(orderSplitDefinition, originalOrder, splitOrder);

            Assert.NotEmpty(originalOrder.OrderCharges);
            Assert.Empty(splitOrder.OrderCharges);
        }
    }
}
