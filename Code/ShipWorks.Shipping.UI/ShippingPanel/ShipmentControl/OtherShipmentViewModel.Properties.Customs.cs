using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using GalaSoft.MvvmLight.Command;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class OtherShipmentViewModel
    {
        private IShipmentCustomsItemAdapter selectedCustomsItem;
        private ObservableCollection<IShipmentCustomsItemAdapter> customsItems;
        private readonly ICustomsManager customsManager;
        private bool customsAllowed;
        private decimal totalCustomsValue;
        private double shipmentContentWeight;

        /// <summary>
        /// The list of customs items
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<IShipmentCustomsItemAdapter> CustomsItems
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
        /// Sum of customs values
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = @"Total customs value is required.")]
        [Range(0.0001, 999999999, ErrorMessage = @"Please enter a valid customs value.")]
        [DecimalCompare(0, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Total customs value must be greater than or equal $0.00.")]
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
        public IShipmentCustomsItemAdapter SelectedCustomsItem
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
            return SelectedCustomsItem != null &&
                CustomsItems != null &&
                CustomsItems.Count > 0 &&
                CustomsItems.Contains(SelectedCustomsItem);
        }
    }
}
