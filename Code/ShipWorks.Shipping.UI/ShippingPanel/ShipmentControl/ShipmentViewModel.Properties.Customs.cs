using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using Shared.System.ComponentModel.DataAnnotations;
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
        private double totalCustomsValue;
        private double shipmentContentWeight;

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
        [Required(AllowEmptyStrings = false, ErrorMessage = @"Total customs value is required")]
        [Range(0.0001, 999999999, ErrorMessage = @"Shipment weight must be greater than 0 and less than 999,999,999")]
        [DoubleCompare(0, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Total customs value must be greater than 0.")]
        public double TotalCustomsValue
        {
            get { return totalCustomsValue; }
            set
            {
                handler.Set(nameof(TotalCustomsValue), ref totalCustomsValue, value);

                // Finally update the shipment's total customs value.
                shipmentAdapter.Shipment.CustomsValue = decimal.Parse(TotalCustomsValue.ToString());
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
        [Range(0.001, 999999, ErrorMessage = @"Shipment weight must be between 0 and 999,999")]
        public double ShipmentContentWeight
        {
            get { return shipmentContentWeight; }
            set
            {
                handler.Set(nameof(ShipmentContentWeight), ref shipmentContentWeight, value);
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
