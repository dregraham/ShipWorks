﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Services;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// View model for shipment details
    /// </summary>
    [KeyedComponent(typeof(IDetailsViewModel), ShipmentTypeCode.AmazonSWA)]
    [WpfView(typeof(AmazonSWAShipmentDetailsControl))]
    public class AmazonSWAShipmentDetailsViewModel : OrderLookupViewModelBase, IDetailsViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly Func<DimensionsManagerDlg> getDimensionsManagerDlg;
        private readonly ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider;
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private IDictionary<long, string> dimensionProfiles;
        private readonly IDisposable updateServicesWhenRatesRetrieved;
        private Dictionary<ShipmentTypeCode, string> providers;
        private IEnumerable<KeyValuePair<int, string>> serviceTypes;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWAShipmentDetailsViewModel(
            IOrderLookupShipmentModel shipmentModel,
            Func<DimensionsManagerDlg> getDimensionsManagerDlg,
            ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider,
            IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, fieldLayoutProvider)
        {
            this.getDimensionsManagerDlg = getDimensionsManagerDlg;
            this.carrierShipmentAdapterOptionsProvider = carrierShipmentAdapterOptionsProvider;
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;

            ManageDimensionalProfiles = new RelayCommand(ManageDimensionalProfilesAction);

            RefreshDimensionalProfiles();
            RefreshServiceTypes();
            RefreshProviders();

            updateServicesWhenRatesRetrieved = this.messenger.OfType<RatesRetrievedMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(x =>
                {
                    if (x.Success && ShipmentModel?.ShipmentAdapter != null)
                    {
                        RefreshServiceTypes();
                    }
                });
        }

        /// <summary>
        /// Field layout repository
        /// </summary>
        public override IOrderLookupFieldLayoutProvider FieldLayoutProvider => ShipmentModel.FieldLayoutProvider;

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.ShipmentDetails;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string Title { get; protected set; } = "Shipment Details";

        /// <summary>
        /// Manages Dimensional Profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ManageDimensionalProfiles { get; set; }

        /// <summary>
        /// The dimension profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IDictionary<long, string> DimensionProfiles
        {
            get => dimensionProfiles;
            set { Handler.Set(nameof(DimensionProfiles), ref dimensionProfiles, value); }
        }

        /// <summary>
        /// True if a profile is selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsProfileSelected => ShipmentModel.ShipmentAdapter.Shipment.AmazonSWA.DimsProfileID > 0;

        /// <summary>
        /// Collection of ServiceTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<ShipmentTypeCode, string> Providers
        {
            get => providers;
            set => Handler.Set(nameof(Providers), ref providers, value);
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
        /// Collection of ServiceTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<KeyValuePair<int, string>> ServiceTypes
        {
            get => serviceTypes;
            set => Handler.Set(nameof(ServiceTypes), ref serviceTypes, value);
        }

        /// <summary>
        /// Update when order changes
        /// </summary>
        protected override void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.ShipmentModelPropertyChanged(sender, e);

            if (e.PropertyName == nameof(AmazonSWAShipmentFields.DimsProfileID))
            {
                ApplyDimensionalProfile();
            }

            if (e.PropertyName == nameof(AmazonSWAShipmentFields.Service))
            {
                Handler.RaisePropertyChanged(nameof(ShipmentModel));
                Handler.RaisePropertyChanged(nameof(ShipmentModel.ShipmentAdapter.ServiceType));
            }

            if (e.PropertyName == ShipmentFields.ShipCountryCode.Name)
            {
                RefreshServiceTypes();
            }
        }

        /// <summary>
        /// Apply a dimensional profile
        /// </summary>
        private void ApplyDimensionalProfile()
        {
            IPackageAdapter packageAdapter = ShipmentModel.PackageAdapters.First();
            if (packageAdapter?.DimsProfileID > 0)
            {
                var profile = carrierShipmentAdapterOptionsProvider.GetDimensionsProfile(packageAdapter.DimsProfileID);

                if (profile != null)
                {
                    packageAdapter.DimsLength = profile.Length;
                    packageAdapter.DimsWidth = profile.Width;
                    packageAdapter.DimsHeight = profile.Height;
                    packageAdapter.AdditionalWeight = profile.Weight;
                }
            }
            else
            {
                packageAdapter.DimsLength = 0;
                packageAdapter.DimsWidth = 0;
                packageAdapter.DimsHeight = 0;
                packageAdapter.AdditionalWeight = 0;
            }

            Handler.RaisePropertyChanged(nameof(IsProfileSelected));
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
                Providers = carrierShipmentAdapterOptionsProvider.GetProviders(ShipmentModel.ShipmentAdapter, ShipmentModel.OriginalShipmentTypeCode);
            }
        }

        /// <summary>
        /// Refresh the dimension profiles
        /// </summary>
        private void RefreshDimensionalProfiles()
        {
            DimensionProfiles = carrierShipmentAdapterOptionsProvider.GetDimensionsProfiles(null);

            if (ShipmentModel.ShipmentAdapter.Shipment.AmazonSWA != null &&
                !DimensionProfiles.ContainsKey(ShipmentModel.ShipmentAdapter.Shipment.AmazonSWA.DimsProfileID))
            {
                ShipmentModel.ShipmentAdapter.Shipment.AmazonSWA.DimsProfileID = 0;
            }

            handler.RaisePropertyChanged(null);
        }

        /// <summary>
        /// Refresh the ServiceTypes
        /// </summary>
        private void RefreshServiceTypes()
        {
            ServiceTypes = carrierShipmentAdapterOptionsProvider.GetServiceTypes(ShipmentModel.ShipmentAdapter);
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

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            updateServicesWhenRatesRetrieved?.Dispose();
        }


        #region IDataErrorInfo

        /// <summary>
        /// Do nothing
        /// </summary>
        public string Error => null;

        /// <summary>
        /// Validate the ColumnNames value
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                // If the shipment is null or processed, don't validate anything.
                if (ShipmentModel.ShipmentAdapter?.Shipment == null || ShipmentModel.ShipmentAdapter.Shipment.Processed)
                {
                    return string.Empty;
                }

                return InputValidation<AmazonSWAShipmentDetailsViewModel>.Validate(this, columnName);
            }
        }

        #endregion
    }
}
