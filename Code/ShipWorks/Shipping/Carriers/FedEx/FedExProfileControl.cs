﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Control for editing fedex profiles
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.FedEx)]
    public partial class FedExProfileControl : ShippingProfileControlBase
    {
        private BindingList<KeyValuePair<long, string>> includeReturnProfiles = new BindingList<KeyValuePair<long, string>>();
        private BindingSource bindingSource = new BindingSource();

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPageSettings);
            ResizeGroupBoxes(tabPagePackages);

            packagesCount.Items.Clear();

            for (int i = 1; i <= 25; i++)
            {
                packagesCount.Items.Add(i);
            }

            // ShipEngine does not support EPL
            requestedLabelFormat.ExcludeFormats(ThermalLanguage.EPL);
        }

        /// <summary>
        /// Load the data from the given profile into the UI
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            FedExProfileEntity fedex = profile.FedEx;

            LoadFedExAccounts();
            LoadOrigins();
            LoadServiceTypes();
            LoadPackagingTypes();
            LoadDropoffTypes();
            LoadPayorTypes();
            LoadSignatureTypes();

            EnumHelper.BindComboBox<ResidentialDeterminationType>(residentialDetermination);

            EnumHelper.BindComboBox<ResidentialDeterminationType>(senderResidentialCombo);
            // Don't give the user the option to perform the address look up; the thought it that the shipper will know
            // what type of address they are shipping from, and it saves delays associated with a service call
            EnumHelper.BindComboBox<ResidentialDeterminationType>(senderResidentialCombo, t => t != ResidentialDeterminationType.FromAddressValidation);

            EnumHelper.BindComboBox<FedExSmartPostIndicia>(smartIndicia);
            EnumHelper.BindComboBox<FedExSmartPostEndorsement>(smartEndorsement);

            EnumHelper.BindComboBox<FedExReturnType>(returnType);

            EnumHelper.BindComboBox<FedexTINType>(customsRecipientTINType);

            FedExUtility.LoadSmartPostComboBox(smartHubID);

            customsTinIssuingAuthority.DisplayMember = "Key";
            customsTinIssuingAuthority.ValueMember = "Value";
            customsTinIssuingAuthority.DataSource = Geography.Countries.Select(n => new KeyValuePair<string, string>(n, Geography.GetCountryCode(n))).ToList();

            if (ShippingSettings.Fetch().FedExInsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                insuranceControl.UseInsuranceBoxLabel = "FedEx Declared Value";
                insuranceControl.InsuredValueLabel = "Declared value:";
            }

            AddValueMapping(fedex, FedExProfileFields.FedExAccountID, accountState, fedexAccount, labelAccount);
            AddValueMapping(profile, ShippingProfileFields.OriginID, senderState, originCombo, labelSender);

            AddValueMapping(fedex, FedExProfileFields.ResidentialDetermination, residentialState, residentialDetermination, labelResidential);

            AddValueMapping(fedex, FedExProfileFields.OriginResidentialDetermination, senderResidentialState, senderResidentialCombo, labelSenderResidential);

            AddValueMapping(fedex, FedExProfileFields.Service, serviceState, service, labelService);
            AddValueMapping(fedex, FedExProfileFields.PackagingType, packagingState, packaging, labelPackaging);
            AddValueMapping(fedex, FedExProfileFields.NonStandardContainer, packagingState, nonStandardPackaging);
            AddValueMapping(fedex, FedExProfileFields.DropoffType, dropoffTypeState, dropoffType, labelDropoffType);

            AddValueMapping(fedex, FedExProfileFields.Signature, signatureState, signature, labelSignature);
            AddValueMapping(fedex, FedExProfileFields.ReferenceFIMS, referenceFimsState, referenceFIMS, labelFimsReference);
            AddValueMapping(fedex, FedExProfileFields.ReferenceCustomer, referenceCustomerState, referenceCustomer, labelReference);
            AddValueMapping(fedex, FedExProfileFields.ReferenceInvoice, referenceInvoiceState, referenceInvoice, labelInvoice);
            AddValueMapping(fedex, FedExProfileFields.ReferencePO, referencePoState, referencePO, labelPO);
            AddValueMapping(fedex, FedExProfileFields.ReferenceShipmentIntegrity, referenceShipmentIntegrityState, referenceShipmentIntegrity, labelShipmentIntegrity);

            AddValueMapping(fedex, FedExProfileFields.SmartPostHubID, smartHubIDState, smartHubID, labelSmartHubID);
            AddValueMapping(fedex, FedExProfileFields.SmartPostIndicia, smartIndiciaState, smartIndicia, labelSmartEndicia);
            AddValueMapping(fedex, FedExProfileFields.SmartPostEndorsement, smartEndoresmentState, smartEndorsement, labelSmartAncillary);
            AddValueMapping(fedex, FedExProfileFields.SmartPostConfirmation, smartConfirmationState, smartConfirmation, labelSmartConfirmation);
            AddValueMapping(fedex, FedExProfileFields.SmartPostCustomerManifest, smartManifestIDState, smartManifestID, labelSmartManifestID);

            AddValueMapping(fedex, FedExProfileFields.PayorTransportType, payorTransportTypeState, payorTransport, labelPayorTransport);
            AddValueMapping(fedex, FedExProfileFields.PayorTransportAccount, payorTransportAccountState, transportAccount, labelTransportAccount);
            //Commented out for FedEx platform. Leaving this causes errors and its not currently needed
            //AddValueMapping(fedex, FedExProfileFields.PayorDutiesType, payorDutiesTypeState, payorDuties, labelPayorDuties);
            AddValueMapping(fedex, FedExProfileFields.PayorDutiesAccount, payorDutiesAccountState, dutiesAccount, labelDutiesAccount);
            AddValueMapping(fedex, FedExProfileFields.PayorCountryCode, payorCountryCodeState, payorCountryCode, labelPayorCountryCode);
            AddValueMapping(fedex, FedExProfileFields.PayorPostalCode, payorPostalCodeState, payorPostalCode, labelPayorPostalCode);

            AddValueMapping(fedex, FedExProfileFields.DeliveredDutyPaid, deliveredDutyPaidState, deliveredDutyPaid, labelDeliveredDutyPaid);

            AddValueMapping(profile, ShippingProfileFields.ReturnShipment, returnShipmentState, returnShipment);
            AddValueMapping(fedex, FedExProfileFields.ReturnType, returnTypeState, returnType, labelReturnType);
            AddValueMapping(fedex, FedExProfileFields.RmaNumber, rmaNumberState, rmaNumber, labelRmaNumber);
            AddValueMapping(fedex, FedExProfileFields.RmaReason, rmaReasonState, rmaReason, labelRmaReason);
            AddValueMapping(fedex, FedExProfileFields.ReturnSaturdayPickup, saturdayReturnState, saturdayReturn);

            AddValueMapping(fedex, FedExProfileFields.SaturdayDelivery, saturdayState, saturdayDelivery, labelSaturday);
            AddValueMapping(fedex, FedExProfileFields.ReturnsClearance, returnsClearanceState, returnsClearance, labelReturnsClearance);
            AddValueMapping(fedex, FedExProfileFields.ThirdPartyConsignee, consigneeState, thirdPartyConsignee, labelConsignee);

            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifySender, emailNotifySenderState, emailNotifySenderShip, labelEmailSender);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifySender, emailNotifySenderState, emailNotifySenderException);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifySender, emailNotifySenderState, emailNotifySenderDelivery);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifySender, emailNotifySenderState, emailNotifySenderEstimatedDelivery);

            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyRecipient, emailNotifyRecipientState, emailNotifyRecipientShip, labelEmailRecipient);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyRecipient, emailNotifyRecipientState, emailNotifyRecipientException, labelEmailRecipient);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyRecipient, emailNotifyRecipientState, emailNotifyRecipientDelivery, labelEmailRecipient);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyRecipient, emailNotifyRecipientState, emailNotifyRecipientEstimatedDelivery, labelEmailRecipient);

            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyOther, emailNotifyOtherState, emailNotifyOtherShip);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyOther, emailNotifyOtherState, emailNotifyOtherException);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyOther, emailNotifyOtherState, emailNotifyOtherDelivery);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyOther, emailNotifyOtherState, emailNotifyOtherEstimatedDelivery);

            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyBroker, emailNotifyBrokerState, emailNotifyBrokerShip, labelEmailBroker);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyBroker, emailNotifyBrokerState, emailNotifyBrokerException, labelEmailBroker);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyBroker, emailNotifyBrokerState, emailNotifyBrokerDelivery, labelEmailBroker);
            AddEnabledStateMapping(fedex, FedExProfileFields.EmailNotifyBroker, emailNotifyBrokerState, emailNotifyBrokerEstimatedDelivery, labelEmailBroker);

            ApplyEmailNotificationValues(fedex.EmailNotifySender, emailNotifySenderShip, emailNotifySenderException, emailNotifySenderDelivery, emailNotifySenderEstimatedDelivery);
            ApplyEmailNotificationValues(fedex.EmailNotifyRecipient, emailNotifyRecipientShip, emailNotifyRecipientException, emailNotifyRecipientDelivery, emailNotifyRecipientEstimatedDelivery);
            ApplyEmailNotificationValues(fedex.EmailNotifyOther, emailNotifyOtherShip, emailNotifyOtherException, emailNotifyOtherDelivery, emailNotifyOtherEstimatedDelivery);
            ApplyEmailNotificationValues(fedex.EmailNotifyBroker, emailNotifyBrokerShip, emailNotifyBrokerException, emailNotifyBrokerDelivery, emailNotifyBrokerEstimatedDelivery);

            AddValueMapping(fedex, FedExProfileFields.EmailNotifyOtherAddress, emailNotifyOtherState, emailNotifyOtherAddress, labelEmailOther);
            AddValueMapping(fedex, FedExProfileFields.EmailNotifyMessage, emailNotifyMessageState, emailNotifyMessage, labelPersonalMessage);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);

            // Returns
            RefreshIncludeReturnProfileMenu(profile.ShipmentType);
            returnProfileID.DisplayMember = "Value";
            returnProfileID.ValueMember = "Key";
            returnProfileID.SelectedIndex = -1;

            AddValueMapping(profile, ShippingProfileFields.ReturnShipment, returnShipmentState, returnShipment);
            AddValueMapping(profile, ShippingProfileFields.IncludeReturn, includeReturnState, includeReturn);
            AddValueMapping(profile, ShippingProfileFields.ReturnProfileID, applyReturnProfile, returnProfileID);
            AddValueMapping(profile, ShippingProfileFields.ApplyReturnProfile, applyReturnProfileState, applyReturnProfile);

            // Customs
            AddValueMapping(fedex, FedExProfileFields.CreateCommercialInvoice, createCommercialInvoiceState, createCommercialInvoice);
            AddValueMapping(fedex, FedExProfileFields.FileElectronically, fileElectronicallyState, fileElectronically);
            AddValueMapping(fedex, FedExProfileFields.CustomsRecipientTIN, customsRecipientTINState, customsRecipientTIN);
            AddValueMapping(fedex, FedExProfileFields.CustomsRecipientTINType, customsRecipientTINTypeState, customsRecipientTINType);
            AddValueMapping(fedex, FedExProfileFields.CustomsTinIssuingAuthority, customsTinIssuingAuthorityState, customsTinIssuingAuthority);


            // Map parent/child relationships
            SetParentCheckBox(includeReturnState, includeReturn, applyReturnProfileState, applyReturnProfile);
            SetParentCheckBox(applyReturnProfileState, applyReturnProfile, applyReturnProfileState, returnProfileID);
            SetParentCheckBox(applyReturnProfileState, applyReturnProfile, applyReturnProfileState, returnProfileIDLabel);
            SetParentCheckBox(returnShipmentState, returnShipment, saturdayReturnState, saturdayReturn);
            SetParentCheckBox(returnShipmentState, returnShipment, returnTypeState, returnType);
            SetParentCheckBox(returnShipmentState, returnShipment, returnTypeState, labelReturnType);
            SetParentCheckBox(returnShipmentState, returnShipment, rmaReasonState, rmaReason);
            SetParentCheckBox(returnShipmentState, returnShipment, rmaReasonState, labelRmaReason);
            SetParentCheckBox(returnShipmentState, returnShipment, rmaNumberState, rmaNumber);
            SetParentCheckBox(returnShipmentState, returnShipment, rmaNumberState, labelRmaNumber);
            SetParentCheckBox(createCommercialInvoiceState, createCommercialInvoice, fileElectronicallyState, fileElectronically);

            packagesState.Checked = profile.Packages.Count > 0;
            packagesCount.SelectedIndex = packagesState.Checked ? profile.Packages.Count - 1 : -1;
            packagesCount.Enabled = packagesState.Checked;

            LoadPackageEditingUI();

            packagesState.CheckedChanged += new EventHandler(OnChangePackagesChecked);
            packagesCount.SelectedIndexChanged += new EventHandler(OnChangePackagesCount);
        }

        /// <summary>
        /// Apply the value (if present) to the given checkboxes
        /// </summary>
        private void ApplyEmailNotificationValues(int? value, CheckBox ship, CheckBox except, CheckBox delivery, CheckBox estimatedDelivery)
        {
            if (value != null)
            {
                ship.Checked = (value.Value & (int) FedExEmailNotificationType.Ship) != 0;
                except.Checked = (value.Value & (int) FedExEmailNotificationType.Exception) != 0;
                delivery.Checked = (value.Value & (int) FedExEmailNotificationType.Deliver) != 0;
                estimatedDelivery.Checked = (value.Value & (int) FedExEmailNotificationType.EstimatedDelivery) != 0;
            }
        }

        /// <summary>
        /// Read the effective state of the email notification values
        /// </summary>
        private int? ReadEmailNotificationValues(CheckBox ship, CheckBox except, CheckBox deliver, CheckBox estimatedDelivery)
        {
            if (!ship.Enabled)
            {
                return null;
            }

            int value = 0;

            if (ship.Checked)
            {
                value |= (int) FedExEmailNotificationType.Ship;
            }

            if (except.Checked)
            {
                value |= (int) FedExEmailNotificationType.Exception;
            }

            if (deliver.Checked)
            {
                value |= (int) FedExEmailNotificationType.Deliver;
            }

            if (estimatedDelivery.Checked)
            {
                value |= (int) FedExEmailNotificationType.EstimatedDelivery;
            }

            return value;
        }

        /// <summary>
        /// Save all the package profile stuff to their entities
        /// </summary>
        public override void SaveToEntity()
        {
            base.SaveToEntity();

            Profile.FedEx.EmailNotifySender = ReadEmailNotificationValues(emailNotifySenderShip, emailNotifySenderException, emailNotifySenderDelivery, emailNotifySenderEstimatedDelivery);
            Profile.FedEx.EmailNotifyRecipient = ReadEmailNotificationValues(emailNotifyRecipientShip, emailNotifyRecipientException, emailNotifyRecipientDelivery, emailNotifyRecipientEstimatedDelivery);
            Profile.FedEx.EmailNotifyOther = ReadEmailNotificationValues(emailNotifyOtherShip, emailNotifyOtherException, emailNotifyOtherDelivery, emailNotifyOtherEstimatedDelivery);
            Profile.FedEx.EmailNotifyBroker = ReadEmailNotificationValues(emailNotifyBrokerShip, emailNotifyBrokerException, emailNotifyBrokerDelivery, emailNotifyBrokerEstimatedDelivery);

            foreach (FedExProfilePackageControl control in panelPackageControls.Controls)
            {
                // If it's visible it means it's being edited
                if (control.Visible)
                {
                    control.SaveToEntity();
                }
            }
        }
        /// <summary>
        /// Cancel any changes that have not yet been committed
        /// </summary>
        public override void CancelChanges()
        {
            base.CancelChanges();

            // Go through the list of packages
            foreach (PackageProfileEntity package in Profile.Packages.ToList())
            {
                // If its new, then we created it, and we gots to get rid of it
                if (package.IsNew)
                {
                    Profile.Packages.Remove(package);
                }
                // If its marked as deleted, we have to restore it
                else if (package.Fields.State == EntityState.Deleted)
                {
                    package.Fields.State = EntityState.Fetched;
                }
            }
        }

        /// <summary>
        /// Load the list of FedEx accounts
        /// </summary>
        private void LoadFedExAccounts()
        {
            fedexAccount.DisplayMember = "Key";
            fedexAccount.ValueMember = "Value";

            if (FedExAccountManager.Accounts.Count > 0)
            {
                fedexAccount.DataSource = FedExAccountManager.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.FedExAccountID)).ToList();
                fedexAccount.Enabled = true;
            }
            else
            {
                fedexAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                fedexAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Load all the origins
        /// </summary>
        private void LoadOrigins()
        {
            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).GetOrigins();

            originCombo.DisplayMember = "Key";
            originCombo.ValueMember = "Value";
            originCombo.DataSource = origins;
        }

        /// <summary>
        /// Load all the fedex service types
        /// </summary>
        private void LoadServiceTypes()
        {
            EnumHelper.BindComboBox<FedExServiceType>(service);
        }

        /// <summary>
        /// Load all the fedex Dropoff Types
        /// </summary>
        private void LoadDropoffTypes()
        {
            EnumHelper.BindComboBox<FedExDropoffType>(dropoffType);
        }

        /// <summary>
        /// Load all the fedex packaging types
        /// </summary>
        private void LoadPackagingTypes()
        {
            FedExServiceType? serviceType = (FedExServiceType?) service.SelectedValue;
            List<FedExPackagingType> packagingTypes = new List<FedExPackagingType>();

            if (serviceType == null)
            {
                packagingTypes.AddRange(EnumHelper.GetEnumList<FedExPackagingType>().Select(e => e.Value));
            }
            else
            {
                packagingTypes.AddRange(FedExUtility.GetValidPackagingTypes(serviceType.Value));
            }

            FedExPackagingType? previousValue = (FedExPackagingType?) packaging.SelectedValue;

            packaging.DisplayMember = "Key";
            packaging.ValueMember = "Value";
            packaging.DataSource = packagingTypes.Select(p => new KeyValuePair<string, FedExPackagingType>(
                EnumHelper.GetDescription(p), p)).ToList();

            if (previousValue == null)
            {
                packaging.SelectedIndex = -1;
            }
            else
            {
                packaging.SelectedValue = previousValue.Value;
                if (packaging.SelectedIndex < 0)
                {
                    packaging.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Load the payor types
        /// </summary>
        private void LoadPayorTypes()
        {
            EnumHelper.BindComboBox<FedExPayorType>(payorTransport, t => t != FedExPayorType.Collect);
            EnumHelper.BindComboBox<FedExPayorType>(payorDuties);
        }

        /// <summary>
        /// Load signature confirmation options
        /// </summary>
        private void LoadSignatureTypes()
        {
            EnumHelper.BindComboBox<FedExSignatureType>(signature);
        }

        /// <summary>
        /// The selected fedex service has changed
        /// </summary>
        private void OnChangeService(object sender, EventArgs e)
        {
            LoadPackagingTypes();
        }

        /// <summary>
        /// The selected packaging type has changed
        /// </summary>
        private void OnChangePackaging(object sender, EventArgs e)
        {
            bool okService = true;

            if (service.SelectedValue != null)
            {
                FedExServiceType serviceType = (FedExServiceType) service.SelectedValue;

                okService = FedExUtility.IsGroundService(serviceType);
            }

            // No packaging selected
            if (packaging.SelectedValue == null)
            {
                nonStandardPackaging.Visible = okService;
            }
            else
            {
                FedExPackagingType packagingType = (FedExPackagingType) packaging.SelectedValue;

                nonStandardPackaging.Visible = okService && packagingType == FedExPackagingType.Custom;
            }
        }

        /// <summary>
        /// Changing if packages are enabled
        /// </summary>
        void OnChangePackagesChecked(object sender, EventArgs e)
        {
            if (packagesState.Checked)
            {
                packagesCount.Enabled = true;
                packagesCount.SelectedIndex = 0;
            }
            else
            {
                packagesCount.Enabled = false;
                packagesCount.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Changing the count of profile packages
        /// </summary>
        void OnChangePackagesCount(object sender, EventArgs e)
        {
            int count;

            if (packagesCount.SelectedIndex == -1)
            {
                count = 0;
            }
            else
            {
                count = packagesCount.SelectedIndex + 1;
            }

            // Go through each package that already exists
            foreach (PackageProfileEntity package in Profile.Packages)
            {
                // If we need more live packages, mark this one as alive
                if (count > 0)
                {
                    if (package.Fields.State == EntityState.Deleted)
                    {
                        package.Fields.State = package.IsNew ? EntityState.New : EntityState.Fetched;
                    }
                }
                // Otherwise mark this one as deleted
                else
                {
                    package.Fields.State = EntityState.Deleted;
                }

                count--;
            }

            // While we still need to create more, create more
            for (int i = 0; i < count; i++)
            {
                Profile.Packages.Add(new FedExProfilePackageEntity());
            }

            LoadPackageEditingUI();
        }

        /// <summary>
        /// Load the UI for editing all the package profile controls
        /// </summary>
        private void LoadPackageEditingUI()
        {
            // Get all the not marked for deleted packages
            List<PackageProfileEntity> packages = Profile.Packages.Where(p => p.Fields.State != EntityState.Deleted).ToList();

            int index = 0;
            Control lastControl = null;

            // Ensure each one has a UI control
            foreach (FedExProfilePackageEntity package in packages)
            {
                FedExProfilePackageControl control;

                // If there is a control for it already, it should match up with this package
                if (panelPackageControls.Controls.Count > index)
                {
                    control = (FedExProfilePackageControl) panelPackageControls.Controls[index];
                    control.Visible = true;

                    Debug.Assert(control.ProfilePackage == package);
                }
                else
                {
                    control = new FedExProfilePackageControl();

                    int top = 0;
                    if (panelPackageControls.Controls.Count > 0)
                    {
                        top = panelPackageControls.Controls[panelPackageControls.Controls.Count - 1].Bottom + 4;
                    }

                    control.Width = panelPackageControls.Width;
                    control.Top = top;
                    control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

                    panelPackageControls.Controls.Add(control);
                    control.LoadProfilePackage(package);
                }

                lastControl = control;
                index++;
            }

            // Make all the ones we don't need not visible
            for (int i = packages.Count; i < panelPackageControls.Controls.Count; i++)
            {
                panelPackageControls.Controls[i].Visible = false;
            }

            panelPackageControls.Height = lastControl == null ? 0 : lastControl.Bottom + 4;
        }

        /// <summary>
        /// Click of the Include Return checkbox
        /// </summary>
        private void OnIncludeReturnChanged(object sender, EventArgs e)
        {
            if (includeReturn.Checked)
            {
                returnShipment.Checked = false;
            }
        }

        /// <summary>
        /// Opening the return profiles menu
        /// </summary>
        private void OnReturnProfileIDOpened(object sender, EventArgs e)
        {
            RefreshIncludeReturnProfileMenu(Profile.ShipmentType);
        }

        /// <summary>
        /// Click of the Return Shipment checkbox
        /// </summary>
        protected virtual void OnReturnShipmentChanged(object sender, EventArgs e)
        {
            if (returnShipment.Checked)
            {
                includeReturn.Checked = false;
            }
        }

        /// <summary>
        /// When ReturnProfileID dropdown is enabled
        /// </summary>
        protected void OnReturnProfileIDEnabledChanged(object sender, EventArgs e)
        {
            if (returnProfileID.Enabled)
            {
                RefreshIncludeReturnProfileMenu(Profile.ShipmentType);
            }
        }

        /// <summary>
        /// Add applicable profiles for the given shipment type to the context menu
        /// </summary>
        private void RefreshIncludeReturnProfileMenu(ShipmentTypeCode? shipmentTypeCode)
        {
            BindingList<KeyValuePair<long, string>> newReturnProfiles = new BindingList<KeyValuePair<long, string>>();

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingProfileService shippingProfileService = lifetimeScope.Resolve<IShippingProfileService>();

                List<KeyValuePair<long, string>> returnProfiles = shippingProfileService
                    .GetConfiguredShipmentTypeProfiles()
                    .Where(p => p.ShippingProfileEntity.ShippingProfileID != Profile.ShippingProfileID)
                    .Where(p => p.ShippingProfileEntity.ShipmentType.HasValue)
                    .Where(p => p.IsApplicable(shipmentTypeCode))
                    .Where(p => p.ShippingProfileEntity.ReturnShipment == true)
                    .Select(s => new KeyValuePair<long, string>(s.ShippingProfileEntity.ShippingProfileID, s.ShippingProfileEntity.Name))
                    .OrderBy(g => g.Value)
                    .ToList<KeyValuePair<long, string>>();

                newReturnProfiles = new BindingList<KeyValuePair<long, string>>(returnProfiles);
            }

            // Always add No Profile so if a selected profile is no longer a return profile, this becomes the default
            newReturnProfiles.Insert(0, new KeyValuePair<long, string>(-1, "(No Profile)"));

            includeReturnProfiles = newReturnProfiles;

            // Reset data sources because calling resetbindings() doesn't work
            bindingSource.DataSource = includeReturnProfiles;
            returnProfileID.DataSource = bindingSource;
        }

    }
}
