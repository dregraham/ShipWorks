using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Core.UI;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Services;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Customs
{
    /// <summary>
    /// View model for the OrderLookupCustomsControl
    /// </summary>
    [KeyedComponent(typeof(IOrderLookupCustomsViewModel), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(IOrderLookupCustomsViewModel), ShipmentTypeCode.Endicia)]
    [WpfView(typeof(OrderLookupGenericCustomsControl))]
    public class OrderLookupGenericCustomsViewModel : IOrderLookupCustomsViewModel
    {
        private IShipmentCustomsItemAdapter selectedCustomsItem;
        private ObservableCollection<IShipmentCustomsItemAdapter> customsItems;
        private double contentWeight;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private Dictionary<int, string> customsContentTypes;
        private bool customsContentTypeAllowed;
        private bool visible;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupGenericCustomsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;
            this.shipmentTypeManager = shipmentTypeManager;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Is the section expanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Expanded { get; set; } = true;

        /// <summary>
        /// The title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title => "Customs";

        /// <summary>
        /// Is Customs Allowed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Visible
        {
            get => visible;
            private set
            {
                bool shouldLoadCustoms = (value && !visible);

                handler.Set(nameof(Visible), ref visible, value);

                if (shouldLoadCustoms)
                {
                    LoadCustoms();
                }
            }
        }

        /// <summary>
        /// The order lookup shipment model
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

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

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteCustomsItemCommand)));
            }
        }

        /// <summary>
        /// List of available customs content types for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> CustomsContentTypes
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
            get => contentWeight;
            set => handler.Set(nameof(ContentWeight), ref contentWeight, value);
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
                   CustomsItems.Contains(SelectedCustomsItem);
        }

        /// <summary>
        /// Load customs
        /// </summary>
        private void LoadCustoms()
        {
            ICarrierShipmentAdapter shipmentAdapter = ShipmentModel.ShipmentAdapter;

            if (shipmentAdapter == null || !shipmentAdapter.CustomsAllowed)
            {
                return;
            }

            CustomsContentTypeAllowed = shipmentTypeManager.IsPostal(shipmentAdapter.ShipmentTypeCode);
            if (CustomsContentTypeAllowed)
            {
                CustomsContentTypes = EnumHelper.GetEnumList<PostalCustomsContentType>().ToDictionary(x => (int) x.Value, x => x.Description);
            }
            else
            {
                CustomsContentTypes = new Dictionary<int, string>();
            }

            CustomsItems = new ObservableCollection<IShipmentCustomsItemAdapter>(shipmentAdapter.GetCustomsItemAdapters());

            ShipmentModel.RefreshShipmentFromDatabase();

            SelectedCustomsItem = CustomsItems.FirstOrDefault();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteCustomsItemCommand)));
        }

        /// <summary>
        /// Add a customs item
        /// </summary>
        private void AddCustomsItem()
        {
            IShipmentCustomsItemAdapter newItem = ShipmentModel.ShipmentAdapter.AddCustomsItem();
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
            ShipmentModel.ShipmentAdapter.Shipment.CustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal) ci.Quantity);

            IShipmentCustomsItemAdapter selectedItem = SelectedCustomsItem;
            int location = CustomsItems.IndexOf(selectedItem);

            if (CustomsItems.Count > 1)
            {
                SelectedCustomsItem = CustomsItems.Last() == selectedItem ?
                    CustomsItems.ElementAt(location - 1) :
                    CustomsItems.ElementAt(location + 1);
            }

            CustomsItems.Remove(selectedItem);
            ShipmentModel.ShipmentAdapter.DeleteCustomsItem(selectedItem);
        }

        /// <summary>
        /// Handle the ShipmentCustomsItemEntity property changed event.
        /// </summary>
        private void OnSelectedCustomsItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IShipmentCustomsItemAdapter.UnitValue) ||
                e.PropertyName == nameof(IShipmentCustomsItemAdapter.Quantity))
            {
                ShipmentModel.ShipmentAdapter.Shipment.CustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal) ci.Quantity);
            }

            if (e.PropertyName == nameof(IShipmentCustomsItemAdapter.Weight) ||
                e.PropertyName == nameof(IShipmentCustomsItemAdapter.Quantity))
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
            if (originalShipmentContentWeight.IsEquivalentTo(ContentWeight))
            {
                IEnumerable<IPackageAdapter> packageAdapters = ShipmentModel.ShipmentAdapter.GetPackageAdapters();
                foreach (IPackageAdapter packageAdapter in packageAdapters)
                {
                    packageAdapter.Weight = ContentWeight / packageAdapters.Count();
                }
            }
        }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Visible = ShipmentModel.ShipmentAdapter?.CustomsAllowed ?? false;

            if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) && ShipmentModel.SelectedOrder != null)
            {
                LoadCustoms();

                handler.RaisePropertyChanged(nameof(ShipmentModel));
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
        }
    }
}
