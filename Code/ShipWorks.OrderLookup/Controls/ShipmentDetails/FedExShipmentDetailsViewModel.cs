using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// View model for shipment details
    /// </summary>
    [KeyedComponent(typeof(IDetailsViewModel), ShipmentTypeCode.FedEx)]
    [WpfView(typeof(FedExShipmentDetailsControl))]
    public class FedExShipmentDetailsViewModel : GenericMultiPackageShipmentDetailsViewModel
    {
        private IEnumerable<KeyValuePair<int, string>> signatureTypes;
        
        /// <summary>
        /// Ctor
        /// </summary>
        public FedExShipmentDetailsViewModel(IOrderLookupShipmentModel shipmentModel,
                                             IInsuranceViewModel insuranceViewModel,
                                             Func<DimensionsManagerDlg> getDimensionsManagerDlg,
                                             ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider)
            : base(shipmentModel, insuranceViewModel, getDimensionsManagerDlg, carrierShipmentAdapterOptionsProvider)
        {
            SignatureTypes = EnumHelper.GetEnumList<FedExSignatureType>()
                                       .Select(e => new KeyValuePair<int, string>((int) e.Value, e.Description));
        }

        /// <summary>
        /// Collection of SignatureTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<KeyValuePair<int, string>> SignatureTypes
        {
            get => signatureTypes;
            set => handler.Set(nameof(SignatureTypes), ref signatureTypes, value);
        }

        /// <summary>
        /// Update when order changes
        /// </summary>
        protected override void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShipmentModel.PackageAdapters) && ShipmentModel.PackageAdapters != null)
            {
                int selectedIndex = Packages.IndexOf(SelectedPackage);
                Packages = new System.Collections.ObjectModel.ObservableCollection<PackageAdapterWrapper>(
                    ShipmentModel.PackageAdapters.Select(x => new PackageAdapterWrapper(x)));

                if (Packages.IsCountGreaterThan(selectedIndex))
                {
                    SelectedPackage = Packages[selectedIndex];
                }
                else
                {
                    SelectedPackage = Packages.First();
                }
            }

            if (e.PropertyName == FedExShipmentFields.Service.Name ||
                e.PropertyName == ShipmentFields.ShipCountryCode.Name)
            {
                RefreshInsurance();
            }

            if (e.PropertyName == PostalShipmentFields.Service.Name)
            {
                handler.RaisePropertyChanged(nameof(ShipmentModel));
            }

            if (e.PropertyName == ShipmentFields.ShipCountryCode.Name)
            {
                RefreshServiceTypes();
            }
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

                return InputValidation<FedExShipmentDetailsViewModel>.Validate(this, columnName);
            }
        }
    }
}