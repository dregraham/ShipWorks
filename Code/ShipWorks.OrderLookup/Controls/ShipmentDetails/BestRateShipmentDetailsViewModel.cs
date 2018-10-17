using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// View model for shipment details
    /// </summary>
    [KeyedComponent(typeof(IDetailsViewModel), ShipmentTypeCode.BestRate)]
    [WpfView(typeof(BestRateShipmentDetailsControl))]
    public class BestRateShipmentDetailsViewModel : IDetailsViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IDimensionsManager dimensionsManager;
        private readonly ShipmentTypeProvider shipmentTypeProvider;
        private readonly Func<DimensionsManagerDlg> getDimensionsManagerDlg;
        private List<DimensionsProfileEntity> dimensionProfiles;
        private Dictionary<ShipmentTypeCode, string> providers;

        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateShipmentDetailsViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IDimensionsManager dimensionsManager,
            IInsuranceViewModel insuranceViewModel,
            ShipmentTypeProvider shipmentTypeProvider,
            Func<DimensionsManagerDlg> getDimensionsManagerDlg)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;

            this.dimensionsManager = dimensionsManager;
            this.shipmentTypeProvider = shipmentTypeProvider;
            this.getDimensionsManagerDlg = getDimensionsManagerDlg;

            InsuranceViewModel = insuranceViewModel;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ManageDimensionalProfiles = new RelayCommand(ManageDimensionalProfilesAction);

            RefreshDimensionalProfiles();
            RefreshInsurance();
            RefreshProviders();
        }

        /// <summary>
        /// Is the section expanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Expanded { get; set; } = true;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title => "Shipment Details";

        /// <summary>
        /// Is the section visible
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Visible => true;

        /// <summary>
        /// Manages Dimensional Profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ManageDimensionalProfiles { get; set; }

        /// <summary>
        /// The ViewModel ShipmentModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// Insurance information
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IInsuranceViewModel InsuranceViewModel { get; }

        /// <summary>
        /// The dimension profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<DimensionsProfileEntity> DimensionProfiles
        {
            get => dimensionProfiles;
            set { handler.Set(nameof(DimensionProfiles), ref dimensionProfiles, value); }
        }

        /// <summary>
        /// True if a profile is selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsProfileSelected => ShipmentModel.ShipmentAdapter.Shipment.BestRate.DimsProfileID > 0;

        /// <summary>
        /// Collection of ServiceTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<ShipmentTypeCode, string> Providers
        {
            get => providers;
            set => handler.Set(nameof(Providers), ref providers, value);
        }

        /// <summary>
        /// Shipment type code
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode ShipmentTypeCode
        {
            get => ShipmentModel.ShipmentAdapter?.ShipmentTypeCode ?? ShipmentTypeCode.None;
            set
            {
                if (value != ShipmentModel.ShipmentAdapter.ShipmentTypeCode)
                {
                    ShipmentModel.ChangeShipmentType(value);
                }
            }
        }

        /// <summary>
        /// Update when order changes
        /// </summary>
        private void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ShipmentModel.SelectedOrder != null)
            {
                if (e.PropertyName == nameof(ShipmentModel.SelectedOrder))
                {
                    RefreshProviders();
                    RefreshDimensionalProfiles();
                }

                if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) ||
                    e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.ShipmentTypeCode) ||
                    e.PropertyName == ShipmentFields.ShipCountryCode.Name)
                {
                    RefreshInsurance();
                }

                if (e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.Shipment.BestRate.DimsProfileID))
                {
                    IPackageAdapter packageAdapter = ShipmentModel.PackageAdapters.First();
                    if (packageAdapter?.DimsProfileID > 0)
                    {
                        DimensionsProfileEntity profile =
                            DimensionProfiles.SingleOrDefault(p => p.DimensionsProfileID == packageAdapter.DimsProfileID);

                        if (profile != null)
                        {
                            packageAdapter.DimsLength = profile.Length;
                            packageAdapter.DimsWidth = profile.Width;
                            packageAdapter.DimsHeight = profile.Height;
                            packageAdapter.Weight = profile.Weight;
                        }
                    }

                    handler.RaisePropertyChanged(nameof(IsProfileSelected));
                }
            }
        }

        /// <summary>
        /// Refresh the providers
        /// </summary>
        private void RefreshProviders()
        {
            if (ShipmentModel.ShipmentAdapter?.Shipment == null)
            {
                Providers = new Dictionary<ShipmentTypeCode, string>();
            }
            else
            {
                Providers = shipmentTypeProvider
                    .GetAvailableShipmentTypes(ShipmentModel.ShipmentAdapter)
                    .ToDictionary(s => s, s => EnumHelper.GetDescription(s));
            }
        }

        /// <summary>
        /// Refreshes Insurance
        /// </summary>
        private void RefreshInsurance()
        {
            InsuranceViewModel.Load(ShipmentModel.PackageAdapters, ShipmentModel.PackageAdapters.FirstOrDefault(), ShipmentModel.ShipmentAdapter);
        }

        /// <summary>
        /// Refresh the dimension profiles
        /// </summary>
        private void RefreshDimensionalProfiles()
        {
            DimensionProfiles =
                dimensionsManager.Profiles(ShipmentModel.PackageAdapters.FirstOrDefault()).ToList();

            if (ShipmentModel.ShipmentAdapter.Shipment.BestRate != null && DimensionProfiles.None(d => d.DimensionsProfileID ==
                                            ShipmentModel.ShipmentAdapter.Shipment.BestRate.DimsProfileID))
            {
                ShipmentModel.ShipmentAdapter.Shipment.BestRate.DimsProfileID = 0;
            }
        }

        /// <summary>
        /// Shows the manage dimensional profiles dialog and updates the local profile collection after it closes
        /// </summary>
        private void ManageDimensionalProfilesAction()
        {
            using (DimensionsManagerDlg dlg = getDimensionsManagerDlg())
            {
                dlg.ShowDialog();
                RefreshDimensionalProfiles();
            }
        }

        public void Dispose() =>
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
    }
}
