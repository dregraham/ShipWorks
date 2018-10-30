using System.ComponentModel;
using System.Reflection;
using ShipWorks.Core.UI;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Generic view model for order lookup
    /// </summary>
    public abstract class OrderLookupViewModelBase : IOrderLookupViewModel
    {
        private readonly PropertyChangedHandler handler;

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModelBase(IOrderLookupShipmentModel shipmentModel)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public abstract string Title { get; protected set; }

        /// <summary>
        /// Is the section visible
        /// </summary>
        [Obfuscation(Exclude = true)]
        public virtual bool Visible { get; protected set; } = true;

        /// <summary>
        /// Panel ID
        /// </summary>
        public abstract SectionLayoutIDs PanelID { get; }

        /// <summary>
        /// Field layout repository
        /// </summary>
        public IOrderLookupFieldLayoutRepository FieldLayoutRepository => ShipmentModel.FieldLayoutRepository;

        /// <summary>
        /// The order lookup ShipmentModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Property changed handler
        /// </summary>
        protected PropertyChangedHandler Handler => handler;

        /// <summary>
        /// Shipment model property has changed
        /// </summary>
        protected virtual void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // No default implementation
        }

        /// <summary>
        /// Raise a property changed event
        /// </summary>
        protected void RaisePropertyChanged(string property) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
        }
    }
}
