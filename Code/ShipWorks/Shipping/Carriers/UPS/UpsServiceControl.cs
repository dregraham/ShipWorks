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
using ShipWorks.Shipping.Editing.Rating;
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
using Interapptive.Shared.Business.Geography;
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
        /// Initializes a new instance of the <see cref="UpsServiceControl"/> class.
        /// </summary>
        /// <param name="shipmentTypeCode"></param>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public UpsServiceControl(ShipmentTypeCode shipmentTypeCode, RateControl rateControl)
            : base (shipmentTypeCode, rateControl)
        {
            InitializeComponent();

            originControl.Initialize(ShipmentTypeCode.UpsOnLineTools);

            LoadAccounts();

            service.DisplayMember = "Key";
            service.ValueMember = "Value";

            EnumHelper.BindComboBox<UpsCodPaymentType>(codPaymentType);
            EnumHelper.BindComboBox<UpsDeliveryConfirmationType>(confirmation);
            EnumHelper.BindComboBox<UpsPayorType>(payorTransport);
            EnumHelper.BindComboBox<UpsEmailNotificationSubject>(emailSubject);
            EnumHelper.BindComboBox<UspsEndorsementType>(uspsEndorsement);
            EnumHelper.BindComboBox<UpsPostalSubclassificationType>(surePostClassification);
            EnumHelper.BindComboBox<UpsIrregularIndicatorType>(irregularIndicator);
            EnumHelper.BindComboBox<UpsShipmentChargeType>(payorDuties);

            payorCountry.DisplayMember = "Key";
            payorCountry.ValueMember = "Value";
            payorCountry.DataSource = Geography.Countries.Select(n => new KeyValuePair<string, string>(n, Geography.GetCountryCode(n))).ToList();

            dutiesCountryCode.DisplayMember = "Key";
            dutiesCountryCode.ValueMember = "Value";
            dutiesCountryCode.DataSource = Geography.Countries.Select(n => new KeyValuePair<string, string>(n, Geography.GetCountryCode(n))).ToList();

            packageControl.Initialize(shipmentTypeCode);

            // WorldShip takes care of its own printing, so we won't deal with it here
            if (shipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
            {
                sectionLabelOptions.Visible = false;
            }
        }

        public override void Initialize()
        {
            packageControl.PackageCountChanged += packageDetailsControl.PackageCountChanged;
        }

        /// <summary>
        /// Load the list of UPS accounts
        /// </summary>
        public override void LoadAccounts()
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
            packageDetailsControl.LoadShipments(LoadedShipments, enableEditing);

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Load all the shipment details
        /// </summary>
        private void LoadShipmentDetails()
        {
            List<UpsServiceType> serviceTypes = new List<UpsServiceType>();
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
                serviceTypes.Add(thisService);

                if (!UpsUtility.IsCodAvailable((UpsServiceType)overriddenShipment.Ups.Service, overriddenShipment.AdjustedShipCountryCode()))
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

            UpdateMiAndSurePostSpecificVisibility(serviceTypes);

            // Unhook events
            service.SelectedIndexChanged -= new EventHandler(OnChangeService);

            List<UpsServiceType> availableServices = ShipmentTypeManager.GetType(ShipmentTypeCode).GetAvailableServiceTypes().Select(s => (UpsServiceType)s).ToList();

            // If a distinct on country code only returns a count of 1, all countries are the same
            bool allSameCountry = overriddenShipments.Select(s => string.Format("{0} {1}", s.AdjustedShipCountryCode(), s.AdjustedOriginCountryCode())).Distinct().Count() == 1;

            // If they are all of the same service class, we can load the service classes
            if (allSameCountry)
            {
                ShipmentEntity overriddenShipment = overriddenShipments.First();

                var upsServiceManagerFactory = new UpsServiceManagerFactory(overriddenShipment);
                IUpsServiceManager carrierServiceManager = upsServiceManagerFactory.Create(overriddenShipment);

                // Get a list of service types that are valid for the overriddenShipments
                List<UpsServiceType> validServiceTypes = carrierServiceManager.GetServices(overriddenShipment)
                    .Select(s => s.UpsServiceType).ToList();

                // only include service types that are valid and enabled (availalbeServices)
                List<UpsServiceType> upsServiceTypesToLoad = validServiceTypes.Intersect(availableServices).ToList();

                if (LoadedShipments.Any())
                {
                    // Always include the service type that the shipment is currently configured in the 
                    // event the shipment was configured prior to a service being excluded
                    // Always include the service that the shipments are currently configured with
                    // Only if the ServiceType is valid for the shipment type
                    IEnumerable<UpsServiceType> loadedServices = LoadedShipments.Select(s => (UpsServiceType)s.Ups.Service)
                        .Intersect(validServiceTypes).Distinct();
                    upsServiceTypesToLoad = upsServiceTypesToLoad.Union(loadedServices).ToList();
                }

                List<KeyValuePair<string, UpsServiceType>> services = upsServiceTypesToLoad
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
                    
                    payorTransport.ApplyMultiValue((UpsPayorType) shipment.Ups.PayorType);
                    payorAccount.ApplyMultiText(shipment.Ups.PayorAccount);
                    payorPostalCode.ApplyMultiText(shipment.Ups.PayorPostalCode);
                    payorCountry.ApplyMultiValue(shipment.Ups.PayorCountryCode);

                    payorDuties.ApplyMultiValue((UpsShipmentChargeType) shipment.Ups.ShipmentChargeType);
                    dutiesAccount.ApplyMultiText(shipment.Ups.ShipmentChargeAccount);
                    dutiesPostalCode.ApplyMultiText(shipment.Ups.ShipmentChargePostalCode);
                    dutiesCountryCode.ApplyMultiValue(shipment.Ups.ShipmentChargeCountryCode);

                    LoadEmailNotificationSettings(shipment.Ups);

                    codEnabled.ApplyMultiCheck(shipment.Ups.CodEnabled);
                    codAmount.ApplyMultiAmount(shipment.Ups.CodAmount);
                    codPaymentType.ApplyMultiValue((UpsCodPaymentType) shipment.Ups.CodPaymentType);

                    surePostClassification.ApplyMultiValue((UpsPostalSubclassificationType)shipment.Ups.Subclassification);
                    costCenter.ApplyMultiText(shipment.Ups.CostCenter);
                    packageID.ApplyMultiText(shipment.Ups.UspsPackageID);
                    irregularIndicator.ApplyMultiValue((UpsIrregularIndicatorType)shipment.Ups.IrregularIndicator);

                    uspsEndorsement.ApplyMultiValue((UspsEndorsementType)shipment.Ups.Endorsement);
                }
            }

            // Rehook events
            service.SelectedIndexChanged += new EventHandler(OnChangeService);

            UpdateBillingSectionDisplay();
            UpdateSectionDescription();
        }

        /// <summary>
        /// Gets the loaded service types.
        /// </summary>
        /// <returns></returns>
        public List<UpsServiceType> GetLoadedServiceTypes()
        {
            return LoadedShipments.Select(s => (UpsServiceType)s.Ups.Service).ToList();
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            base.SaveToShipments();

            // Save the origin
            originControl.SaveToEntities();

            // Save the package crap
            packageControl.SaveToEntities();
            packageDetailsControl.SaveToEntities();

            // Save the 
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                shipment.ContentWeight = shipment.Ups.Packages.Sum(p => p.Weight);

                upsAccount.ReadMultiValue(v => shipment.Ups.UpsAccountID = (long) v);
                service.ReadMultiValue(v => { if (v != null) shipment.Ups.Service = (int) v; });
                shipDate.ReadMultiDate(d => shipment.ShipDate = d.Date.AddHours(12));
                saturdayDelivery.ReadMultiCheck(c => shipment.Ups.SaturdayDelivery = c);
               
                confirmation.ReadMultiValue(v => shipment.Ups.DeliveryConfirmation = (int) v);
                shipperRelease.ReadMultiCheck(c=> shipment.Ups.ShipperRelease = c);
                carbonNeutral.ReadMultiCheck(c => shipment.Ups.CarbonNeutral = c);


                referenceNumber.ReadMultiText(t => shipment.Ups.ReferenceNumber = DetermineReferenceNumber(shipment.Ups, t)); // shipment.Ups.ReferenceNumber = t);
                reference2Number.ReadMultiText(t => shipment.Ups.ReferenceNumber2 = DetermineReferenceNumber(shipment.Ups, t)); // = t);

                payorTransport.ReadMultiValue(v => shipment.Ups.PayorType = (int) v);
                payorAccount.ReadMultiText(t => shipment.Ups.PayorAccount = t);
                payorPostalCode.ReadMultiText(t => shipment.Ups.PayorPostalCode = t);
                payorCountry.ReadMultiValue(v => shipment.Ups.PayorCountryCode = (string) v);

                payorDuties.ReadMultiValue(v => shipment.Ups.ShipmentChargeType = (int) v);
                dutiesAccount.ReadMultiText(t => shipment.Ups.ShipmentChargeAccount = t);
                dutiesPostalCode.ReadMultiText(t => shipment.Ups.ShipmentChargePostalCode = t);
                dutiesCountryCode.ReadMultiValue(v => shipment.Ups.ShipmentChargeCountryCode = (string) v);

                SaveEmailNotificationSettings(shipment.Ups);

                codEnabled.ReadMultiCheck(c => shipment.Ups.CodEnabled = c);
                codAmount.ReadMultiAmount(a => shipment.Ups.CodAmount = a);
                codPaymentType.ReadMultiValue(v => shipment.Ups.CodPaymentType = (int) v);

                surePostClassification.ReadMultiValue(v => shipment.Ups.Subclassification = (int)v);
                costCenter.ReadMultiText(t => shipment.Ups.CostCenter = t);
                packageID.ReadMultiText(t => shipment.Ups.UspsPackageID = t);
                irregularIndicator.ReadMultiValue(v => shipment.Ups.IrregularIndicator = (int)v);

                uspsEndorsement.ReadMultiValue(v => shipment.Ups.Endorsement = (int) v);
            }

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Determines what reference number should be used for the specified shipment.
        /// </summary>
        /// <param name="upsShipmentEntity">The shipment to check.</param>
        /// <param name="requestedReferenceNumber">The current reference number as it is.  </param>
        /// <returns>A correct reference number for the shipment.</returns>
        private static string DetermineReferenceNumber(UpsShipmentEntity upsShipmentEntity, string requestedReferenceNumber)
        {
            UpsServiceType upsServiceType = (UpsServiceType) upsShipmentEntity.Service;

            return UpsUtility.IsUpsMiService(upsServiceType) ? string.Empty : requestedReferenceNumber;
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
            if (service.SelectedValue == null)
            {
                MessageHelper.ShowError(this, "The selected service is not supported by ShipWorks.  Please contact customer support for assistance.");
                return;
            }

            SaveToShipments();

            UpdateSectionDescription();
            UpdateSaturdayAvailability();
            UpdateCodVisibility();
            UpdateMiAndSurePostSpecificVisibility(new List<UpsServiceType>() {(UpsServiceType)service.SelectedValue});

            RaiseShipmentServiceChanged();
            RaiseRateCriteriaChanged();

            if (!service.MultiValued && service.SelectedValue != null)
            {
                UpdateBillingSectionDisplay();

                SyncSelectedRate();
            }
        }

        /// <summary>
        /// Synchronizes the selected rate in the rate control.
        /// </summary>
        public override void SyncSelectedRate()
        {
            if (!service.MultiValued && service.SelectedValue != null)
            {
                // Update the selected rate in the rate control to coincide with the service change
                UpsServiceType selectedServiceType = (UpsServiceType)service.SelectedValue;

                RateResult matchingRate = RateControl.RateGroup.Rates.FirstOrDefault(r =>
                {
                    List<ShipmentTypeCode> upsTypeCodes = new List<ShipmentTypeCode> { ShipmentTypeCode.UpsWorldShip, ShipmentTypeCode.UpsOnLineTools };

                    if (!upsTypeCodes.Contains(r.ShipmentType))
                    {
                        return false;
                    }

                    if (r.Tag == null || !(r.OriginalTag is UpsServiceType))
                    {
                        return false;
                    }

                    return (UpsServiceType)r.OriginalTag == selectedServiceType;
                });

                RateControl.SelectRate(matchingRate);
            }
            else
            {
                RateControl.ClearSelection();
            }
        }

        /// <summary>
        /// Updates MI and SurePost specific visibility.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        private void UpdateMiAndSurePostSpecificVisibility(List<UpsServiceType> serviceType)
        {            
			bool isSurePost = false;
            bool isMi = false;
            bool showEndorsement = false;

            if (serviceType.Any())
            {
                isSurePost = serviceType.Any(UpsUtility.IsUpsSurePostService);
                isMi = serviceType.Any(UpsUtility.IsUpsMiService);
                showEndorsement = isSurePost || isMi;

                // Because endorsements are not utilized on international packages, use value 5 for international packages.
                if (serviceType.All(a => (a == UpsServiceType.UpsMailInnovationsIntEconomy) || (a == UpsServiceType.UpsMailInnovationsIntPriority)))
                {
                    showEndorsement = false;
                }
            }

            endorsementPanel.Visible = showEndorsement; 

            confirmationPanel.Visible = !isSurePost;
            sectionCod.Visible = !isSurePost && !isMi;

            sectionReturns.Visible = ShipmentTypeManager.GetType(ShipmentTypeCode).SupportsReturns && !isSurePost && !isMi;    

            sectionSurePost.Visible = isSurePost || isMi;

            carbonNeutralPanel.Visible = !isMi;

            shipperReleasePanel.Visible = !isSurePost && !isMi;

            sectionBilling.Visible = !isSurePost;

            otherPackageDetails.Visible = !isSurePost && !isMi;

            // Reference numbers are not allowed with Mail Innovations.
            referencePanel.Visible = !isMi;
            reference2Panel.Visible = !isMi;

            UpdateIrregularVisibility();

            UpdateSectionOptionsHeight();
        }

        /// <summary>
        /// Updates the irregular visibility.
        /// </summary>
        private void UpdateIrregularVisibility()
        {
            bool isOlt = ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools;

            labelIrregularIndicator.Visible = isOlt;
            irregularIndicator.Visible = isOlt;

            sectionSurePost.Height = (isOlt ?
                                          irregularIndicator.Bottom : packageID.Bottom) 
                                          + (sectionSurePost.Height - sectionSurePost.ContentPanel.Height) + 8;
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

            UpdateMiAndSurePostSpecificVisibility(GetLoadedServiceTypes());
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
                text += (account != null) ? account.Description : "(None)";
            }

            sectionFrom.ExtraText = text + ", " + originControl.OriginDescription;

            RaiseRateCriteriaChanged();
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
            sectionBilling.ExtraText = "Bill shipment to: ";
            if (payorTransport.MultiValued)
            {
                sectionBilling.ExtraText += "(Multiple)";
            }
            else if (payorTransport.SelectedValue != null)
            {
                sectionBilling.ExtraText += EnumHelper.GetDescription((UpsPayorType) payorTransport.SelectedValue);
            }
            else
            {
                sectionBilling.ExtraText = "";
            }

            // See if we have any customs to determine if we are domestic/international.  LoadedShipments can be null when being called
            // from some of the index changed events, so we'll default to no customs in those cases.  Eventually, this will get called
            // after LoadShipmentDetails.
            bool hasCustoms = LoadedShipments != null ? LoadedShipments.Any(s => CustomsManager.IsCustomsRequired(s)) : false;

            // We don't want to show duties/payor for MI or SurePost, so see if we have any shipments that AREN'T either of those two.
            bool hasNonMiOrSurePost = LoadedShipments != null ? LoadedShipments.Any(s => !UpsUtility.IsUpsMiOrSurePostService((UpsServiceType)s.Ups.Service)) : false;

            if (hasCustoms && hasNonMiOrSurePost)
            {
                panelPayorDuties.Visible = true;
                if (payorDuties.MultiValued)
                {
                    sectionBilling.ExtraText += "     Bill duties/Fees to: (Multiple)";
                }
                else if (payorDuties.SelectedValue != null)
                {
                    sectionBilling.ExtraText += "     Bill duties/Fees to: ";
                    sectionBilling.ExtraText += EnumHelper.GetDescription((UpsShipmentChargeType)payorDuties.SelectedValue);
                }

                if (panelTransportAccount.Visible)
                {
                    panelPayorDuties.Top = panelTransportAccount.Bottom + 4;
                }
                else
                {
                    panelPayorDuties.Top = panelPayorTransport.Bottom + 4; 
                }
            }
            else
            {
                panelPayorDuties.Visible = false;
            }

            int bottom = panelTransportAccount.Visible ? panelTransportAccount.Bottom : panelPayorTransport.Bottom + 4;
            bottom = panelPayorDuties.Visible ? panelPayorDuties.Bottom : bottom + 4;

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
        /// The size of the package details control has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPackageDetailsControlSizeChanged(object sender, EventArgs e)
        {
            otherPackageDetails.Height = (otherPackageDetails.Height - otherPackageDetails.ContentPanel.Height) + packageDetailsControl.Bottom + 4;
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
        /// One of the values that affects rates has changed
        /// </summary>
        private void OnRateCriteriaChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Some aspect of the shipment that affects ShipSense has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            RaiseShipSenseFieldChanged();
        }

        /// <summary>
        /// A rate has been selected
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            int oldIndex = service.SelectedIndex;

            if (e.Rate.OriginalTag is UpsServiceType)
            {
                UpsServiceType serviceType = (UpsServiceType)e.Rate.OriginalTag;

                service.SelectedValue = serviceType;
                if (service.SelectedIndex == -1 && oldIndex != -1)
                {
                    service.SelectedIndex = oldIndex;
                }
            }
        }

        /// <summary>
        /// The number of packages has changed
        /// </summary>
        private void OnPackageCountChanged(object sender, EventArgs e)
        {
            //LoadInsuranceValueUI(true);
            RaiseShipSenseFieldChanged();
        }

        private void OnPayorDutiesFeesChanged(object sender, EventArgs e)
        {
            bool countryVisible = false;

            if (payorDuties.MultiValued)
            {
                labelDutiesCountryCode.Visible = true;
                dutiesCountryCode.Visible = true;
                panelDutiesAccount.Visible = true;
                panelDutiesAccount.Height = dutiesCountryCode.Bottom + 4;
                panelPayorDuties.Height = panelDutiesAccount.Bottom + 4;
            }
            else
            {
                UpsShipmentChargeType selectedShipmentChargeType = (UpsShipmentChargeType)payorDuties.SelectedValue;

                switch (selectedShipmentChargeType)
                {
                    case UpsShipmentChargeType.BillShipper:
                        panelDutiesAccount.Visible = false;
                        panelPayorDuties.Height = panelDutiesAccount.Visible ? panelDutiesAccount.Bottom + 4 : payorDuties.Bottom + 4;
                        countryVisible = false;
                        break;
                    case UpsShipmentChargeType.BillReceiver:
                        panelDutiesAccount.Visible = true;
                        countryVisible = false;
                        panelDutiesAccount.Height = dutiesPostalCode.Bottom + 4;
                        panelPayorDuties.Height = panelDutiesAccount.Bottom + 4;
                        break;
                    case UpsShipmentChargeType.BillThirdParty:
                        panelDutiesAccount.Visible = true;
                        countryVisible = true;
                        panelDutiesAccount.Height = dutiesCountryCode.Bottom + 4;
                        panelPayorDuties.Height = panelDutiesAccount.Bottom + 4;
                        break;
                }

                labelDutiesCountryCode.Visible = countryVisible;
                dutiesCountryCode.Visible = countryVisible;
            }

            UpdateBillingSectionDisplay();
        }

        /// <summary>
        /// Changing the payor transport account
        /// </summary>
        private void OnChangePayorType(object sender, EventArgs e)
        {
            bool countryVisible = false;

            if (payorTransport.MultiValued)
            {
                labelPayorCountry.Visible = true;
                payorCountry.Visible = true;
                panelTransportAccount.Visible = true;
                panelTransportAccount.Height = payorCountry.Bottom + 4;
            }
            else
            {
                UpsPayorType selectedPayorType = (UpsPayorType)payorTransport.SelectedValue;

                switch (selectedPayorType)
                {
                    case UpsPayorType.Sender:
                        panelTransportAccount.Visible = false;
                        countryVisible = false;
                        break;
                    case UpsPayorType.Receiver:
                        panelTransportAccount.Visible = true;
                        countryVisible = false;
                        panelTransportAccount.Height = payorPostalCode.Bottom + 4;
                        break;
                    case UpsPayorType.ThirdParty:
                        panelTransportAccount.Visible = true;
                        countryVisible = true;
                        panelTransportAccount.Height = payorCountry.Bottom + 4;
                        break;
                }

                labelPayorCountry.Visible = countryVisible;
                payorCountry.Visible = countryVisible;
            }

            UpdateBillingSectionDisplay();
        }
    }
}
