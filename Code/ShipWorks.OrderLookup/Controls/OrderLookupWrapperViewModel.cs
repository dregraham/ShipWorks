using System.ComponentModel;
using System.Reflection;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls
{
    /// <summary>
    /// Generic view model wrapper
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.ShipmentDetails)]
    [WpfView(typeof(OrderLookupWrapperControl))]
    public class OrderLookupViewModelWrapper<T> : IOrderLookupWrapperViewModel<T> where T : class, IOrderLookupViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IOrderLookupShipmentModel shipmentModel;
        private T context;
        private readonly IIndex<ShipmentTypeCode, T> createSectionViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModelWrapper(
            IOrderLookupShipmentModel shipmentModel,
            IIndex<ShipmentTypeCode, T> createSectionViewModel)
        {
            this.createSectionViewModel = createSectionViewModel;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.shipmentModel = shipmentModel;
            shipmentModel.PropertyChanged += OnShipmentModelPropertyChanged;
        }

        /// <summary>
        /// Handle when a shipment model property changes
        /// </summary>
        private void OnShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderLookupShipmentModel) ||
                e.PropertyName == nameof(shipmentModel.ShipmentAdapter.ShipmentTypeCode) || 
                e.PropertyName == nameof(shipmentModel.SelectedOrder))
            {
                var key = shipmentModel.ShipmentAdapter?.ShipmentTypeCode;
                var old = Context;
                old?.Dispose();
                Context = key.HasValue && createSectionViewModel.TryGetValue(key.Value, out T newModel) ? newModel : null;
            }
        }

        /// <summary>
        /// The dimension profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public T Context
        {
            get => context;
            set { handler.Set(nameof(Context), ref context, value); }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() =>
            shipmentModel.PropertyChanged -= OnShipmentModelPropertyChanged;
    }
}
