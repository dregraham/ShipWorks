using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;
using ShipWorks.UI.Utility;
using System.Collections;
using ShipWorks.Editions;
using ShipWorks.Stores;
using ShipWorks.Editions.Brown;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// UserControl for editing the UPS OLT service settings
    /// </summary>
    public partial class UpsServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsServiceControl(ShipmentTypeCode ShipmentTypeCode)
            : base (ShipmentTypeCode)
        {
            InitializeComponent();
            this.rateControl.ReloadRatesRequired += new System.EventHandler(this.OnReloadRatesRequired);

            originControl.Initialize(ShipmentTypeCode.UpsOnLineTools);

            LoadUpsAccounts();

            service.DisplayMember = "Key";
            service.ValueMember = "Value";

            EnumHelper.BindComboBox<UpsCodPaymentType>(codPaymentType);
            EnumHelper.BindComboBox<UpsDeliveryConfirmationType>(confirmation);
            EnumHelper.BindComboBox<UpsPayorType>(payorType);
            EnumHelper.BindComboBox<UpsEmailNotificationSubject>(emailSubject);
            EnumHelper.BindComboBox<UspsEndorsementType>(uspsEndorsement);
            EnumHelper.BindComboBox<UpsSurePostSubclassificationType>(surePostClassification);

            payorCountry.DisplayMember = "Key";
            payorCountry.ValueMember = "Value";
            payorCountry.DataSource = Geography.Countries.Select(n => new KeyValuePair<string, string>(n, Geography.GetCountryCode(n))).ToList();

            packageControl.Initialize(ShipmentTypeCode);

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
                upsAccount.DataSource = UpsAccountManager.Accounts.Select(s => new KeyValuePair<string, long>(s.Description, s.UpsAccountID)).ToList();
                upsAccount.Enabled = true;
            }
            else
            {
                upsAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                upsAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Load the shipment data into the ui
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();

            // Load the base
            base.RecipientDestinationChanged -= new EventHandler(OnRecipientDestinationChanged);
            base.LoadShipments(shipments, enableEditing, enableShippingAddress);
            base.RecipientDestinationChanged += new EventHandler(OnRecipientDestinationChanged);

            // The base will disable if editing is not enabled, but due to the packaging selction, we need to customize how it works
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

            // Load the origin
            originControl.LoadShipments(shipments);

            upsAccount.SelectedIndexChanged -= new EventHandler(OnChangeUpsAccount);
            codEnabled.CheckedChanged -= new EventHandler(OnChangeCodEnabled);

            // Load shipment details
            LoadShipmentDetails();

            upsAccount.SelectedIndexChanged += new EventHandler(OnChangeUpsAccount);
            codEnabled.CheckedChanged += new EventHandler(OnChangeCodEnabled);

            // Load the package information
            packageControl.LoadShipments(LoadedShipments, enableEditing);

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// Load all the shipment details
        /// </summary>
        private void LoadShipmentDetails()
        {
            UpsServiceType? serviceType = null;
            bool allServicesSame = true;
            bool allCodAvailable = true;
            bool anyCodEnabled = false;
            bool anySaturday = false;

            // Grab a list of overridden shipments to work with because we
            // need to check with the store  to see if anything about the shipment was overridden in case
            // it may have effected the shipping services available (i.e. the eBay GSP program)
            List<ShipmentEntity> overriddenShipments = new List<ShipmentEntity>();
            LoadedShipments.ForEach(s => overriddenShipments.Add(ShippingManager.GetOverriddenStoreShipment(s)));

            // Determine if all shipments will have the same destination service types
            foreach (ShipmentEntity overriddenShipment in overriddenShipments)
            {
                UpsServiceType thisService = (UpsServiceType)overriddenShipment.Ups.Service;

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

                if (!UpsUtility.IsCodAvailable((UpsServiceType)overriddenShipment.Ups.Service, overriddenShipment.ShipCountryCode))
                {
                    allCodAvailable = false;
                }
                else
                {
                    if (overriddenShipment.Ups.CodEnabled)
                    {
                        anyCodEnabled = true;
                    }
                }

                if (UpsUtility.CanDeliverOnSaturday(thisService, overriddenShipment.ShipDate))
                {
                    anySaturday = true;
                }
            }

            // If there not all the same clear the value we had
            if (!allServicesSame)
            {
                serviceType = null;
            }

            UpdateMiAndSurePostSpecificVisibility(serviceType);

            // Unhook events
            service.SelectedIndexChanged -= new EventHandler(OnChangeService);

            // If a distinct on country code only returns a count of 1, all countries are the same
            bool allSameCountry = overriddenShipments.Select(s => string.Format("{0} {1}", s.ShipCountryCode, s.OriginCountryCode)).Distinct().Count() == 1;

            // If they are all of the same service class, we can load the service classes
            if (allSameCountry)
            {
                ShipmentEntity overriddenShipment = overriddenShipments.First();

                var upsServiceManagerFactory = new UpsServiceManagerFactory();
                IUpsServiceManager carrierServiceManager = upsServiceManagerFactory.Create(overriddenShipment);
                List<UpsServiceType> serviceTypes = carrierServiceManager.GetServices(overriddenShipment).Select(s => s.UpsServiceType).ToList();

                List<KeyValuePair<string, UpsServiceType>> services = serviceTypes
                    .Select(type => new KeyValuePair<string, UpsServiceType>(EnumHelper.GetDescription(type), (UpsServiceType) type))
                    .ToList();

                service.DataSource = services;
            }
            else
            {
                service.DataSource = new KeyValuePair<string, UpsServiceType>[0];
            }

            // Make it visible if any of them have saturday dates
            saturdayDelivery.Visible = anySaturday;

            // Show COD only if all have it
            sectionCod.Visible = allCodAvailable;

            // Enable the COD editing ui if any shipments have COD enabled
            EnableCodUI(anyCodEnabled);

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    upsAccount.ApplyMultiValue(shipment.Ups.UpsAccountID);

                    service.ApplyMultiValue((UpsServiceType) shipment.Ups.Service);
                    shipDate.ApplyMultiDate(shipment.ShipDate);
                    saturdayDelivery.ApplyMultiCheck(shipment.Ups.SaturdayDelivery);

                    confirmation.ApplyMultiValue((UpsDeliveryConfirmationType) shipment.Ups.DeliveryConfirmation);
                    referenceNumber.ApplyMultiText(shipment.Ups.ReferenceNumber);
                    reference2Number.ApplyMultiText(shipment.Ups.ReferenceNumber2);
                    shipperRelease.ApplyMultiCheck(shipment.Ups.ShipperRelease);
                    carbonNeutral.ApplyMultiCheck(shipment.Ups.CarbonNeutral);
                    
                    payorType.ApplyMultiValue((UpsPayorType) shipment.Ups.PayorType);
                    payorAccount.ApplyMultiText(shipment.Ups.PayorAccount);
                    payorPostalCode.ApplyMultiText(shipment.Ups.PayorPostalCode);
                    payorCountry.ApplyMultiValue(shipment.Ups.PayorCountryCode);

                    LoadEmailNotificationSettings(shipment.Ups);

                    codEnabled.ApplyMultiCheck(shipment.Ups.CodEnabled);
                    codAmount.ApplyMultiAmount(shipment.Ups.CodAmount);
                    codPaymentType.ApplyMultiValue((UpsCodPaymentType) shipment.Ups.CodPaymentType);

                    surePostClassification.ApplyMultiValue((UpsSurePostSubclassificationType)shipment.Ups.Subclassification);
                    uspsEndorsement.ApplyMultiValue((UspsEndorsementType)shipment.Ups.Endorsement);
                }
            }

            // Rehook events
            service.SelectedIndexChanged += new EventHandler(OnChangeService);

            UpdateBillingSectionDisplay();
            UpdateSectionDescription();
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();

            base.SaveToShipments();

            // Save the origin
            originControl.SaveToEntities();

            // Save the package crap
            packageControl.SaveToEntities();

            // Save the 
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                shipment.ContentWeight = shipment.Ups.Packages.Sum(p => p.Weight);

                upsAccount.ReadMultiValue(v => shipment.Ups.UpsAccountID = (long) v);
                service.ReadMultiValue(v => { if (v != null) shipment.Ups.Service = (int) v; });
                shipDate.ReadMultiDate(d => shipment.ShipDate = d.Date.AddHours(12));
                saturdayDelivery.ReadMultiCheck(c => shipment.Ups.SaturdayDelivery = c);
               
                confirmation.ReadMultiValue(v => shipment.Ups.DeliveryConfirmation = (int) v);
                referenceNumber.ReadMultiText(t => shipment.Ups.ReferenceNumber = t);
                reference2Number.ReadMultiText(t => shipment.Ups.ReferenceNumber2 = t);
                shipperRelease.ReadMultiCheck(c=> shipment.Ups.ShipperRelease = c);
                carbonNeutral.ReadMultiCheck(c => shipment.Ups.CarbonNeutral = c);

                payorType.ReadMultiValue(v => shipment.Ups.PayorType = (int) v);
                payorAccount.ReadMultiText(t => shipment.Ups.PayorAccount = t);
                payorPostalCode.ReadMultiText(t => shipment.Ups.PayorPostalCode = t);
                payorCountry.ReadMultiValue(v => shipment.Ups.PayorCountryCode = (string) v);

                SaveEmailNotificationSettings(shipment.Ups);

                codEnabled.ReadMultiCheck(c => shipment.Ups.CodEnabled = c);
                codAmount.ReadMultiAmount(a => shipment.Ups.CodAmount = a);
                codPaymentType.ReadMultiValue(v => shipment.Ups.CodPaymentType = (int) v);

                surePostClassification.ReadMultiValue(v => shipment.Ups.Subclassification = (int) v);
                uspsEndorsement.ReadMultiValue(v => shipment.Ups.Endorsement = (int) v);
            }

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// Update the insurance display for the given shipments
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            packageControl.UpdateInsuranceDisplay();
        }

        /// <summary>
        /// The selected ups account has changed
        /// </summary>
        void OnChangeUpsAccount(object sender, EventArgs e)
        {
            if (upsAccount.SelectedValue != null)
            {
                long accountID = (long) upsAccount.SelectedValue;

                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipment.Ups.UpsAccountID = accountID;
                }

                originControl.NotifySelectedAccountChanged();
            }
        }

        /// <summary>
        /// The selected service is changing
        /// </summary>
        void OnChangeService(object sender, EventArgs e)
        {
            if (!service.MultiValued && service.SelectedValue != null)
            {
                UpdateBillingSectionDisplay();
            }

            UpdateSectionDescription();
            UpdateSaturdayAvailability();
            UpdateCodVisibility();
            UpdateMiAndSurePostSpecificVisibility(service.SelectedValue == null ? null : (UpsServiceType?)service.SelectedValue);
            
            SaveToShipments();
            RaiseShipmentServiceChanged();
        }

        /// <summary>
        /// Updates MI and SurePost specific visibility.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        private void UpdateMiAndSurePostSpecificVisibility(UpsServiceType? serviceType)
        {            
			bool isSurePost=false;
            bool isMi = false;
            bool showEndorsement = false;

            if (serviceType.HasValue)
            {
                isSurePost = UpsUtility.IsUpsSurePostService(serviceType.Value);
                isMi = UpsUtility.IsUpsMiService(serviceType.Value);
                showEndorsement = isSurePost || isMi;

                if (serviceType.Value == UpsServiceType.UpsMailInnovationsIntEconomy ||
                    serviceType.Value == UpsServiceType.UpsMailInnovationsIntEconomy)
                {
                    showEndorsement = false;
                }
            }
            endorsementPanel.Visible = showEndorsement; 

            confirmationPanel.Visible = !isSurePost;
            sectionCod.Visible = !isSurePost;
            sectionReturns.Visible = !isSurePost;

            sectionSurePost.Visible = isSurePost;

            carbonNeutralPanel.Visible = !isMi;

            shipperReleasePanel.Visible = !isSurePost && !isMi;

            sectionBilling.Visible = !isSurePost;

            UpdateSectionOptionsHeight();
        }

        /// <summary>
        /// Updates the height of the section options.
        /// </summary>
        private void UpdateSectionOptionsHeight()
        {
            sectionOptions.Height = confirmationAndReferenceFlowPanel.Bottom + (sectionOptions.Height - sectionOptions.ContentPanel.Height);
        }

        /// <summary>
        /// Called whent the recipient country has changed.  We may have to switch from an international to domestic UI
        /// </summary>
        void OnRecipientDestinationChanged(object sender, EventArgs e)
        {
            SaveToShipments();
            LoadShipmentDetails();
            UpdateCodVisibility();
            UpdateMiAndSurePostSpecificVisibility(service.SelectedValue == null ? null : (UpsServiceType?) service.SelectedValue);
        }

        /// <summary>
        /// Update COD visibility.  SurePost does not allow COD.
        /// </summary>
        private void UpdateCodVisibility()
        {
            UpsServiceType? serviceType = (UpsServiceType?) service.SelectedValue;

            if (serviceType.HasValue && UpsUtility.IsUpsSurePostService(serviceType.Value))
            {
                sectionCod.Visible = false;
            }
            else if (personControl.CountryCode != null)
            {
                sectionCod.Visible = UpsUtility.IsCodAvailable(personControl.CountryCode);
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
        /// Load into the UI the configured values for email notifications
        /// </summary>
        private void LoadEmailNotificationSettings(UpsShipmentEntity ups)
        {
            bool senderShip = (ups.EmailNotifySender & (int) UpsEmailNotificationType.Ship) != 0;
            bool senderException = (ups.EmailNotifySender & (int) UpsEmailNotificationType.Exception) != 0;
            bool senderDelivery = (ups.EmailNotifySender & (int) UpsEmailNotificationType.Deliver) != 0;

            bool recipientShip = (ups.EmailNotifyRecipient & (int) UpsEmailNotificationType.Ship) != 0;
            bool recipientException = (ups.EmailNotifyRecipient & (int) UpsEmailNotificationType.Exception) != 0;
            bool recipientDelivery = (ups.EmailNotifyRecipient & (int) UpsEmailNotificationType.Deliver) != 0;

            bool otherShip = (ups.EmailNotifyOther & (int) UpsEmailNotificationType.Ship) != 0;
            bool otherException = (ups.EmailNotifyOther & (int) UpsEmailNotificationType.Exception) != 0;
            bool otherDelivery = (ups.EmailNotifyOther & (int) UpsEmailNotificationType.Deliver) != 0;

            emailNotifySenderShip.ApplyMultiCheck(senderShip);
            emailNotifySenderException.ApplyMultiCheck(senderException);
            emailNotifySenderDelivery.ApplyMultiCheck(senderDelivery);

            emailNotifyRecipientShip.ApplyMultiCheck(recipientShip);
            emailNotifyRecipientException.ApplyMultiCheck(recipientException);
            emailNotifyRecipientDelivery.ApplyMultiCheck(recipientDelivery);

            emailNotifyOtherShip.ApplyMultiCheck(otherShip);
            emailNotifyOtherException.ApplyMultiCheck(otherException);
            emailNotifyOtherDelivery.ApplyMultiCheck(otherDelivery);

            emailNotifyOtherAddress.ApplyMultiText(ups.EmailNotifyOtherAddress);
            emailNotifyMessage.ApplyMultiText(ups.EmailNotifyMessage);
            emailFrom.ApplyMultiText(ups.EmailNotifyFrom);
            emailSubject.ApplyMultiValue((UpsEmailNotificationSubject) ups.EmailNotifySubject);
        }

        /// <summary>
        /// Save the email settings currently in the UI to the shipment
        /// </summary>
        private void SaveEmailNotificationSettings(UpsShipmentEntity ups)
        {
            emailNotifySenderShip.ReadMultiCheck(c => { ups.EmailNotifySender = ApplyEmailNotificationType(c, ups.EmailNotifySender, UpsEmailNotificationType.Ship); });
            emailNotifySenderException.ReadMultiCheck(c => { ups.EmailNotifySender = ApplyEmailNotificationType(c, ups.EmailNotifySender, UpsEmailNotificationType.Exception); });
            emailNotifySenderDelivery.ReadMultiCheck(c => { ups.EmailNotifySender = ApplyEmailNotificationType(c, ups.EmailNotifySender, UpsEmailNotificationType.Deliver); });

            emailNotifyRecipientShip.ReadMultiCheck(c => { ups.EmailNotifyRecipient = ApplyEmailNotificationType(c, ups.EmailNotifyRecipient, UpsEmailNotificationType.Ship); });
            emailNotifyRecipientException.ReadMultiCheck(c => { ups.EmailNotifyRecipient = ApplyEmailNotificationType(c, ups.EmailNotifyRecipient, UpsEmailNotificationType.Exception); });
            emailNotifyRecipientDelivery.ReadMultiCheck(c => { ups.EmailNotifyRecipient = ApplyEmailNotificationType(c, ups.EmailNotifyRecipient, UpsEmailNotificationType.Deliver); });

            emailNotifyOtherShip.ReadMultiCheck(c => { ups.EmailNotifyOther = ApplyEmailNotificationType(c, ups.EmailNotifyOther, UpsEmailNotificationType.Ship); });
            emailNotifyOtherException.ReadMultiCheck(c => { ups.EmailNotifyOther = ApplyEmailNotificationType(c, ups.EmailNotifyOther, UpsEmailNotificationType.Exception); });
            emailNotifyOtherDelivery.ReadMultiCheck(c => { ups.EmailNotifyOther = ApplyEmailNotificationType(c, ups.EmailNotifyOther, UpsEmailNotificationType.Deliver); });

            emailNotifyOtherAddress.ReadMultiText(t => ups.EmailNotifyOtherAddress = t);
            emailNotifyMessage.ReadMultiText(t => ups.EmailNotifyMessage = t);
            emailFrom.ReadMultiText(t => ups.EmailNotifyFrom = t);
            emailSubject.ReadMultiValue(v => ups.EmailNotifySubject = (int) v);
        }

        /// <summary>
        /// Turn the given notification type on or off depending on the enabled state given
        /// </summary>
        private int ApplyEmailNotificationType(bool enabled, int previous, UpsEmailNotificationType notificationType)
        {
            if (enabled)
            {
                previous |= (int) notificationType;
            }
            else
            {
                previous &= ~(int) notificationType;
            }

            return previous;
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
                changes |= UpsShipmentType.RedistributeContentWeight(shipment);
            }

            // If there were changes we have to reload the package ui
            if (changes)
            {
                packageControl.LoadShipments(LoadedShipments, base.EnableEditing);
            }
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (upsAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                UpsAccountEntity account = upsAccount.SelectedIndex >= 0 ? UpsAccountManager.GetAccount((long) upsAccount.SelectedValue) : null;
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
        }

        /// <summary>
        /// Update the description of the section basedon the configured options
        /// </summary>
        private void UpdateSectionDescription()
        {
            UpsServiceType? serviceType = (UpsServiceType?) service.SelectedValue;

            if (serviceType != null)
            {
                sectionShipment.ExtraText = EnumHelper.GetDescription(serviceType.Value);
            }
            else
            {
                sectionShipment.ExtraText = "(Multiple Services)";
            }
        }

        /// <summary>
        /// Update the description of the billing section
        /// </summary>
        private void UpdateBillingSectionDisplay()
        {
            if (payorType.MultiValued)
            {
                sectionBilling.ExtraText = "(Multiple)";
            }
            else if (payorType.SelectedValue != null)
            {
                sectionBilling.ExtraText = EnumHelper.GetDescription((UpsPayorType) payorType.SelectedValue);
            }
            else
            {
                sectionBilling.ExtraText = "";
            }

            int bottom = panelPayorThirdParty.Visible ? panelPayorThirdParty.Bottom : payorType.Bottom + 4;

            sectionBilling.Height = bottom + (sectionBilling.Height - sectionBilling.ContentPanel.Height) + 4;
        }

        /// <summary>
        /// Enable the UI for editing the COD values
        /// </summary>
        private void EnableCodUI(bool enable)
        {
            codAmount.Enabled = enable;
            codPaymentType.Enabled = enable;
        }

        /// <summary>
        /// Changing whether COD is selectecd
        /// </summary>
        void OnChangeCodEnabled(object sender, EventArgs e)
        {
            EnableCodUI(codEnabled.Checked);
        }

        /// <summary>
        /// The size of the package control is changing
        /// </summary>
        private void OnPackageControlSizeChanged(object sender, EventArgs e)
        {
            sectionShipment.Height = (sectionShipment.Height - sectionShipment.ContentPanel.Height) + packageControl.Bottom + 4;
        }

        /// <summary>
        /// Update the displayed availability of the saturday delivery option
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
                    saturdayDelivery.Visible = UpsUtility.CanDeliverOnSaturday((UpsServiceType) service.SelectedValue, shipDate.Value);
                }
            }
        }

        /// <summary>
        /// Changing the state of the saturday delivery flag
        /// </summary>
        private void OnSaturdayDeliveryChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Changing the payor transport account
        /// </summary>
        private void OnChangePayorType(object sender, EventArgs e)
        {
            panelPayorThirdParty.Visible = payorType.MultiValued ||
                (payorType.SelectedValue != null && (UpsPayorType) payorType.SelectedValue != UpsPayorType.Sender);

            bool countryVisible = payorType.MultiValued || (UpsPayorType) payorType.SelectedValue == UpsPayorType.ThirdParty;
            labelPayorCountry.Visible = countryVisible;
            payorCountry.Visible = countryVisible;

            UpdateBillingSectionDisplay();
        }

        /// <summary>
        /// One of the values that affects rates has changed
        /// </summary>
        private void OnRateCriteriaChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// A rate has been selected
        /// </summary>
        private void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            int oldIndex = service.SelectedIndex;

            UpsServiceType servicetype = (UpsServiceType) e.Rate.Tag;

            service.SelectedValue = servicetype;
            if (service.SelectedIndex == -1 && oldIndex != -1)
            {
                service.SelectedIndex = oldIndex;
            }
        }

        /// <summary>
        /// The number of packages has changed
        /// </summary>
        private void OnPackageCountChanged(object sender, EventArgs e)
        {
            //LoadInsuranceValueUI(true);
        }
    }
}
