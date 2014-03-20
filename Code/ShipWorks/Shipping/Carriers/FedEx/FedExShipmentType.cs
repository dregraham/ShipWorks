﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using Interapptive.Shared.Enums;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// FedEx implementation of ShipmentType
    /// </summary>
    public class FedExShipmentType : ShipmentType
    {
        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return ShipmentTypeCode.FedEx;
            }
        }

        /// <summary>
        /// FedEx accounts have an address that can be used as the shipment origin
        /// </summary>
        public override bool SupportsAccountAsOrigin
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// FedEx supports rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns are supported for Online Tools
        /// </summary>
        public override bool SupportsReturns
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Create the setup wizard used for setting up the shipment type
        /// </summary>
        public override Form CreateSetupWizard()
        {
            return new FedExSetupWizard();
        }

        /// <summary>
        /// Create the UserControl used to handle FedEx shipments
        /// </summary>
        public override ServiceControlBase CreateServiceControl()
        {
            return new FedExServiceControl();
        }

        /// <summary>
        /// Create the UserControl used to edit the overall fedex settings
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new FedExSettingsControl();
        }

        /// <summary>
        /// Custom customs settings for FedEx
        /// </summary>
        public override CustomsControlBase CreateCustomsControl()
        {
            return new FedExCustomsControl();
        }

        /// <summary>
        /// Create the UserControl used to edit FedEx profiles.
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new FedExProfileControl();
        }

        /// <summary>
        /// Ensures that the FedEx specific data for the shipment is loaded.  If the data already exists, nothing is done.  It is not refreshed.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "FedEx", typeof(FedExShipmentEntity), refreshIfPresent);

            // This will exist now
            FedExShipmentEntity fedex = shipment.FedEx;

            // If refreshing, start over on the packages
            if (refreshIfPresent)
            {
                fedex.Packages.Clear();
            }

            // If there are no packages load them now
            if (fedex.Packages.Count == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(fedex.Packages, new RelationPredicateBucket(FedExPackageFields.ShipmentID == shipment.ShipmentID));
                    fedex.Packages.Sort((int) FedExPackageFieldIndex.FedExPackageID, ListSortDirection.Ascending);
                }

                // We reloaded the packages, so reset the tracker
                fedex.Packages.RemovedEntitiesTracker = new FedExPackageCollection();
            }

            // There has to be at least one package.  Really the only way there would not already be a package is if this is a new shipment,
            // and the default profile set included no package stuff.
            if (fedex.Packages.Count == 0)
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
            shipment.FedEx.MasterFormID = "";

            shipment.FedEx.HomeDeliveryType = (int) FedExHomeDeliveryType.None;
            shipment.FedEx.HomeDeliveryInstructions = "";
            shipment.FedEx.HomeDeliveryDate = shipment.ShipDate;
            shipment.FedEx.HomeDeliveryPhone = shipment.ShipPhone;

            shipment.FedEx.FreightInsidePickup = false;
            shipment.FedEx.FreightInsideDelivery = false;
            shipment.FedEx.FreightBookingNumber = "";
            shipment.FedEx.FreightLoadAndCount = 0;

            shipment.FedEx.CodEnabled = false;
            shipment.FedEx.CodAmount = shipment.Order.OrderTotal;
            shipment.FedEx.CodPaymentType = (int) FedExCodPaymentType.Any;
            shipment.FedEx.CodAddFreight = true;
            shipment.FedEx.CodOriginID = (int) ShipmentOriginSource.Account;
            shipment.FedEx.CodTrackingFormID = "";
            shipment.FedEx.CodTrackingNumber = "";
            shipment.FedEx.CodTIN = string.Empty;
            shipment.FedEx.CodChargeBasis = (int) FedExCodAddTransportationChargeBasisType.NetCharge; 
            shipment.FedEx.CodCountryCode = string.Empty;
            shipment.FedEx.CodAccountNumber = string.Empty;

            shipment.FedEx.BrokerEnabled = false;
            shipment.FedEx.BrokerAccount = "";
            shipment.FedEx.BrokerFirstName = "";
            shipment.FedEx.BrokerLastName = "";
            shipment.FedEx.BrokerCompany = "";
            shipment.FedEx.BrokerCity = "";
            shipment.FedEx.BrokerStreet1 = "";
            shipment.FedEx.BrokerStreet2 = "";
            shipment.FedEx.BrokerStreet3 = "";
            shipment.FedEx.BrokerStateProvCode = "";
            shipment.FedEx.BrokerPostalCode = "";
            shipment.FedEx.BrokerCountryCode = "US";
            shipment.FedEx.BrokerPhone = "";
            shipment.FedEx.BrokerPhoneExtension = string.Empty;
            shipment.FedEx.BrokerEmail = "";

            shipment.FedEx.PayorTransportName = string.Empty;
            shipment.FedEx.PayorDutiesCountryCode = string.Empty;
            shipment.FedEx.PayorDutiesName = string.Empty;

            shipment.FedEx.CustomsAdmissibilityPackaging = (int) FedExPhysicalPackagingType.Box;
            shipment.FedEx.CustomsDocumentsOnly = false;
            shipment.FedEx.CustomsDocumentsDescription = "General Documents";
            shipment.FedEx.CustomsRecipientTIN = "";
            shipment.FedEx.CustomsAESEEI = string.Empty;
            shipment.FedEx.CustomsRecipientIdentificationType = (int) FedExCustomsRecipientIdentificationType.None;
            shipment.FedEx.CustomsRecipientIdentificationValue = string.Empty;
            shipment.FedEx.CustomsOptionsType = (int) FedExCustomsOptionType.None;
            shipment.FedEx.CustomsOptionsDesription = string.Empty;
            shipment.FedEx.CustomsExportFilingOption = (int) FedExCustomsExportFilingOption.NotRequired;
            shipment.FedEx.CustomsNaftaEnabled = false;
            shipment.FedEx.CustomsNaftaDeterminationCode = (int) FedExNaftaDeterminationCode.ProducerOfCommodity;
            shipment.FedEx.CustomsNaftaNetCostMethod = (int) FedExNaftaNetCostMethod.NotCalculated;
            shipment.FedEx.CustomsNaftaPreferenceType = (int) FedExNaftaPreferenceCriteria.A;
            shipment.FedEx.CustomsNaftaProducerId = string.Empty;

            shipment.FedEx.CommercialInvoice = false;
            shipment.FedEx.CommercialInvoiceTermsOfSale = (int) FedExTermsOfSale.FOB_or_FCA;
            shipment.FedEx.CommercialInvoicePurpose = (int) FedExCommercialInvoicePurpose.Sold;
            shipment.FedEx.CommercialInvoiceComments = "";
            shipment.FedEx.CommercialInvoiceFreight = 0;
            shipment.FedEx.CommercialInvoiceInsurance = 0;
            shipment.FedEx.CommercialInvoiceOther = 0;
            shipment.FedEx.CommercialInvoiceReference = string.Empty;

            shipment.FedEx.ImporterOfRecord = false;
            shipment.FedEx.ImporterTIN = "";
            shipment.FedEx.ImporterAccount = "";
            shipment.FedEx.ImporterFirstName = "";
            shipment.FedEx.ImporterLastName = "";
            shipment.FedEx.ImporterCompany = "";
            shipment.FedEx.ImporterStreet1 = "";
            shipment.FedEx.ImporterStreet2 = "";
            shipment.FedEx.ImporterStreet3 = "";
            shipment.FedEx.ImporterCity = "";
            shipment.FedEx.ImporterStateProvCode = "";
            shipment.FedEx.ImporterPostalCode = "";
            shipment.FedEx.ImporterCountryCode = "US";
            shipment.FedEx.ImporterPhone = "";

            // If we couldn't apply the COD origin address here (then the FedEx account must have been deleted), fallback to blank
            if (!UpdatePersonAddress(shipment, new PersonAdapter(shipment.FedEx, "Cod"), shipment.FedEx.CodOriginID))
            {
                PersonAdapter.ApplyDefaults(shipment.FedEx, "Cod");
            }
            shipment.FedEx.IntlExportDetailType = 0;
            shipment.FedEx.IntlExportDetailForeignTradeZoneCode = string.Empty;
            shipment.FedEx.IntlExportDetailEntryNumber = string.Empty;
            shipment.FedEx.IntlExportDetailLicenseOrPermitNumber = string.Empty;

            shipment.FedEx.FedExHoldAtLocationEnabled = false;
            shipment.FedEx.HoldCity = null;
            shipment.FedEx.HoldCompanyName = null;
            shipment.FedEx.HoldContactId = null;
            shipment.FedEx.HoldCountryCode = null;
            shipment.FedEx.HoldEmailAddress = null;
            shipment.FedEx.HoldFaxNumber = null;
            shipment.FedEx.HoldLocationId = null;
            shipment.FedEx.HoldLocationType = null;
            shipment.FedEx.HoldPagerNumber = null;
            shipment.FedEx.HoldPersonName = null;
            shipment.FedEx.HoldPhoneExtension = null;
            shipment.FedEx.HoldPhoneNumber = null;
            shipment.FedEx.HoldPostalCode = null;
            shipment.FedEx.HoldResidential = null;
            shipment.FedEx.HoldStateOrProvinceCode = null;
            shipment.FedEx.HoldStreet1 = null;
            shipment.FedEx.HoldStreet2 = null;
            shipment.FedEx.HoldStreet3 = null;
            shipment.FedEx.HoldTitle = null;
            shipment.FedEx.HoldUrbanizationCode = null;

            shipment.FedEx.ReturnType = 0;
            shipment.FedEx.RmaNumber = string.Empty;
            shipment.FedEx.RmaReason = string.Empty;
            shipment.FedEx.ReturnSaturdayPickup = false;

            shipment.FedEx.TrafficInArmsLicenseNumber = string.Empty;

            // Purely for certification purposes at this point
            shipment.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Pounds;
            shipment.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.IN;

            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;

            FedExPackageEntity package = FedExUtility.CreateDefaultPackage();
            shipment.FedEx.Packages.Add(package);

            // Weight of the first package equals the total shipment content weight
            package.Weight = shipment.ContentWeight;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            bool existed = profile.FedEx != null;

            // Load the profile data
            ShipmentTypeDataService.LoadProfileData(profile, "FedEx", typeof(FedExProfileEntity), refreshIfPresent);

            FedExProfileEntity fedex = profile.FedEx;

            // If this is the first time loading it, or we are supposed to refresh, do it now
            if (!existed || refreshIfPresent)
            {
                fedex.Packages.Clear();

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(fedex.Packages, new RelationPredicateBucket(FedExProfilePackageFields.ShippingProfileID == profile.ShippingProfileID));
                    fedex.Packages.Sort((int) FedExProfilePackageFieldIndex.FedExProfilePackageID, ListSortDirection.Ascending);
                }
            }
        }

        /// <summary>
        /// Save FedEx specific profile data
        /// </summary>
        public override bool SaveProfileData(ShippingProfileEntity profile, SqlAdapter adapter)
        {
            bool changes = base.SaveProfileData(profile, adapter);

            // First delete out anything that needs deleted
            foreach (FedExProfilePackageEntity package in profile.FedEx.Packages.ToList())
            {
                // If its new but deleted, just get rid of it
                if (package.Fields.State == EntityState.Deleted)
                {
                    if (package.IsNew)
                    {
                        profile.FedEx.Packages.Remove(package);
                    }

                    // If its deleted, delete it
                    else
                    {
                        package.Fields.State = EntityState.Fetched;
                        profile.FedEx.Packages.Remove(package);

                        adapter.DeleteEntity(package);

                        changes = true;
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            long shipperID = FedExAccountManager.Accounts.Count > 0 ? FedExAccountManager.Accounts[0].FedExAccountID : 0;

            profile.FedEx.FedExAccountID = shipperID;
            profile.OriginID = (int) ShipmentOriginSource.Account;

            profile.FedEx.ResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;

            profile.FedEx.Service = (int) FedExServiceType.FedExGround;
            profile.FedEx.Signature = (int) FedExSignatureType.ServiceDefault;
            profile.FedEx.PackagingType = (int) FedExPackagingType.Custom;
            profile.FedEx.DropoffType = (int) FedExDropoffType.RegularPickup;
            profile.FedEx.NonStandardContainer = false;
            profile.FedEx.ReferenceCustomer = "Order {//Order/Number}";
            profile.FedEx.ReferenceInvoice = "";
            profile.FedEx.ReferencePO = "";
            profile.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;

            profile.FedEx.PayorTransportType = (int) FedExPayorType.Sender;
            profile.FedEx.PayorTransportAccount = "";
            profile.FedEx.PayorDutiesType = (int) FedExPayorType.Sender;
            profile.FedEx.PayorDutiesAccount = "";

            profile.FedEx.SaturdayDelivery = false;

            profile.FedEx.EmailNotifySender = (int) FedExEmailNotificationType.None;
            profile.FedEx.EmailNotifyRecipient = (int) FedExEmailNotificationType.None;
            profile.FedEx.EmailNotifyBroker = (int) FedExEmailNotificationType.None;
            profile.FedEx.EmailNotifyOther = (int) FedExEmailNotificationType.None;
            profile.FedEx.EmailNotifyOtherAddress = "";
            profile.FedEx.EmailNotifyMessage = "";

            profile.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.ParcelSelect;
            profile.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.AddressCorrection;
            profile.FedEx.SmartPostConfirmation = true;
            profile.FedEx.SmartPostCustomerManifest = "";
            profile.FedEx.SmartPostHubID = "0";

            profile.FedEx.ReturnSaturdayPickup = false;
            profile.FedEx.RmaNumber = string.Empty;
            profile.FedEx.RmaReason = string.Empty;
            profile.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;
            profile.FedEx.ReturnSaturdayPickup = false;

        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            FedExShipmentEntity fedex = shipment.FedEx;
            FedExProfileEntity source = profile.FedEx;

            bool changedPackageWeights = false;

            // Apply all package profiles
            for (int i = 0; i < profile.FedEx.Packages.Count; i++)
            {
                // Get the profile to apply
                FedExProfilePackageEntity packageProfile = profile.FedEx.Packages[i];

                FedExPackageEntity package;

                // Get the existing, or create a new package
                if (fedex.Packages.Count > i)
                {
                    package = fedex.Packages[i];
                }
                else
                {
                    package = FedExUtility.CreateDefaultPackage();
                    fedex.Packages.Add(package);
                }

                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, package, FedExPackageFields.Weight);
                changedPackageWeights |= (packageProfile.Weight != null);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, package, FedExPackageFields.DimsProfileID);
                if (packageProfile.DimsProfileID != null)
                {
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, package, FedExPackageFields.DimsLength);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, package, FedExPackageFields.DimsWidth);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, package, FedExPackageFields.DimsHeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, package, FedExPackageFields.DimsWeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, package, FedExPackageFields.DimsAddWeight);
                }

                if (packageProfile.DryIceWeight > 0)
                {
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DryIceWeight, package, FedExPackageFields.DryIceWeight);
                }

                ShippingProfileUtility.ApplyProfileValue(packageProfile.ContainsAlcohol, package, FedExPackageFields.ContainsAlcohol);

                if (packageProfile.PriorityAlert.HasValue && packageProfile.PriorityAlert.Value)
                {
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.PriorityAlert, package, FedExPackageFields.PriorityAlert);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.PriorityAlertDetailContent, package, FedExPackageFields.PriorityAlertDetailContent);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.PriorityAlertEnhancementType, package, FedExPackageFields.PriorityAlertEnhancementType);
                }

                ShippingProfileUtility.ApplyProfileValue(packageProfile.DangerousGoodsEnabled, package, FedExPackageFields.DangerousGoodsEnabled);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DangerousGoodsType, package, FedExPackageFields.DangerousGoodsType);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DangerousGoodsAccessibilityType, package, FedExPackageFields.DangerousGoodsAccessibilityType);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DangerousGoodsCargoAircraftOnly, package, FedExPackageFields.DangerousGoodsCargoAircraftOnly);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DangerousGoodsEmergencyContactPhone, package, FedExPackageFields.DangerousGoodsEmergencyContactPhone);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DangerousGoodsOfferor, package, FedExPackageFields.DangerousGoodsOfferor);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.DangerousGoodsPackagingCount, package, FedExPackageFields.DangerousGoodsPackagingCount);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.HazardousMaterialNumber, package, FedExPackageFields.HazardousMaterialNumber);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.HazardousMaterialClass, package, FedExPackageFields.HazardousMaterialClass);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.HazardousMaterialProperName, package, FedExPackageFields.HazardousMaterialProperName);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.HazardousMaterialPackingGroup, package, FedExPackageFields.HazardousMaterialPackingGroup);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.HazardousMaterialQuantityValue, package, FedExPackageFields.HazardousMaterialQuantityValue);
                ShippingProfileUtility.ApplyProfileValue(packageProfile.HazardousMaterialQuanityUnits, package, FedExPackageFields.HazardousMaterialQuanityUnits);
            }

            // Remove any packages that are too many for the profile
            if (profile.FedEx.Packages.Count > 0)
            {
                // Go through each package that needs removed
                foreach (FedExPackageEntity package in fedex.Packages.Skip(profile.FedEx.Packages.Count).ToList())
                {
                    if (package.Weight != 0)
                    {
                        changedPackageWeights = true;
                    }

                    // Remove it from the list
                    fedex.Packages.Remove(package);

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

            long? accountID = (source.FedExAccountID == 0 && FedExAccountManager.Accounts.Count > 0) ?
                                  FedExAccountManager.Accounts[0].FedExAccountID :
                                  source.FedExAccountID;

            ShippingProfileUtility.ApplyProfileValue(source.ResidentialDetermination, shipment, ShipmentFields.ResidentialDetermination);

            ShippingProfileUtility.ApplyProfileValue(accountID, fedex, FedExShipmentFields.FedExAccountID);
            ShippingProfileUtility.ApplyProfileValue(source.Service, fedex, FedExShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(source.PackagingType, fedex, FedExShipmentFields.PackagingType);
            ShippingProfileUtility.ApplyProfileValue(source.DropoffType, fedex, FedExShipmentFields.DropoffType);
            ShippingProfileUtility.ApplyProfileValue(source.NonStandardContainer, fedex, FedExShipmentFields.NonStandardContainer);
            ShippingProfileUtility.ApplyProfileValue(source.OriginResidentialDetermination, fedex, FedExShipmentFields.OriginResidentialDetermination);

            ShippingProfileUtility.ApplyProfileValue(source.Signature, fedex, FedExShipmentFields.Signature);
            ShippingProfileUtility.ApplyProfileValue(source.ReferenceCustomer, fedex, FedExShipmentFields.ReferenceCustomer);
            ShippingProfileUtility.ApplyProfileValue(source.ReferenceInvoice, fedex, FedExShipmentFields.ReferenceInvoice);
            ShippingProfileUtility.ApplyProfileValue(source.ReferencePO, fedex, FedExShipmentFields.ReferencePO);

            ShippingProfileUtility.ApplyProfileValue(source.PayorTransportType, fedex, FedExShipmentFields.PayorTransportType);
            ShippingProfileUtility.ApplyProfileValue(source.PayorTransportAccount, fedex, FedExShipmentFields.PayorTransportAccount);
            ShippingProfileUtility.ApplyProfileValue(source.PayorDutiesType, fedex, FedExShipmentFields.PayorDutiesType);
            ShippingProfileUtility.ApplyProfileValue(source.PayorDutiesAccount, fedex, FedExShipmentFields.PayorDutiesAccount);

            ShippingProfileUtility.ApplyProfileValue(source.SaturdayDelivery, fedex, FedExShipmentFields.SaturdayDelivery);

            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifySender, fedex, FedExShipmentFields.EmailNotifySender);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyRecipient, fedex, FedExShipmentFields.EmailNotifyRecipient);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyBroker, fedex, FedExShipmentFields.EmailNotifyBroker);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyOther, fedex, FedExShipmentFields.EmailNotifyOther);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyOtherAddress, fedex, FedExShipmentFields.EmailNotifyOtherAddress);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyMessage, fedex, FedExShipmentFields.EmailNotifyMessage);

            ShippingProfileUtility.ApplyProfileValue(source.SmartPostIndicia, fedex, FedExShipmentFields.SmartPostIndicia);
            ShippingProfileUtility.ApplyProfileValue(source.SmartPostEndorsement, fedex, FedExShipmentFields.SmartPostEndorsement);
            ShippingProfileUtility.ApplyProfileValue(source.SmartPostConfirmation, fedex, FedExShipmentFields.SmartPostConfirmation);
            ShippingProfileUtility.ApplyProfileValue(source.SmartPostCustomerManifest, fedex, FedExShipmentFields.SmartPostCustomerManifest);
            ShippingProfileUtility.ApplyProfileValue(source.SmartPostHubID, fedex, FedExShipmentFields.SmartPostHubID);

            ShippingProfileUtility.ApplyProfileValue(profile.ReturnShipment, shipment, ShipmentFields.ReturnShipment);
            ShippingProfileUtility.ApplyProfileValue(source.ReturnType, fedex, FedExShipmentFields.ReturnType);
            ShippingProfileUtility.ApplyProfileValue(source.RmaNumber, fedex, FedExShipmentFields.RmaNumber);
            ShippingProfileUtility.ApplyProfileValue(source.RmaReason, fedex, FedExShipmentFields.RmaReason);
            ShippingProfileUtility.ApplyProfileValue(source.ReturnSaturdayPickup, fedex, FedExShipmentFields.ReturnSaturdayPickup);

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
            if (shipment.Processed)
            {
                return true;
            }

            if (originID == (int) ShipmentOriginSource.Account)
            {
                FedExAccountEntity account = FedExAccountManager.GetAccount(shipment.FedEx.FedExAccountID);
                if (account == null)
                {
                    account = FedExAccountManager.Accounts.FirstOrDefault();
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

            if (shipment.FedEx.HomeDeliveryDate < DateTime.Now.Date)
            {
                shipment.FedEx.HomeDeliveryDate = DateTime.Now.Date.AddHours(12);
            }

            // Ensure the cod address is up-to-date
            if (!UpdatePersonAddress(shipment, new PersonAdapter(shipment.FedEx, "Cod"), shipment.FedEx.CodOriginID))
            {
                shipment.FedEx.CodOriginID = (long) ShipmentOriginSource.Other;
            }

            FedExServiceType serviceType = (FedExServiceType) shipment.FedEx.Service;

            // Need to check with the store  to see if anything about the shipment was overridden in case
            // it may have effected the shipping services available (i.e. the eBay GSP program)            
            ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);

            // If its international ensure an international service
            if (!FedExUtility.GetValidServiceTypes(overriddenShipment).Contains(serviceType))
            {
                shipment.FedEx.Service = (int) FedExUtility.GetValidServiceTypes(overriddenShipment).First();
            }

            RedistributeContentWeight(shipment);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            // Consider the shipment insured of any package is insured
            shipment.Insurance = shipment.FedEx.Packages.Any(p => p.Insurance);

            // Set the provider type based on fedex settings
            shipment.InsuranceProvider = settings.FedExInsuranceProvider;

            // Right now ShipWorks Insurance (due to Tango limitation) doesn't support multi-package - so in that case just auto-revert to carrier insurance
            if (shipment.FedEx.Packages.Count > 1)
            {
                shipment.InsuranceProvider = (int) InsuranceProvider.Carrier;
            }

            // If it's international we have to make sure we wouldn't send more total declared value than the customs value - use the overridden shipment
            // to compare the country code in case it's been overridden such as an eBay GSP order
            decimal? maxPackageDeclaredValue = (overriddenShipment.ShipCountryCode != "US") ? (shipment.CustomsValue / shipment.FedEx.Packages.Count) : (decimal?) null;

            // Check the FedEx wide PennyOne settings and get them updated
            foreach (var package in shipment.FedEx.Packages)
            {
                package.InsurancePennyOne = settings.FedExInsurancePennyOne;

                // For SmartPost, we force Penny One since FedEx does not provide the first $100 for that
                if (shipment.FedEx.Service == (int) FedExServiceType.SmartPost)
                {
                    package.InsurancePennyOne = true;
                }

                // The only time we send the full insured value as declared value is if insurance is enabled, and they are using carrier insurance.
                if (shipment.Insurance && shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
                {
                    package.DeclaredValue = package.InsuranceValue;
                }
                else if (shipment.FedEx.Service == (int)FedExServiceType.SmartPost)
                {
                    // SmartPost shouldn't be sending any insurance value
                    package.DeclaredValue = 0;
                }
                else
                {
                    // Otherwise, regardless of if they are insuring or not, penny one or not, etc., we just send up to the first $100 since it's free anyway
                    package.DeclaredValue = Math.Min(100, package.InsuranceValue);

                    // We may have to lower it some more for international shipments so we aren't higher than the CustomsValue
                    if (maxPackageDeclaredValue != null)
                    {
                        package.DeclaredValue = Math.Min(package.DeclaredValue, (decimal) maxPackageDeclaredValue);
                    }
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
            double contentWeight = shipment.FedEx.Packages.Sum(p => p.Weight);

            // If the content weight changed outside of us, redistribute what the new weight among the packages
            if (contentWeight != shipment.ContentWeight)
            {
                foreach (FedExPackageEntity package in shipment.FedEx.Packages)
                {
                    package.Weight = shipment.ContentWeight / shipment.FedEx.Packages.Count;
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

            foreach (FedExPackageEntity package in shipment.FedEx.Packages)
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
        /// Indicates if the residential status indicator is required for the given shipment
        /// </summary>
        public override bool IsResidentialStatusRequired(ShipmentEntity shipment)
        {
            return IsDomestic(shipment); // shipment.ShipCountryCode == "US";
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return string.Format("{0}", EnumHelper.GetDescription((FedExServiceType) shipment.FedEx.Service));
        }

        /// <summary>
        /// Get the FedEx account number used for the shipment
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            FedExShipmentEntity fedex = shipment.FedEx;
            FedExAccountEntity account = FedExAccountManager.GetAccount(fedex.FedExAccountID);

            commonDetail.OriginAccount = (account == null) ? "" : account.AccountNumber;
            commonDetail.ServiceType = fedex.Service;

            commonDetail.PackagingType = fedex.PackagingType;
            commonDetail.PackageLength = fedex.Packages[0].DimsLength;
            commonDetail.PackageWidth = fedex.Packages[0].DimsWidth;
            commonDetail.PackageHeight = fedex.Packages[0].DimsHeight;

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

            return shipment.FedEx.Packages.Count;
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

            if (parcelIndex >= 0 && parcelIndex < shipment.FedEx.Packages.Count)
            {
                var package = shipment.FedEx.Packages[parcelIndex];

                return new ShipmentParcel(shipment, package.FedExPackageID,
                    new InsuranceChoice(shipment, package, package, package),
                    new DimensionsAdapter(package));
            }

            throw new ArgumentException(string.Format("'{0}' is out of range for the shipment.", parcelIndex), "parcelIndex");
        }

        /// <summary>
        /// Get all the tracking numbers for the shipment
        /// </summary>
        public override List<string> GetTrackingNumbers(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                return new List<string>();
            }

            FedExShipmentEntity fedex = shipment.FedEx;

            if (fedex.Packages.Count == 1)
            {
                return base.GetTrackingNumbers(shipment);
            }
            else
            {
                List<string> trackingList = new List<string>();

                for (int i = 0; i < fedex.Packages.Count; i++)
                {
                    trackingList.Add(string.Format("Package {0}: {1}", i + 1, fedex.Packages[i].TrackingNumber));
                }

                return trackingList;
            }
        }

        /// <summary>
        /// Get a list of rates for the FedEx shipment
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                return new FedExShippingClerk().GetRates(shipment);
            }
            catch (FedExException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Provide FedEx tracking results for the given shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            try
            {
                IShippingClerk shippingClerk = new FedExShippingClerk();
                return shippingClerk.Track(shipment);
            }
            catch (FedExException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates the UserControl that is used to edit return shipments
        /// </summary>
        /// <returns></returns>
        public override ReturnsControlBase CreateReturnsControl()
        {
            return new FedExReturnsControl();
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            try
            {
                // Okay to "new up" the shipping clerk here, as this class is the root consumer 
                // that drives the underlying FedEx API and outside of the current scope of unit testing,
                // so there isn't a need to be able to specify the dependencies of the shipping clerk
                IShippingClerk shippingClerk = new FedExShippingClerk();
                shippingClerk.Ship(shipment);
            }
            catch (FedExException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Void the given fedex shipment
        /// </summary>
        public override void VoidShipment(ShipmentEntity shipment)
        {
            try
            {
                IShippingClerk shippingClerk = new FedExShippingClerk();
                shippingClerk.Void(shipment);
            }
            catch (FedExException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generate the carrier specific template xml
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            var labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement("Labels",
                new LabelsOutline(container.Context, shipment, labels, ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));

            Lazy<TemplateLabelData> codReturn = new Lazy<TemplateLabelData>(() => labels.Value.FirstOrDefault(l => l.Name == "COD"));

            // Legacy stuff
            ElementOutline outline = container.AddElement("FedEx", ElementOutline.If(() => shipment().Processed));
            outline.AddAttributeLegacy2x();
            outline.AddElement("Voided", () => shipment().Voided);
            outline.AddElement("LabelCODReturn",
                () => TemplateLabelUtility.GenerateRotatedLabel(RotateFlipType.Rotate90FlipNone, codReturn.Value.Resource.GetAlternateFilename(TemplateLabelUtility.GetFileExtension(ImageFormat.Png))),
                ElementOutline.If(() => shipment().ThermalType == null && codReturn.Value != null));
            outline.AddElement("Package",
                new FedExLegacyPackageTemplateOutline(container.Context),
                () => labels.Value.Where(l => l.Category == TemplateLabelCategory.Primary),
                ElementOutline.If(() => shipment().ThermalType == null));
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            List<TemplateLabelData> labelData = new List<TemplateLabelData>();

            // FedEx stores some stuff at the shipmentID level, but we include it as apart of the first package
            List<DataResourceReference> shipmentResources = DataResourceManager.GetConsumerResourceReferences(shipment().ShipmentID);

            // Add labels for each package
            foreach (long packageID in DataProvider.GetRelatedKeys(shipment().ShipmentID, EntityType.FedExPackageEntity))
            {
                // Get the resource list for our shipment
                List<DataResourceReference> packageResources = DataResourceManager.GetConsumerResourceReferences(packageID);

                // Could be none for upgraded 2x shipments
                if (packageResources.Count > 0)
                {
                    // Add our standard label output
                    DataResourceReference labelResource = packageResources.Single(i => i.Label == "LabelImage");
                    labelData.Add(new TemplateLabelData(packageID, "Label", TemplateLabelCategory.Primary, labelResource));

                    // For Ground it will be at the package level,
                    DataResourceReference codPackageResource = packageResources.SingleOrDefault(r => r.Label == "COD");
                    if (codPackageResource != null)
                    {
                        labelData.Add(new TemplateLabelData(packageID, "COD", TemplateLabelCategory.Supplemental, codPackageResource));
                    }

                    // Will be non-null first time through
                    if (shipmentResources != null)
                    {
                        DataResourceReference codShipmentResource = shipmentResources.SingleOrDefault(r => r.Label == "COD");

                        if (codShipmentResource != null)
                        {
                            labelData.Add(new TemplateLabelData(packageID, "COD", TemplateLabelCategory.Supplemental, codShipmentResource));
                        }

                        var shipmentDocuments = shipmentResources.Where(r => r.Label.StartsWith("Document"));
                        foreach (DataResourceReference shipmentDocument in shipmentDocuments)
                        {
                            labelData.Add(new TemplateLabelData(packageID, shipmentDocument.Label.Replace("Document", ""), TemplateLabelCategory.Supplemental, shipmentDocument));
                        }

                        // Don't need it anymore
                        shipmentResources = null;
                    }

                    // Add any supporting package documents that may exist
                    var documentResources = packageResources.Where(r => r.Label.StartsWith("Document"));
                    foreach (DataResourceReference documentResource in documentResources)
                    {
                        labelData.Add(new TemplateLabelData(packageID, documentResource.Label.Replace("Document", ""), TemplateLabelCategory.Supplemental, documentResource));
                    }
                }
            }

            return labelData;
        }

        /// <summary>
        /// Determines if a shipment will be domestic or international
        /// </summary>
        public override bool IsDomestic(ShipmentEntity shipmentEntity)
        {
            return base.IsDomestic(shipmentEntity) && !IsShipmentBetweenUnitedStatesAndPuertoRico(shipmentEntity);
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the FedEx shipment type.
        /// </summary>
        /// <returns>An instance of a FedExBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker()
        {
            return new FedExBestRateBroker();
        }

        /// <summary>
        /// Outline for the legacy FedEx 'Package' element
        /// </summary>
        private class FedExLegacyPackageTemplateOutline : ElementOutline
        {
            private TemplateLabelData labelData;

            /// <summary>
            /// Constructor
            /// </summary>
            public FedExLegacyPackageTemplateOutline(TemplateTranslationContext context)
                : base(context)
            {
                Lazy<string> labelFile = new Lazy<string>(() => labelData.Resource.GetAlternateFilename(TemplateLabelUtility.GetFileExtension(ImageFormat.Png)));

                AddAttribute("ID", () => labelData.PackageID);
                AddElement("LabelOnly", () => TemplateLabelUtility.GenerateRotatedLabel(RotateFlipType.Rotate90FlipNone, labelFile.Value));
            }

            /// <summary>
            /// Create the bound clone
            /// </summary>
            public override ElementOutline CreateDataBoundClone(object data)
            {
                return new FedExLegacyPackageTemplateOutline(Context)
                {
                    labelData = (TemplateLabelData) data
                };
            }
        }
    }
}
