using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Loading;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    public class ShipmentLoaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly OrderEntity order;
        private readonly ShipmentLoader testObject;

        public ShipmentLoaderTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;

            order = Modify.Order(context.Order)
                .WithOrderNumber(12345)
                .WithItem()
                .WithItem()
                .WithShipment()
                .WithShipment()
                .Save();

            testObject = mock.Create<ShipmentLoader>();
        }

        [Fact]
        public async Task Load_ReturnsOrder_WhenOrderHasNoShipments()
        {
            var loadedOrder = await testObject.Load(order.OrderID);

            Assert.Equal(order.OrderID, loadedOrder.Order.OrderID);
        }

        [Fact]
        public async Task Load_ReturnsOrder_WhenOrderHasShipment()
        {
            Create.Shipment(order).AsUps().Save();

            var loadedOrder = await testObject.Load(order.OrderID);

            Assert.Equal(order.OrderID, loadedOrder.Order.OrderID);
        }

        [Fact]
        public async Task Load_ReturnsShipmentAdapters_FromLoadedShipments()
        {
            Modify.Shipment(order.Shipments[0]).AsPostal(x => x.AsUsps()).Save();
            Modify.Shipment(order.Shipments[1]).AsFedEx().Save();

            var loadedOrder = await testObject.Load(order.OrderID);

            Assert.NotNull(loadedOrder.ShipmentAdapters.OfType<UspsShipmentAdapter>().Single());
            Assert.NotNull(loadedOrder.ShipmentAdapters.OfType<FedExShipmentAdapter>().Single());
        }

        [Fact]
        public async Task Load_CreatesShipment_WhenOrderHasNoShipments()
        {
            OrderEntity testOrder = Create.Order(order.Store, order.Customer).Save();

            mock.Override<IShippingConfiguration>()
                .Setup(x => x.ShouldAutoCreateShipment(It.IsAny<OrderEntity>()))
                .Returns(true);

            var loadedOrder = await mock.Create<ShipmentLoader>().Load(testOrder.OrderID);

            Assert.Equal(1, loadedOrder.Order.Shipments.Count);
        }

        [Fact]
        public async Task Load_IncludesOrderItems()
        {
            var response = await testObject.Load(order.OrderID);

            OrderEntity loadedOrder = response.Order;

            Assert.Equal(2, loadedOrder.OrderItems.Count);
        }

        [Fact]
        public async Task Load_IncludesStore()
        {
            var response = await testObject.Load(order.OrderID);

            OrderEntity loadedOrder = response.Order;

            Assert.NotNull(loadedOrder.Store);
        }

        [Fact]
        public async Task Load_DoesNotIncludeCustomer()
        {
            var response = await testObject.Load(order.OrderID);

            OrderEntity loadedOrder = response.Order;

            Assert.Null(loadedOrder.Customer);
        }

        [Fact]
        public async Task Load_IncludesShipments()
        {
            var response = await testObject.Load(order.OrderID);

            OrderEntity loadedOrder = response.Order;

            Assert.Equal(2, loadedOrder.Shipments.Count);
        }

        [Fact]
        public async Task Load_IncludesAllCarrierShipmentData()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .AsUps()
                .AsPostal(x => x.AsUsps().AsEndicia())
                .AsIParcel()
                .AsOnTrac()
                .AsAmazon()
                .AsBestRate()
                .AsFedEx()
                .AsOther()
                .Save();

            var response = await testObject.Load(order.OrderID);

            OrderEntity loadedOrder = response.Order;

            ShipmentEntity loadedShipment = loadedOrder.Shipment(shipment.ShipmentID);
            Assert.NotNull(loadedShipment.Postal.Endicia);
            Assert.NotNull(loadedShipment.Postal.Usps);
            Assert.NotNull(loadedShipment.OnTrac);
            Assert.NotNull(loadedShipment.Amazon);
            Assert.NotNull(loadedShipment.BestRate);
            Assert.NotNull(loadedShipment.Other);
        }

        [Fact]
        public async Task Load_IncludesFedExShipmentAndPackages()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .AsFedEx(fedEx => fedEx.WithPackage().WithPackage())
                .Save();

            var response = await testObject.Load(order.OrderID);

            OrderEntity loadedOrder = response.Order;

            Assert.NotNull(loadedOrder.Shipment(shipment.ShipmentID).FedEx);
            Assert.Equal(2, loadedOrder.Shipment(shipment.ShipmentID).FedEx.Packages.Count);
        }

        [Fact]
        public async Task Load_IncludesUpsShipmentAndPackages()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .AsUps(Ups => Ups.WithPackage().WithPackage())
                .Save();

            var response = await testObject.Load(order.OrderID);

            OrderEntity loadedOrder = response.Order;

            Assert.NotNull(loadedOrder.Shipment(shipment.ShipmentID).Ups);
            Assert.Equal(2, loadedOrder.Shipment(shipment.ShipmentID).Ups.Packages.Count);
        }

        [Fact]
        public async Task Load_IncludesIParcelShipmentAndPackages()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .AsIParcel(IParcel => IParcel.WithPackage().WithPackage())
                .Save();

            var response = await testObject.Load(order.OrderID);

            OrderEntity loadedOrder = response.Order;

            Assert.NotNull(loadedOrder.Shipment(shipment.ShipmentID).IParcel);
            Assert.Equal(2, loadedOrder.Shipment(shipment.ShipmentID).IParcel.Packages.Count);
        }

        [Fact]
        public async Task Load_IncludesCustomsItems()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .WithCustomsItem()
                .WithCustomsItem()
                .Save();

            var response = await testObject.Load(order.OrderID);

            OrderEntity loadedOrder = response.Order;

            Assert.Equal(2, loadedOrder.Shipment(shipment.ShipmentID).CustomsItems.Count);
        }

        [Fact]
        public async Task Load_IncludesInsurance()
        {
            long shipmentId = Create.Shipment(order)
                .WithInsurancePolicy()
                .Save()
                .ShipmentID;

            var response = await testObject.Load(order.OrderID);

            OrderEntity loadedOrder = response.Order;

            Assert.NotNull(loadedOrder.Shipment(shipmentId).InsurancePolicy);
        }

        public void Dispose() => context.Dispose();
    }
}
