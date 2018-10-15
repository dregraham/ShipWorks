using System.ComponentModel;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// View model for the From address
    /// </summary>
    [KeyedComponent(typeof(IOrderLookupFromViewModel), ShipmentTypeCode.Usps)]
    [WpfView(typeof(OrderLookupUspsFromControl))]
    public class OrderLookupUspsFromViewModel : OrderLookupGenericFromViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupUspsFromViewModel(
                IOrderLookupShipmentModel shipmentModel,
                IShipmentTypeManager shipmentTypeManager,
                ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory,
                AddressViewModel addressViewModel) :
            base(shipmentModel, shipmentTypeManager, carrierAccountRetrieverFactory, addressViewModel)
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
            ShipmentModel.ShipmentAdapter.Shipment.Postal.Usps.RateShop ?
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
