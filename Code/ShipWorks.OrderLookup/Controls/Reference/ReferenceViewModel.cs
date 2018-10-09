using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.Reference)]
    public class ReferenceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;

        /// <summary>
        /// ctor
        /// </summary>
        public ReferenceViewModel(IOrderLookupShipmentModel shipmentModel)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The order lookup ShipmentModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) && ShipmentModel.SelectedOrder != null)
            {
                handler.RaisePropertyChanged(nameof(ShipmentModel));
            }
        }
    }
}
