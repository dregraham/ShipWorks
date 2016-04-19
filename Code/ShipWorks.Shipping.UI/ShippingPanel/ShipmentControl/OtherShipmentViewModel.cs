using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class OtherShipmentViewModel : IShipmentViewModel, INotifyPropertyChanged, INotifyPropertyChanging, IDataErrorInfo
    {
        private readonly PropertyChangedHandler handler;

        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual",
            Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        private IPackageAdapter packageAdapter;

        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        public OtherShipmentViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
        }

        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        public OtherShipmentViewModel(IShippingViewModelFactory shippingViewModelFactory, ICustomsManager customsManager) : this()
        {
            this.customsManager = customsManager;

            InsuranceViewModel = shippingViewModelFactory.GetInsuranceViewModel();
        }

        /// <summary>
        /// Stream of property change events
        /// </summary>
        public IObservable<string> PropertyChangeStream => handler;

        /// <summary>
        /// Load the shipment
        /// </summary>
        public virtual void Load(ICarrierShipmentAdapter newShipmentAdapter)
        {
            shipmentAdapter = newShipmentAdapter;

            ShipDate = shipmentAdapter.ShipDate;
            TotalWeight = shipmentAdapter.TotalWeight;
            ShipmentContentWeight = shipmentAdapter.ContentWeight;

            packageAdapter = shipmentAdapter.GetPackageAdapters().FirstOrDefault();
            InsuranceViewModel.Load(new[] { packageAdapter }, packageAdapter, shipmentAdapter);

            OtherShipmentEntity otherShipment = shipmentAdapter.Shipment.Other;

            Debug.Assert(otherShipment != null);

            CarrierName = otherShipment.Carrier;
            Service = otherShipment.Service;
            Cost = shipmentAdapter.Shipment.ShipmentCost;
            TrackingNumber = shipmentAdapter.Shipment.TrackingNumber;

            LoadCustoms();
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public virtual void Save()
        {
            shipmentAdapter.ShipDate = ShipDate;
            shipmentAdapter.Shipment.TotalWeight = TotalWeight;

            shipmentAdapter.Shipment.ShipmentCost = Cost;
            shipmentAdapter.Shipment.TrackingNumber = TrackingNumber;

            OtherShipmentEntity otherShipment = shipmentAdapter.Shipment.Other;
            Debug.Assert(otherShipment != null);

            otherShipment.Carrier = CarrierName;
            otherShipment.Service = Service;

            shipmentAdapter.ContentWeight = ShipmentContentWeight;
        }

        #region Customs

        /// <summary>
        /// Load customs
        /// </summary>
        public void LoadCustoms()
        {
            CustomsAllowed = shipmentAdapter?.CustomsAllowed == true;

            if (!CustomsAllowed)
            {
                return;
            }

            CustomsItems = new ObservableCollection<IShipmentCustomsItemAdapter>(shipmentAdapter.GetCustomsItemAdapters());

            TotalCustomsValue = shipmentAdapter.Shipment.CustomsValue;

            SelectedCustomsItem = CustomsItems.FirstOrDefault();
        }

        /// <summary>
        /// Add a customs item
        /// </summary>
        private void AddCustomsItem()
        {
            IShipmentCustomsItemAdapter newItem = shipmentAdapter.AddCustomsItem();
            CustomsItems.Add(newItem);
            SelectedCustomsItem = newItem;
        }

        /// <summary>
        /// Delete a customs item
        /// </summary>
        private void DeleteCustomsItem()
        {
            ShipmentContentWeight = CustomsItems.Sum(ci => ci.Weight * ci.Quantity);
            TotalCustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal) ci.Quantity);

            IShipmentCustomsItemAdapter selectedItem = SelectedCustomsItem;
            int location = CustomsItems.IndexOf(selectedItem);

            if (CustomsItems.Count > 1)
            {
                SelectedCustomsItem = CustomsItems.Last() == selectedItem ?
                    CustomsItems.ElementAt(location - 1) :
                    CustomsItems.ElementAt(location + 1);
            }

            CustomsItems.Remove(selectedItem);
            shipmentAdapter.DeleteCustomsItem(selectedItem);
        }

        /// <summary>
        /// Handle the ShipmentCustomsItemEntity property changed event.
        /// </summary>
        private void OnSelectedCustomsItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.UnitValue), StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.Quantity), StringComparison.OrdinalIgnoreCase))
            {
                TotalCustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal) ci.Quantity);
            }

            if (e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.Weight), StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.Quantity), StringComparison.OrdinalIgnoreCase))
            {
                ShipmentContentWeight = CustomsItems.Sum(ci => ci.Weight * ci.Quantity);
            }
        }

        #endregion Customs

        #region IDataErrorInfo
        /// <summary>
        /// Accessor for property validation
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                // If the shipment is null or processed, don't validate anything.
                if (shipmentAdapter?.Shipment == null || shipmentAdapter.Shipment.Processed)
                {
                    return string.Empty;
                }

                return InputValidation<OtherShipmentViewModel>.Validate(this, columnName);
            }
        }

        /// <summary>
        /// IDataErrorInfo Error implementation
        /// </summary>
        public string Error => null;

        /// <summary>
        /// List of all validation errors
        /// </summary>
        /// <returns></returns>
        public ICollection<string> AllErrors()
        {
            return InputValidation<OtherShipmentViewModel>.Validate(this);
        }

        #endregion

        /// <summary>
        /// Dispose of any held resources
        /// </summary>
        public void Dispose()
        {
            // No resources to dispose
        }

        /// <summary>
        /// Updates the services
        /// </summary>
        public void RefreshServiceTypes()
        {
            // Other shipment type does not support services
        }

        /// <summary>
        /// Updates the packages
        /// </summary>
        public void RefreshPackageTypes()
        {
            // Other shipment type does not support packages
        }

        /// <summary>
        /// Updates the insurance view for the shipment.
        /// </summary>
        public void RefreshInsurance()
        {
            InsuranceViewModel.Load(new[] { packageAdapter }, packageAdapter, shipmentAdapter);
        }

        /// <summary>
        /// Select the given rate
        /// </summary>
        public void SelectRate(RateResult selectedRate)
        {
            // Other shipment type does not support service types
        }
    }
}