using System.ComponentModel;
using System.Reflection;
using ShipWorks.Core.UI;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    public class GenericReferenceViewModel : IReferenceViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericReferenceViewModel(IOrderLookupShipmentModel shipmentModel)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Panel ID
        /// </summary>
        public virtual SectionLayoutIDs PanelID => SectionLayoutIDs.Undefined;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title => "Reference";

        /// <summary>
        /// Is the section visible
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Visible => true;

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

        /// <summary>
        /// Dispose the view model
        /// </summary>
        public void Dispose() =>
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
    }
}
