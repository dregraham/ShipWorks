using ShipWorks.ApplicationCore;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.Postal.Usps;
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

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        public ShippingPanelViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            shipmentTypes = new ObservableCollection<ShipmentType>(new ShipmentType[] {
                new FedExShipmentType(),
                new UspsShipmentType()
            });

            //selectedShipmentType = shipmentTypes.FirstOrDefault();
            //PropertyChanged += ShippingPanelViewModel_PropertyChanged;
        }

        private void ShippingPanelViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //throw new System.NotImplementedException();

            var foo = e.PropertyName;
            //Console.WriteLine("")
        }

        /// <summary>
        /// Load the shipment from the given order
        /// </summary>
        public void LoadOrder(OrderEntity orderEntity)
        {
            SelectedShipmentType = ShipmentTypes.FirstOrDefault();
        }

        public ObservableCollection<ShipmentType> ShipmentTypes => shipmentTypes;

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

        [Obfuscation(Exclude = true)]
        public bool SupportsMultiplePackages
        {
            get { return supportsMultiplePackages; }
            set {
                handler.Set(nameof(SupportsMultiplePackages), ref supportsMultiplePackages, value);
                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(SupportsMultiplePackages)));
            }
        }
    }
}
