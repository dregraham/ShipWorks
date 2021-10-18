﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// The Service tab control for FedEx
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class FedExServiceControl : ServiceControlBase
    {
        bool updatingPayorChoices = false;
        private ImmutableList<FedExEmailNotificationControlContainer> senderNotificationControls;
        private ImmutableList<FedExEmailNotificationControlContainer> recipientNotificationControls;
        private ImmutableList<FedExEmailNotificationControlContainer> otherNotificationControls;
        private ImmutableList<FedExEmailNotificationControlContainer> brokerNotificationControls;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public FedExServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.FedEx, rateControl)
        {
            InitializeComponent();
            fedExFreightContainerControl.RateCriteriaChanged += OnRateCriteriaChanged;
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            senderNotificationControls = BuildNotificationControlsList(
                emailNotifySenderShip, emailNotifySenderException, emailNotifySenderDelivery, emailNotifySenderEstimatedDelivery);
            recipientNotificationControls = BuildNotificationControlsList(
                emailNotifyRecipientShip, emailNotifyRecipientException, emailNotifyRecipientDelivery, emailNotifyRecipientEstimatedDelivery);
            otherNotificationControls = BuildNotificationControlsList(
                emailNotifyOtherShip, emailNotifyOtherException, emailNotifyOtherDelivery, emailNotifyOtherEstimatedDelivery);
            brokerNotificationControls = BuildNotificationControlsList(
                emailNotifyBrokerShip, emailNotifyBrokerException, emailNotifyBrokerDelivery, emailNotifyBrokerEstimatedDelivery);

            originControl.Initialize(ShipmentTypeCode.FedEx);
            codOrigin.Initialize(ShipmentTypeCode.FedEx);

            LoadAccounts();

            service.DisplayMember = "Value";
            service.ValueMember = "Key";

            packagingType.DisplayMember = "Key";
            packagingType.ValueMember = "Value";

            EnumHelper.BindComboBox<FedExDropoffType>(dropoffType);
            EnumHelper.BindComboBox<FedExPayorType>(payorDuties, t => t != FedExPayorType.Collect);
            EnumHelper.BindComboBox<FedExHomeDeliveryType>(homePremiumService);
            EnumHelper.BindComboBox<FedExSignatureType>(signature);
            EnumHelper.BindComboBox<FedExCodPaymentType>(codPaymentType);
            EnumHelper.BindComboBox<FedExSmartPostIndicia>(smartIndicia);
            EnumHelper.BindComboBox<FedExSmartPostEndorsement>(smartEndorsement);

            // Don't give the user the option to have FedEx perform the address look up; the thought it that the shipper will know
            // what type of address they are shipping from, and it saves delays associated with a service call
            EnumHelper.BindComboBox<ResidentialDeterminationType>(fromAddressType, t => t != ResidentialDeterminationType.FedExAddressLookup && t != ResidentialDeterminationType.FromAddressValidation);

            packageControl.Initialize();


            cutoffDateDisplay.ShipmentType = ShipmentTypeCode;
        }

        /// <summary>
        /// Build a list of notification controls with their corresponding notification type
        /// </summary>
        private ImmutableList<FedExEmailNotificationControlContainer> BuildNotificationControlsList(CheckBox ship, CheckBox exception, CheckBox delivery, CheckBox estimated) =>
            ImmutableList.Create(
                new FedExEmailNotificationControlContainer(ship, FedExEmailNotificationType.Ship),
                new FedExEmailNotificationControlContainer(exception, FedExEmailNotificationType.Exception),
                new FedExEmailNotificationControlContainer(delivery, FedExEmailNotificationType.Deliver),
                new FedExEmailNotificationControlContainer(estimated, FedExEmailNotificationType.EstimatedDelivery));

        /// <summary>
        /// Loads the list of FedEx accounts into the account drop down list.
        /// </summary>
        public override void LoadAccounts()
        {
            fedexAccount.DisplayMember = "Key";
            fedexAccount.ValueMember = "Value";

            if (FedExAccountManager.Accounts.Count > 0)
            {
                fedexAccount.DataSource = FedExAccountManager.Accounts.Select(s => new KeyValuePair<string, long>(s.Description, s.FedExAccountID)).ToList();
                fedexAccount.Enabled = true;
            }
            else
            {
                fedexAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                fedexAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Load the shipment data into the ui
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            // Load the base
            RecipientDestinationChanged -= OnRecipientDestinationChanged;
            originControl.DestinationChanged -= OnOriginDestinationChanged;

            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            RecipientDestinationChanged += OnRecipientDestinationChanged;
            originControl.DestinationChanged += OnOriginDestinationChanged;

            // The base will disable if editing is not enabled, but due to the packaging selection, we need to customize how it works
            sectionShipment.ContentPanel.Enabled = true;

            // Manually disable all shipment panel controls, except the packaging control.  They still need to be able to switch packages
            // even after processing
            foreach (Control control in sectionShipment.ContentPanel.Controls)
            {
                if (control == packageControl)
                {
                    continue;
                }

                control.Enabled = enableEditing;
            }

            FedExUtility.LoadSmartPostComboBox(smartHubID);

            // Load the origin
            originControl.LoadShipments(shipments);

            fedexAccount.SelectedIndexChanged -= new EventHandler(OnChangeFedExAccount);
            codEnabled.CheckedChanged -= new EventHandler(OnChangeCodEnabled);

            // Load shipment details
            LoadShipmentDetails();

            fedexAccount.SelectedIndexChanged += new EventHandler(OnChangeFedExAccount);
            codEnabled.CheckedChanged += new EventHandler(OnChangeCodEnabled);

            // Load the package information
            packageControl.LoadShipments(LoadedShipments, enableEditing);

            // Load the package details and resize package details section to accommodate.
            packageDetailsControl.LoadShipments(LoadedShipments, enableEditing);
            ResizePackageDetails();

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Load all the shipment details
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadShipmentDetails()
        {
            bool anyInternational = false;

            FedExServiceType? serviceType = null;
            bool allServicesSame = true;
            bool anyGround = false;

            bool anyCodEnabled = false;

            bool anySaturday = false;

            // Determine if all shipments will have the same destination service types
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                // Need to check with the store  to see if anything about the shipment was overridden in case
                // it may have affected the shipping services available (i.e. the eBay GSP program)
                ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);

                anyInternational = !ShipmentTypeManager.GetType(shipment).IsDomestic(overriddenShipment);

                FedExServiceType thisService = (FedExServiceType) shipment.FedEx.Service;
                if (FedExUtility.IsGroundService(thisService))
                {
                    anyGround = true;
                }

                anyCodEnabled = FedExUtility.IsCodAvailable(thisService) && shipment.FedEx.CodEnabled;

                if (serviceType == null)
                {
                    serviceType = thisService;
                }
                else
                {
                    if (serviceType != thisService)
                    {
                        allServicesSame = false;
                    }
                }

                if (FedExUtility.CanDeliverOnSaturday(thisService, shipment.ShipDate))
                {
                    anySaturday = true;
                }
            }

            // If there not all the same clear the value we had
            if (!allServicesSame)
            {
                serviceType = null;
            }

            // Unhook events
            service.SelectedIndexChanged -= new EventHandler(OnChangeService);

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                service.DataSource = lifetimeScope.ResolveKeyed<IShipmentServicesBuilder>(ShipmentTypeCode.FedEx)
                    .BuildServiceTypeDictionary(LoadedShipments)
                    .Select(entry => new KeyValuePair<FedExServiceType, string>((FedExServiceType) entry.Key, entry.Value)).ToList();
            }

            UpdatePackagingChoices(allServicesSame && serviceType.HasValue ? serviceType.Value : (FedExServiceType?) null);
            UpdatePayorChoices(anyGround, anyInternational);

            // Make it visible if any of them have Saturday dates
            saturdayDelivery.Visible = anySaturday;

            // Enable the COD editing ui if any shipments have COD enabled
            EnableCodUI(anyCodEnabled);

            // Load the COD values and update the COD Tax UI
            codOrigin.LoadShipments(LoadedShipments, s => new PersonAdapter(s.FedEx, "Cod"));
            EnableCodTaxId(anyCodEnabled && codOrigin.SelectedOrigin == ShipmentOriginSource.Other);

            // Only show non-standard for a ground (home or not)
            nonStandardPackaging.Visible = anyGround;

            // Show freight if they are all freight
            UpdateFreightSection();

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    fedexAccount.ApplyMultiValue(shipment.FedEx.FedExAccountID);
                    service.ApplyMultiValue((FedExServiceType) shipment.FedEx.Service);
                    dropoffType.ApplyMultiValue((FedExDropoffType) shipment.FedEx.DropoffType);
                    returnsClearance.ApplyMultiCheck(shipment.FedEx.ReturnsClearance);
                    thirdPartyConsignee.ApplyMultiCheck(shipment.FedEx.ThirdPartyConsignee);
                    shipDate.ApplyMultiDate(shipment.ShipDate.ToLocalTime());
                    packagingType.ApplyMultiValue((FedExPackagingType) shipment.FedEx.PackagingType);
                    nonStandardPackaging.ApplyMultiCheck(shipment.FedEx.NonStandardContainer);
                    fromAddressType.ApplyMultiValue((ResidentialDeterminationType) shipment.FedEx.OriginResidentialDetermination);

                    signature.ApplyMultiValue((FedExSignatureType) shipment.FedEx.Signature);
                    referenceCustomer.ApplyMultiText(shipment.FedEx.ReferenceCustomer);
                    referenceInvoice.ApplyMultiText(shipment.FedEx.ReferenceInvoice);
                    referencePO.ApplyMultiText(shipment.FedEx.ReferencePO);
                    referenceShipmentIntegrity.ApplyMultiText(shipment.FedEx.ReferenceShipmentIntegrity);

                    payorTransport.ApplyMultiValue((FedExPayorType) shipment.FedEx.PayorTransportType);
                    transportAccount.ApplyMultiText(shipment.FedEx.PayorTransportAccount);
                    payorTransportName.ApplyMultiText(shipment.FedEx.PayorTransportName);

                    payorDuties.ApplyMultiValue((FedExPayorType) shipment.FedEx.PayorDutiesType);
                    dutiesAccount.ApplyMultiText(shipment.FedEx.PayorDutiesAccount);

                    saturdayDelivery.ApplyMultiCheck(shipment.FedEx.SaturdayDelivery);

                    homeInstructions.ApplyMultiText(shipment.FedEx.HomeDeliveryInstructions);
                    homePremiumService.ApplyMultiValue((FedExHomeDeliveryType) shipment.FedEx.HomeDeliveryType);
                    homePremiumDate.ApplyMultiDate(shipment.FedEx.HomeDeliveryDate);
                    homePremiumPhone.ApplyMultiText(shipment.FedEx.HomeDeliveryPhone);

                    codEnabled.ApplyMultiCheck(shipment.FedEx.CodEnabled);
                    codAmount.ApplyMultiAmount(shipment.FedEx.CodAmount);
                    codAddFreight.ApplyMultiCheck(shipment.FedEx.CodAddFreight);
                    codPaymentType.ApplyMultiValue((FedExCodPaymentType) shipment.FedEx.CodPaymentType);
                    codTaxId.ApplyMultiText(shipment.FedEx.CodTIN);

                    smartHubID.ApplyMultiValue(shipment.FedEx.SmartPostHubID);
                    smartIndicia.ApplyMultiValue((FedExSmartPostIndicia) shipment.FedEx.SmartPostIndicia);
                    smartEndorsement.ApplyMultiValue((FedExSmartPostEndorsement) shipment.FedEx.SmartPostEndorsement);
                    smartConfirmation.ApplyMultiCheck(shipment.FedEx.SmartPostConfirmation);
                    smartManifestID.ApplyMultiText(shipment.FedEx.SmartPostCustomerManifest);

                    LoadEmailNotificationSettings(shipment.FedEx);
                }

                fedExHoldAtLocationControl.LoadFromShipment(LoadedShipments);
                fimsOptionsControl.LoadFromShipment(LoadedShipments);
            }

            OnChangeService(this, EventArgs.Empty);

            // Rehook events
            service.SelectedIndexChanged += new EventHandler(OnChangeService);

            UpdateBillingSectionDisplay();
            UpdateSectionDescription();
        }

        /// <summary>
        /// Hide/Show the freight section
        /// </summary>
        private void UpdateFreightSection()
        {
            List<int> allDistinct = LoadedShipments.Select(s => s.FedEx.Service).Distinct().ToList();

            sectionFreight.Visible = allDistinct.All(x => FedExUtility.IsFreightLtlService(x)) ||
                                     allDistinct.All(FedExUtility.IsFreightExpressService);

            if (sectionFreight.Visible)
            {
                fedExFreightContainerControl.LoadShipmentDetails(LoadedShipments);
                sectionFreight.Height = fedExFreightContainerControl.Bottom + 30;
            }
        }

        /// <summary>
        /// Load into the UI the configured values for email notifications
        /// </summary>
        private void LoadEmailNotificationSettings(FedExShipmentEntity fedex)
        {
            LoadEmailNotificationSettings(fedex.EmailNotifySender, senderNotificationControls);
            LoadEmailNotificationSettings(fedex.EmailNotifyRecipient, recipientNotificationControls);
            LoadEmailNotificationSettings(fedex.EmailNotifyOther, otherNotificationControls);
            LoadEmailNotificationSettings(fedex.EmailNotifyBroker, brokerNotificationControls);

            emailNotifyOtherAddress.ApplyMultiText(fedex.EmailNotifyOtherAddress);
            emailNotifyMessage.ApplyMultiText(fedex.EmailNotifyMessage);
        }

        /// <summary>
        /// Load email notification settings for a group of controls
        /// </summary>
        private void LoadEmailNotificationSettings(int notificationValue, IEnumerable<FedExEmailNotificationControlContainer> map)
        {
            var notification = (FedExEmailNotificationType) notificationValue;

            foreach (var item in map)
            {
                item.Control.ApplyMultiCheck(notification.HasFlag(item.NotificationType));
            }
        }

        /// <summary>
        /// Save the email settings currently in the UI to the shipment
        /// </summary>
        private void SaveEmailNotificationSettings(FedExShipmentEntity fedex)
        {
            fedex.EmailNotifySender = SaveEmailNotificationSettings(fedex.EmailNotifySender, senderNotificationControls);
            fedex.EmailNotifyRecipient = SaveEmailNotificationSettings(fedex.EmailNotifyRecipient, recipientNotificationControls);
            fedex.EmailNotifyOther = SaveEmailNotificationSettings(fedex.EmailNotifyOther, otherNotificationControls);
            fedex.EmailNotifyBroker = SaveEmailNotificationSettings(fedex.EmailNotifyBroker, brokerNotificationControls);

            emailNotifyOtherAddress.ReadMultiText(t => fedex.EmailNotifyOtherAddress = t);
            emailNotifyMessage.ReadMultiText(t => fedex.EmailNotifyMessage = t);
        }

        /// <summary>
        /// Save email notification settings for the given control map
        /// </summary>
        private int SaveEmailNotificationSettings(int currentNotificationValue, IEnumerable<FedExEmailNotificationControlContainer> map) =>
            map.Aggregate(currentNotificationValue, ReadNotificationValueFromControl);

        /// <summary>
        /// Read the notification value from the control
        /// </summary>
        private int ReadNotificationValueFromControl(int currentNotificationValue, FedExEmailNotificationControlContainer item)
        {
            item.Control.ReadMultiCheck(c => currentNotificationValue = ApplyEmailNotificationType(c, currentNotificationValue, item.NotificationType));
            return currentNotificationValue;
        }

        /// <summary>
        /// Turn the given notification type on or off depending on the enabled state given
        /// </summary>
        private int ApplyEmailNotificationType(bool enabled, int previous, FedExEmailNotificationType notificationTypes) =>
            enabled ?
                previous | (int) notificationTypes :
                previous & ~(int) notificationTypes;

        /// <summary>
        /// Update the insurance cost display
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            packageControl.UpdateInsuranceDisplay();
        }

        /// <summary>
        /// Update the description of the section based on the configured options
        /// </summary>
        private void UpdateSectionDescription()
        {
            if (service.MultiValued)
            {
                sectionShipment.ExtraText = "(Multiple Services)";
            }
            else
            {
                object selectedValue = service.SelectedValue;

                if (selectedValue != null)
                {
                    FedExServiceType serviceType = (FedExServiceType) selectedValue;
                    sectionShipment.ExtraText = EnumHelper.GetDescription(serviceType);
                }
            }
        }

        /// <summary>
        /// Enable the UI for editing the COD values
        /// </summary>
        private void EnableCodUI(bool enable)
        {
            codAmount.Enabled = enable;
            codAddFreight.Enabled = enable;
            codPaymentType.Enabled = enable;
            codOrigin.Enabled = enable;
        }

        /// <summary>
        /// Enables the cod tax ID values.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        private void EnableCodTaxId(bool enable)
        {
            taxInfoLabel.Enabled = enable;
            CodTINLabel.Enabled = enable;
            codTaxId.Enabled = enable;
        }

        /// <summary>
        /// Changing whether COD is selected
        /// </summary>
        void OnChangeCodEnabled(object sender, EventArgs e)
        {
            EnableCodUI(codEnabled.Checked);

            EnableCodTaxId(codEnabled.Checked && codOrigin.SelectedOrigin == ShipmentOriginSource.Other);
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            base.SaveToShipments();

            // Save the origin
            originControl.SaveToEntities();

            // Save the packages
            packageControl.SaveToEntities();
            packageDetailsControl.SaveToEntities();

            // Save cod address
            codOrigin.SaveToEntities();

            this.fedExFreightContainerControl.SaveToShipments(LoadedShipments);
            // Save the whales
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                shipment.ContentWeight = shipment.FedEx.Packages.Sum(p => p.Weight);

                fedexAccount.ReadMultiValue(v => shipment.FedEx.FedExAccountID = (long) v);
                service.ReadMultiValue(v => { if (v != null) shipment.FedEx.Service = (int) v; });
                dropoffType.ReadMultiValue(v => shipment.FedEx.DropoffType = (int) v);
                returnsClearance.ReadMultiCheck(v => shipment.FedEx.ReturnsClearance = v);
                thirdPartyConsignee.ReadMultiCheck(v => shipment.FedEx.ThirdPartyConsignee = v);
                shipDate.ReadMultiDate(d => shipment.ShipDate = d.Date.ToUniversalTime());
                packagingType.ReadMultiValue(v => shipment.FedEx.PackagingType = (int) v);
                nonStandardPackaging.ReadMultiCheck(c => shipment.FedEx.NonStandardContainer = c);
                fromAddressType.ReadMultiValue(v => shipment.FedEx.OriginResidentialDetermination = (int) v);

                signature.ReadMultiValue(v => shipment.FedEx.Signature = (int) v);
                referenceCustomer.ReadMultiText(t => shipment.FedEx.ReferenceCustomer = t);
                referenceInvoice.ReadMultiText(t => shipment.FedEx.ReferenceInvoice = t);
                referencePO.ReadMultiText(t => shipment.FedEx.ReferencePO = t);
                referenceShipmentIntegrity.ReadMultiText(t => shipment.FedEx.ReferenceShipmentIntegrity = t);

                payorTransport.ReadMultiValue(v => shipment.FedEx.PayorTransportType = (int) v);
                transportAccount.ReadMultiText(t => shipment.FedEx.PayorTransportAccount = t);
                payorTransportName.ReadMultiText(t => shipment.FedEx.PayorTransportName = t);

                payorDuties.ReadMultiValue(v => shipment.FedEx.PayorDutiesType = (int) v);
                dutiesAccount.ReadMultiText(t => shipment.FedEx.PayorDutiesAccount = t);

                saturdayDelivery.ReadMultiCheck(c => shipment.FedEx.SaturdayDelivery = c);

                homeInstructions.ReadMultiText(t => shipment.FedEx.HomeDeliveryInstructions = t);
                homePremiumService.ReadMultiValue(v => shipment.FedEx.HomeDeliveryType = (int) v);
                homePremiumDate.ReadMultiDate(d => shipment.FedEx.HomeDeliveryDate = (d.Date.AddHours(12)));
                homePremiumPhone.ReadMultiText(t => shipment.FedEx.HomeDeliveryPhone = t);

                codEnabled.ReadMultiCheck(c => shipment.FedEx.CodEnabled = c);
                codAmount.ReadMultiAmount(a => shipment.FedEx.CodAmount = a);
                codAddFreight.ReadMultiCheck(c => shipment.FedEx.CodAddFreight = c);
                codPaymentType.ReadMultiValue(v => shipment.FedEx.CodPaymentType = (int) v);
                codTaxId.ReadMultiText(t => shipment.FedEx.CodTIN = t);

                smartHubID.ReadMultiValue(v => shipment.FedEx.SmartPostHubID = (string) v);
                smartIndicia.ReadMultiValue(v => shipment.FedEx.SmartPostIndicia = (int) v);
                smartEndorsement.ReadMultiValue(v => shipment.FedEx.SmartPostEndorsement = (int) v);
                smartConfirmation.ReadMultiCheck(c => shipment.FedEx.SmartPostConfirmation = c);
                smartManifestID.ReadMultiText(t => shipment.FedEx.SmartPostCustomerManifest = t);

                SaveEmailNotificationSettings(shipment.FedEx);
                fedExHoldAtLocationControl.SaveToShipment(shipment);
                fimsOptionsControl.SaveToShipment(shipment);
            }

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// The selected fedex account has changed
        /// </summary>
        void OnChangeFedExAccount(object sender, EventArgs e)
        {
            if (fedexAccount.SelectedValue != null)
            {
                long accountID = (long) fedexAccount.SelectedValue;

                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipment.FedEx.FedExAccountID = accountID;
                }

                originControl.NotifySelectedAccountChanged();
                codOrigin.NotifySelectedAccountChanged();
            }
        }

        /// <summary>
        /// The selected service is changing
        /// </summary>
        void OnChangeService(object sender, EventArgs e)
        {
            SuspendLayout();

            if (!service.MultiValued && service.SelectedValue != null)
            {
                UpdateLayoutForSingleService();
            }
            else
            {
                UpdateLayoutForMultipleServices();
            }

            packageControl.PackageCountChanged = packageCount =>
            {
                fedExFreightContainerControl.PackageCountChanged(packageCount);
                packageDetailsControl.PackageCountChanged(packageCount);
            };

            UpdateLabelFormat();

            ResumeLayout();
            PerformLayout();

            UpdateSectionDescription();
            UpdateSaturdayAvailability();

            // To support showing/hiding the customs tab for SmartPost, we need to raise the shipment service changed event.
            RaiseShipmentServiceChanged();
        }

        /// <summary>
        /// Update the service control for multiple services
        /// </summary>
        private void UpdateLayoutForMultipleServices()
        {
            UpdatePackagingChoices(null);

            SetStandardControlVisibility(true);

            UpdateFreightSection();

            // Don't show any selection when multiple services are selected
            RateControl.ClearSelection();
        }

        /// <summary>
        /// Update the service control for a single service
        /// </summary>
        private void UpdateLayoutForSingleService()
        {
            FedExServiceType serviceType = (FedExServiceType) service.SelectedValue;

            UpdatePackagingChoices(serviceType);

            UpdatePayorChoices(FedExUtility.IsGroundService(serviceType), null);
            UpdateBillingSectionDisplay();

            // Only show home delivery section if they are all home delivery
            sectionHomeDelivery.Visible = serviceType == FedExServiceType.GroundHomeDelivery;

            bool isSmartPost = serviceType == FedExServiceType.SmartPost;
            bool isFims = FedExUtility.IsFimsService(serviceType);

            // Only show SmartPost if they are all smart post
            sectionSmartPost.Visible = isSmartPost;

            // Update the SmartPost ui
            sectionPackageDetails.Visible = !isSmartPost && !isFims;

            // Hide Hold At Location if we are SmartPost
            sectionHoldAtLocation.Visible = !isSmartPost && !isFims;

            // Update the packages\skids ui
            packageDetailsControl.UpdateFreightUI(sectionFreight.Visible);

            // Show COD only if applicable
            sectionCOD.Visible = FedExUtility.IsCodAvailable(serviceType);

            // Only show non-standard for a ground (home or not)
            nonStandardPackaging.Visible = FedExUtility.IsGroundService(serviceType);

            sectionFimsOptions.Visible = isFims;

            SetStandardControlVisibility(!isFims);

            RaiseRateCriteriaChanged();
            SyncSelectedRate();

            // Only show freight if its a freight service
            UpdateFreightSection();

            Messenger.Current.Send(new FedExServiceTypeChangedMessage(this, serviceType));
        }

        /// <summary>
        /// Set visibility of the standard service control panels
        /// </summary>
        private void SetStandardControlVisibility(bool visible)
        {
            sectionOptions.Visible = visible;
            sectionBilling.Visible = visible;
            sectionEmail.Visible = visible;
            sectionServiceOptions.Visible = visible;
        }

        /// <summary>
        /// Synchronizes the selected rate in the rate control.
        /// </summary>
        public override void SyncSelectedRate()
        {
            if (!service.MultiValued && service.SelectedValue != null)
            {
                FedExServiceType serviceType = (FedExServiceType) service.SelectedValue;
                RateResult matchingRate = RateControl.RateGroup.Rates.FirstOrDefault(r =>
                {
                    if (r.Tag == null || r.ShipmentType != ShipmentTypeCode.FedEx)
                    {
                        return false;
                    }

                    return ((FedExRateSelection) r.OriginalTag).ServiceType == serviceType;
                });

                RateControl.SelectRate(matchingRate);
            }
            else
            {
                RateControl.ClearSelection();
            }
        }

        /// <summary>
        /// Update the available choices for packaging type
        /// </summary>
        private void UpdatePackagingChoices(FedExServiceType? serviceType)
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            IEnumerable<FedExPackagingType> applicablePackageTypes;

            // If all the services are the same, then load up the valid packaging values
            if (serviceType != null)
            {
                List<FedExPackagingType> validPackagingTypes = FedExUtility.GetValidPackagingTypes(serviceType.Value);

                applicablePackageTypes = validPackagingTypes
                    .Intersect(GetAvailablePackages(LoadedShipments))
                    .DefaultIfEmpty(validPackagingTypes.FirstOrDefault());
            }
            else
            {
                applicablePackageTypes = new[] { FedExPackagingType.Custom };
            }

            packagingType.BindDataSourceAndPreserveSelection(applicablePackageTypes.ToDictionary(x => EnumHelper.GetDescription(x), x => x).ToList());

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Update the payor choices
        /// </summary>
        private void UpdatePayorChoices(bool anyGround, bool? anyInternational)
        {
            updatingPayorChoices = true;

            if (anyInternational != null)
            {
                // Only can do duties if there are international
                panelPayorDuties.Visible = anyInternational.Value;
            }

            int oldIndex = payorTransport.SelectedIndex;

            EnumHelper.BindComboBox<FedExPayorType>(payorTransport, t => anyGround || t != FedExPayorType.Collect);

            payorTransport.SelectedIndex = (oldIndex >= 0 && oldIndex < payorTransport.Items.Count) ? oldIndex : 0;

            updatingPayorChoices = false;
        }

        /// <summary>
        /// Called when the recipient country has changed.  We may have to switch from an international to domestic UI
        /// </summary>
        void OnRecipientDestinationChanged(object sender, EventArgs e)
        {
            SaveToShipments();
            LoadShipmentDetails();
        }

        /// <summary>
        /// Called when the recipient country has changed.  We may have to switch from an international to domestic UI
        /// </summary>
        protected override void OnOriginDestinationChanged(object sender, EventArgs e)
        {
            SaveToShipments();
            LoadShipmentDetails();

            base.OnOriginDestinationChanged(sender, e);
        }

        /// <summary>
        /// Refresh the weight box with the latest weight information from the loaded shipments
        /// </summary>
        public override void RefreshContentWeight()
        {
            // We need to save the package stuff so we know what weights we are dealing with
            packageControl.SaveToEntities();

            bool changes = false;

            // Go through each shipment
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                changes |= FedExShipmentType.RedistributeContentWeight(shipment);
            }

            // If there were changes we have to reload the package ui
            if (changes)
            {
                packageControl.LoadShipments(LoadedShipments, base.EnableEditing);
            }
        }

        /// <summary>
        /// The ship date for the shipment is changing
        /// </summary>
        private void OnChangeShipDate(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
            UpdateSaturdayAvailability();
        }

        /// <summary>
        /// Packaging type has changed
        /// </summary>
        private void OnChangePackaging(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Update the displayed availability of the Saturday delivery option
        /// </summary>
        private void UpdateSaturdayAvailability()
        {
            if (service.MultiValued || service.SelectedValue == null)
            {
                return;
            }
            else
            {
                if (shipDate.MultiValued)
                {
                    return;
                }
                else
                {
                    saturdayDelivery.Visible = FedExUtility.CanDeliverOnSaturday((FedExServiceType) service.SelectedValue, shipDate.Value);
                }
            }
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (fedexAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                FedExAccountEntity account = fedexAccount.SelectedIndex >= 0 ? FedExAccountManager.GetAccount((long) fedexAccount.SelectedValue) : null;
                if (account != null)
                {
                    text += account.Description;
                }
                else
                {
                    text += "(None)";
                }
            }

            sectionFrom.ExtraText = text + ", " + originControl.OriginDescription;

            OnRateCriteriaChanged(sender, e);
        }

        /// <summary>
        /// The COD origin has changed
        /// </summary>
        private void OnCodOriginChanged(object sender, EventArgs e)
        {
            EnableCodTaxId(codEnabled.Checked && codOrigin.SelectedOrigin == ShipmentOriginSource.Other);
        }

        /// <summary>
        /// The size of the package control is changing
        /// </summary>
        private void OnPackageControlSizeChanged(object sender, EventArgs e)
        {
            sectionShipment.Height = (sectionShipment.Height - sectionShipment.ContentPanel.Height) + packageControl.Bottom + 4;
        }

        /// <summary>
        /// Called when [dangerous goods checked].
        /// </summary>
        private void OnDangerousGoodsChecked(object sender, EventArgs e)
        {
            SaveToShipments();
            UpdateLabelFormat();
        }

        /// <summary>
        /// A rate has been selected
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            int oldIndex = service.SelectedIndex;

            FedExRateSelection rate = e.Rate.OriginalTag as FedExRateSelection;

            service.SelectedValue = rate?.ServiceType;
            if (service.SelectedIndex == -1 && oldIndex != -1)
            {
                service.SelectedIndex = oldIndex;
            }
        }

        /// <summary>
        /// Flush any in-progress changes before saving
        /// </summary>
        /// <remarks>This should cause weight controls to finish, etc.</remarks>
        public override void FlushChanges()
        {
            base.FlushChanges();

            packageControl?.FlushChanges();
        }

        /// <summary>
        /// Changing the payor transport account
        /// </summary>
        private void OnChangePayorTransport(object sender, EventArgs e)
        {
            panelTransportAccount.Visible = payorTransport.MultiValued ||
                (payorTransport.SelectedValue != null && (FedExPayorType) payorTransport.SelectedValue != FedExPayorType.Sender);

            if (payorTransport.SelectedValue != null && (FedExPayorType) payorTransport.SelectedValue == FedExPayorType.Recipient)
            {
                // Default the payor name to that of the recipient if one is not already present. We don't want to
                // change the value in case the user has previously indicated the payor name.
                if (string.IsNullOrEmpty(payorTransportName.Text))
                {
                    List<Control> personControls = sectionRecipient.Controls.Find("personControl", true).ToList();
                    if (personControls.Count > 0)
                    {
                        PersonControl recipient = personControls[0] as PersonControl;
                        if (recipient != null)
                        {
                            payorTransportName.Text = recipient.FullName;
                        }
                    }
                }
            }

            UpdateBillingSectionDisplay();
        }

        /// <summary>
        /// Changing the payor duties account
        /// </summary>
        private void OnChangePayorDuties(object sender, EventArgs e)
        {
            bool showAccount = payorDuties.MultiValued ||
                (payorDuties.SelectedValue != null && (FedExPayorType) payorDuties.SelectedValue != FedExPayorType.Sender);

            labelDutiesAccount.Visible = showAccount;
            dutiesAccount.Visible = showAccount;

            UpdateBillingSectionDisplay();
        }

        /// <summary>
        /// Changing the state of the Saturday delivery flag
        /// </summary>
        private void OnSaturdayDeliveryChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Update the description of the billing section
        /// </summary>
        private void UpdateBillingSectionDisplay()
        {
            if (updatingPayorChoices)
            {
                return;
            }

            if (payorTransport.MultiValued)
            {
                sectionBilling.ExtraText = "(Multiple)";
            }
            else if (payorTransport.SelectedValue != null)
            {
                sectionBilling.ExtraText = EnumHelper.GetDescription((FedExPayorType) payorTransport.SelectedValue);
            }
            else
            {
                sectionBilling.ExtraText = "";
            }

            int bottom;

            if (!panelPayorDuties.Visible)
            {
                bottom = panelTransportAccount.Visible ? panelTransportAccount.Bottom : panelPayorTransport.Bottom;
            }
            else
            {
                bottom = dutiesAccount.Visible ? panelPayorDuties.Bottom : panelPayorDuties.Top + dutiesAccount.Top;
            }

            sectionBilling.Height = bottom + (sectionBilling.Height - sectionBilling.ContentPanel.Height) + 4;
        }

        /// <summary>
        /// Changing the home extra services stuff
        /// </summary>
        private void OnChangeHomePremiumService(object sender, EventArgs e)
        {
            UpdateHomeDeliveryUI();
        }

        /// <summary>
        /// Update the UI\options for home delivery
        /// </summary>
        private void UpdateHomeDeliveryUI()
        {
            bool showDate = true;
            bool showPhone = true;

            if (!homePremiumService.MultiValued && homePremiumService.SelectedValue != null)
            {
                FedExHomeDeliveryType type = (FedExHomeDeliveryType) homePremiumService.SelectedValue;

                if (type == FedExHomeDeliveryType.None)
                {
                    sectionHomeDelivery.ExtraText = "";
                }
                else
                {
                    sectionHomeDelivery.ExtraText = EnumHelper.GetDescription(type);
                }

                showDate = type == FedExHomeDeliveryType.DateCertain;
                showPhone = type != FedExHomeDeliveryType.None;
            }
            else
            {
                sectionHomeDelivery.ExtraText = "(Multiple)";
            }

            homePremiumDate.Visible = showDate;

            homePremiumPhone.Visible = showPhone;
            labelHomePremiumPhone.Visible = showPhone;

            homeInstructions.Visible = showPhone;
            labelHomeInstructions.Visible = showPhone;

            int bottom = homeInstructions.Visible ? homeInstructions.Bottom : homePremiumService.Bottom;

            sectionHomeDelivery.Height = bottom + (sectionHomeDelivery.Height - sectionHomeDelivery.ContentPanel.Height) + 10;
        }

        /// <summary>
        /// The selected SmartPost indicia has changed
        /// </summary>
        private void OnChangeSmartPostIndicia(object sender, EventArgs e)
        {
            if (!smartIndicia.MultiValued && smartIndicia.SelectedValue != null)
            {
                FedExSmartPostIndicia indicia = (FedExSmartPostIndicia) smartIndicia.SelectedValue;

                // Only show for parcel select
                infotipSmartPostConfirmation.Visible = (indicia == FedExSmartPostIndicia.ParcelSelect);

                // Can't edit it, and always selected
                smartConfirmation.Enabled = (indicia != FedExSmartPostIndicia.ParcelSelect);
            }
            else
            {
                infotipSmartPostConfirmation.Visible = true;
                smartConfirmation.Enabled = true;
            }

            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Opening the trademark info link
        /// </summary>
        private void OnLinkTrademarkInfo(object sender, EventArgs e)
        {
            string info = "FedEx® is a registered service mark of Federal Express Corporation.\n\n" +

                "Some service marks have been abbreviated in ShipWorks for clarity and are listed in full below.\n\n" +

                "FedEx First Overnight®\n" +
                "FedEx Priority Overnight®\n" +
                "FedEx Standard Overnight®\n" +
                "FedEx 2Day®\n" +
                "FedEx 2Day® A.M.\n" +
                "FedEx Express Saver®\n" +
                "FedEx Ground®\n" +

                "FedEx One Rate®\n" +

                "FedEx Home Delivery®\n" +
                "FedEx Ground® C.O.D.\n" +
                "FedEx International First®\n" +
                "FedEx International Priority®\n" +
                "FedEx International Economy®\n" +
                "FedEx International Priority® Freight\n" +
                "FedEx International Economy® Freight\n" +
                "FedEx First Overnight® Freight\n" +
                "FedEx 1Day® Freight\n" +
                "FedEx 2Day® Freight\n" +
                "FedEx 3Day® Freight\n" +
                "FedEx International Broker Select®\n" +
                "FedEx® Collect on Delivery (C.O.D.)\n" +
                "FedEx Ground® Electronic C.O.D. (E.C.O.D.)\n" +
                "FedEx Date Certain Home Delivery®\n" +
                "FedEx Evening Home Delivery®\n" +
                "FedEx Appointment Home Delivery®\n" +
                "FedEx SmartPost®\n" +
                "FedEx SmartPost parcel select lightweight\n" +
                "FedEx SmartPost® Bound Printed Matter\n" +
                "FedEx SmartPost® Media\n" +
                "FedEx SmartPost parcel select\n" +
                "FedEx ShipAlert®\n" +
                "FedEx Priority Alert Plus™\n" +
                "FedEx International Ground® Distribution\n" +
                "FedEx® International DirectDistribution\n" +
                "FedEx International Economy DirectDistribution℠ Service\n" +
                "FedEx International Priority DirectDistribution®\n" +
                "FedEx International DirectDistribution Surface Solutions U.S. to Canada\n" +
                "FedEx International Priority DirectDistribution® Freight\n\n" +

                "FedEx® Envelope\n" +
                "FedEx® Pak\n" +
                "FedEx® Box\n" +
                "FedEx® Tube\n" +
                "FedEx® 10kg Box\n" +
                "FedEx® 25kg Box";

            MessageHelper.ShowInformation(this, info);
        }

        /// <summary>
        /// Called when [package details resize].
        /// </summary>
        private void OnPackageDetailsResize(object sender, EventArgs e)
        {
            ResizePackageDetails();
        }

        /// <summary>
        /// Called when freight container resizes.
        /// </summary>
        private void OnFreightContainerControlResize(object sender, EventArgs e)
        {
            ResizePackageDetails();
        }

        /// <summary>
        /// Resizes the package details.
        /// </summary>
        private void ResizePackageDetails()
        {
            otherPackageHolder.Height = packageDetailsControl.Bottom;
            sectionPackageDetails.Height = packageDetailsControl.Bottom + (sectionHomeDelivery.Height - sectionHomeDelivery.ContentPanel.Height) + 4;

            sectionFreight.Height = fedExFreightContainerControl.Bottom + (sectionHomeDelivery.Height - sectionHomeDelivery.ContentPanel.Height) + 4;
        }

        /// <summary>
        /// Signal that rates are now invalid due to the residential flag changing.
        /// </summary>
        private void OnResidentialDeterminationChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Called when [non standard packaging changed].
        /// </summary>
        private void OnNonStandardPackagingChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Return false if EPL and there is a FIMS service.
        /// </summary>
        protected override bool ShouldIncludeLabelFormatInList(ThermalLanguage format)
        {
            List<FedExShipmentEntity> fedExShipments =
                LoadedShipments.Where(shipment => shipment.FedEx != null).Select(shipment => shipment.FedEx).ToList();

            if (format == ThermalLanguage.EPL &&
                fedExShipments.Any(fedExShipment => FedExUtility.IsFimsService((FedExServiceType) fedExShipment.Service)))
            {
                return false;
            }

            if (format != ThermalLanguage.None &&
                fedExShipments.Any(fedExShipment => fedExShipment.Packages?.Any(package => package.DangerousGoodsEnabled) ?? false))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the available package types
        /// </summary>
        private static IEnumerable<FedExPackagingType> GetAvailablePackages(IEnumerable<ShipmentEntity> shipments)
        {
            return new FedExShipmentType()
                .GetAvailablePackageTypes()
                .Union(shipments.Select(x => x.FedEx)
                    .Where(x => x != null)
                    .Select(x => x.PackagingType))
                .Cast<FedExPackagingType>();
        }

        /// <summary>
        /// One of the values that affects rates has changed
        /// </summary>
        private void OnRateCriteriaChanged(object sender, EventArgs e) => RaiseRateCriteriaChanged();

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();

                fedExFreightContainerControl.RateCriteriaChanged -= OnRateCriteriaChanged;
            }

            base.Dispose(disposing);
        }
    }
}
