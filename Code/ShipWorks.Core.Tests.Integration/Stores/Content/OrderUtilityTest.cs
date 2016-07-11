using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Stores.Content
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OrderUtilityTest : IDisposable
    {
        private readonly DataContext context;

        public OrderUtilityTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            OrderItemAttributeCollection orderItemAttributes = new OrderItemAttributeCollection();
            orderItemAttributes.Add(new OrderItemAttributeEntity
                {
                    UnitPrice = 2.00M,
                    IsManual = false,
                    Name = "attr1",
                    Description = "attr1 desc"
                });
            orderItemAttributes.Add(new OrderItemAttributeEntity
                {
                    UnitPrice = 0.10M,
                IsManual = false,
                Name = "attr2",
                Description = "attr2 desc"
            });

            Modify.Order(context.Order)
                .WithOrderNumber(123999)
                .WithItem(x =>
                {
                    x.Set(i => i.Quantity, 2);
                    x.Set(i => i.UnitPrice, 1.23M); // 2 * 1.23 = 2.46
                })
                .WithItem(x =>
                {
                    x.Set(i => i.Quantity, 1);
                    x.Set(i => i.UnitPrice, 5.95M); // 1 * 5.95 = 5.95
                })
                .WithShipment()
                .WithShipment()
                .Save();

            context.Order.OrderItems.First().OrderItemAttributes.AddRange(orderItemAttributes); // (2 * 2) + (2 * 0.10) = 4.20

            OrderChargeEntity charge = new OrderChargeEntity();
            charge.Amount = 1.00M;
            charge.Type = "Tax";
            charge.Description = "Tax";
            charge.OrderID = context.Order.OrderID;

            context.Order.OrderCharges.Add(charge);
            Modify.Order(context.Order).Save();

            // Total = 2.46 + 5.95 + 4.20 + 1.00 = 13.61
        }

        [Fact]
        public void CalculateTotalByOrderEntity_ReturnsCorrectTotal_WhenChargesIncluded()
        {
            decimal actualValue = OrderUtility.CalculateTotal(context.Order);

            Assert.Equal(13.61M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderEntity_ReturnsCorrectTotal_WhenChargesNotIncluded()
        {
            context.Order.OrderCharges.Clear();
            decimal actualValue = OrderUtility.CalculateTotal(context.Order);

            Assert.Equal(12.61M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderEntity_ReturnsCorrectTotal_WhenItemsNotIncluded()
        {
            context.Order.OrderItems.Clear();
            decimal actualValue = OrderUtility.CalculateTotal(context.Order);

            Assert.Equal(1.00M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderEntity_ReturnsCorrectTotal_WhenItemsAndChargesNotIncluded()
        {
            context.Order.OrderItems.Clear();
            context.Order.OrderCharges.Clear();
            decimal actualValue = OrderUtility.CalculateTotal(context.Order);

            Assert.Equal(0M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderProperties_ReturnsCorrectTotal_WhenChargesIncluded()
        {
            decimal actualValue = OrderUtility.CalculateTotal(context.Order.OrderItems, context.Order.OrderCharges);

            Assert.Equal(13.61M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderProperties_ReturnsCorrectTotal_WhenChargesNotIncluded()
        {
            context.Order.OrderCharges.Clear();
            decimal actualValue = OrderUtility.CalculateTotal(context.Order.OrderItems, context.Order.OrderCharges);

            Assert.Equal(12.61M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderProperties_ReturnsCorrectTotal_WhenItemsNotIncluded()
        {
            context.Order.OrderItems.Clear();
            decimal actualValue = OrderUtility.CalculateTotal(context.Order.OrderItems, context.Order.OrderCharges);

            Assert.Equal(1.00M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderProperties_ReturnsCorrectTotal_WhenItemsAndChargesNotIncluded()
        {
            context.Order.OrderItems.Clear();
            context.Order.OrderCharges.Clear();
            decimal actualValue = OrderUtility.CalculateTotal(context.Order.OrderItems, context.Order.OrderCharges);

            Assert.Equal(0M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderId_ReturnsCorrectTotal_WhenIncludeChargesIsTrue()
        {
            decimal actualValue = OrderUtility.CalculateTotal(context.Order, true);

            Assert.Equal(13.61M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderId_ReturnsCorrectTotal_WhenIncludeChargesIsFalse()
        {
            decimal actualValue = OrderUtility.CalculateTotal(context.Order, false);

            Assert.Equal(12.61M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderId_ReturnsCorrectTotal_WhenChargesButNoItemsAndIncludeChargesIsTrue()
        {
            context.Order.OrderItems.ToList().ForEach(oi => Modify.Order(context.Order).Delete(oi.OrderItemAttributes));
            Modify.Order(context.Order).Delete(context.Order.OrderItems);

            context.Order.OrderItems.Clear();

            decimal actualValue = OrderUtility.CalculateTotal(context.Order, true);

            Assert.Equal(1.00M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderId_ReturnsCorrectTotal_WhenNoChargesAndNotItemsAndIncludeChargesIsTrue()
        {
            context.Order.OrderItems.ToList().ForEach(oi => Modify.Order(context.Order).Delete(oi.OrderItemAttributes));
            Modify.Order(context.Order).Delete(context.Order.OrderItems);
            Modify.Order(context.Order).Delete(context.Order.OrderCharges);

            context.Order.OrderItems.Clear();
            context.Order.OrderCharges.Clear();

            decimal actualValue = OrderUtility.CalculateTotal(context.Order, true);

            Assert.Equal(0M, actualValue);
        }

        [Fact]
        public void CalculateTotalByOrderId_ReturnsCorrectTotal_WhenNoChargesAndNotItemsAndIncludeChargesIsFalse()
        {
            context.Order.OrderItems.ToList().ForEach(oi => Modify.Order(context.Order).Delete(oi.OrderItemAttributes));
            Modify.Order(context.Order).Delete(context.Order.OrderItems);
            Modify.Order(context.Order).Delete(context.Order.OrderCharges);

            context.Order.OrderItems.Clear();
            context.Order.OrderCharges.Clear();

            decimal actualValue = OrderUtility.CalculateTotal(context.Order, false);

            Assert.Equal(0M, actualValue);
        }

        public void Dispose() => context.Dispose();
    }
}
