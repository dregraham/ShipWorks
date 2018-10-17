using System.ComponentModel;
using System.Reflection;
using Autofac;
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
        private readonly PropertyChangedHandler handler;
        private T context;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModelWrapper()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Update the view model
        /// </summary>
        public void UpdateViewModel(IOrderLookupShipmentModel shipmentModel, ILifetimeScope innerScope)
        {
            IIndex<ShipmentTypeCode, T> createSectionViewModel = innerScope.Resolve<IIndex<ShipmentTypeCode, T>>();

            var key = shipmentModel.ShipmentAdapter?.ShipmentTypeCode;
            var old = Context;
            old?.Dispose();
            Context = key.HasValue && createSectionViewModel.TryGetValue(key.Value, out T newModel) ? newModel : null;
        }

        /// <summary>
        /// View model specific context
        /// </summary>
        [Obfuscation(Exclude = true)]
        public T Context
        {
            get => context;
            set { handler.Set(nameof(Context), ref context, value); }
        }
    }
}
