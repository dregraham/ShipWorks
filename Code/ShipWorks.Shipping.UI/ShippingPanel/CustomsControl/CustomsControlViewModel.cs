using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;

namespace ShipWorks.Shipping.UI.ShippingPanel.CustomsControl
{
    /// <summary>
    /// Customs control view model for modifying a shipment's customs items
    /// </summary>
    public class CustomsControlViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        private ICarrierShipmentAdapter shipmentAdapter;
        private ShipmentCustomsItemEntity selectedCustomsItem;
        private EntityCollection<ShipmentCustomsItemEntity> customsItems;
        private readonly ICustomsManager customsManager;
        private bool customsAllowed;
        private decimal totalCustomsValue;

        /// <summary>
        /// Constructor that should only be used by WPF
        /// </summary>
        public CustomsControlViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomsControlViewModel(ICustomsManager customsManager) : this()
        {
            this.customsManager = customsManager;
        }

        /// <summary>
        /// Expose a stream of property changes
        /// </summary>
        public IObservable<string> PropertyChangeStream => handler;

        /// <summary>
        /// The list of customs items
        /// </summary>
        [Obfuscation(Exclude = true)]
        public EntityCollection<ShipmentCustomsItemEntity> CustomsItems
        {
            get { return customsItems; }
            private set { handler.Set(nameof(CustomsItems), ref customsItems, value, true); }
        }

        /// <summary>
        /// Are customs allowed for this shipment?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool CustomsAllowed
        {
            get { return customsAllowed; }
            set { handler.Set(nameof(CustomsAllowed), ref customsAllowed, value, true); }
        }

        /// <summary>
        /// Sum of customs values?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal TotalCustomsValue
        {
            get { return totalCustomsValue; }
            set
            {
                //RedistributeUnitValue(value);

                handler.Set(nameof(TotalCustomsValue), ref totalCustomsValue, value);

                // Finally update the shipment's total customs value.
                shipmentAdapter.Shipment.CustomsValue = TotalCustomsValue;
            }
        }

        /// <summary>
        /// The selected customs item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentCustomsItemEntity SelectedCustomsItem
        {
            get { return selectedCustomsItem; }
            set
            {
                if (SelectedCustomsItem != null)
                {
                    SelectedCustomsItem.PropertyChanged -= OnSelectedCustomsItemPropertyChanged;
                }

                handler.Set(nameof(SelectedCustomsItem), ref selectedCustomsItem, value);

                if (SelectedCustomsItem != null)
                {
                    SelectedCustomsItem.PropertyChanged += OnSelectedCustomsItemPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Determines if the newTotalValue needs to be distributed across all customs items.  If it does,
        /// the newTotalValue is equally split across all unit items.
        /// </summary>
        private void RedistributeUnitValue(decimal newTotalValue)
        {
            decimal customsItemsTotalValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal)ci.Quantity);

            // Check the first 2 decimals to see if we match
            // If not, we need to spread the value across all customs times
            if (decimal.Round(newTotalValue, 2) != decimal.Round(customsItemsTotalValue, 2))
            {
                // Disable the property changed for the shipment and shipment customs item so that we don't do 
                // updating behind the scense of what we are doing here.
                DisableEntityPropertyChanged();

                // Get the new average unit value for each item
                decimal newUnitValue = newTotalValue / CustomsItems.Sum(ci => (decimal)ci.Quantity);

                foreach (var shipmentCustomsItemEntity in CustomsItems)
                {
                    shipmentCustomsItemEntity.UnitValue = newUnitValue;
                }

                // Due to rounding, see what the current total is
                customsItemsTotalValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal)ci.Quantity);

                // See what the difference is so that we can add it to the first item to make it match the requested
                // total value
                decimal remainderValue = (newTotalValue - customsItemsTotalValue) / (decimal)CustomsItems[0].Quantity;

                // Update the first customs item's unit value with the remainder
                decimal currentFirstValue = CustomsItems[0].UnitValue;
                currentFirstValue += remainderValue;
                CustomsItems[0].UnitValue = currentFirstValue;

                // Re-enable the entity property changed event.
                EnableEntityPropertyChanged();
            }
        }

        /// <summary>
        /// RelayCommand for adding a customs item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand AddCustomsItemCommand => new RelayCommand(AddCustomsItem);

        /// <summary>
        /// Add a customs item
        /// </summary>
        private void AddCustomsItem()
        {
            SelectedCustomsItem = customsManager.CreateCustomsItem(shipmentAdapter.Shipment);
        }

        /// <summary>
        /// RelayCommand for deleting a customs item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand DeleteCustomsItemCommand => new RelayCommand(DeleteCustomsItem, DeleteCustomsItemCanExecute);

        /// <summary>
        /// Determines if deleting a customs item is allowed
        /// </summary>
        private bool DeleteCustomsItemCanExecute()
        {
            return SelectedCustomsItem != null && CustomsItems != null && CustomsItems.Contains(SelectedCustomsItem); 
        }

        /// <summary>
        /// Delete a customs item
        /// </summary>
        private void DeleteCustomsItem()
        {
            if (!CustomsItems.Contains(SelectedCustomsItem))
            {
                ShipmentCustomsItemEntity shipmentCustomsItem = CustomsItems.FirstOrDefault(sci => sci.ShipmentCustomsItemID == SelectedCustomsItem.ShipmentCustomsItemID);

                if (shipmentCustomsItem != null)
                {
                    CustomsItems.Remove(shipmentCustomsItem);
                }
            }
            else
            {
                CustomsItems.Remove(SelectedCustomsItem);
            }

            SelectedCustomsItem = CustomsItems.FirstOrDefault();
        }

        /// <summary>
        /// Load the person
        /// </summary>
        public virtual void Load(ICarrierShipmentAdapter newShipmentAdapter)
        {
            shipmentAdapter = newShipmentAdapter;
            CustomsAllowed = !shipmentAdapter.IsDomestic;

            shipmentAdapter.Shipment.PropertyChanged += OnShipmentPropertyChanged;

            if (!CustomsAllowed)
            {
                return;
            }

            CustomsItems = shipmentAdapter.CustomsItems;

            TotalCustomsValue = shipmentAdapter.Shipment.CustomsValue;

            SelectedCustomsItem = CustomsItems.FirstOrDefault();
        }

        /// <summary>
        /// Disable current entity property changed event handling.
        /// </summary>
        private void DisableEntityPropertyChanged()
        {
            if (shipmentAdapter?.Shipment != null)
            {
                shipmentAdapter.Shipment.PropertyChanged -= OnShipmentPropertyChanged;
            }

            if (SelectedCustomsItem != null)
            {
                SelectedCustomsItem.PropertyChanged -= OnSelectedCustomsItemPropertyChanged;
            }
        }

        /// <summary>
        /// Enable current entity property changed event handling.
        /// </summary>
        private void EnableEntityPropertyChanged()
        {
            if (shipmentAdapter?.Shipment != null)
            {
                shipmentAdapter.Shipment.PropertyChanged += OnShipmentPropertyChanged;
            }

            if (SelectedCustomsItem != null)
            {
                SelectedCustomsItem.PropertyChanged += OnSelectedCustomsItemPropertyChanged;
            }
        }

        /// <summary>
        /// Handle the ShipmentEntity property changed event.
        /// </summary>
        private void OnShipmentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(ShipmentFields.ShipCountryCode.Name, StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(ShipmentFields.OriginCountryCode.Name, StringComparison.OrdinalIgnoreCase))
            {
                CustomsAllowed = !shipmentAdapter.IsDomestic;
            }
        }

        /// <summary>
        /// Handle the ShipmentCustomsItemEntity property changed event.
        /// </summary>
        private void OnSelectedCustomsItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(ShipmentCustomsItemFields.UnitValue.Name, StringComparison.OrdinalIgnoreCase))
            {
                TotalCustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal)ci.Quantity);
            }

            if (e.PropertyName.Equals(ShipmentCustomsItemFields.Weight.Name, StringComparison.OrdinalIgnoreCase))
            {
                shipmentAdapter.Shipment.ContentWeight = CustomsItems.Sum(ci => ci.Weight * ci.Quantity);

                FedExShipmentType.RedistributeContentWeight(shipmentAdapter.Shipment);
            }
        }

        /// <summary>
        /// Dispose of resources
        /// </summary>
        public void Dispose()
        {
            CustomsItems?.Dispose();
        }
    }
}
