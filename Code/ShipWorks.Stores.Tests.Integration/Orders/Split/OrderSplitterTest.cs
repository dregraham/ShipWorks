using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Orders.Split
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OrderSplitterTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ThreeDCartStoreEntity threeDCartStore;

        public OrderSplitterTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            threeDCartStore = CreateThreeDCartStore();
        }

        [Fact]
        public async Task Split_MovesAllItemsAndChargesToNewOrder_WhenAllRequested()
        {
            ThreeDCartOrderEntity originalOrder = CreateThreeDCartOrder(threeDCartStore, 1234, 2, 3, "", false, true);

            IOrderSplitter testObject = context.Mock.Create<OrderSplitter>();

            Dictionary<long, decimal> itemQuanities = new Dictionary<long, decimal>();
            foreach (var orderItem in originalOrder.OrderItems)
            {
                itemQuanities.Add(orderItem.OrderItemID, (decimal) orderItem.Quantity);
            }

            Dictionary<long, decimal> chargeAmounts = new Dictionary<long, decimal>();
            foreach (var orderCharge in originalOrder.OrderCharges)
            {
                chargeAmounts.Add(orderCharge.OrderChargeID, orderCharge.Amount);
            }

            string newOrderNumber = $"1234-1";

            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(originalOrder, itemQuanities, chargeAmounts, newOrderNumber);

            var result = await testObject.Split(orderSplitDefinition, new ProgressItem("Foo"));
            long newOrderID = result.First(o => o.Key != originalOrder.OrderID).Key;

            IOrderSplitGateway orderSplitGateway = context.Mock.Container.Resolve<IOrderSplitGateway>();
            ThreeDCartOrderEntity newOrder = (ThreeDCartOrderEntity) await orderSplitGateway.LoadOrder(newOrderID);
            originalOrder = (ThreeDCartOrderEntity) await orderSplitGateway.LoadOrder(originalOrder.OrderID);

            Assert.Equal(1234, originalOrder.OrderNumber);
            Assert.Equal("1234", originalOrder.OrderNumberComplete);
            Assert.Equal(0, originalOrder.OrderItems.Count);
            Assert.Equal(0, originalOrder.OrderCharges.Count);
            Assert.Equal(0, originalOrder.OrderTotal);

            ThreeDCartOrderEntity originalOrderForCheckingValues = CreateThreeDCartOrder(threeDCartStore, 1234, 2, 3, "", false, false);
            decimal originalOrderTotal = OrderUtility.CalculateTotal(originalOrderForCheckingValues, true);

            Assert.Equal(1234, newOrder.OrderNumber);
            Assert.Equal("1234-1", newOrder.OrderNumberComplete);
            Assert.Equal(itemQuanities.Count, newOrder.OrderItems.Count);
            Assert.Equal(chargeAmounts.Count, newOrder.OrderCharges.Count);
            Assert.Equal(originalOrderTotal, newOrder.OrderTotal);
            Assert.Equal(originalOrderForCheckingValues.OrderCharges.Sum(oc => oc.Amount), newOrder.OrderCharges.Sum(oc => oc.Amount));
            Assert.Equal(originalOrderForCheckingValues.OrderItems.Sum(oi => oi.Quantity), newOrder.OrderItems.Sum(oi => oi.Quantity));
        }

        [Fact]
        public async Task Split_MovesNoItemsAndChargesToNewOrder_WhenNoneRequested()
        {
            ThreeDCartOrderEntity originalOrder = CreateThreeDCartOrder(threeDCartStore, 1234, 2, 3, "", false, true);

            IOrderSplitter testObject = context.Mock.Create<OrderSplitter>();

            Dictionary<long, decimal> itemQuanities = new Dictionary<long, decimal>();
            Dictionary<long, decimal> chargeAmounts = new Dictionary<long, decimal>();

            string newOrderNumber = $"1234-1";

            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(originalOrder, itemQuanities, chargeAmounts, newOrderNumber);

            var result = await testObject.Split(orderSplitDefinition, new ProgressItem("Foo"));
            long newOrderID = result.First(o => o.Key != originalOrder.OrderID).Key;

            IOrderSplitGateway orderSplitGateway = context.Mock.Container.Resolve<IOrderSplitGateway>();
            ThreeDCartOrderEntity newOrder = (ThreeDCartOrderEntity) await orderSplitGateway.LoadOrder(newOrderID);
            originalOrder = (ThreeDCartOrderEntity) await orderSplitGateway.LoadOrder(originalOrder.OrderID);

            ThreeDCartOrderEntity originalOrderForCheckingValues = CreateThreeDCartOrder(threeDCartStore, 1, 2, 3, "", false, false);
            OrderUtility.CalculateTotal(originalOrderForCheckingValues, true);

            Assert.Equal(originalOrderForCheckingValues.OrderItems.Count, originalOrder.OrderItems.Count);
            Assert.Equal(originalOrderForCheckingValues.OrderCharges.Count, originalOrder.OrderCharges.Count);
            Assert.Equal(originalOrder.OrderTotal, originalOrder.OrderTotal);
            Assert.Equal(originalOrderForCheckingValues.OrderCharges.Sum(oc => oc.Amount), originalOrder.OrderCharges.Sum(oc => oc.Amount));
            Assert.Equal(originalOrderForCheckingValues.OrderItems.Sum(oi => oi.Quantity), originalOrder.OrderItems.Sum(oi => oi.Quantity));

            Assert.Equal(0, newOrder.OrderItems.Count);
            Assert.Equal(0, newOrder.OrderCharges.Count);
            Assert.Equal(0, newOrder.OrderTotal);
            Assert.Equal(0, newOrder.OrderCharges.Sum(oc => oc.Amount));
            Assert.Equal(0, newOrder.OrderItems.Sum(oi => oi.Quantity));
        }

        [Fact]
        public async Task Split_MovesSomeItemsAndChargesToNewOrder_WhenSomeRequested()
        {
            ThreeDCartOrderEntity originalOrder = CreateThreeDCartOrder(threeDCartStore, 1234, 2, 3, "", false, true);

            IOrderSplitter testObject = context.Mock.Create<OrderSplitter>();

            Dictionary<long, decimal> itemQuanities = new Dictionary<long, decimal>();
            OrderItemEntity orderItem = originalOrder.OrderItems.First();
            itemQuanities.Add(orderItem.OrderItemID, (decimal) orderItem.Quantity);

            Dictionary<long, decimal> chargeAmounts = new Dictionary<long, decimal>();
            OrderChargeEntity orderCharge = originalOrder.OrderCharges.First();
            chargeAmounts.Add(orderCharge.OrderChargeID, orderCharge.Amount);

            string newOrderNumber = $"1234-1";

            OrderSplitDefinition orderSplitDefinition = new OrderSplitDefinition(originalOrder, itemQuanities, chargeAmounts, newOrderNumber);

            var result = await testObject.Split(orderSplitDefinition, new ProgressItem("Foo"));
            long newOrderID = result.First(o => o.Key != originalOrder.OrderID).Key;

            IOrderSplitGateway orderSplitGateway = context.Mock.Container.Resolve<IOrderSplitGateway>();
            ThreeDCartOrderEntity newOrder = (ThreeDCartOrderEntity) await orderSplitGateway.LoadOrder(newOrderID);
            originalOrder = (ThreeDCartOrderEntity) await orderSplitGateway.LoadOrder(originalOrder.OrderID);

            ThreeDCartOrderEntity originalOrderForCheckingValues = CreateThreeDCartOrder(threeDCartStore, 1234, 2, 3, "", false, false);
            originalOrderForCheckingValues.OrderItems.Remove(originalOrderForCheckingValues.OrderItems.FirstOrDefault());
            originalOrderForCheckingValues.OrderCharges.Remove(originalOrderForCheckingValues.OrderCharges.FirstOrDefault());
            decimal originalOrderTotal = OrderUtility.CalculateTotal(originalOrderForCheckingValues, true);

            Assert.Equal(1234, originalOrder.OrderNumber);
            Assert.Equal("1234", originalOrder.OrderNumberComplete);
            Assert.Equal(originalOrderForCheckingValues.OrderItems.Count, originalOrder.OrderItems.Count);
            Assert.Equal(originalOrderForCheckingValues.OrderCharges.Count, originalOrder.OrderCharges.Count);
            Assert.Equal(originalOrderTotal, originalOrder.OrderTotal);


            ThreeDCartOrderEntity newOrderForCheckingValues = CreateThreeDCartOrder(threeDCartStore, 1234, 2, 3, "", false, false);
            newOrderForCheckingValues.OrderItems.Clear();
            newOrderForCheckingValues.OrderItems.Add(orderItem);
            newOrderForCheckingValues.OrderCharges.Clear();
            newOrderForCheckingValues.OrderCharges.Add(orderCharge);
            decimal newOrderTotal = OrderUtility.CalculateTotal(newOrderForCheckingValues, true);

            Assert.Equal(1234, newOrder.OrderNumber);
            Assert.Equal("1234-1", newOrder.OrderNumberComplete);
            Assert.Equal(itemQuanities.Count, newOrder.OrderItems.Count);
            Assert.Equal(chargeAmounts.Count, newOrder.OrderCharges.Count);
            Assert.Equal(newOrderTotal, newOrder.OrderTotal);
            Assert.Equal(newOrderForCheckingValues.OrderCharges.Sum(oc => oc.Amount), newOrder.OrderCharges.Sum(oc => oc.Amount));
            Assert.Equal(newOrderForCheckingValues.OrderItems.Sum(oi => oi.Quantity), newOrder.OrderItems.Sum(oi => oi.Quantity));
        }

        private ThreeDCartStoreEntity CreateThreeDCartStore()
        {
            return Create.Store<ThreeDCartStoreEntity>(StoreTypeCode.ThreeDCart)
                .Set(x => x.StoreUrl, "https://shipworks.3dcartstores.com")
                .Set(x => x.ApiUserKey, "12288569944166522122885699441665")
                .Set(x => x.TimeZoneID, "Central Standard Time")
                .Set(x => x.StatusCodes, @"<StatusCodes><StatusCode><Code>1</Code><Name>New and Fresh</Name></StatusCode></StatusCodes>")
                .Set(x => x.DownloadModifiedNumberOfDaysBack, 7)
                .Set(x => x.RestUser, true)
                .Save();
        }

        private ThreeDCartOrderEntity CreateThreeDCartOrder(ThreeDCartStoreEntity store, int orderNumber, int item2Quantity, int item3Quantity, string trackingNumber, bool manual, bool save)
        {
            var orderBuilder = Create.Order<ThreeDCartOrderEntity>(store, context.Customer)
                .WithItem<ThreeDCartOrderItemEntity>(i =>
                {
                    i.Set(x => x.ThreeDCartShipmentID, item2Quantity * 100L);
                    i.Set(x => x.Name, "Foo");
                    i.Set(x => x.Code = "Foo");
                    i.Set(x => x.SKU = "Foo");
                    i.Set(x => x.Location = "Foo");
                    i.Set(x => x.UnitPrice, 1.8M);
                    i.Set(x => x.Quantity, 3);
                    i.Set(x => x.Weight, 2.5);
                    i.Set(x => x.HarmonizedCode, "HAR123");
                    i.WithItemAttribute(ia =>
                    {
                        ia.Set(x => x.Name, "ItemAttr1.1");
                        ia.Set(x => x.UnitPrice, 1.23M);
                    });
                    i.WithItemAttribute(ia =>
                    {
                        ia.Set(x => x.Name, "ItemAttr1.2");
                        ia.Set(x => x.UnitPrice, 2.59M);
                    });
                })
                .WithItem<ThreeDCartOrderItemEntity>(i => i
                    .Set(x => x.ThreeDCartShipmentID, item2Quantity * 100L)
                    .Set(x => x.Code = item2Quantity.ToString())
                    .Set(x => x.SKU = item2Quantity.ToString())
                    .Set(x => x.Location = item2Quantity.ToString())
                    .Set(x => x.Name = item2Quantity.ToString())
                    .Set(x => x.Weight = item2Quantity)
                    .Set(x => x.UnitPrice = item2Quantity)
                    .Set(x => x.Quantity, item2Quantity))
                .WithItem<ThreeDCartOrderItemEntity>(i => i
                    .Set(x => x.ThreeDCartShipmentID, item3Quantity * 100L)
                    .Set(x => x.Code = item3Quantity.ToString())
                    .Set(x => x.SKU = item3Quantity.ToString())
                    .Set(x => x.Location = item3Quantity.ToString())
                    .Set(x => x.Name = item3Quantity.ToString())
                    .Set(x => x.Weight = item3Quantity)
                    .Set(x => x.UnitPrice = item3Quantity)
                    .Set(x => x.Quantity, item3Quantity))
                .WithNote(i => i.Set(x => x.Text, "Foo"))
                .WithPaymentDetail(i => i
                    .Set(x => x.Label, "Foo")
                    .Set(x => x.Value = "3"))
                .WithCharge(o => o
                    .Set(c => c.Description = "Charge1")
                    .Set(c => c.Amount = item2Quantity))
                .WithCharge(o => o
                    .Set(c => c.Description = "Charge2")
                    .Set(c => c.Amount = item3Quantity))
                .WithOrderSearch(o => o
                    .Set(os => os.OrderNumber = context.Order.OrderNumber)
                    .Set(os => os.OriginalOrderID = context.Order.OrderID)
                    .Set(os => os.OrderID = context.Order.OrderID)
                    .Set(os => os.StoreID = context.Store.StoreID)
                    .Set(os => os.IsManual = false)
                    .Set(os => os.OrderNumberComplete = context.Order.OrderNumberComplete))
                .Set(x => x.OrderNumber, orderNumber)
                .Set(x => x.OrderNumberComplete, orderNumber.ToString())
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.None)
                .Set(x => x.RollupItemCount = 0)
                .Set(x => x.IsManual, manual)
                .Set(x => x.ThreeDCartOrderID, orderNumber)
                .Set(o => o.OnlineStatus, EnumHelper.GetApiValue(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.New));

            ThreeDCartOrderEntity order = save ? orderBuilder.Save() : orderBuilder.Build();

            order.OrderTotal = OrderUtility.CalculateTotal(order);

            var shipmentBuilder = Create.Shipment(order)
                .AsUps(builder => builder.WithPackage(pBuilder => pBuilder.Set(p =>
                {
                    p.Weight = 10;
                    p.DimsWidth = 10;
                    p.DimsHeight = 10;
                    p.DimsLength = 10;
                })))
                .Set(x => x.TrackingNumber, trackingNumber)
                .Set(x => x.Processed, false);

            if (save)
            {
                shipmentBuilder.Save();
            }
            else
            {
                shipmentBuilder.Build();
            }

            return order;
        }

        public void Dispose() => context.Dispose();
    }
}
