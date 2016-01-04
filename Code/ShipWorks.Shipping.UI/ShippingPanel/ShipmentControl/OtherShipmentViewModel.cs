using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class OtherShipmentViewModel : IShipmentViewModel, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private readonly PropertyChangedHandler handler;

        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual",
            Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

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
        public OtherShipmentViewModel(ICustomsManager customsManager) : this()
        {
            this.customsManager = customsManager;
        }

        /// <summary>
        /// Load the shipment
        /// </summary>
        public virtual void Load(ICarrierShipmentAdapter newShipmentAdapter)
        {
            shipmentAdapter = newShipmentAdapter;

            ShipDate = shipmentAdapter.ShipDate;
            TotalWeight = shipmentAdapter.TotalWeight;
            ShipmentContentWeight = shipmentAdapter.ContentWeight;
            UsingInsurance = shipmentAdapter.UsingInsurance;

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
            shipmentAdapter.UsingInsurance = UsingInsurance;

            shipmentAdapter.Shipment.ShipmentCost = Cost;
            shipmentAdapter.Shipment.TrackingNumber = TrackingNumber;

            OtherShipmentEntity otherShipment = shipmentAdapter.Shipment.Other;
            Debug.Assert(otherShipment != null);

            otherShipment.Carrier = CarrierName;
            otherShipment.Service = Service;

            if (CustomsAllowed && CustomsItems != null)
            { 
                shipmentAdapter.CustomsItems = new EntityCollection<ShipmentCustomsItemEntity>(CustomsItems);
            }

            shipmentAdapter.ContentWeight = ShipmentContentWeight;
        }
        #region Customs

        /// <summary>
        /// Load customs
        /// </summary>
        private void LoadCustoms()
        {
            CustomsAllowed = !shipmentAdapter.IsDomestic;

            if (!CustomsAllowed)
            {
                return;
            }

            CustomsItems = new ObservableCollection<ShipmentCustomsItemEntity>(shipmentAdapter.CustomsItems);

            TotalCustomsValue = shipmentAdapter.Shipment.CustomsValue;

            SelectedCustomsItem = CustomsItems.FirstOrDefault();
        }

        /// <summary>
        /// Add a customs item
        /// </summary>
        private void AddCustomsItem()
        {
            // Pass null as the shipment for now so that we don't have db updates/syncing until we actually want to save.
            ShipmentCustomsItemEntity shipmentCustomsItemEntity = customsManager.CreateCustomsItem(null);
            CustomsItems.Add(shipmentCustomsItemEntity);
            SelectedCustomsItem = shipmentCustomsItemEntity;
        }

        /// <summary>
        /// Delete a customs item
        /// </summary>
        private void DeleteCustomsItem()
        {
            customsItems.Remove(SelectedCustomsItem);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(CustomsItems)));
            SelectedCustomsItem = CustomsItems.FirstOrDefault();

            ShipmentContentWeight = CustomsItems.Sum(ci => ci.Weight * ci.Quantity);

            TotalCustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal)ci.Quantity);
        }

        /// <summary>
        /// Handle the ShipmentCustomsItemEntity property changed event.
        /// </summary>
        private void OnSelectedCustomsItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(SelectedCustomsItem.Fields[ShipmentCustomsItemFields.UnitValue.FieldIndex].Name, StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(SelectedCustomsItem.Fields[ShipmentCustomsItemFields.Quantity.FieldIndex].Name, StringComparison.OrdinalIgnoreCase))
            {
                TotalCustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal)ci.Quantity);
            }

            if (e.PropertyName.Equals(SelectedCustomsItem.Fields[ShipmentCustomsItemFields.Weight.FieldIndex].Name, StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(SelectedCustomsItem.Fields[ShipmentCustomsItemFields.Quantity.FieldIndex].Name, StringComparison.OrdinalIgnoreCase))
            {
                ShipmentContentWeight = CustomsItems.Sum(ci => ci.Weight * ci.Quantity);
            }
        }

        #endregion Customs



        /// <summary>
        /// Updates the service types.
        /// </summary>
        public void RefreshServiceTypes()
        {
            // Nothing to do for Other shipment type.
        }

        /// <summary>
        /// Updates the package types.
        /// </summary>
        public void RefreshPackageTypes()
        {
            // Nothing to do for Other shipment type.
        }

        /// <summary>
        /// Dispose of any held resources
        /// </summary>
        public void Dispose()
        {
            // No resources to dispose
        }
    }
}