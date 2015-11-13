using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;
using System.ComponentModel;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Data;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using Interapptive.Shared.Business;
using Interapptive.Shared.Win32;
using ShipWorks.UI;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores;
using log4net;
using log4net.Repository.Hierarchy;
using System.Globalization;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// ShipmentType implementation for UPS
    /// </summary>
    public abstract class UpsShipmentType : ShipmentType
    {
        protected UpsShipmentType()
        {
            // Use the "live" versions of the repository by default
            AccountRepository = new UpsAccountRepository();
            SettingsRepository = new UpsSettingsRepository();
        }

        /// <summary>
        /// UPS supports getting rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get { return true; }
        }

        /// <summary>
        /// UPS accounts have an address that can be used as the shipment origin
        /// </summary>
        public override bool SupportsAccountAsOrigin
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the account repository that the shipment type should use
        /// to obtain Ups related account information. This provides
        /// the ability to use different UPS account info depending on how the shipment
        /// type is going to be used. For example, to obtain counter rates with a
        /// generic UPS account intended to be used with ShipWorks, all that would
        /// have to be done is to assign this property with a repository that contains
        /// the appropriate account information for getting counter rates.
        /// </summary>
        public ICarrierAccountRepository<UpsAccountEntity> AccountRepository { get; set; }

        /// <summary>
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts
        {
	        get 
	        {
                return AccountRepository.Accounts.Any();
	        }
        }

        /// <summary>
        /// Gets or sets the settings repository that the shipment type should use
        /// to obtain Ups related settings information. This provides
        /// the ability to use different UPS settings depending on how the shipment
        /// type is going to be used. For example, to obtain counter rates with a
        /// generic UPS account intended to be used with ShipWorks, all that would
        /// have to be done is to assign this property with a repository that contains
        /// the appropriate account information for getting counter rates.
        /// </summary>
        public ICarrierSettingsRepository SettingsRepository { get; set; }

        /// <summary>
        /// UPS always uses the residential indicator
        /// </summary>
        public override bool IsResidentialStatusRequired(ShipmentEntity shipment)
        {
            return true;
        }

        /// <summary>
        /// Create the wizard used to do the one-time initial service setup
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            return new UpsSetupWizard(ShipmentTypeCode);
        }

        /// <summary>
        /// Create the user control used to edit UPS shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new UpsServiceControl(ShipmentTypeCode, rateControl);
        }

        /// <summary>
        /// Create the UPS specific profile control
        /// </summary>
        protected override ShippingProfileControlBase CreateProfileControl()
        {
            return new UpsProfileControl();
        }
        
        /// <summary>
        /// Create the UPS specific customs control
        /// </summary>
        public override CustomsControlBase CreateCustomsControl()
        {
            return new UpsCustomsControl();
        }

        /// <summary>
        /// Create the UPS specific table rows
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "Ups", typeof(UpsShipmentEntity), refreshIfPresent);

            // This will exist now
            UpsShipmentEntity ups = shipment.Ups;

            // If refreshing, start over on the packages
            if (refreshIfPresent)
            {
                ups.Packages.Clear();
            }

            // If there are no packages load them now
            if (ups.Packages.Count == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(ups.Packages, new RelationPredicateBucket(UpsPackageFields.ShipmentID == shipment.ShipmentID));
                    ups.Packages.Sort((int)UpsPackageFieldIndex.UpsPackageID, ListSortDirection.Ascending);
                }

                // We reloaded the packages, so reset the tracker
                ups.Packages.RemovedEntitiesTracker = new UpsPackageCollection();
            }

            // There has to be at least one package.  Really the only way there would not already be a package is if this is a new shipment,
            // and the default profile set included no package stuff.
            if (ups.Packages.Count == 0)
            {
                // This was changed to an exception instead of creating the package when the creation was moved to ConfigureNewShipment
                throw new NotFoundException("Primary package not found.");
            }
        }

        /// <summary>
        /// Configure data for the newly created shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            // A null reference error was being thrown.  Discoverred by Crash Reports.
            // Let's figure out what is null....
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (shipment.Ups == null)
            {
                throw new NullReferenceException("shipment.Ups cannot be null.");
            }

            if (shipment.Order == null)
            {
                throw new NullReferenceException("shipment.Order cannot be null.");
            }

            if (shipment.Ups.Packages == null)
            {
                throw new NullReferenceException("shipment.Ups.Packages cannot be null.");
            }

            shipment.Ups.CodEnabled = false;
            shipment.Ups.CodAmount = shipment.Order.OrderTotal;
            shipment.Ups.CodPaymentType = (int)UpsCodPaymentType.Cash;

            shipment.Ups.CustomsDocumentsOnly = false;
            shipment.Ups.CustomsDescription = "Goods";

            shipment.Ups.CommercialPaperlessInvoice = false;
            shipment.Ups.CommercialInvoiceTermsOfSale = (int)UpsTermsOfSale.NotSpecified;
            shipment.Ups.CommercialInvoicePurpose = (int)UpsExportReason.Sale;
            shipment.Ups.CommercialInvoiceComments = "";
            shipment.Ups.CommercialInvoiceFreight = 0;
            shipment.Ups.CommercialInvoiceInsurance = 0;
            shipment.Ups.CommercialInvoiceOther = 0;

            shipment.Ups.WorldShipStatus = (int)WorldShipStatusType.None;

            shipment.Ups.NegotiatedRate = false;
            shipment.Ups.PublishedCharges = 0;

            shipment.Ups.UspsTrackingNumber = "";

            shipment.Ups.Endorsement = 0;
            shipment.Ups.Subclassification = 0;

            shipment.Ups.PaperlessAdditionalDocumentation = false;
            shipment.Ups.ShipperRelease = false;
            shipment.Ups.CarbonNeutral = false;

            shipment.Ups.UspsPackageID = string.Empty;
            shipment.Ups.CostCenter = string.Empty;
            shipment.Ups.IrregularIndicator = (int)UpsIrregularIndicatorType.NotApplicable;
            shipment.Ups.Cn22Number = string.Empty;

            shipment.Ups.ShipmentChargeAccount = string.Empty;
            shipment.Ups.ShipmentChargeCountryCode = string.Empty;
            shipment.Ups.ShipmentChargePostalCode = string.Empty;
            shipment.Ups.ShipmentChargeType = (int) UpsShipmentChargeType.BillReceiver;

            UpsPackageEntity package = UpsUtility.CreateDefaultPackage();
            shipment.Ups.Packages.Add(package);

            // Weight of the first package equals the total shipment content weight
            package.Weight = shipment.ContentWeight;

            shipment.Ups.RequestedLabelFormat = (int)ThermalLanguage.None;

            base.ConfigureNewShipment(shipment);
        }


        /// <summary>
        /// Configures the shipment for ShipSense. This is useful for carriers that support
        /// multiple package shipments, allowing the shipment type a chance to add new packages
        /// to coincide with the ShipSense knowledge base entry.
        /// </summary>
        /// <param name="knowledgebaseEntry">The knowledge base entry.</param>
        /// <param name="shipment">The shipment.</param>
        protected override void SyncNewShipmentWithShipSense(ShipSense.KnowledgebaseEntry knowledgebaseEntry, ShipmentEntity shipment)
        {
            if (shipment.Ups.Packages.RemovedEntitiesTracker == null)
            {
                shipment.Ups.Packages.RemovedEntitiesTracker = new UpsPackageCollection();
            }

            base.SyncNewShipmentWithShipSense(knowledgebaseEntry, shipment);

            while (shipment.Ups.Packages.Count < knowledgebaseEntry.Packages.Count())
            {
                UpsPackageEntity package = UpsUtility.CreateDefaultPackage();
                shipment.Ups.Packages.Add(package);
            }

            while (shipment.Ups.Packages.Count > knowledgebaseEntry.Packages.Count())
            {
                // Remove the last package until the packages counts match
                shipment.Ups.Packages.RemoveAt(shipment.Ups.Packages.Count - 1);
            }
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            bool existed = profile.Ups != null;

            // Load the profile data
            ShipmentTypeDataService.LoadProfileData(profile, "Ups", typeof(UpsProfileEntity), refreshIfPresent);

            UpsProfileEntity ups = profile.Ups;

            // If this is the first time loading it, or we are supposed to refresh, do it now
            if (!existed || refreshIfPresent)
            {
                ups.Packages.Clear();

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(ups.Packages, new RelationPredicateBucket(UpsProfilePackageFields.ShippingProfileID == profile.ShippingProfileID));
                    ups.Packages.Sort((int)UpsProfilePackageFieldIndex.UpsProfilePackageID, ListSortDirection.Ascending);
                }
            }
        }

        /// <summary>
        /// Save UPS specific profile data
        /// </summary>
        public override bool SaveProfileData(ShippingProfileEntity profile, SqlAdapter adapter)
        {
            bool changes = base.SaveProfileData(profile, adapter);

            // First delete out anything that needs deleted
            foreach (UpsProfilePackageEntity package in profile.Ups.Packages.ToList())
            {
                // If its new but deleted, just get rid of it
                if (package.Fields.State == EntityState.Deleted)
                {
                    if (package.IsNew)
                    {
                        profile.Ups.Packages.Remove(package);
                    }

                        // If its deleted, delete it
                    else
                    {
                        package.Fields.State = EntityState.Fetched;
                        profile.Ups.Packages.Remove(package);

                        adapter.DeleteEntity(package);

                        changes = true;
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Gets the service types that have been available for this shipment type (i.e have not 
        /// been excluded). The integer values are intended to correspond to the appropriate 
        /// enumeration values of the specific shipment type (i.e. the integer values would 
        /// correspond to PostalServiceType values for a UspsShipmentType)
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            IEnumerable<int> allServiceTypes = Enum.GetValues(typeof(UpsServiceType)).Cast<int>();
            return allServiceTypes.Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Gets the package types that have been available for this shipment type (i.e have not 
        /// been excluded). The integer values are intended to correspond to the appropriate 
        /// enumeration values of the specific shipment type (i.e. the integer values would 
        /// correspond to UpsPackagingType values for a UpsShipmentType)
        /// </summary>
        public override IEnumerable<int> GetAvailablePackageTypes(IExcludedPackageTypeRepository repository)
        {
            IEnumerable<int> allPackageTypes = Enum.GetValues(typeof(UpsPackagingType)).Cast<int>();
            return allPackageTypes.Except(GetExcludedPackageTypes(repository));
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            long shipperID = UpsAccountManager.Accounts.Count > 0 ? UpsAccountManager.Accounts[0].UpsAccountID : (long)0;

            profile.Ups.UpsAccountID = shipperID;
            profile.OriginID = (int)ShipmentOriginSource.Account;
            profile.Ups.ResidentialDetermination = (int)ResidentialDeterminationType.FromAddressValidation;

            profile.Ups.DeliveryConfirmation = (int)UpsDeliveryConfirmationType.None;
            profile.Ups.ReferenceNumber = "Order {//Order/Number}";
            profile.Ups.ReferenceNumber2 = "";

            profile.Ups.Service = (int)UpsServiceType.UpsGround;
            profile.Ups.SaturdayDelivery = false;

            profile.Ups.PayorType = (int)UpsPayorType.Sender;
            profile.Ups.PayorAccount = "";
            profile.Ups.PayorPostalCode = "";
            profile.Ups.PayorCountryCode = UpsAccountManager.Accounts.Count > 0 ? UpsAccountManager.Accounts[0].CountryCode : "US";

            profile.Ups.EmailNotifySender = (int)UpsEmailNotificationType.None;
            profile.Ups.EmailNotifyRecipient = (int)UpsEmailNotificationType.None;
            profile.Ups.EmailNotifyOther = (int)UpsEmailNotificationType.None;
            profile.Ups.EmailNotifyOtherAddress = "";
            profile.Ups.EmailNotifyFrom = "";
            profile.Ups.EmailNotifySubject = (int)UpsEmailNotificationSubject.TrackingNumber;
            profile.Ups.EmailNotifyMessage = "";

            profile.Ups.ReturnService = (int)UpsReturnServiceType.ElectronicReturnLabel;
            profile.Ups.ReturnContents = "";
            profile.Ups.ReturnUndeliverableEmail = UpsAccountManager.Accounts.Count > 0 ? UpsAccountManager.Accounts[0].Email : "";

            profile.Ups.Endorsement = 0;
            profile.Ups.Subclassification = 0;

            profile.Ups.ShipperRelease = false;
            profile.Ups.CommercialPaperlessInvoice = false;
            profile.Ups.PaperlessAdditionalDocumentation = false;
            profile.Ups.CarbonNeutral = false;

            profile.Ups.UspsPackageID = string.Empty;
            profile.Ups.CostCenter = string.Empty;
            profile.Ups.IrregularIndicator = (int)UpsIrregularIndicatorType.NotApplicable;
            profile.Ups.Cn22Number = string.Empty;

            profile.Ups.ShipmentChargeAccount = string.Empty;
            profile.Ups.ShipmentChargeCountryCode = string.Empty;
            profile.Ups.ShipmentChargePostalCode = string.Empty;
            profile.Ups.ShipmentChargeType = (int)UpsShipmentChargeType.BillReceiver;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            UpsShipmentEntity ups = shipment.Ups;
            UpsProfileEntity source = profile.Ups;

            bool changedPackageWeights = false;

            // Apply all package profiles
            for (int i = 0; i < profile.Ups.Packages.Count; i++)
            {
                // Get the profile to apply
                UpsProfilePackageEntity packageProfile = profile.Ups.Packages[i];

                UpsPackageEntity package;

                // Get the existing, or create a new package
                if (ups.Packages.Count > i)
                {
                    package = ups.Packages[i];
                }
                else
                {
                    package = UpsUtility.CreateDefaultPackage();
                    ups.Packages.Add(package);

                    if (ups.Packages.Count == 1)
                    {
                        // Weight of the first package equals the total shipment content weight
                        package.Weight = shipment.ContentWeight;
                        changedPackageWeights = true;

                        package.InsuranceValue = InsuranceUtility.GetInsuranceValue(shipment);
                        package.DeclaredValue = 0;
                    }
                }

                ShippingProfileUtility.ApplyProfileValue(packageProfile.PackagingType, package, UpsPackageFields.PackagingType);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, package, UpsPackageFields.Weight);
                changedPackageWeights |= (packageProfile.Weight != null);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, package, UpsPackageFields.DimsProfileID);
                if (packageProfile.DimsProfileID != null)
                {
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, package, UpsPackageFields.DimsLength);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, package, UpsPackageFields.DimsWidth);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, package, UpsPackageFields.DimsHeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, package, UpsPackageFields.DimsWeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, package, UpsPackageFields.DimsAddWeight);
                }

                ShippingProfileUtility.ApplyProfileValue(packageProfile.AdditionalHandlingEnabled, package, UpsPackageFields.AdditionalHandlingEnabled);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.DryIceEnabled, package, UpsPackageFields.DryIceEnabled);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DryIceIsForMedicalUse, package, UpsPackageFields.DryIceIsForMedicalUse);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DryIceRegulationSet, package, UpsPackageFields.DryIceRegulationSet);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DryIceWeight, package, UpsPackageFields.DryIceWeight);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.VerbalConfirmationEnabled, package, UpsPackageFields.VerbalConfirmationEnabled);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.VerbalConfirmationName, package, UpsPackageFields.VerbalConfirmationName);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.VerbalConfirmationPhone, package, UpsPackageFields.VerbalConfirmationPhone);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.VerbalConfirmationPhoneExtension, package, UpsPackageFields.VerbalConfirmationPhoneExtension);
            }

            // Remove any packages that are too many for the profile
            if (profile.Ups.Packages.Count > 0)
            {
                // Go through each package that needs removed
                foreach (UpsPackageEntity package in ups.Packages.Skip(profile.Ups.Packages.Count).ToList())
                {
                    if (package.Weight != 0)
                    {
                        changedPackageWeights = true;
                    }

                    // Remove it from the list
                    ups.Packages.Remove(package);

                    // If its saved in the database, we have to delete it
                    if (!package.IsNew)
                    {
                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            adapter.DeleteEntity(package);
                        }
                    }
                }
            }

            base.ApplyProfile(shipment, profile);

            long? accountID = (source.UpsAccountID == 0 && UpsAccountManager.Accounts.Count > 0) ?
                                  (long?)UpsAccountManager.Accounts[0].UpsAccountID :
                                  source.UpsAccountID;

            ShippingProfileUtility.ApplyProfileValue(accountID, ups, UpsShipmentFields.UpsAccountID);
            ShippingProfileUtility.ApplyProfileValue(source.ResidentialDetermination, shipment, ShipmentFields.ResidentialDetermination);

            ShippingProfileUtility.ApplyProfileValue(source.DeliveryConfirmation, ups, UpsShipmentFields.DeliveryConfirmation);
            ShippingProfileUtility.ApplyProfileValue(source.ReferenceNumber, ups, UpsShipmentFields.ReferenceNumber);
            ShippingProfileUtility.ApplyProfileValue(source.ReferenceNumber2, ups, UpsShipmentFields.ReferenceNumber2);

            ShippingProfileUtility.ApplyProfileValue(source.Service, ups, UpsShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(source.SaturdayDelivery, ups, UpsShipmentFields.SaturdayDelivery);

            ShippingProfileUtility.ApplyProfileValue(source.PayorType, ups, UpsShipmentFields.PayorType);
            ShippingProfileUtility.ApplyProfileValue(source.PayorAccount, ups, UpsShipmentFields.PayorAccount);
            ShippingProfileUtility.ApplyProfileValue(source.PayorPostalCode, ups, UpsShipmentFields.PayorPostalCode);
            ShippingProfileUtility.ApplyProfileValue(source.PayorCountryCode, ups, UpsShipmentFields.PayorCountryCode);

            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifySender, ups, UpsShipmentFields.EmailNotifySender);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyRecipient, ups, UpsShipmentFields.EmailNotifyRecipient);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyOther, ups, UpsShipmentFields.EmailNotifyOther);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyOtherAddress, ups, UpsShipmentFields.EmailNotifyOtherAddress);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyFrom, ups, UpsShipmentFields.EmailNotifyFrom);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifySubject, ups, UpsShipmentFields.EmailNotifySubject);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyMessage, ups, UpsShipmentFields.EmailNotifyMessage);

            ShippingProfileUtility.ApplyProfileValue(source.ReturnService, ups, UpsShipmentFields.ReturnService);
            ShippingProfileUtility.ApplyProfileValue(source.ReturnContents, ups, UpsShipmentFields.ReturnContents);
            ShippingProfileUtility.ApplyProfileValue(source.ReturnUndeliverableEmail, ups, UpsShipmentFields.ReturnUndeliverableEmail);

            ShippingProfileUtility.ApplyProfileValue(source.Subclassification, ups, UpsShipmentFields.Subclassification);
            ShippingProfileUtility.ApplyProfileValue(source.Endorsement, ups, UpsShipmentFields.Endorsement);

            ShippingProfileUtility.ApplyProfileValue(source.PaperlessAdditionalDocumentation, ups, UpsShipmentFields.PaperlessAdditionalDocumentation);
            ShippingProfileUtility.ApplyProfileValue(source.CommercialPaperlessInvoice, ups, UpsShipmentFields.CommercialPaperlessInvoice);
            ShippingProfileUtility.ApplyProfileValue(source.ShipperRelease, ups, UpsShipmentFields.ShipperRelease);
            ShippingProfileUtility.ApplyProfileValue(source.CarbonNeutral, ups, UpsShipmentFields.CarbonNeutral);

            ShippingProfileUtility.ApplyProfileValue(source.UspsPackageID, ups, UpsShipmentFields.UspsPackageID);
            ShippingProfileUtility.ApplyProfileValue(source.CostCenter, ups, UpsShipmentFields.CostCenter);
            ShippingProfileUtility.ApplyProfileValue(source.IrregularIndicator, ups, UpsShipmentFields.IrregularIndicator);
            ShippingProfileUtility.ApplyProfileValue(source.Cn22Number, ups, UpsShipmentFields.Cn22Number);

            ShippingProfileUtility.ApplyProfileValue(source.ShipmentChargeType, ups, UpsShipmentFields.ShipmentChargeType);
            ShippingProfileUtility.ApplyProfileValue(source.ShipmentChargePostalCode, ups, UpsShipmentFields.ShipmentChargePostalCode);
            ShippingProfileUtility.ApplyProfileValue(source.ShipmentChargeCountryCode, ups, UpsShipmentFields.ShipmentChargeCountryCode);
            ShippingProfileUtility.ApplyProfileValue(source.ShipmentChargeAccount, ups, UpsShipmentFields.ShipmentChargeAccount);

            if (changedPackageWeights)
            {
                UpdateTotalWeight(shipment);
            }

            UpdateDynamicShipmentData(shipment);
        }

        /// <summary>
        /// Update the origin address based on the given originID value.  If the shipment has already been processed, nothing is done.  If
        /// the originID is no longer valid and the address could not be updated, false is returned.
        /// </summary>
        public override bool UpdatePersonAddress(ShipmentEntity shipment, PersonAdapter person, long originID)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (shipment.Processed)
            {
                return true;
            }

            // The Ups object may not yet be set if we are in the middle of creating a new shipment
            if (originID == (int)ShipmentOriginSource.Account && shipment.Ups != null)
            {
                UpsAccountEntity account = UpsAccountManager.GetAccount(shipment.Ups.UpsAccountID);
                if (account == null)
                {
                    account = UpsAccountManager.Accounts.FirstOrDefault();
                }

                if (account != null)
                {
                    PersonAdapter.Copy(account, "", person);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return base.UpdatePersonAddress(shipment, person, originID);
        }

        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            UpsServiceType serviceType = (UpsServiceType)shipment.Ups.Service;

            // Need to check with the store  to see if anything about the shipment was overridden in case
            // it may have effected the shipping services available (i.e. the eBay GSP program)            
            ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);

            var upsServiceManagerFactory = new UpsServiceManagerFactory(overriddenShipment);
            IUpsServiceManager carrierServiceManager = upsServiceManagerFactory.Create(overriddenShipment);
            List<UpsServiceType> serviceTypes = carrierServiceManager.GetServices(overriddenShipment).Select(s => s.UpsServiceType).ToList();

            // Default to a valid service type if the current one is invalid
            if (!serviceTypes.Contains(serviceType))
            {
                shipment.Ups.Service = (int)(UpsServiceType)serviceTypes.First();
            }

            RedistributeContentWeight(shipment);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            // Consider the shipment insured of any package is insured
            shipment.Insurance = shipment.Ups.Packages.Any(p => p.Insurance);

            // Set the provider type based on UPS settings
            shipment.InsuranceProvider = settings.UpsInsuranceProvider;

            // Right now ShipWorks Insurance (due to Tango limitation) doesn't support multi-package - so in that case just auto-revert to carrier insurance
            if (shipment.Ups.Packages.Count > 1)
            {
                shipment.InsuranceProvider = (int)InsuranceProvider.Carrier;
            }

            shipment.RequestedLabelFormat = shipment.Ups.RequestedLabelFormat;

            // Check the UPS wide PennyOne settings and get them updated
            foreach (var package in shipment.Ups.Packages)
            {
                package.InsurancePennyOne = settings.UpsInsurancePennyOne;

                // For WorldShip... if using a MailInnovations service class, we force Penny One since UPS does not provide the first $100 for that
                if (shipment.ShipmentType == (int)ShipmentTypeCode.UpsWorldShip)
                {
                    if (UpsUtility.IsUpsMiOrSurePostService((UpsServiceType)shipment.Ups.Service))
                    {
                        package.InsurancePennyOne = true;
                    }
                }

                // The only time we send the full insuredvalue as declared value is if insurance is enabled, and they are using carrier insurance.
                if (shipment.Insurance && shipment.InsuranceProvider == (int)InsuranceProvider.Carrier)
                {
                    package.DeclaredValue = package.InsuranceValue;
                }
                else if (UpsUtility.IsUpsSurePostService((UpsServiceType)shipment.Ups.Service))
                {
                    // If Surepost, don't send any declared value.
                    package.DeclaredValue = 0;
                }
                else
                {
                    // Otherwise, regardless of if they are insuring or not, penny one or not, etc., we just send up to the first $100 since it's free anyway
                    package.DeclaredValue = Math.Min(100, package.InsuranceValue);
                }
            }
        }

        /// <summary>
        /// Redistribute the ContentWeight from the shipment to each package in the shipment.  This only does something
        /// if the ContentWeight is different from the total Content.  Returns true if weight had to be redistributed.
        /// </summary>
        public static bool RedistributeContentWeight(ShipmentEntity shipment)
        {
            // Determine what our content weight should be
            double contentWeight = shipment.Ups.Packages.Sum(p => p.Weight);

            // If the content weight changed outside of us, redistribute what the new weight among the packages
            if (contentWeight != shipment.ContentWeight)
            {
                foreach (UpsPackageEntity package in shipment.Ups.Packages)
                {
                    package.Weight = shipment.ContentWeight/shipment.Ups.Packages.Count;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Update the total weight of the shipment based on the individual package weights
        /// </summary>
        public override void UpdateTotalWeight(ShipmentEntity shipment)
        {
            double contentWeight = 0;
            double totalWeight = 0;

            foreach (UpsPackageEntity package in shipment.Ups.Packages)
            {
                contentWeight += package.Weight;
                totalWeight += package.Weight;

                if (package.DimsAddWeight)
                {
                    totalWeight += package.DimsWeight;
                }
            }

            shipment.ContentWeight = contentWeight;
            shipment.TotalWeight = totalWeight;
        }

        /// <summary>
        /// Get a user-friendly description of the UPS service used by the shipment
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return EnumHelper.GetDescription((UpsServiceType)shipment.Ups.Service);
        }

        /// <summary>
        /// Get the ups shipment detail
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            UpsShipmentEntity ups = shipment.Ups;
            UpsAccountEntity account = UpsAccountManager.GetAccount(ups.UpsAccountID);

            commonDetail.OriginAccount = (account == null) ? "" : account.AccountNumber;
            commonDetail.ServiceType = ups.Service;

            commonDetail.PackagingType = ups.Packages[0].PackagingType;
            commonDetail.PackageLength = ups.Packages[0].DimsLength;
            commonDetail.PackageWidth = ups.Packages[0].DimsWidth;
            commonDetail.PackageHeight = ups.Packages[0].DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Get the total packages contained by the shipment
        /// </summary>
        public override int GetParcelCount(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return shipment.Ups.Packages.Count;
        }

        /// <summary>
        /// Get the parcel data for the shipment
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (parcelIndex >= 0 && parcelIndex < shipment.Ups.Packages.Count)
            {
                var package = shipment.Ups.Packages[parcelIndex];
               
                return new ShipmentParcel(shipment, package.UpsPackageID, package.TrackingNumber,
                    new InsuranceChoice(shipment, package, package, package),
                    new DimensionsAdapter(package))
                {
                    TotalWeight =  package.Weight + package.DimsWeight
                };
            }

            throw new ArgumentException(string.Format("'{0}' is out of range for the shipment.", parcelIndex), "parcelIndex");
        }

        /// <summary>
        /// Get the tracking numbers for the shipment
        /// </summary>
        public override List<string> GetTrackingNumbers(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                return new List<string>();
            }

            UpsShipmentEntity ups = shipment.Ups;

            if (ups.Packages.Count == 1)
            {
                return base.GetTrackingNumbers(shipment);
            }
            else
            {
                List<string> trackingList = new List<string>();

                for (int i = 0; i < ups.Packages.Count; i++)
                {
                    string trackingNumber = ups.Packages[i].TrackingNumber;
                    trackingNumber = string.IsNullOrWhiteSpace(trackingNumber) ? ups.Packages[i].UspsTrackingNumber : trackingNumber;

                    trackingList.Add(string.Format("Package {0}: {1}", i + 1, trackingNumber));
                }

                return trackingList;
            }
        }

        /// <summary>
        /// Get the UPS rates for the given shipment
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            ICarrierSettingsRepository originalSettings = SettingsRepository;
            ICertificateInspector originalInspector = CertificateInspector;
            ICarrierAccountRepository<UpsAccountEntity> originalAccountRepository = AccountRepository;

            try
            {
                // Check with the SettingsRepository here rather than UpsAccountManager, so getting 
                // counter rates from the broker is not impacted
                if (!SettingsRepository.GetAccounts().Any() && !IsShipmentTypeRestricted)
                {
                    CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);

                    // We need to swap out the SettingsRepository and certificate inspector 
                    // to get UPS counter rates
                    SettingsRepository = new UpsCounterRateSettingsRepository(TangoCredentialStore.Instance);
                    CertificateInspector = new CertificateInspector(TangoCredentialStore.Instance.UpsCertificateVerificationData);
                    AccountRepository = new UpsCounterRateAccountRepository(TangoCredentialStore.Instance);
                }

                return GetCachedRates<UpsException>(shipment, GetRatesFromApi);
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(this));
                return errorRates;
            }
            finally
            {
                // Switch the settings repository back to the original now that we have counter rates
                SettingsRepository = originalSettings;
                CertificateInspector = originalInspector;
                AccountRepository = originalAccountRepository;
            }
        }

        /// <summary>
        /// Checks each packages dimensions, making sure that each is valid.  If one or more packages have invalid dimensions, 
        /// a ShippingException is thrown informing the user.
        /// </summary>
        private void ValidatePackageDimensions(ShipmentEntity shipment)
        {
            string exceptionMessage = string.Empty;
            int packageIndex = 1;

            foreach (UpsPackageEntity upsPackage in shipment.Ups.Packages)
            {
                if (upsPackage.PackagingType == (int) UpsPackagingType.Custom)
                {
                    if (!DimensionsAreValid(upsPackage.DimsLength, upsPackage.DimsWidth, upsPackage.DimsHeight))
                    {
                        exceptionMessage += string.Format("Package {0} has invalid dimensions.{1}", packageIndex, Environment.NewLine);
                    }
                }

                packageIndex++;
            }

            if (exceptionMessage.Length > 0)
            {
                exceptionMessage += "Package dimensions must be greater than 0 and not 1x1x1.  ";
                throw new InvalidPackageDimensionsException(exceptionMessage);
            }
        }

        /// <summary>
        /// Get the UPS rates from the UPS api
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            try
            {
                // Validate each of the packages dimensions.
                ValidatePackageDimensions(shipment);

                List<RateResult> rates = new List<RateResult>();

                // Get the transit times and services
                List<UpsTransitTime> transitTimes = UpsApiTransitTimeClient.GetTransitTimes(shipment, AccountRepository, SettingsRepository, CertificateInspector);

                UpsApiRateClient upsApiRateClient = new UpsApiRateClient(AccountRepository, SettingsRepository, CertificateInspector);
                List<UpsServiceRate> serviceRates = upsApiRateClient.GetRates(shipment);

                if (!serviceRates.Any())
                {
                    rates.Add(new RateResult("* No rates were returned for the selected Service.", ""));
                }
                else
                {
                    // Determine if the user is hoping to get negotiated rates back
                    bool wantedNegotiated = UpsApiCore.GetUpsAccount(shipment, AccountRepository).RateType == (int)UpsRateType.Negotiated;

                    // Indicates if any of the rates returned were negotiated.
                    bool anyNegotiated = serviceRates.Any(s => s.Negotiated);
                    bool allNegotiated = serviceRates.All(s => s.Negotiated);

                    // Add a rate for each service
                    foreach (UpsServiceRate serviceRate in serviceRates)
                    {
                        UpsServiceType service = serviceRate.Service;

                        UpsTransitTime transitTime = transitTimes.SingleOrDefault(t => t.Service == service);

                        RateResult rateResult = new RateResult(
                            (serviceRate.Negotiated && !allNegotiated ? "* " : "") + EnumHelper.GetDescription(service),
                            GetServiceTransitDays(transitTime) + " " + GetServiceEstimatedArrivalTime(transitTime),
                            serviceRate.Amount,
                            service)
                        {
                            ServiceLevel = UpsServiceLevelConverter.GetServiceLevel(serviceRate, transitTime),
                            ExpectedDeliveryDate = transitTime == null ? ShippingManager.CalculateExpectedDeliveryDate(serviceRate.GuaranteedDaysToDelivery, DayOfWeek.Saturday, DayOfWeek.Sunday) : transitTime.ArrivalDate,
                            ShipmentType = ShipmentTypeCode.UpsOnLineTools,
                            ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.UpsOnLineTools)
                        };

                        rates.Add(rateResult);
                    }

                    // If they wanted negotiated rates, we have to show some results
                    if (wantedNegotiated)
                    {
                        if (allNegotiated)
                        {
                            rates.Add(new RateResult("* All rates are negotiated rates.", ""));
                        }
                        else if (anyNegotiated)
                        {
                            rates.Add(new RateResult("* Indicates a negotiated rate.", ""));
                        }
                        else
                        {
                            rates.Add(new RateResult("* Negotiated rates were not returned. Contact Interapptive.", ""));
                        }
                    }

                    if (shipment.ReturnShipment)
                    {
                        rates.Add(new RateResult("* Rates reflect the service charge only. This does not include additional fees for returns.", ""));
                    }
                }
                
                // Filter out any excluded services, but always include the service that the shipment is configured with
                List<RateResult> finalRatesFilteredByAvailableServices = FilterRatesByExcludedServices(shipment, rates);

                RateGroup finalGroup = new RateGroup(finalRatesFilteredByAvailableServices);

                return finalGroup;
            }
            catch (InvalidPackageDimensionsException ex)
            {
                throw new UpsException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the filtered rates based on any excluded services configured for this ups shipment type.
        /// </summary>
        private List<RateResult> FilterRatesByExcludedServices(ShipmentEntity shipment, List<RateResult> rates)
        {
            List<UpsServiceType> availableServices = ShipmentTypeManager.GetType(ShipmentTypeCode).GetAvailableServiceTypes()
                .Select(s => (UpsServiceType)s).Union(new List<UpsServiceType> { (UpsServiceType)shipment.Ups.Service }).ToList();

            return rates.Where(r => !(r.Tag is UpsServiceType) || availableServices.Contains(((UpsServiceType)r.Tag))).ToList();
        }

        /// <summary>
        /// Get the number of days of transit it takes for the given service.  The transit time can be looked up in the given list.  If not present, then 
        /// empty string is returned.
        /// </summary>
        private string GetServiceTransitDays(UpsTransitTime transitTime)
        {
            if (transitTime != null)
            {
                return transitTime.BusinessDays.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the service estimated arrival time for the given service type if it's available.
        /// </summary>
        /// <param name="transitTime">The transit time.</param>
        /// <returns>A string value of the arrival time in the format of "DayOfWeek h:mm tt" (e.g. Friday 4:00 PM)</returns>
        private static string GetServiceEstimatedArrivalTime(UpsTransitTime transitTime)
        {
            string arrivalInfo = string.Empty;

            if (transitTime != null)
            {
                DateTime localArrival = transitTime.ArrivalDate.ToLocalTime();
                arrivalInfo = ShippingManager.GetArrivalDescription(localArrival);
            }

            return arrivalInfo;
        }

        /// <summary>
        /// Provide UPS tracking results for the given shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            try
            {
                if (!InterapptiveOnly.Registry.GetValue("UpsTrackingAgreement", false))
                {
                    using (UpsTrackingAgreementDlg dlg = new UpsTrackingAgreementDlg())
                    {
                        if (dlg.ShowDialog(DisplayHelper.GetActiveForm()) != DialogResult.OK)
                        {
                            throw new ShippingException("You must agree to the UPS Developer Kit Tracking Agreement.");
                        }
                    }

                    InterapptiveOnly.Registry.SetValue("UpsTrackingAgreement", true);
                }

                return UpsApiTrackClient.TrackShipment(shipment);
            }
            catch (UpsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Void the given shipment
        /// </summary>
        public override void VoidShipment(ShipmentEntity shipment)
        {
            try
            {
                if (!UpsUtility.IsUpsMiService((UpsServiceType)shipment.Ups.Service))
                {
                    UpsApiVoidClient.VoidShipment(shipment);
                }
            }
            catch (UpsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        protected override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            return new UpsShipmentProcessingSynchronizer();
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            UpsShipmentEntity upsShipmentEntity = shipment.Ups;
            UpsServiceType upsServiceType = (UpsServiceType)upsShipmentEntity.Service;

            if (UpsUtility.IsUpsSurePostService(upsServiceType) &&
                (shipment.InsuranceProvider == (int)InsuranceProvider.Carrier) &&
                upsShipmentEntity.Packages.Any(p => p.Insurance && p.InsuranceValue > 0))
            {
                throw new CarrierException("UPS declared value is not supported for SurePost shipments. For insurance coverage, go to Shipping Settings and enable ShipWorks Insurance for this carrier.");
            }

            // Make sure package dimensions are valid.
            ValidatePackageDimensions(shipment);

            // Clear out any values that aren't allowed for SurePost or MI
            if (UpsUtility.IsUpsMiOrSurePostService(upsServiceType))
            {
                shipment.ReturnShipment = false;
                upsShipmentEntity.ReturnContents = string.Empty;
                upsShipmentEntity.ReturnService = (int)UpsReturnServiceType.ElectronicReturnLabel;
                upsShipmentEntity.ReturnUndeliverableEmail = string.Empty;

                upsShipmentEntity.CodEnabled = false;
                upsShipmentEntity.CodAmount = 0;
                upsShipmentEntity.CodPaymentType = (int)UpsCodPaymentType.Cash;

                upsShipmentEntity.ShipperRelease = false;

                UpsPackageEntity upsPackageEntity = upsShipmentEntity.Packages.FirstOrDefault();
                upsPackageEntity.AdditionalHandlingEnabled = false;
                upsPackageEntity.DryIceEnabled = false;
                upsPackageEntity.DryIceIsForMedicalUse = false;
                upsPackageEntity.DryIceRegulationSet = (int)UpsDryIceRegulationSet.Cfr;
                upsPackageEntity.DryIceWeight = 0;
                upsPackageEntity.VerbalConfirmationEnabled = false;
                upsPackageEntity.VerbalConfirmationName = string.Empty;
                upsPackageEntity.VerbalConfirmationPhone = string.Empty;
                upsPackageEntity.VerbalConfirmationPhoneExtension = string.Empty;

                // Clear out any specific to MI
                if (UpsUtility.IsUpsMiService(upsServiceType))
                {
                    upsShipmentEntity.CarbonNeutral = false;
                    upsShipmentEntity.ReferenceNumber = string.Empty;
                    upsShipmentEntity.ReferenceNumber2 = string.Empty;
                }

                // Clear out any specific to SurePost
                if (UpsUtility.IsUpsSurePostService(upsServiceType))
                {
                    upsShipmentEntity.DeliveryConfirmation = (int)UpsDeliveryConfirmationType.None;
                    upsShipmentEntity.PayorAccount = string.Empty;
                    upsShipmentEntity.PayorCountryCode = string.Empty;
                    upsShipmentEntity.PayorPostalCode = string.Empty;
                    upsShipmentEntity.PayorType = (int)UpsPayorType.Sender;
                }
            }
        }

        /// <summary>
        /// Determines whether [is mail innovations enabled] for OLT.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsMailInnovationsEnabled()
        {
            return ShippingSettings.Fetch().UpsMailInnovationsEnabled;
        }

        /// <summary>
        /// Determines if a shipment will be domestic or international
        /// </summary>
        public override bool IsDomestic(ShipmentEntity shipmentEntity)
        {
            if (shipmentEntity == null)
            {
                throw new ArgumentNullException("shipmentEntity");
            }

            string originCountryCode = shipmentEntity.AdjustedOriginCountryCode();
            string destinationCountryCode = shipmentEntity.AdjustedShipCountryCode();

            return string.Equals(originCountryCode, destinationCountryCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the UPS shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an UpsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return UpsAccountManager.Accounts.Any() ? new UpsBestRateBroker() : new UpsCounterRatesBroker();
        }

        /// <summary>
        /// Clear any data that should not be part of a shipment after it has been copied.
        /// </summary>
        public override void ClearDataForCopiedShipment(ShipmentEntity shipment)
        {
            if (shipment.Ups != null && shipment.Ups.Packages != null)
            {
                shipment.Ups.UspsTrackingNumber = String.Empty;
                shipment.Ups.Cn22Number = String.Empty;
                foreach (UpsPackageEntity package in shipment.Ups.Packages)
                {
                    package.TrackingNumber = String.Empty;
                    package.UspsTrackingNumber = String.Empty;
                    EntityUtility.MarkAsNew(package);
                }
            }
        }

        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        public override RatingFields RatingFields
        {
            get
            {
                if (ratingField != null)
                {
                    return ratingField;
                }

                ratingField = base.RatingFields;
                ratingField.ShipmentFields.Add(UpsShipmentFields.UpsAccountID);
                ratingField.ShipmentFields.Add(UpsShipmentFields.SaturdayDelivery);
                ratingField.ShipmentFields.Add(UpsShipmentFields.SaturdayDelivery);
                ratingField.ShipmentFields.Add(UpsShipmentFields.CodAmount);
                ratingField.ShipmentFields.Add(UpsShipmentFields.CodEnabled);
                ratingField.ShipmentFields.Add(UpsShipmentFields.CodPaymentType);
                ratingField.ShipmentFields.Add(UpsShipmentFields.Service);
                ratingField.ShipmentFields.Add(UpsShipmentFields.DeliveryConfirmation);
                ratingField.PackageFields.Add(UpsPackageFields.PackagingType);
                ratingField.PackageFields.Add(UpsPackageFields.DeclaredValue);
                ratingField.PackageFields.Add(UpsPackageFields.VerbalConfirmationEnabled);
                ratingField.PackageFields.Add(UpsPackageFields.DimsWeight);
                ratingField.PackageFields.Add(UpsPackageFields.DimsLength);
                ratingField.PackageFields.Add(UpsPackageFields.DimsHeight);
                ratingField.PackageFields.Add(UpsPackageFields.DimsWidth);
                ratingField.PackageFields.Add(UpsPackageFields.DryIceEnabled);
                ratingField.PackageFields.Add(UpsPackageFields.DryIceRegulationSet);
                ratingField.PackageFields.Add(UpsPackageFields.DryIceWeight);
                ratingField.PackageFields.Add(UpsPackageFields.DryIceEnabled);
                ratingField.PackageFields.Add(UpsPackageFields.DryIceIsForMedicalUse);

                return ratingField;
            }
        }

        /// <summary>
        /// Gets the rating hash based on the shipment's configuration.
        /// </summary>
        public override string GetRatingHash(ShipmentEntity shipment)
        {
            return RatingFields.GetRatingHash(shipment, shipment.Ups.Packages);
        }

        /// <summary>
        /// Gets a value indicating whether multiple packages are supported by this shipment type.
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple packages]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsMultiplePackages
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.Ups == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            if (!shipment.Ups.Packages.Any())
            {
                throw new UpsException("There must be at least one package to create the UPS package adapter.");
            }

            // Return an adapter per package
            List<IPackageAdapter> adapters = new List<IPackageAdapter>();
            foreach (UpsPackageEntity packageEntity in shipment.Ups.Packages)
            {
                adapters.Add(new UpsPackageAdapter(shipment, packageEntity));
            }

            return adapters;
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address.
        /// </summary>
        protected override bool IsCustomsRequiredByShipment(ShipmentEntity shipment)
        {
            if (shipment.AdjustedOriginCountryCode() == "PR" && 
                shipment.AdjustedShipCountryCode() == "US")
            {
                return false;
            }

            return base.IsCustomsRequiredByShipment(shipment);
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.Ups != null)
            {
                shipment.Ups.RequestedLabelFormat = (int)requestedLabelFormat;
            }
        }
    }
}
