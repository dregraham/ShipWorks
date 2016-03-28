using System;
using Autofac;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Ups
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class UpsShipmentTypeTest : IDisposable
    {
        private readonly DataContext context;

        public UpsShipmentTypeTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            context.Mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());

            var settings = ShippingSettings.Fetch();
            settings.UpsInsuranceProvider = (int) InsuranceProvider.ShipWorks;
            ShippingSettings.Save(settings);
        }

        [Fact]
        public void SavingThroughShippingManager_ShouldNotUpdateRowVersion_WhenNoDataHasChanged()
        {
            var shipment = Create.Shipment(context.Order)
                .AsUps(x => x.WithPackage()).Save();

            VerifyRowVersionDoesNotChangeAfterSecondSave(shipment);
        }

        [Fact]
        public void SavingThroughShippingManager_ShouldNotUpdateRowVersion_WhenNoDataHasChangedWithMultiplePackages()
        {
            var shipment = Create.Shipment(context.Order)
                .AsUps(x => x.WithPackage().WithPackage())
                .Save();

            VerifyRowVersionDoesNotChangeAfterSecondSave(shipment);
        }

        /// <summary>
        /// Verify that saving a shipment twice through the shipping panel view model does not change the row version
        /// </summary>
        private void VerifyRowVersionDoesNotChangeAfterSecondSave(ShipmentEntity shipment)
        {
            var adapter = IoC.UnsafeGlobalLifetimeScope.Resolve<ICarrierShipmentAdapterFactory>().Get(shipment);

            var viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<ShippingPanelViewModel>();
            viewModel.LoadOrder(new OrderSelectionChangedMessage(this, new IOrderSelection[] {
                new LoadedOrderSelection(shipment.Order, new[] { adapter }, ShippingAddressEditStateType.Editable)
            }));

            viewModel.SaveToDatabase();
            var rowVersion = shipment.RowVersion.ToHexString();

            viewModel.SaveToDatabase();

            Assert.Equal(rowVersion, shipment.RowVersion.ToHexString());
        }

        public void Dispose() => context.Dispose();
    }
}