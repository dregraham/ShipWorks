using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Main view model for the shipment panel
    /// </summary>
    public class ShippingPanelViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private bool supportsMultiplePackages;
        private ShipmentType selectedShipmentType;
        private ObservableCollection<ShipmentType> shipmentTypes;
        private PropertyChangedHandler handler;
        private AddressViewModel origin;
        private AddressViewModel destination;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingPanelViewModel(IShippingPanelConfigurator configurator)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);

            shipmentTypes = new ObservableCollection<ShipmentType>(configurator.AvailableShipmentTypes);
        }

        /// <summary>
        /// Load the shipment from the given order
        /// </summary>
        public void LoadOrder(OrderEntity orderEntity)
        {
            SelectedShipmentType = ShipmentTypes.FirstOrDefault();
            Origin.Load(new Interapptive.Shared.Business.PersonAdapter(orderEntity, "Ship"));
        }

        /// <summary>
        /// Gets a list of available shipment types
        /// </summary>
        public ObservableCollection<ShipmentType> ShipmentTypes => shipmentTypes;

        /// <summary>
        /// Selected shipment type for the current shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentType SelectedShipmentType
        {
            get { return selectedShipmentType; }
            set
            {
                if (handler.Set(nameof(SelectedShipmentType), ref selectedShipmentType, value))
                {
                    SupportsMultiplePackages = selectedShipmentType?.SupportsMultiplePackages ?? false;
                }
            }
        }

        /// <summary>
        /// Are multiple packages supported
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SupportsMultiplePackages
        {
            get { return supportsMultiplePackages; }
            set { handler.Set(nameof(SupportsMultiplePackages), ref supportsMultiplePackages, value); }
        }

        public AddressViewModel Origin => origin ?? (origin = new AddressViewModel());

        public AddressViewModel Destination => destination ?? (destination = new AddressViewModel());
    }
}
