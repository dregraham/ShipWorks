using System;
using System.Collections.ObjectModel;
using System.Reflection;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class OtherShipmentViewModel
    {
        private bool customsAllowed;
        private decimal totalCustomsValue;
        private double shipmentContentWeight;
        private ObservableCollection<ShipmentCustomsItemEntity> customsItems;
        private ShipmentCustomsItemEntity selectedCustomsItem;
        private readonly ICustomsManager customsManager;
        
        /// <summary>
        /// The list of customs items
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<ShipmentCustomsItemEntity> CustomsItems
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
        /// The shipment content weight
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double ShipmentContentWeight
        {
            get { return shipmentContentWeight; }
            set
            {
                handler.Set(nameof(ShipmentContentWeight), ref shipmentContentWeight, value, true);
            }
        }

        /// <summary>
        /// RelayCommand for adding a customs item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand AddCustomsItemCommand => new RelayCommand(AddCustomsItem);

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
    }
}
