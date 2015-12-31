using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    public class ShippingManagerWrapperTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OrderEntity order;

        public ShippingManagerWrapperTest(DatabaseFixture db)
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            ContainerInitializer.Initialize(mock.Container);

            db.CreateDataContext(mock);

            var store = Create.Entity<GenericModuleStoreEntity>().Save();
            var customer = Create.Entity<CustomerEntity>().Save();

            order = Create.Order(store, customer).WithOrderNumber(123999)
                .WithItem()
                .WithItem()
                .WithShipment()
                .WithShipment()
                .Save();
        }

        [Fact]
        public void LoadFullShipmentGraph_LoadsOrder()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            Assert.Equal(123999, loadedOrder.OrderNumber);
        }

        [Fact]
        public void LoadFullShipmentGraph_LoadsOrderItems()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            Assert.Equal(2, loadedOrder.OrderItems.Count);
        }

        [Fact]
        public void LoadFullShipmentGraph_LoadsStore()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            Assert.NotNull(loadedOrder.Store);
        }

        [Fact]
        public void LoadFullShipmentGraph_DoesNotLoadCustomerForOrder()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            Assert.Null(loadedOrder.Customer);
        }

        [Fact]
        public void LoadFullShipmentGraph_LoadsShipments()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            Assert.Equal(2, loadedOrder.Shipments.Count);
        }

        [Fact]
        public void LoadFullShipmentGraph_LoadsAllCarrierShipmentData()
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

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            ShipmentEntity loadedShipment = loadedOrder.Shipment(shipment.ShipmentID);
            Assert.NotNull(loadedShipment.Postal.Endicia);
            Assert.NotNull(loadedShipment.Postal.Usps);
            Assert.NotNull(loadedShipment.OnTrac);
            Assert.NotNull(loadedShipment.Amazon);
            Assert.NotNull(loadedShipment.BestRate);
            Assert.NotNull(loadedShipment.Other);
        }

        [Fact]
        public void LoadFullShipmentGraph_LoadsFedExShipmentAndPackages()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .AsFedEx(fedEx => fedEx.WithPackage().WithPackage())
                .Save();

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            Assert.NotNull(loadedOrder.Shipment(shipment.ShipmentID).FedEx);
            Assert.Equal(2, loadedOrder.Shipment(shipment.ShipmentID).FedEx.Packages.Count);
        }

        [Fact]
        public void LoadFullShipmentGraph_LoadsUpsShipmentAndPackages()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .AsUps(Ups => Ups.WithPackage().WithPackage())
                .Save();

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            Assert.NotNull(loadedOrder.Shipment(shipment.ShipmentID).Ups);
            Assert.Equal(2, loadedOrder.Shipment(shipment.ShipmentID).Ups.Packages.Count);
        }

        [Fact]
        public void LoadFullShipmentGraph_LoadsIParcelShipmentAndPackages()
        {
            ShipmentEntity shipment = null;

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                shipment = Create.Shipment(order)
                    .AsIParcel(IParcel => IParcel.WithPackage().WithPackage())
                    .Save(sqlAdapter);
            }

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            Assert.NotNull(loadedOrder.Shipment(shipment.ShipmentID).IParcel);
            Assert.Equal(2, loadedOrder.Shipment(shipment.ShipmentID).IParcel.Packages.Count);
        }

        [Fact]
        public void LoadFullShipmentGraph_LoadsCustomsItems()
        {
            ShipmentEntity shipment = Create.Shipment(order)
                .WithCustomsItem()
                .WithCustomsItem()
                .Save();

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            Assert.Equal(2, loadedOrder.Shipment(shipment.ShipmentID).CustomsItems.Count);
        }

        [Fact]
        public void LoadFullShipmentGraph_LoadsInsurance()
        {
            long shipmentId = Create.Shipment(order)
                .WithInsurancePolicy()
                .Save()
                .ShipmentID;

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            OrderEntity loadedOrder = wrapper.LoadFullShipmentGraph(order.OrderID);

            Assert.NotNull(loadedOrder.Shipment(shipmentId).InsurancePolicy);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
