using System;
using System.ComponentModel;
using System.Reflection;
using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Newtonsoft.Json;
using ShipWorks.Core.UI;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls
{
    /// <summary>
    /// Generic view model panel
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.ShipmentDetails)]
    [WpfView(typeof(OrderLookupPanelControl))]
    [JsonObject(MemberSerialization.OptIn)]
    public class OrderLookupViewModelPanel<T> : IOrderLookupPanelViewModel<T> where T : class, IOrderLookupViewModel
    {
        private readonly PropertyChangedHandler handler;
        private T context;
        private bool expanded;
        private bool visible;
        private bool isOriginallyVisible;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModelPanel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Name of the panel
        /// </summary>
        [JsonProperty]
        [Obfuscation(Exclude = true)]
        public string Name => typeof(T).Name;

        /// <summary>
        /// Whether or not the panel is expanded
        /// </summary>
        [JsonProperty]
        [Obfuscation(Exclude = true)]
        public bool Expanded
        {
            get => expanded;
            set => handler.Set(nameof(Expanded), ref expanded, value);
        }

        /// <summary>
        /// View model specific context
        /// </summary>
        [Obfuscation(Exclude = true)]
        public T Context
        {
            get => context;
            set => handler.Set(nameof(Context), ref context, value);
        }

        /// <summary>
        /// Whether or not the panel is visible
        /// </summary>
        public bool Visible
        {
            get => visible;
            set => handler.Set(nameof(Visible), ref visible, value);
        }

        /// <summary>
        /// Update the view model
        /// </summary>
        public void UpdateViewModel(IOrderLookupShipmentModel shipmentModel, ILifetimeScope innerScope, Func<SectionLayoutIDs, bool> isPanelVisible)
        {
            isOriginallyVisible = false;
            if (Context != null)
            {
                Context.PropertyChanged -= OnContextPropertyChanged;
            }

            IIndex<ShipmentTypeCode, T> createSectionViewModel = innerScope.Resolve<IIndex<ShipmentTypeCode, T>>();

            var key = shipmentModel.ShipmentAdapter?.ShipmentTypeCode;
            if (!key.HasValue)
            {
                Context = null;
            }
            else if (createSectionViewModel.TryGetValue(key.Value, out T newModel))
            {
                Context = newModel;
                isOriginallyVisible = isPanelVisible(Context.PanelID);
                Context.PropertyChanged += OnContextPropertyChanged;
                Visible = isOriginallyVisible && Context.Visible;
            }
            else if (innerScope.TryResolve(out T nullModel))
            {
                Context = nullModel;
            }
            else
            {
                Context = null;
            }
        }

        private void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Context.Visible))
            {
                Visible = isOriginallyVisible && Context.Visible;
            }
        }
    }
}
