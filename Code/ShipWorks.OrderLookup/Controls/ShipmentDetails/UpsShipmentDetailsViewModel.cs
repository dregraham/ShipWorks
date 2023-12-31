﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// View model for shipment details
    /// </summary>
    [KeyedComponent(typeof(IDetailsViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [WpfView(typeof(UpsShipmentDetailsControl))]
    public class UpsShipmentDetailsViewModel : GenericMultiPackageShipmentDetailsViewModel
    {
        private IEnumerable<KeyValuePair<int, string>> confirmationTypes;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipmentDetailsViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IInsuranceViewModel insuranceViewModel,
            Func<DimensionsManagerDlg> getDimensionsManagerDlg,
            ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider,
            IShipmentTypeManager shipmentTypeManager,
            OrderLookupFieldLayoutProvider fieldLayoutProvider)
            : base(shipmentModel, insuranceViewModel, getDimensionsManagerDlg, carrierShipmentAdapterOptionsProvider, fieldLayoutProvider)
        {
            ConfirmationTypes = EnumHelper.GetEnumList<UpsDeliveryConfirmationType>()
                                          .Select(e => new KeyValuePair<int, string>((int) e.Value, e.Description));
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Collection of ConfirmationTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<KeyValuePair<int, string>> ConfirmationTypes
        {
            get => confirmationTypes;
            set => Handler.Set(nameof(ConfirmationTypes), ref confirmationTypes, value);
        }

        /// <summary>
        /// Update when order changes
        /// </summary>
        protected override void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.ShipmentModelPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ShipmentModel.PackageAdapters) && ShipmentModel.PackageAdapters != null)
            {
                int selectedIndex = Packages.IndexOf(SelectedPackage);
                Packages = new System.Collections.ObjectModel.ObservableCollection<PackageAdapterWrapper>(ShipmentModel.PackageAdapters.Select(x => new PackageAdapterWrapper(x)));

                if (Packages.IsCountGreaterThan(selectedIndex))
                {
                    SelectedPackage = Packages[selectedIndex];
                }
                else
                {
                    SelectedPackage = Packages.First();
                }
            }

            if (e.PropertyName == UpsShipmentFields.Service.Name ||
                e.PropertyName == ShipmentFields.ShipCountryCode.Name)
            {
                RefreshInsurance();
            }

            if (e.PropertyName == ShipmentFields.ShipCountryCode.Name ||
                e.PropertyName == ShipmentFields.OriginCountryCode.Name)
            {
                RefreshServiceTypes();
            }

            if (e.PropertyName == ShipmentFields.ShipCountryCode.Name ||
                e.PropertyName == ShipmentFields.OriginCountryCode.Name ||
                e.PropertyName == UpsShipmentFields.Service.Name ||
                (e.PropertyName == nameof(ShipmentModel.PackageAdapters) && ShipmentModel.PackageAdapters != null))
            {
                UpdateDynamicData();
            }
        }

        /// <summary>
        /// Ensure that the properties of the shipment are correct
        /// and that our lists contain the selected values in the shipment
        /// </summary>
        private void UpdateDynamicData()
        {
            ShipmentType shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.UpsOnLineTools);
            shipmentType.RectifyCarrierSpecificData(ShipmentModel.ShipmentAdapter.Shipment);

            if (!ServiceTypes.ContainsKey(ShipmentModel.ShipmentAdapter.Shipment.Ups.Service))
            {
                RefreshServiceTypes();
            }

            if (SelectedPackage != null && !PackageTypes.ContainsKey(SelectedPackage.PackagingType))
            {
                RefreshPackageTypes();
            }

            Handler.RaisePropertyChanged(null);
        }

        /// <summary>
        /// Validate the ColumnNames value
        /// </summary>
        public override string this[string columnName]
        {
            get
            {
                // If the shipment is null or processed, don't validate anything.
                if (ShipmentModel.ShipmentAdapter?.Shipment == null || ShipmentModel.ShipmentAdapter.Shipment.Processed)
                {
                    return string.Empty;
                }

                return InputValidation<UpsShipmentDetailsViewModel>.Validate(this, columnName);
            }
        }
    }
}
