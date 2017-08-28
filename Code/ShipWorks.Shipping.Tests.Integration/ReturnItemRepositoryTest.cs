using System;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ReturnItemRepositoryTest : IDisposable
    {
        private readonly DataContext context;

        public ReturnItemRepositoryTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock => mock.Provide(mock.Create<ISqlAdapter>()));
        }

        [Fact]
        public void LoadReturnData_LoadsExistingReturnItems_WhenExistingReturnItemsExist()
        {
            var shipment = Create.Shipment(context.Order).WithReturnItem().Save();
            var returnItem = shipment.ReturnItems.Single();

            var testObject = context.Mock.Create<ReturnItemRepository>();

            shipment.ReturnItems.Clear();
            testObject.LoadReturnData(shipment, false);
            
            Assert.Equal(returnItem, shipment.ReturnItems.Single());
        }

        [Fact]
        public void LoadReturnData_LoadsNewShipmentReturnItems_WhenNoExistingReturnItemsExist_AndCreateIfNoneIsTrue()
        {
            var order = Create.Order(context.Store, context.Customer).WithItem(itemBuilder => itemBuilder.Set(item =>
            {
                item.Name = "Joe";
                item.Quantity = 10;
                item.Weight = 20;
                item.SKU = "the sku";
                item.Code = "code";
            })).Save();

            var shipment = Create.Shipment(order).Save();

            var testObject = context.Mock.Create<ReturnItemRepository>();

            testObject.LoadReturnData(shipment, true);

            var returnItem = shipment.ReturnItems.Single();

            Assert.Equal("Joe", returnItem.Name);
            Assert.Equal(10, returnItem.Quantity);
            Assert.Equal(20, returnItem.Weight);
            Assert.Equal("the sku", returnItem.SKU);
            Assert.Equal("code", returnItem.Code);
        }

        public void Dispose() => context.Dispose();
    }
}