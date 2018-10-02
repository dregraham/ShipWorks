using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Core.UI;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup.Controls.Customs
{
    /// <summary>
    /// View model for the OrderLookupCustomsControl
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.Customs)]
    public class OrderLookupCustomsControlViewModel : INotifyPropertyChanged
    {
        private IShipmentCustomsItemAdapter selectedCustomsItem;
        private ObservableCollection<IShipmentCustomsItemAdapter> customsItems;
        private double contentWeight;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private List<PostalCustomsContentType> customsContentTypes;
        private bool customsContentTypeAllowed;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// ctor
        /// </summary>
        public OrderLookupCustomsControlViewModel(IOrderLookupMessageBus messageBus, IShipmentTypeManager shipmentTypeManager)
        {
            MessageBus = messageBus;
            MessageBus.PropertyChanged += MessageBusPropertyChanged;
            this.shipmentTypeManager = shipmentTypeManager;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }
        
        /// <summary>
        /// The order lookup message bus
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupMessageBus MessageBus { get; }


        /// <summary>
        /// The list of customs items
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<IShipmentCustomsItemAdapter> CustomsItems
        {
            get => customsItems;
            private set => handler.Set(nameof(CustomsItems), ref customsItems, value);
        }
        /// <summary>
        /// The selected customs item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IShipmentCustomsItemAdapter SelectedCustomsItem
        {
            get => selectedCustomsItem;
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
        /// List of available customs content types for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<PostalCustomsContentType> CustomsContentTypes
        {
            get => customsContentTypes;
            set => handler.Set(nameof(CustomsContentTypes), ref customsContentTypes, value);
        }

        /// <summary>
        /// Whether or not customs content types are supported for this shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool CustomsContentTypeAllowed
        {
            get => customsContentTypeAllowed;
            set => handler.Set(nameof(CustomsContentTypeAllowed), ref customsContentTypeAllowed, value);
        }

        /// <summary>
        /// The shipment content weight
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = @"Weight value is required.")]
        [Range(0.0001, 999999999, ErrorMessage = @"Please enter a valid weight.")]
        [DoubleCompare(0, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage =
            @"Weight must be greater than or equal $0.00.")]
        public double ContentWeight
        {
            get { return contentWeight; }
            set { handler.Set(nameof(ContentWeight), ref contentWeight, value); }
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
        public RelayCommand DeleteCustomsItemCommand =>
            new RelayCommand(DeleteCustomsItem, DeleteCustomsItemCanExecute);

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
        
        /// <summary>
        /// Load customs
        /// </summary>
        private void LoadCustoms()
        {
            ICarrierShipmentAdapter shipmentAdapter = MessageBus.ShipmentAdapter;
            
            if (shipmentAdapter == null || !shipmentAdapter.CustomsAllowed )
            {
                return;
            }
            
            CustomsContentTypeAllowed = shipmentTypeManager.IsPostal(shipmentAdapter.ShipmentTypeCode);
            if (CustomsContentTypeAllowed)
            {
                CustomsContentTypes = EnumHelper.GetEnumList<PostalCustomsContentType>().Select(x => x.Value).ToList();
            }
            else
            {
                CustomsContentTypes = new List<PostalCustomsContentType>();
            }
            
            CustomsItems = new ObservableCollection<IShipmentCustomsItemAdapter>(shipmentAdapter.GetCustomsItemAdapters());

            SelectedCustomsItem = CustomsItems.FirstOrDefault();
            DeleteCustomsItemCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Add a customs item
        /// </summary>
        private void AddCustomsItem()
        {
            IShipmentCustomsItemAdapter newItem = MessageBus.ShipmentAdapter.AddCustomsItem();
            CustomsItems.Add(newItem);
            SelectedCustomsItem = newItem;
        }

        /// <summary>
        /// Delete a customs item
        /// </summary>
        private void DeleteCustomsItem()
        {
            double originalShipmentContentWeight = ContentWeight;
            ContentWeight = CustomsItems.Sum(ci => ci.Weight * ci.Quantity);
            RedistributeContentWeight(originalShipmentContentWeight);
            MessageBus.ShipmentAdapter.Shipment.CustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal) ci.Quantity);

            IShipmentCustomsItemAdapter selectedItem = SelectedCustomsItem;
            int location = CustomsItems.IndexOf(selectedItem);

            if (CustomsItems.Count > 1)
            {
                SelectedCustomsItem = CustomsItems.Last() == selectedItem ?
                    CustomsItems.ElementAt(location - 1) :
                    CustomsItems.ElementAt(location + 1);
            }

            CustomsItems.Remove(selectedItem);
            MessageBus.ShipmentAdapter.DeleteCustomsItem(selectedItem);
        }

        /// <summary>
        /// Handle the ShipmentCustomsItemEntity property changed event.
        /// </summary>
        private void OnSelectedCustomsItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.UnitValue), StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.Quantity), StringComparison.OrdinalIgnoreCase))
            {
                MessageBus.ShipmentAdapter.Shipment.CustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal) ci.Quantity);
            }

            if (e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.Weight), StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.Quantity), StringComparison.OrdinalIgnoreCase))
            {
                double originalShipmentContentWeight = ContentWeight;
                ContentWeight = CustomsItems.Sum(ci => ci.Weight * ci.Quantity);
                RedistributeContentWeight(originalShipmentContentWeight);
            }
        }

        /// <summary>
        /// Redistribute the ContentWeight from the shipment to each package in the shipment.  This only does something
        /// if the ContentWeight is different from the total Content.
        /// </summary>
        private void RedistributeContentWeight(double originalShipmentContentWeight)
        {
            // If the content weight changed outside of us, redistribute what the new weight among the packages
            if (Math.Abs(originalShipmentContentWeight - ContentWeight) > 0.001)
            {
                IEnumerable<IPackageAdapter> packageAdapters = MessageBus.ShipmentAdapter.GetPackageAdapters();
                foreach (IPackageAdapter packageAdapter in packageAdapters)
                {
                    packageAdapter.Weight = ContentWeight / packageAdapters.Count();
                }

                // Not dealing with packages just yet, but might in the future, so keeping this here for reference
                //LoadDimensionsFromSelectedPackageAdapter();
            }
        }
        
        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && MessageBus.Order != null)
            {
                LoadCustoms();
                
                handler.RaisePropertyChanged(nameof(MessageBus));
            }
        }
   }
}
