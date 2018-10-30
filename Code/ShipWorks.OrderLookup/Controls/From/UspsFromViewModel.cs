using System.ComponentModel;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// View model for the From address
    /// </summary>
    [KeyedComponent(typeof(IFromViewModel), ShipmentTypeCode.Usps)]
    [WpfView(typeof(UspsFromControl))]
    public class UspsFromViewModel : GenericFromViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsFromViewModel(
                IOrderLookupShipmentModel shipmentModel,
                IShipmentTypeManager shipmentTypeManager,
                ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory,
                AddressViewModel addressViewModel,
                ISchedulerProvider schedulerProvider,
				OrderLookupFromFieldLayoutProvider fieldLayoutProvider) :
            base(shipmentModel, shipmentTypeManager, carrierAccountRetrieverFactory, addressViewModel, schedulerProvider, fieldLayoutProvider)
        {
            ShipmentModel.PropertyChanged += UspsShipmentModelPropertyChanged;
        }

        /// <summary>
        /// Handle when a USPS specific property changes
        /// </summary>
        private void UspsShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.Shipment.Postal.Usps.RateShop))
            {
                UpdateTitle();
            }
        }

        /// <summary>
        /// Get the text for the account header
        /// </summary>
        protected override string GetHeaderAccountText() =>
            ShipmentModel.ShipmentAdapter.Shipment?.Postal?.Usps?.RateShop ?? false ?
                "(Rate Shopping)" :
                base.GetHeaderAccountText();

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            ShipmentModel.PropertyChanged -= UspsShipmentModelPropertyChanged;
        }
    }
}
