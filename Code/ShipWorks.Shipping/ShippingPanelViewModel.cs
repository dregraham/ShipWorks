using System;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ShipWorks.Data;

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
        private ShipmentViewModel shipment;
        private ILoader<ShippingPanelLoadedShipment> shipmentLoader;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingPanelViewModel(IShippingPanelConfigurator configurator, ILoader<ShippingPanelLoadedShipment> shipmentLoader)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);

            this.shipmentLoader = shipmentLoader;
        }

        /// <summary>
        /// Load the shipment from the given order
        /// </summary>
        //public async Task LoadOrder(OrderEntity orderEntity)
        public async Task LoadOrder(long orderID)
        {
            ShippingPanelLoadedShipment loadedShipment = await shipmentLoader.LoadAsync(orderID);

            if (loadedShipment.Shipment == null)
            {
                // No shipment was created.  Show a message and return.
                // TODO: Show a message

                return;
            }

            //SelectedShipmentType = ShipmentTypes.FirstOrDefault();
            Origin.Load(loadedShipment.Shipment.OriginPerson);

            Destination.Load(loadedShipment.Shipment.ShipPerson);

            Shipment.Load(loadedShipment.Shipment);
        }

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

        public ShipmentViewModel Shipment => shipment ?? (shipment = new ShipmentViewModel());
    }
}
