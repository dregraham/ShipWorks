using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// UserControl for editing ups specific profile settings
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.UpsOnLineTools)]
    public partial class UpsProfileControl : ShippingProfileControlBase
    {
        private BindingList<KeyValuePair<long, string>> includeReturnProfiles = new BindingList<KeyValuePair<long, string>>();
        private BindingSource bindingSource = new BindingSource();

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPageSettings);
            ResizeGroupBoxes(tabPagePackages);

            packagesCount.Items.Clear();

            for (int i = 1; i <= 25; i++)
            {
                packagesCount.Items.Add(i);
            }
        }

        /// <summary>
        /// Label group box
        /// </summary>
        protected GroupBox GroupLabels
        {
            get
            {
                return groupLabels;
            }
        }

        /// <summary>
        /// Tab page settings control
        /// </summary>
        protected Control TabPageSettings
        {
            get
            {
                return tabPageSettings;
            }
        }

        /// <summary>
        /// Load the data from the given profile into the UI
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            UpsProfileEntity ups = profile.Ups;

            if (ShippingSettings.Fetch().UpsInsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                insuranceControl.UseInsuranceBoxLabel = "UPS Declared Value";
                insuranceControl.InsuredValueLabel = "Declared value:";
            }

            UpsShipmentType shipmentType = (UpsShipmentType) ShipmentTypeManager.GetType((ShipmentTypeCode) profile.ShipmentType);

            bool isMIAvailable = shipmentType.IsMailInnovationsEnabled();

            bool isSurePostAvailable;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.UpsSurePost, null);

                isSurePostAvailable = restrictionLevel == EditionRestrictionLevel.None;
            }

            if (isSurePostAvailable || isMIAvailable)
            {
                surePostGroup.Visible = true;
            }

            LoadUpsAccounts();
            LoadOrigins();

            EnumHelper.BindComboBox<UpsServiceType>(service, t => ShowService(t, isMIAvailable, isSurePostAvailable));
            EnumHelper.BindComboBox<UpsDeliveryConfirmationType>(confirmationType);
            EnumHelper.BindComboBox<ResidentialDeterminationType>(residentialDetermination);
            EnumHelper.BindComboBox<UpsPayorType>(payorType);
            EnumHelper.BindComboBox<UpsEmailNotificationSubject>(emailSubject);
            EnumHelper.BindComboBox<UpsReturnServiceType>(returnService);
            EnumHelper.BindComboBox<UpsPostalSubclassificationType>(surePostClassification);
            EnumHelper.BindComboBox<UspsEndorsementType>(uspsEndorsement);
            EnumHelper.BindComboBox<UpsIrregularIndicatorType>(irregularIndicator);
            EnumHelper.BindComboBox<UpsShipmentChargeType>(payorDuties);
            EnumHelper.BindComboBox<UpsCustomsRecipientTINType>(customsRecipientTINType);

            payorCountry.DisplayMember = "Key";
            payorCountry.ValueMember = "Value";
            payorCountry.DataSource = Geography.Countries.Select(n => new KeyValuePair<string, string>(n, Geography.GetCountryCode(n))).ToList();

            payorDutiesCountry.DisplayMember = "Key";
            payorDutiesCountry.ValueMember = "Value";
            payorDutiesCountry.DataSource = Geography.Countries.Select(n => new KeyValuePair<string, string>(n, Geography.GetCountryCode(n))).ToList();

            AddValueMapping(ups, UpsProfileFields.UpsAccountID, accountState, upsAccount, labelAccount);
            AddValueMapping(profile, ShippingProfileFields.OriginID, senderState, originCombo, labelSender);

            AddValueMapping(ups, UpsProfileFields.ResidentialDetermination, residentialState, residentialDetermination, labelResidential);

            AddValueMapping(ups, UpsProfileFields.Service, serviceState, service, labelService);
            AddValueMapping(ups, UpsProfileFields.SaturdayDelivery, saturdayState, saturdayDelivery, labelSaturday);

            AddValueMapping(ups, UpsProfileFields.CarbonNeutral, carbonNeutralState, carbonNeutral, carbonNeutralLabel);
            AddValueMapping(ups, UpsProfileFields.ShipperRelease, shipperReleaseState, shipperRelease, shipperReleaseLabel);
            AddValueMapping(ups, UpsProfileFields.CommercialPaperlessInvoice, usePaperlessInvoiceState, usePaperlessInvoice, usePaperlessInvoiceLabel);
            AddValueMapping(ups, UpsProfileFields.PaperlessAdditionalDocumentation, extraDocumentsState, extraDocuments, extraDocumentsLabel);

            AddValueMapping(ups, UpsProfileFields.DeliveryConfirmation, confirmationState, confirmationType, labelConfirmation);
            AddValueMapping(ups, UpsProfileFields.ReferenceNumber, referenceState, referenceNumber, labelReference);
            AddValueMapping(ups, UpsProfileFields.ReferenceNumber2, reference2State, referenceNumber2, labelReference2);

            AddValueMapping(ups, UpsProfileFields.PayorType, payorTypeState, payorType, labelPayor);
            AddValueMapping(ups, UpsProfileFields.PayorAccount, payorAccountState, payorAccount, labelPayorAccount);
            AddValueMapping(ups, UpsProfileFields.PayorPostalCode, payorAccountState, payorPostalCode, labelPayorPostalCode);
            AddValueMapping(ups, UpsProfileFields.PayorCountryCode, payorAccountState, payorCountry, labelPayorCountry);

            AddValueMapping(ups, UpsProfileFields.ShipmentChargeType, payorDutiesState, payorDuties, labelPayorDuties);
            AddValueMapping(ups, UpsProfileFields.ShipmentChargeAccount, payorDutiesAccountState, payorDutiesAccount, labelPayorDutiesAccount);
            AddValueMapping(ups, UpsProfileFields.ShipmentChargePostalCode, payorDutiesAccountState, payorDutiesPostalCode, labelPayorDutiesPostalCode);
            AddValueMapping(ups, UpsProfileFields.ShipmentChargeCountryCode, payorDutiesAccountState, payorDutiesCountry, labelPayorDutiesCountry);

            //Customs
            AddValueMapping(ups, UpsProfileFields.CustomsDescription, customsDescState, customsDescription, labelCustomsDescription);
            AddValueMapping(ups, UpsProfileFields.CustomsRecipientTIN, customsRecipientTINState, customsRecipientTIN, labelCustomsRecipientTIN);
            AddValueMapping(ups, UpsProfileFields.CustomsRecipientTINType, customsRecipientTINTypeState, customsRecipientTINType, labelCustomsRecipientTINType);

            AddEnabledStateMapping(ups, UpsProfileFields.EmailNotifySender, emailNotifySenderState, emailNotifySenderShip, labelEmailSender);
            AddEnabledStateMapping(ups, UpsProfileFields.EmailNotifySender, emailNotifySenderState, emailNotifySenderException);
            AddEnabledStateMapping(ups, UpsProfileFields.EmailNotifySender, emailNotifySenderState, emailNotifySenderDelivery);
            AddEnabledStateMapping(ups, UpsProfileFields.EmailNotifyRecipient, emailNotifyRecipientState, emailNotifyRecipientShip, labelEmailRecipient);
            AddEnabledStateMapping(ups, UpsProfileFields.EmailNotifyRecipient, emailNotifyRecipientState, emailNotifyRecipientException, labelEmailRecipient);
            AddEnabledStateMapping(ups, UpsProfileFields.EmailNotifyRecipient, emailNotifyRecipientState, emailNotifyRecipientDelivery, labelEmailRecipient);
            AddEnabledStateMapping(ups, UpsProfileFields.EmailNotifyOther, emailNotifyOtherState, emailNotifyOtherShip);
            AddEnabledStateMapping(ups, UpsProfileFields.EmailNotifyOther, emailNotifyOtherState, emailNotifyOtherException);
            AddEnabledStateMapping(ups, UpsProfileFields.EmailNotifyOther, emailNotifyOtherState, emailNotifyOtherDelivery);

            ApplyEmailNotificationValues(ups.EmailNotifySender, emailNotifySenderShip, emailNotifySenderException, emailNotifySenderDelivery);
            ApplyEmailNotificationValues(ups.EmailNotifyRecipient, emailNotifyRecipientShip, emailNotifyRecipientException, emailNotifyRecipientDelivery);
            ApplyEmailNotificationValues(ups.EmailNotifyOther, emailNotifyOtherShip, emailNotifyOtherException, emailNotifyOtherDelivery);

            AddValueMapping(ups, UpsProfileFields.EmailNotifyOtherAddress, emailNotifyOtherState, emailNotifyOtherAddress, labelEmailOther);
            AddValueMapping(ups, UpsProfileFields.EmailNotifyMessage, emailNotifyMessageState, emailNotifyMessage, labelPersonalMessage);
            AddValueMapping(ups, UpsProfileFields.EmailNotifyFrom, emailNotifyFromState, emailFrom, labelEmailFrom);
            AddValueMapping(ups, UpsProfileFields.EmailNotifySubject, emailNotifySubjectState, emailSubject, labelEmailSubject);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);

            // SurePost
            AddValueMapping(ups, UpsProfileFields.Subclassification, surePostClassificationState, surePostClassification, labelSurePostClassification);
            AddValueMapping(ups, UpsProfileFields.Endorsement, uspsEndorsementState, uspsEndorsement, labelUspsEndorsement);
            AddValueMapping(ups, UpsProfileFields.CostCenter, costCenterState, costCenter, labelCostCenter);
            AddValueMapping(ups, UpsProfileFields.UspsPackageID, packageIdState, packageId, labelPackageId);
            AddValueMapping(ups, UpsProfileFields.IrregularIndicator, irregularIndicatorState, irregularIndicator, labelIrregularIndicator);

            // Returns
            RefreshIncludeReturnProfileMenu(profile.ShipmentType);
            returnProfileID.DisplayMember = "Value";
            returnProfileID.ValueMember = "Key";
            returnProfileID.SelectedIndex = -1;

            AddValueMapping(profile, ShippingProfileFields.ReturnShipment, returnState, returnShipment);
            AddValueMapping(profile, ShippingProfileFields.IncludeReturn, includeReturnState, includeReturn);
            AddValueMapping(profile, ShippingProfileFields.ReturnProfileID, applyReturnProfile, returnProfileID);
            AddValueMapping(profile, ShippingProfileFields.ApplyReturnProfile, applyReturnProfileState, applyReturnProfile);
            AddValueMapping(ups, UpsProfileFields.ReturnService, returnServiceState, returnService, labelReturnService);
            AddValueMapping(ups, UpsProfileFields.ReturnContents, returnContentsState, returnContents, labelReturnContents);
            AddValueMapping(ups, UpsProfileFields.ReturnUndeliverableEmail, returnUndeliverableState, returnUndeliverable, labelUndeliverableMail);

            // Map parent/child relationships
            SetParentCheckBox(includeReturnState, includeReturn, applyReturnProfileState, applyReturnProfile);
            SetParentCheckBox(applyReturnProfileState, applyReturnProfile, applyReturnProfileState, returnProfileID);
            SetParentCheckBox(applyReturnProfileState, applyReturnProfile, applyReturnProfileState, returnProfileIDLabel);
            SetParentCheckBox(returnState, returnShipment, returnContentsState, returnContents);
            SetParentCheckBox(returnState, returnShipment, returnServiceState, returnService);
            SetParentCheckBox(returnState, returnShipment, returnUndeliverableState, returnUndeliverable);
            SetParentCheckBox(returnState, returnShipment, returnUndeliverableState, labelUndeliverableMail);
            SetParentCheckBox(returnState, returnShipment, returnServiceState, labelReturnService);
            SetParentCheckBox(returnState, returnShipment, returnContentsState, labelReturnContents);

            packagesState.Checked = profile.Packages.Count > 0;
            packagesCount.SelectedIndex = packagesState.Checked ? profile.Packages.Count - 1 : -1;
            packagesCount.Enabled = packagesState.Checked;

            LoadPackageEditingUI();

            packagesState.CheckedChanged += new EventHandler(OnChangePackagesChecked);
            packagesCount.SelectedIndexChanged += new EventHandler(OnChangePackagesCount);
        }

        /// <summary>
        /// Returns true if we should show the service. Else false.
        /// </summary>
        private bool ShowService(UpsServiceType upsServiceType, bool isMiAvailable, bool isSurePostAvailable)
        {
            if (UpsUtility.IsUpsSurePostService(upsServiceType))
            {
                return isSurePostAvailable;
            }

            if (UpsUtility.IsUpsMiService(upsServiceType))
            {
                return isMiAvailable;
            }

            return true;
        }

        /// <summary>
        /// Apply the value (if present) to the given checkboxes
        /// </summary>
        private void ApplyEmailNotificationValues(int? value, CheckBox ship, CheckBox except, CheckBox delivery)
        {
            if (value != null)
            {
                ship.Checked = (value.Value & (int) UpsEmailNotificationType.Ship) != 0;
                except.Checked = (value.Value & (int) UpsEmailNotificationType.Exception) != 0;
                delivery.Checked = (value.Value & (int) UpsEmailNotificationType.Deliver) != 0;
            }
        }

        /// <summary>
        /// Read the effective state of the email notification values
        /// </summary>
        private int? ReadEmailNotificationValues(CheckBox ship, CheckBox except, CheckBox deliver)
        {
            if (!ship.Enabled)
            {
                return null;
            }

            int value = 0;

            if (ship.Checked)
            {
                value |= (int) UpsEmailNotificationType.Ship;
            }

            if (except.Checked)
            {
                value |= (int) UpsEmailNotificationType.Exception;
            }

            if (deliver.Checked)
            {
                value |= (int) UpsEmailNotificationType.Deliver;
            }

            return value;
        }

        /// <summary>
        /// Save all the package profile stuff to their entities
        /// </summary>
        public override void SaveToEntity()
        {
            base.SaveToEntity();

            Profile.Ups.EmailNotifySender = ReadEmailNotificationValues(emailNotifySenderShip, emailNotifySenderException, emailNotifySenderDelivery);
            Profile.Ups.EmailNotifyRecipient = ReadEmailNotificationValues(emailNotifyRecipientShip, emailNotifyRecipientException, emailNotifyRecipientDelivery);
            Profile.Ups.EmailNotifyOther = ReadEmailNotificationValues(emailNotifyOtherShip, emailNotifyOtherException, emailNotifyOtherDelivery);

            foreach (UpsProfilePackageControl control in panelPackageControls.Controls)
            {
                // If its visible it means ite being edited
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
            foreach (UpsProfilePackageEntity package in Profile.Packages.ToList())
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
        /// Load the list of UPS accounts
        /// </summary>
        private void LoadUpsAccounts()
        {
            upsAccount.DisplayMember = "Key";
            upsAccount.ValueMember = "Value";

            if (UpsAccountManager.Accounts.Count > 0)
            {
                upsAccount.DataSource = UpsAccountManager.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.UpsAccountID)).ToList();
                upsAccount.Enabled = true;
            }
            else
            {
                upsAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                upsAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Load all the origins
        /// </summary>
        private void LoadOrigins()
        {
            List<KeyValuePair<string, long>> origins = ShipmentTypeManager.GetType(ShipmentTypeCode.UpsOnLineTools).GetOrigins();

            originCombo.DisplayMember = "Key";
            originCombo.ValueMember = "Value";
            originCombo.DataSource = origins;
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
                Profile.Packages.Add(new UpsProfilePackageEntity());
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
            foreach (UpsProfilePackageEntity package in packages)
            {
                UpsProfilePackageControl control;

                // If there is a control for it already, it should match up with this package
                if (panelPackageControls.Controls.Count > index)
                {
                    control = (UpsProfilePackageControl) panelPackageControls.Controls[index];
                    control.Visible = true;

                    Debug.Assert(control.ProfilePackage == package);
                }
                else
                {
                    control = new UpsProfilePackageControl();

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
