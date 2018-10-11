using System.ComponentModel;
using System.Reflection;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Shipping;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// Viewmodel for orderlookup
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.ShipmentDetails)]
    public class OrderLookupShipmentDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupShipmentDetailsViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IIndex<ShipmentTypeCode, IOrderLookupDetailsViewModel> createDetailsViewModel)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            shipmentModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(shipmentModel.ShipmentAdapter.ShipmentTypeCode) || e.PropertyName == nameof(shipmentModel.SelectedOrder))
                {
                    var old = Context;
                    Context = createDetailsViewModel[shipmentModel.ShipmentAdapter.ShipmentTypeCode];
                    old?.Dispose();
                }
            };

        }

        private IOrderLookupDetailsViewModel context;

        /// <summary>
        /// The dimension profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupDetailsViewModel Context
        {
            get => context;
            set { handler.Set(nameof(Context), ref context, value); }
        }
    }
}
