using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    /// <summary>
    /// Utilities for testing carrier specific features
    /// </summary>
    public static class CarrierTestUtilities
    {
        /// <summary>
        /// Verify that saving a shipment twice through the shipping panel view model does not change the row version
        /// </summary>
        public static void VerifyRowVersionDoesNotChangeAfterSecondSave(object sender, ShipmentEntity shipment)
        {
            var adapter = IoC.UnsafeGlobalLifetimeScope.Resolve<ICarrierShipmentAdapterFactory>().Get(shipment);

            var viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<ShippingPanelViewModel>();
            viewModel.LoadOrder(new OrderSelectionChangedMessage(sender, new IOrderSelection[] {
                new LoadedOrderSelection(shipment.Order, new[] { adapter }, ShippingAddressEditStateType.Editable)
            }));

            viewModel.SaveToDatabase();
            var rowVersion = shipment.RowVersion.ToHexString();

            viewModel.SaveToDatabase();

            Assert.Equal(rowVersion, shipment.RowVersion.ToHexString());
        }
    }
}
