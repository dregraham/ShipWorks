using System;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.ShippingPanel
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ShipmentViewModelTest : IDisposable
    {
        private readonly DataContext context;

        public ShipmentViewModelTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void DeletePackage_UpdatesWeight()
        {
            var shipment = Create.Shipment(context.Order)
                .AsFedEx(x => x.WithPackage(p => p.Set(f => f.Weight, 3.0))
                               .WithPackage(p => p.Set(f => f.Weight, 1.0)))
                .Set(x => x.ContentWeight, 4.0)
                .Set(x => x.TotalWeight, 4.0)
                .Save();

            var testObject = context.Mock.Create<ShipmentViewModel>();
            testObject.Load(IoC.UnsafeGlobalLifetimeScope.Resolve<ICarrierShipmentAdapterFactory>().Get(shipment));
            testObject.SelectedPackageAdapter = testObject.PackageAdapters.Last();

            testObject.DeletePackageCommand.Execute(null);

            Assert.Equal(3.0, shipment.ContentWeight);
            Assert.Equal(3.0, shipment.TotalWeight);
            Assert.Equal(3.0, shipment.FedEx.Packages.First().Weight);
        }

        public void Dispose() => context.Dispose();
    }
}