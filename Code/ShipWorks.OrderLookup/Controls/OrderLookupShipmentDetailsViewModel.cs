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
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.OrderLookup.Controls
{
    /// <summary>
    /// Viewmodel for orderlookup
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.ShipmentDetails)]
    public class OrderLookupShipmentDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected readonly PropertyChangedHandler handler;
        private readonly IDimensionsManager dimensionsManager;
        private readonly IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;
        private List<DimensionsProfileEntity> dimensionProfiles;
        private IEnumerable<KeyValuePair<int, string>> packageTypes;
        private IEnumerable<KeyValuePair<int, string>> confirmationTypes;
        private IEnumerable<KeyValuePair<int, string>> serviceTypes;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupShipmentDetailsViewModel(
            IOrderLookupMessageBus messageBus,
            IDimensionsManager dimensionsManager,
            IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory,
            IShipmentTypeManager shipmentTypeManager,
            IShipmentServicesBuilderFactory shipmentServicesBuilderFactory)
        {
            MessageBus = messageBus;
            this.dimensionsManager = dimensionsManager;
            this.shipmentPackageTypesBuilderFactory = shipmentPackageTypesBuilderFactory;
            this.shipmentTypeManager = shipmentTypeManager;
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory;
            MessageBus.PropertyChanged += MessageBusPropertyChanged;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ManageDimensionalProfiles = new RelayCommand(() => ManageDimensionalProfilesAction());
        }

        /// <summary>
        /// Shows the manage dimensional profiles dialog and updates the local profile collection after it closes
        /// </summary>
        private void ManageDimensionalProfilesAction()
        {
            using (DimensionsManagerDlg dlg = new DimensionsManagerDlg())
            {
                dlg.ShowDialog();
                RefreshDimensionalProfiles();
            }
        }

        /// <summary>
        /// Manages Dimensional Profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ManageDimensionalProfiles { get; set; }

        private void RefreshDimensionalProfiles()
        {
            DimensionProfiles =
                dimensionsManager.Profiles(MessageBus.ShipmentAdapter?.GetPackageAdapters().FirstOrDefault()).ToList();

            if (DimensionProfiles.None(d => d.DimensionsProfileID == MessageBus.ShipmentAdapter.Shipment.Postal.DimsProfileID))
            {
                MessageBus.ShipmentAdapter.Shipment.Postal.DimsProfileID = 0;
            }
        }

        /// <summary>
        /// The ViewModel message bus
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupMessageBus MessageBus { get; private set; }

        public List<DimensionsProfileEntity> DimensionProfiles
        {
            get => dimensionProfiles;
            set { handler.Set(nameof(DimensionProfiles), ref dimensionProfiles, value); }
        }

        /// <summary>
        /// True if a profile is selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsProfileSelected
        {
            get => MessageBus.ShipmentAdapter.Shipment.Postal.DimsProfileID > 0;
        }

        /// <summary>
        /// Collection of valid PackageTypes
        /// </summary>
        public IEnumerable<KeyValuePair<int, string>> PackageTypes
        {
            get => packageTypes;
            set => handler.Set(nameof(PackageTypes), ref packageTypes, value);
        }

        /// <summary>
        /// Refresh the package types
        /// </summary>
        public void RefreshPackageTypes()
        {
            if (MessageBus.ShipmentAdapter?.Shipment == null)
            {
                PackageTypes = Enumerable.Empty<KeyValuePair<int, string>>();
            }
            else
            {
                PackageTypes = shipmentPackageTypesBuilderFactory.Get(MessageBus.ShipmentAdapter.ShipmentTypeCode)
                .BuildPackageTypeDictionary(new[] { MessageBus.ShipmentAdapter.Shipment });
            }
        }

        /// <summary>
        /// Collection of ConfirmationTypes
        /// </summary>
        public IEnumerable<KeyValuePair<int, string>> ConfirmationTypes
        {
            get => confirmationTypes;
            set => handler.Set(nameof(ConfirmationTypes), ref confirmationTypes, value);
        }

        /// <summary>
        /// Refresh the confirmation types
        /// </summary>
        public void RefreshConfirmationTypes()
        {
            // Check to see if object is a postal shipment adapter
            if (MessageBus.ShipmentAdapter != null &&
                !PostalUtility.IsPostalShipmentType(MessageBus.ShipmentAdapter.ShipmentTypeCode))
            {
                ConfirmationTypes = Enumerable.Empty<KeyValuePair<int, string>>();
            }
            else
            {

                PostalShipmentType postalShipmentType = ((PostalShipmentType) shipmentTypeManager.Get(MessageBus.ShipmentAdapter.ShipmentTypeCode));
                PostalServiceType postalServiceType = (PostalServiceType) MessageBus.ShipmentAdapter.ServiceType;

                // See if all have confirmation as an option or not
                PostalPackagingType packagingType = (PostalPackagingType) MessageBus.ShipmentAdapter.Shipment.Postal.PackagingType;
                ConfirmationTypes = postalShipmentType
                    .GetAvailableConfirmationTypes(MessageBus.ShipmentAdapter.Shipment.ShipCountryCode, postalServiceType, packagingType)
                    .ToDictionary(serviceType => (int) serviceType, serviceType => EnumHelper.GetDescription(serviceType));
            }
        }

        /// <summary>
        /// Collection of ServiceTypes
        /// </summary>
        public IEnumerable<KeyValuePair<int, string>> ServiceTypes
        {
            get => serviceTypes;
            set => handler.Set(nameof(ServiceTypes), ref serviceTypes, value);
        }


        /// <summary>
        /// Refresh the ServiceTypes
        /// </summary>
        public void RefreshServiceTypes()
        {
            Dictionary<int, string> updatedServices = new Dictionary<int, string>();

            try
            {
                updatedServices = shipmentServicesBuilderFactory.Get(MessageBus.ShipmentAdapter.ShipmentTypeCode)
                    .BuildServiceTypeDictionary(new[] { MessageBus.ShipmentAdapter.Shipment });
            }
            catch (InvalidRateGroupShippingException)
            {
                updatedServices.Add(MessageBus.ShipmentAdapter.ServiceType, "Error getting service types.");
            }

            // If no service types are returned, the carrier doesn't support service types,
            // so just return.
            if (!updatedServices.Any())
            {
                ServiceTypes = new List<KeyValuePair<int, string>>();
            }
            else
            {
                ServiceTypes = new List<KeyValuePair<int, string>>(updatedServices);
            }
        }

        /// <summary>
        /// Update when order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (MessageBus.Order != null)
            {
                if (DimensionProfiles == null)
                {
                    RefreshDimensionalProfiles();
                }

                if (e.PropertyName == "Order")
                {
                    handler.RaisePropertyChanged(nameof(MessageBus));
                }

                if (e.PropertyName == "DimsProfileID")
                {
                    PostalShipmentEntity postal = MessageBus.ShipmentAdapter.Shipment.Postal;
                    if (postal.DimsProfileID != 0)
                    {
                        DimensionsProfileEntity profile = DimensionProfiles.SingleOrDefault(p => p.DimensionsProfileID == postal.DimsProfileID);

                        if (profile != null)
                        {
                            postal.DimsLength = profile.Length;
                            postal.DimsWidth = profile.Width;
                            postal.DimsHeight = profile.Height;
                            postal.DimsWeight = profile.Weight;
                        }
                    }

                    handler.RaisePropertyChanged(nameof(IsProfileSelected));
                }

                if (e.PropertyName == "ShipmentTypeCode" || e.PropertyName == "Order")
                {
                    RefreshPackageTypes();
                }

                if (e.PropertyName == "Order" || e.PropertyName == "ShipmentTypeCode" || e.PropertyName == "Service" || e.PropertyName == "PackagingType")
                {
                    RefreshConfirmationTypes();
                }

                if (e.PropertyName == "Order" || e.PropertyName=="ShipmentTypeCode" || e.PropertyName == "ShipCountryCode")
                {
                    RefreshServiceTypes();
                }
            }

        }
    }
}