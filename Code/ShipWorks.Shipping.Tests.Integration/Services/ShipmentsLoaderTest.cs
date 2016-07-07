using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Loading;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ShipmentsLoaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly OrderEntity order;
        private readonly ShipmentsLoader testObject;

        public ShipmentsLoaderTest(DatabaseFixture db)
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

            testObject = mock.Create<ShipmentsLoader>();
        }

        [Fact]
        public async Task Load_ReturnsEmptyShipmentCollection_WhenOrderHasNoShipments()
        {
            var newOrder = Create.Order(context.Store, context.Customer)
                .WithOrderNumber(12345)
                .WithItem()
                .WithItem()
                .Save();

            context.UpdateShippingSetting(x => x.AutoCreateShipments = false);

            var results = await testObject.LoadAsync(new[] { newOrder.OrderID });

            Assert.Empty(results.Shipments);
        }

        [Fact]
        public async Task Load_ReturnsShipments_FromLoadedShipments()
        {
            Modify.Shipment(order.Shipments[0]).AsPostal(x => x.AsUsps()).Save();
            Modify.Shipment(order.Shipments[1]).AsFedEx(x => x.WithPackage()).Save();

            var results = await testObject.LoadAsync(new[] { order.OrderID });

            Assert.Contains(ShipmentTypeCode.Usps, results.Shipments.Select(x => x.ShipmentTypeCode));
            Assert.Contains(ShipmentTypeCode.FedEx, results.Shipments.Select(x => x.ShipmentTypeCode));
        }

        [Fact]
        public async Task Load_CreatesShipment_WhenOrderHasNoShipments()
        {
            OrderEntity testOrder = Create.Order(order.Store, order.Customer).Save();

            context.UpdateShippingSetting(x => x.AutoCreateShipments = true);

            var results = await mock.Create<ShipmentsLoader>().LoadAsync(new[] { testOrder.OrderID });

            Assert.Equal(1, results.Shipments.Count);
        }

        [Fact]
        public async Task Load_IncludesOrderItems()
        {
            var response = await testObject.LoadAsync(new[] { order.OrderID });

            OrderEntity results = response.Shipments.First().Order;

            Assert.Equal(2, results.OrderItems.Count);
        }

        [Fact]
        public async Task Load_IncludesStore()
        {
            var response = await testObject.LoadAsync(new[] { order.OrderID });

            OrderEntity results = response.Shipments.First().Order;

            Assert.NotNull(results.Store);
        }

        [Fact]
        public async Task Load_DoesNotIncludeCustomer()
        {
            var response = await testObject.LoadAsync(new[] { order.OrderID });

            OrderEntity results = response.Shipments.First().Order;

            Assert.Null(results.Customer);
        }

        [Fact]
        public async Task Load_IncludesShipments()
        {
            var response = await testObject.LoadAsync(new[] { order.OrderID });

            OrderEntity results = response.Shipments.First().Order;

            Assert.Equal(2, results.Shipments.Count);
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

            var results = await testObject.LoadAsync(new[] { order.OrderID });

            ShipmentEntity loadedShipment = results.Shipments.Where(x => x.ShipmentID == shipment.ShipmentID).First();
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

            var response = await testObject.LoadAsync(new[] { order.OrderID });

            OrderEntity results = response.Shipments.First().Order;

            Assert.NotNull(results.Shipment(shipment.ShipmentID).FedEx);
            Assert.Equal(2, results.Shipment(shipment.ShipmentID).FedEx.Packages.Count);
        }

        [Fact]
        public async Task Load_IncludesUpsShipmentAndPackages()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .AsUps(Ups => Ups.WithPackage().WithPackage())
                .Save();

            var response = await testObject.LoadAsync(new[] { order.OrderID });

            OrderEntity results = response.Shipments.First().Order;

            Assert.NotNull(results.Shipment(shipment.ShipmentID).Ups);
            Assert.Equal(2, results.Shipment(shipment.ShipmentID).Ups.Packages.Count);
        }

        [Fact]
        public async Task Load_IncludesIParcelShipmentAndPackages()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .AsIParcel(IParcel => IParcel.WithPackage().WithPackage())
                .Save();

            var response = await testObject.LoadAsync(new[] { order.OrderID });

            OrderEntity results = response.Shipments.First().Order;

            Assert.NotNull(results.Shipment(shipment.ShipmentID).IParcel);
            Assert.Equal(2, results.Shipment(shipment.ShipmentID).IParcel.Packages.Count);
        }

        [Fact]
        public async Task Load_IncludesCustomsItems()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .WithCustomsItem()
                .WithCustomsItem()
                .Save();

            var response = await testObject.LoadAsync(new[] { order.OrderID });

            OrderEntity results = response.Shipments.First().Order;

            Assert.Equal(2, results.Shipment(shipment.ShipmentID).CustomsItems.Count);
        }

        [Fact]
        public async Task Load_IncludesInsurance()
        {
            long shipmentId = Create.Shipment(order)
                .WithInsurancePolicy()
                .Save()
                .ShipmentID;

            var response = await testObject.LoadAsync(new[] { order.OrderID });

            OrderEntity results = response.Shipments.First().Order;

            Assert.NotNull(results.Shipment(shipmentId).InsurancePolicy);
        }

        public void Dispose() => context.Dispose();
    }
}
