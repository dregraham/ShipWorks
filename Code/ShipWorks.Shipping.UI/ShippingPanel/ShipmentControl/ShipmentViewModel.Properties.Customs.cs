using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model customs properties for use by ShipmentControl
    /// </summary>
    public partial class ShipmentViewModel
    {
        private ShipmentCustomsItemEntity selectedCustomsItem;
        private ObservableCollection<ShipmentCustomsItemEntity> customsItems;
        private readonly ICustomsManager customsManager;
        private bool customsAllowed;
        private decimal totalCustomsValue;

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
                try
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
                catch (Exception ex)
                {
                    
                    throw ex;
                }
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
            bool canExecute = false;

            try
            {
                canExecute = SelectedCustomsItem != null && CustomsItems != null && CustomsItems.Contains(SelectedCustomsItem);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }


            return canExecute;
        }
    }
}
