using System;
using System.Reflection;
using System.Windows.Input;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    public partial class ShippingPanelViewModel
    {
        private ShipmentTypeCode selectedShipmentType;
        private ShipmentTypeCode initialShipmentTypeCode;
        private string requestedShippingMethod;
        private long originAddressType;
        private long initialOriginAddressType;
        private long accountId;
        private ShippingPanelLoadedShipmentResult loadedShipmentResult;
        private bool supportsMultiplePackages;
        private bool allowEditing;
        private ShippingAddressEditStateType destinationAddressEditableState;
        private bool supportsAccounts;
        private string domesticInternationalText;
        private IShipmentViewModel shipmentViewModel;
        private bool isLoading;
        private string trackingNumber;
        private DateTime processedDate;

        /// <summary>
        /// Command to open the shipping dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand OpenShippingDialogCommand { get; }

        /// <summary>
        /// Selected shipment type code for the current shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public virtual ShipmentTypeCode ShipmentType
        {
            get { return selectedShipmentType; }
            set { handler.Set(nameof(ShipmentType), ref selectedShipmentType, value); }
        }

        /// <summary>
        /// Initial shipment type code for the current shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode InitialShipmentTypeCode
        {
            get { return initialShipmentTypeCode; }
            set { handler.Set(nameof(InitialShipmentTypeCode), ref initialShipmentTypeCode, value); }
        }

        /// <summary>
        /// Returns "Domestic" or "International" depending on the shipments to/from fields.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DomesticInternationalText
        {
            get { return domesticInternationalText; }
            set { handler.Set(nameof(DomesticInternationalText), ref domesticInternationalText, value); }
        }

        /// <summary>
        /// Method of shipping requested by the customer
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string RequestedShippingMethod
        {
            get { return requestedShippingMethod; }
            set { handler.Set(nameof(RequestedShippingMethod), ref requestedShippingMethod, value); }
        }

        /// <summary>
        /// The ShippingPanelLoadedShipmentResult for the selected order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingPanelLoadedShipmentResult LoadedShipmentResult
        {
            get { return loadedShipmentResult; }
            set { handler.Set(nameof(LoadedShipmentResult), ref loadedShipmentResult, value); }
        }

        /// <summary>
        /// Are multiple packages supported
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SupportsMultiplePackages
        {
            get { return supportsMultiplePackages; }
            set { handler.Set(nameof(SupportsMultiplePackages), ref supportsMultiplePackages, value); }
        }

        /// <summary>
        /// Is the loaded shipment processed?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public virtual bool AllowEditing
        {
            get { return allowEditing; }
            set { handler.Set(nameof(AllowEditing), ref allowEditing, value); }
        }

        /// <summary>
        /// Is the loaded shipment destination address editable?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingAddressEditStateType DestinationAddressEditableState
        {
            get { return destinationAddressEditableState; }
            set { handler.Set(nameof(DestinationAddressEditableState), ref destinationAddressEditableState, value); }
        }

        /// <summary>
        /// Origin address type that should be used
        /// </summary>
        [Obfuscation(Exclude = true)]
        public virtual long OriginAddressType
        {
            get { return originAddressType; }
            set { handler.Set(nameof(OriginAddressType), ref originAddressType, value); }
        }

        /// <summary>
        /// Original origin address type
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long InitialOriginAddressType
        {
            get { return initialOriginAddressType; }
            set { handler.Set(nameof(InitialOriginAddressType), ref initialOriginAddressType, value); }
        }

        /// <summary>
        /// Id of the account for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public virtual long AccountId
        {
            get { return accountId; }
            set { handler.Set(nameof(AccountId), ref accountId, value); }
        }

        /// <summary>
        /// The origin address view model.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public virtual AddressViewModel Origin { get; }

        /// <summary>
        /// The destination address view model.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public virtual AddressViewModel Destination { get; }

        /// <summary>
        /// The Shipment view model.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IShipmentViewModel ShipmentViewModel
        {
            get { return shipmentViewModel; }
            set { handler.Set(nameof(ShipmentViewModel), ref shipmentViewModel, value); }
        }

        /// <summary>
        /// True if the carrier supports accounts, false otherwise.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SupportsAccounts
        {
            get { return supportsAccounts; }
            set { handler.Set(nameof(SupportsAccounts), ref supportsAccounts, value); }
        }

        /// <summary>
        /// Is the selection loading
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsLoading
        {
            get { return isLoading; }
            set { handler.Set(nameof(IsLoading), ref isLoading, value); }
        }

        /// <summary>
        /// Tracking number of the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string TrackingNumber
        {
            get { return trackingNumber; }
            set { handler.Set(nameof(TrackingNumber), ref trackingNumber, value); }
        }

        /// <summary>
        /// Date the shipment was processed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DateTime ProcessedDate
        {
            get { return processedDate; }
            set { handler.Set(nameof(ProcessedDate), ref processedDate, value); }
        }
    }
}
