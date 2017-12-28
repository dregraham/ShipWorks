using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            testObject.Split(newOrderChargeAmounts, originalOrder, splitOrder);

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

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(newOrderChargeAmounts, originalOrder, splitOrder);

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

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(newOrderChargeAmounts, originalOrder, splitOrder);

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

            OrderChargeSplitter testObject = new OrderChargeSplitter();
            testObject.Split(newOrderChargeAmounts, originalOrder, splitOrder);

            Assert.Equal(0, splitOrder.OrderCharges.Sum(oi => oi.Amount));
            Assert.Equal(4, originalOrder.OrderCharges.Sum(oi => oi.Amount));
            
            Assert.Equal(1M, originalOrder.OrderCharges.First(oc => oc.OrderChargeID == 1).Amount);
            Assert.Equal(3M, originalOrder.OrderCharges.First(oc => oc.OrderChargeID == 2).Amount);
        }
    }
}
