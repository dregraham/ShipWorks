using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Data;
using Interapptive.Shared.Enums;
using Interapptive.Shared.IO.Text.Ini;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Microsoft.Win32;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Settings;
using ShipWorks.Templates.Tokens;
using ShipWorks.Users;
using log4net;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip 
{
    /// <summary>
    /// Utility classes for dealing with the WorldShip integration
    /// </summary>
    public static class WorldShipUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WorldShipUtility));
 
        /// <summary>
        /// Service codes for the UPS API
        /// </summary>
        static readonly Dictionary<UpsServiceType, string> upsServiceCodes;

        /// <summary>
        /// The version of WorldShip currently installed
        /// </summary>
        private static int worldShipMajorVersion = int.MinValue;

        private const int worldShipDefaultMajorVersion = 15;

        /// <summary>
        /// Static constructor
        /// 
        /// Codes listed here: http://www.ups.com/worldshiphelp/WS12/ENU/AppHelp/Codes/UPS_Service_Codes.htm
        /// </summary>
        [NDependIgnoreLongMethod]
        static WorldShipUtility()
        {
            upsServiceCodes = new Dictionary<UpsServiceType, string>();

            upsServiceCodes[UpsServiceType.UpsGround] = "GND";
            upsServiceCodes[UpsServiceType.Ups3DaySelect] = "3DS";
            upsServiceCodes[UpsServiceType.Ups3DaySelectFromCanada] = "3DM";
            upsServiceCodes[UpsServiceType.Ups2DayAir] = "2DA";
            upsServiceCodes[UpsServiceType.Ups2DayAirAM] = "2DM";
            upsServiceCodes[UpsServiceType.UpsNextDayAir] = "1DA";
            upsServiceCodes[UpsServiceType.UpsNextDayAirSaver] = "1DP";
            upsServiceCodes[UpsServiceType.UpsNextDayAirAM] = "1DM";
            upsServiceCodes[UpsServiceType.WorldwideExpress] = "ES";
            upsServiceCodes[UpsServiceType.WorldwideExpedited] = "EX";
            upsServiceCodes[UpsServiceType.WorldwideExpressPlus] = "EP";
            upsServiceCodes[UpsServiceType.UpsStandard] = "ST";
            upsServiceCodes[UpsServiceType.WorldwideSaver] = "SV";

            upsServiceCodes[UpsServiceType.UpsExpressEarlyAm] = "1DM";
            upsServiceCodes[UpsServiceType.UpsExpress] = "ES";
            upsServiceCodes[UpsServiceType.UpsExpressSaver] = "SV";
            upsServiceCodes[UpsServiceType.UpsExpedited] = "EX";

            upsServiceCodes[UpsServiceType.UpsSurePostLessThan1Lb] = "USL";
            upsServiceCodes[UpsServiceType.UpsSurePost1LbOrGreater] = "USG";
            upsServiceCodes[UpsServiceType.UpsSurePostBoundPrintedMatter] = "USB";
            upsServiceCodes[UpsServiceType.UpsSurePostMedia] = "USM";

            upsServiceCodes[UpsServiceType.UpsMailInnovationsFirstClass] = "MIF";
            upsServiceCodes[UpsServiceType.UpsMailInnovationsPriority] = "MIT";
            upsServiceCodes[UpsServiceType.UpsMailInnovationsExpedited] = "MID";
            upsServiceCodes[UpsServiceType.UpsMailInnovationsIntEconomy] = "MIE";
            upsServiceCodes[UpsServiceType.UpsMailInnovationsIntPriority] = "MIP";

            upsServiceCodes[UpsServiceType.UpsCaWorldWideExpress] = "ES";
            upsServiceCodes[UpsServiceType.UpsCaWorldWideExpressPlus] = "EP";
            upsServiceCodes[UpsServiceType.UpsCaWorldWideExpressSaver] = "1DP";
        }

        /// <summary>
        /// Save this shipment to the WorldShip table to be 
        /// </summary>
        /// <exception cref="ShippingException" />
        /// <exception cref="TemplateTokenException" />
        public static void ExportToWorldShip(ShipmentEntity shipment)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                UpsServicePackageTypeSetting.Validate(shipment);

                CustomsManager.LoadCustomsItems(shipment, false);

                bool customsIsRequired = CustomsManager.IsCustomsRequired(shipment);

                PersonAdapter from = new PersonAdapter(shipment, "Origin");
                PersonAdapter to = new PersonAdapter(shipment, "Ship");
                WorldShipShipmentEntity worldship = SaveToShipmentTable(shipment, adapter, customsIsRequired, from, to);

                // Reference
                UpdateReferenceNumbers(shipment);

                // All packages with dry ice must have the same regulation set, so verify that.
                bool tooManyDryIceRegulations = shipment.Ups.Packages
                                                    .Where(p => p.DryIceEnabled)
                                                    .Select(p => p.DryIceRegulationSet)
                                                    .Distinct()
                                                    .Count() > 1;
                if (tooManyDryIceRegulations)
                {
                    throw new UpsException("All packages containing dry ice must have the same dry ice regulation set.");
                }

                // Save the packages
                foreach (UpsPackageEntity package in shipment.Ups.Packages)
                {
                    SaveToPackageTable(package, worldship, adapter, customsIsRequired, from, to);
                }

                // For international save commodities as goods
                if (customsIsRequired)
                {
                    foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
                    {
                        SaveToGoodsTable(customsItem, worldship, adapter);
                    }
                }

                adapter.Commit();
            }
        }

        /// <summary>
        /// Update reference numbers to conform to WorldShip needs
        /// </summary>
        private static void UpdateReferenceNumbers(ShipmentEntity shipment)
        {
            UpsServiceType upsServiceType = (UpsServiceType) shipment.Ups.Service;
            if (UpsUtility.IsUpsMiOrSurePostService(upsServiceType))
            {
                shipment.Ups.UspsPackageID = UpsApiCore.ProcessUspsTokenField(shipment.Ups.UspsPackageID, shipment.ShipmentID, string.Empty);
                shipment.Ups.CostCenter = UpsApiCore.ProcessUspsTokenField(shipment.Ups.CostCenter, shipment.ShipmentID, string.Empty);

                // Throw the same errors that online tools does if either are missing.
                if (string.IsNullOrWhiteSpace(shipment.Ups.UspsPackageID))
                {
                    throw new UpsException("Missing or Invalid Mail Innovations Package Id.");
                }
                if (string.IsNullOrWhiteSpace(shipment.Ups.CostCenter))
                {
                    throw new UpsException("Missing or Invalid Mail Innovations Cost Center.");
                }

                // WorldShip still looks at Reference numbers for package id and cost ceneter, so update those here.
                shipment.Ups.ReferenceNumber = shipment.Ups.UspsPackageID;
                shipment.Ups.ReferenceNumber2 = shipment.Ups.CostCenter;
            }
            else
            {
                // Reference
                shipment.Ups.ReferenceNumber = FixReferenceNumberForWorldShip(shipment.Ups.ReferenceNumber, shipment.Order.OrderNumber.ToString(), shipment.ShipmentID);
                shipment.Ups.ReferenceNumber2 = FixReferenceNumberForWorldShip(shipment.Ups.ReferenceNumber2, shipment.Ups.ReferenceNumber, shipment.ShipmentID);
            }
        }

        /// <summary>
        /// Returns the fixed version of the reference number that is valid for WorldShip
        /// </summary>
        /// <param name="referenceNumber">The reference number to fix.</param>
        /// <param name="defaultValue">If the reference number ends up being blank, use this value instead.</param>
        /// <param name="shipmentID">ShipmentID used for processing tokens.</param>
        private static string FixReferenceNumberForWorldShip(string referenceNumber, string defaultValue, long shipmentID)
        {
            // Set the max length to 35 for non-MI shipments
            int maxLength = 35;

            // Process the reference as a token first
            referenceNumber = TemplateTokenProcessor.ProcessTokens(referenceNumber.Trim(), shipmentID);

            // Also, ref 1/2 cannot be blank
            if (string.IsNullOrWhiteSpace(referenceNumber))
            {
                referenceNumber = defaultValue;
            }

            // Also, the ref 1/2 must be less than max length
            referenceNumber = referenceNumber.Substring(0, referenceNumber.Length >= maxLength ? maxLength : referenceNumber.Length);

            return referenceNumber;
        }

        /// <summary>
        /// Save the given ShipWorks shipment to the WorldShip export table
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        private static WorldShipShipmentEntity SaveToShipmentTable(ShipmentEntity shipment, SqlAdapter adapter, bool customsIsRequired, PersonAdapter from, PersonAdapter to)
        {
            UpsAccountEntity account = UpsApiCore.GetUpsAccount(shipment, new UpsAccountRepository());
            UpsShipmentEntity ups = shipment.Ups;
            bool isDomestic = ShipmentTypeManager.GetType(shipment).IsDomestic(shipment);

            // If the row exists already remove it before we start.
            adapter.DeleteEntity(new WorldShipShipmentEntity(shipment.ShipmentID));

            // Create the new entity
            WorldShipShipmentEntity worldship = new WorldShipShipmentEntity
                {
                    ShipmentID = shipment.ShipmentID,
                    DeliveryConfirmation = "N",
                    DeliveryConfirmationAdult = "N"
                };

            UpsPayorType payorType = (UpsPayorType) ups.PayorType;

            worldship.ShipmentProcessedOnComputerID = UserSession.Computer.ComputerID;

            // Order
            worldship.OrderNumber = shipment.Order.OrderNumberComplete;

            // Shipper
            worldship.ShipperNumber = account.AccountNumber;

            // From
            worldship.FromCompanyOrName = (from.Company.Length > 0) ? from.Company : new PersonName(from).FullName;
            worldship.FromAttention = (from.Company.Length > 0) ? new PersonName(from).FullName : "";
            worldship.FromAddress1 = from.Street1;
            worldship.FromAddress2 = from.Street2;
            worldship.FromAddress3 = from.Street3;
            worldship.FromCity = from.City;
            worldship.FromStateProvCode = UpsApiCore.AdjustUpsStateProvinceCode(from.CountryCode, from.StateProvCode);
            worldship.FromPostalCode = from.PostalCode;
            worldship.FromCountryCode = from.AdjustedCountryCode(ShipmentTypeCode.UpsWorldShip);
            worldship.FromTelephone = PersonUtility.GetPhoneDigits(from.Phone, 15, true);
            worldship.FromEmail = UpsUtility.GetCorrectedEmailAddress(from.Email);
            worldship.FromAccountNumber = account.AccountNumber;

            // To
            worldship.ToCustomerID = shipment.Order.CustomerID.ToString();
            worldship.ToCompanyOrName = (to.Company.Length > 0) ? to.Company : new PersonName(to).FullName;
            worldship.ToAttention = (to.Company.Length > 0) ? new PersonName(to).FullName : "";
            worldship.ToAddress1 = to.Street1;
            worldship.ToAddress2 = to.Street2;
            worldship.ToAddress3 = to.Street3;
            worldship.ToCity = to.City;
            worldship.ToStateProvCode = UpsApiCore.AdjustUpsStateProvinceCode(to.CountryCode, to.StateProvCode);
            worldship.ToPostalCode = to.PostalCode;
            worldship.ToCountryCode = to.AdjustedCountryCode(ShipmentTypeCode.UpsWorldShip);
            worldship.ToTelephone = PersonUtility.GetPhoneDigits(to.Phone, 15, true);
            worldship.ToEmail = UpsUtility.GetCorrectedEmailAddress(to.Email);
            worldship.ToResidential = shipment.ResidentialResult ? "Y" : "N";
            worldship.ToAccountNumber = payorType == UpsPayorType.Sender ? "" : ups.PayorAccount;

            // International attn cannot be blank
            if (worldship.ToAttention.Length == 0 && !isDomestic)
            {
                worldship.ToAttention = worldship.ToCompanyOrName;
            }

            // Package count
            worldship.PackageCount = shipment.Ups.Packages.Count;

            // Service
            worldship.ServiceType = upsServiceCodes[(UpsServiceType) ups.Service];

            // Billing
            if (payorType == UpsPayorType.Sender)
            {
                worldship.BillTransportationTo = "SHP";
            }
            else if (payorType == UpsPayorType.ThirdParty)
            {
                worldship.BillTransportationTo = "TP";
            }
            else
            {
                worldship.BillTransportationTo = "REC";
            }

            // Saturday
            if (ups.SaturdayDelivery && UpsUtility.CanDeliverOnSaturday((UpsServiceType) ups.Service, shipment.ShipDate))
            {
                worldship.SaturdayDelivery = "Y";
            }
            else
            {
                worldship.SaturdayDelivery = "N";
            }

            // Delivery confirm - for international
            if (GetShipmentOrPackageForDeliveryConfirmation(shipment) == UpsDeliveryConfirmationEntityLevel.Shipment)
            {
                SetShipmentDeliveryConfirmation(ups, worldship);
            }

            // International
            if (!isDomestic)
            {
                worldship.CustomsDescriptionOfGoods = ups.CustomsDescription;
                worldship.CustomsDocumentsOnly = ups.CustomsDocumentsOnly ? "Y" : "N";
            }
            else
            {
                worldship.CustomsDescriptionOfGoods = "";
                worldship.CustomsDocumentsOnly = "N";
            }

            if (customsIsRequired)
            {
                worldship.InvoiceCurrencyCode = UpsUtility.GetCurrency(account);
            }

            // International Invoice
            if (!isDomestic && ups.CommercialPaperlessInvoice)
            {
                worldship.InvoiceTermsOfSale = UpsApiShipClient.GetTermsOfShipmentApiCode((UpsTermsOfSale) ups.CommercialInvoiceTermsOfSale);
                worldship.InvoiceReasonForExport = GetReasonForExportCode((UpsExportReason) ups.CommercialInvoicePurpose);
                worldship.InvoiceComments = ups.CommercialInvoiceComments;
                worldship.InvoiceChargesFreight = ups.CommercialInvoiceFreight;
                worldship.InvoiceChargesInsurance = ups.CommercialInvoiceInsurance;
                worldship.InvoiceChargesOther = ups.CommercialInvoiceOther;
            }

            // WorldShip, for MI, doesn't allow QVN
            SetShipmentQvnFields(worldship, ups, from, to);

            // Set the Usps Endorsement, only if it's not blank.  WS treats blanks as values, so we want it to 
            // stay null if we aren't an MI or SurePost shipment.
            string uspsEndorsementCode = GetUspsEndorsementCode(ups);
            if (!string.IsNullOrWhiteSpace(uspsEndorsementCode))
            {
                worldship.UspsEndorsement = uspsEndorsementCode;
            }

            worldship.CarbonNeutral = ups.CarbonNeutral ? "Y" : "N";

            // Save and return the result
            adapter.SaveAndRefetch(worldship);

            return worldship;
        }

        /// <summary>
        /// Get's the endorsement text that WorldShip expects, based on UpsEndorsementType
        /// </summary>
        private static string GetUspsEndorsementCode(UpsShipmentEntity ups)
        {
            UpsServiceType upsServiceType = (UpsServiceType) ups.Service;
            UpsPackagingType upsPackagingType = (UpsPackagingType) ups.Packages[0].PackagingType;
            UspsEndorsementType uspsEndorsementType = (UspsEndorsementType) ups.Endorsement;

            if (UpsUtility.IsUpsSurePostService(upsServiceType))
            {
                switch (uspsEndorsementType)
                {
                    case UspsEndorsementType.CarrierLeaveIfNoResponse:
                        return "Carrier - Leave If No Response";
                    case UspsEndorsementType.ReturnServiceRequested:
                        return "Return Service Requested";
                    case UspsEndorsementType.ForwardingServiceRequested:
                        return "Forward Service Requested";
                    case UspsEndorsementType.AddressServiceRequested:
                        return "Address Service Requested";
                    case UspsEndorsementType.ChangeServiceRequested:
                        return "Change Service Requested";
                    default:
                        return "Carrier - Leave If No Response";
                }
            }
            
            if (UpsUtility.IsUpsMiService(upsServiceType))
            {
                // MI Expedited & International MI does not support endorsements
                if (upsServiceType == UpsServiceType.UpsMailInnovationsIntEconomy ||
                    upsServiceType == UpsServiceType.UpsMailInnovationsIntPriority)
                {
                    return string.Empty;
                }

                // MI Expedited w/ Flat packaging types does not allow endorsements
                if (upsServiceType == UpsServiceType.UpsMailInnovationsExpedited &&
                    (upsPackagingType == UpsPackagingType.BPMFlats || upsPackagingType == UpsPackagingType.StandardFlats))
                {
                    return string.Empty;
                }

                switch (uspsEndorsementType)
                {
                    case UspsEndorsementType.ReturnServiceRequested:
                        return "Return Service Requested";
                    case UspsEndorsementType.ForwardingServiceRequested:
                        return "Forward Service Requested";
                    case UspsEndorsementType.AddressServiceRequested:
                        return "Address Service Requested";
                    case UspsEndorsementType.ChangeServiceRequested:
                        return "Change Service Requested";
                    case UspsEndorsementType.CarrierLeaveIfNoResponse:
                        throw new UpsException("Endorsement, 'Carrier - Leave if No Response', is not valid for Mail Innovations.");
                    default:
                        throw new UpsException("Endorsement type was not found.");
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets QVN fields, taking into consideration MI does not support QVN
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void SetShipmentQvnFields(WorldShipShipmentEntity worldship, UpsShipmentEntity ups, PersonAdapter from, PersonAdapter to)
        {
            bool isMiService = UpsUtility.IsUpsMiService((UpsServiceType) ups.Service); 
            
            // Default to no QVN
            worldship.QvnFrom = "";
            worldship.QvnMemo = "";
            worldship.QvnSubjectLine = "";
            worldship.QvnOption = "N";

            // Sender QVN
            worldship.Qvn1ShipNotify = "N";
            worldship.Qvn1DeliveryNotify = "N";
            worldship.Qvn1ExceptionNotify = "N";
            worldship.Qvn1ContactName = "";
            worldship.Qvn1Email = "";

            // Recipient QVN
            worldship.Qvn2ShipNotify = "N";
            worldship.Qvn2DeliveryNotify = "N";
            worldship.Qvn2ExceptionNotify = "N";
            worldship.Qvn2ContactName = "";
            worldship.Qvn2Email = "";

            // Other QVN
            worldship.Qvn3ShipNotify = "N";
            worldship.Qvn3DeliveryNotify = "N";
            worldship.Qvn3ExceptionNotify = "N";
            worldship.Qvn3ContactName = "";
            worldship.Qvn3Email = "";

            if (isMiService)
            {
                // MI does not support shipment level QVN
                return;
            }

            // SurePost and regular support shipment level QVN, set as needed
            if (ups.EmailNotifySender + ups.EmailNotifyRecipient + ups.EmailNotifyOther > 0)
            {
                worldship.QvnFrom = UpsUtility.GetCorrectedEmailAddress(ups.EmailNotifyFrom);
                worldship.QvnMemo = ups.EmailNotifyMessage;
                worldship.QvnSubjectLine = (ups.EmailNotifySubject == (int)UpsEmailNotificationSubject.ReferenceNumber) ? "Reference Number 1" : "Tracking Number";
                worldship.QvnOption = "Y";

                // Sender QVN
                worldship.Qvn1ShipNotify = (ups.EmailNotifySender & (int)UpsEmailNotificationType.Ship) != 0 ? "Y" : "N";
                worldship.Qvn1DeliveryNotify = (ups.EmailNotifySender & (int)UpsEmailNotificationType.Deliver) != 0 ? "Y" : "N";
                worldship.Qvn1ExceptionNotify = (ups.EmailNotifySender & (int)UpsEmailNotificationType.Exception) != 0 ? "Y" : "N";
                if (ups.EmailNotifySender > 0)
                {
                    // Only write this info out if notify sender was selected.  Otherwise, WorldShip fails on hands off.
                    worldship.Qvn1ContactName = new PersonName(from).FullName;
                    worldship.Qvn1Email = UpsUtility.GetCorrectedEmailAddress(from.Email);
                }

                // Recipient QVN
                worldship.Qvn2ShipNotify = (ups.EmailNotifyRecipient & (int)UpsEmailNotificationType.Ship) != 0 ? "Y" : "N";
                worldship.Qvn2DeliveryNotify = (ups.EmailNotifyRecipient & (int)UpsEmailNotificationType.Deliver) != 0 ? "Y" : "N";
                worldship.Qvn2ExceptionNotify = (ups.EmailNotifyRecipient & (int)UpsEmailNotificationType.Exception) != 0 ? "Y" : "N";
                if (ups.EmailNotifyRecipient > 0)
                {
                    // Only write this info out if notify recipient was selected.  Otherwise, WorldShip fails on hands off.
                    worldship.Qvn2ContactName = new PersonName(to).FullName;
                    worldship.Qvn2Email = UpsUtility.GetCorrectedEmailAddress(to.Email);
                }

                // Other QVN
                worldship.Qvn3ShipNotify = (ups.EmailNotifyOther & (int)UpsEmailNotificationType.Ship) != 0 ? "Y" : "N";
                worldship.Qvn3DeliveryNotify = (ups.EmailNotifyOther & (int)UpsEmailNotificationType.Deliver) != 0 ? "Y" : "N";
                worldship.Qvn3ExceptionNotify = (ups.EmailNotifyOther & (int)UpsEmailNotificationType.Exception) != 0 ? "Y" : "N";
                if (ups.EmailNotifyOther > 0)
                {
                    // Only write this info out if notify other was selected.  Otherwise, WorldShip fails on hands off.
                    worldship.Qvn3ContactName = UpsUtility.GetCorrectedEmailAddress(ups.EmailNotifyOtherAddress);
                    worldship.Qvn3Email = UpsUtility.GetCorrectedEmailAddress(ups.EmailNotifyOtherAddress);
                }
            }
        }

        /// <summary>
        /// Sets QVN fields at the package level
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void SetPackageQvnFields(WorldShipPackageEntity worldshipPackage, UpsShipmentEntity upsShipment, PersonAdapter from, PersonAdapter to)
        {
            bool isMiService = UpsUtility.IsUpsMiService((UpsServiceType)upsShipment.Service); 

            // In v16 (2013), QVN is supported for MI, but not in 2012 and earlier, so see which version we are.
            bool isWsVersionGreaterThanLegacyVersion = WorldShipMajorVersion > worldShipDefaultMajorVersion;

            // If this isn't an MI service, just return as only MI supports package level QVN as of this writing.
            // Also just return if notification wasn't requested.
            // Also if we are not 2013 or greater, just return as it's not supported in previous versions.
            if (!isMiService || !isWsVersionGreaterThanLegacyVersion || (upsShipment.EmailNotifySender + upsShipment.EmailNotifyRecipient + upsShipment.EmailNotifyOther == 0))
            {
                // Default to no QVN
                worldshipPackage.QvnFrom = "";
                worldshipPackage.QvnMemo = "";
                worldshipPackage.QvnSubjectLine = "";
                worldshipPackage.QvnOption = "N";

                // Sender QVN
                worldshipPackage.Qvn1ShipNotify = "N";
                worldshipPackage.Qvn1ContactName = "";
                worldshipPackage.Qvn1Email = "";

                worldshipPackage.Qvn2ShipNotify = "N";
                worldshipPackage.Qvn2ContactName = "";
                worldshipPackage.Qvn2Email = "";

                worldshipPackage.Qvn3ShipNotify = "N";
                worldshipPackage.Qvn3ContactName = "";
                worldshipPackage.Qvn3Email = "";

                return;
            }

            worldshipPackage.QvnFrom = UpsUtility.GetCorrectedEmailAddress(upsShipment.EmailNotifyFrom);
            worldshipPackage.QvnMemo = upsShipment.EmailNotifyMessage;
            worldshipPackage.QvnSubjectLine = (upsShipment.EmailNotifySubject == (int)UpsEmailNotificationSubject.ReferenceNumber) ? "Reference Number 1" : "Tracking Number";
            worldshipPackage.QvnOption = "Y";

            // Sender QVN
            if ((upsShipment.EmailNotifySender & (int) UpsEmailNotificationType.Ship) != 0)
            {
                worldshipPackage.Qvn1ShipNotify = "Y";
                worldshipPackage.Qvn1ContactName = new PersonName(from).FullName;
                worldshipPackage.Qvn1Email = UpsUtility.GetCorrectedEmailAddress(from.Email);
            }

            // Receipient QVN
            if ((upsShipment.EmailNotifyRecipient & (int) UpsEmailNotificationType.Ship) != 0)
            {
                worldshipPackage.Qvn2ShipNotify = "Y";
                worldshipPackage.Qvn2ContactName = new PersonName(to).FullName;
                worldshipPackage.Qvn2Email = UpsUtility.GetCorrectedEmailAddress(to.Email);
            }

            // Other QVN
            if ((upsShipment.EmailNotifyOther & (int) UpsEmailNotificationType.Ship) != 0)
            {
                worldshipPackage.Qvn3ShipNotify = "Y";
                worldshipPackage.Qvn3ContactName = UpsUtility.GetCorrectedEmailAddress(upsShipment.EmailNotifyOtherAddress);
                worldshipPackage.Qvn3Email = UpsUtility.GetCorrectedEmailAddress(upsShipment.EmailNotifyOtherAddress);
            }
        }

        /// <summary>
        /// Save the given UPS package to the WorldShip export table
        /// </summary>
        [NDependIgnoreTooManyParams]
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        private static void SaveToPackageTable(UpsPackageEntity package, WorldShipShipmentEntity worldshipShipment, SqlAdapter adapter, bool customsIsRequired, PersonAdapter from, PersonAdapter to)
        {
            UpsShipmentEntity ups = package.UpsShipment;

            WorldShipPackageEntity worldshipPackage = new WorldShipPackageEntity
                {
                    UpsPackageID = package.UpsPackageID,
                    ShipmentID = worldshipShipment.ShipmentID,
                    PackageType = GetPackageTypeCode((UpsPackagingType) package.PackagingType),
                    DeliveryConfirmation = "N",
                    DeliveryConfirmationAdult = "N",
                    DeliveryConfirmationSignature = "N",
                    MIDeliveryConfirmation = "N"
                };

            double weight = UpsUtility.GetPackageTotalWeight(package);

            // Get the settings for this shipment/package type so we can determine weight unit of measure and declared value setting
            // Before calling this method, the shipment/package type should be validated so that a setting is always found if we make it this far.
            UpsServicePackageTypeSetting upsSetting = UpsServicePackageTypeSetting.ServicePackageValidationSettings.First(s => s.ServiceType == (UpsServiceType)ups.Service &&
                                                        s.PackageType == (UpsPackagingType)package.PackagingType);

            if (upsSetting.WeightUnitOfMeasure == WeightUnitOfMeasure.Ounces)
            {
                weight = weight * 16;
            }

            worldshipPackage.Weight = weight;
            worldshipPackage.Length = (int) Math.Ceiling(package.DimsLength);
            worldshipPackage.Width = (int) Math.Ceiling(package.DimsWidth);
            worldshipPackage.Height = (int) Math.Ceiling(package.DimsHeight);

            // COD
            if (ups.CodEnabled)
            {
                worldshipPackage.CodOption = "Y";
                worldshipPackage.CodAmount = ups.CodAmount;
                worldshipPackage.CodCashOnly = ups.CodPaymentType == (int) UpsCodPaymentType.Cash ? "Y" : "N";
            }
            else
            {
                worldshipPackage.CodOption = "N";
                worldshipPackage.CodAmount = 0;
                worldshipPackage.CodCashOnly = "N";
            }

            // Delivery confirm
            if (GetShipmentOrPackageForDeliveryConfirmation(ups.Shipment) == UpsDeliveryConfirmationEntityLevel.Package)
            {
                SetPackageDeliveryConfirmation(ups, worldshipPackage);
            }

            // Set QVN fields at package level
            SetPackageQvnFields(worldshipPackage, package.UpsShipment, from, to);

            // Reference has to be set on every package (due to the way worldship maps)
            worldshipPackage.ReferenceNumber =  ups.ReferenceNumber;
            worldshipPackage.ReferenceNumber2 = ups.ReferenceNumber2;

            // Since the DeclaredValue amount stays even if the user unchecks the checkbox, we need to check package.Insurance too
            if (package.Insurance && package.DeclaredValue > 0 && upsSetting.DeclaredValueAllowed)
            {
                worldshipPackage.DeclaredValueAmount = (double)package.DeclaredValue;
                worldshipPackage.DeclaredValueOption = "Y";
            }
            else
            {
                worldshipPackage.DeclaredValueOption = "N";
            }

            // CN22 fields
            if (customsIsRequired)
            {
                worldshipPackage.CN22GoodsType = EnumHelper.GetApiValue(UpsCN22GoodsType.Other);
                worldshipPackage.CN22Description = ups.CustomsDescription;
            }

            // Postal sub class
            // From the WS UI, this is only required for SurePost less than 1 pound
            if (UpsServiceType.UpsSurePostLessThan1Lb == (UpsServiceType)ups.Service)
            {
                // The two options are Irregular and Machinable
                worldshipPackage.PostalSubClass = (UpsPostalSubclassificationType)ups.Subclassification == UpsPostalSubclassificationType.Irregular ? "Irregular" : "Machinable";
            }

            worldshipPackage.ShipperRelease = ups.ShipperRelease ? "Y" : "N";

            worldshipPackage.AdditionalHandlingEnabled = package.AdditionalHandlingEnabled ? "Y" : "N";


            // Add dry ice if enabled
            if (package.DryIceEnabled)
            {
                // Verbal confirmation is not allowed with dry ice
                if (package.VerbalConfirmationEnabled)
                {
                    throw new UpsException("Dry ice and verbal confirmation are not allowed on the same package.");
                }

                // Additional handling is not allowed with dry ice
                if (package.AdditionalHandlingEnabled)
                {
                    throw new UpsException("Dry ice and additional handling are not allowed on the same package.");
                }

                worldshipPackage.DryIceWeight = package.DryIceWeight;
                if (upsSetting.WeightUnitOfMeasure == WeightUnitOfMeasure.Ounces)
                {
                    worldshipPackage.DryIceWeight = worldshipPackage.DryIceWeight * 16;
                }

                if (worldshipPackage.DryIceWeight > weight)
                {
                    throw new UpsException("Dry ice weight must be less than the package weight.");
                }

                UpsDryIceRegulationSet upsDryIceRegulationSet = (UpsDryIceRegulationSet) package.DryIceRegulationSet;
                switch (upsDryIceRegulationSet)
                {
                    case UpsDryIceRegulationSet.Cfr:
                        worldshipPackage.DryIceRegulationSet = "49CFR";
                        break;
                    case UpsDryIceRegulationSet.Iata:
                        worldshipPackage.DryIceRegulationSet = "IATA";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(string.Format("The dry ice regulation set '{0}' is unknown.", EnumHelper.GetDescription(upsDryIceRegulationSet)));
                }

                worldshipPackage.DryIceWeightUnitOfMeasure = "LBS";
                worldshipPackage.DryIceOption = "Y";
                worldshipPackage.DryIceMedicalPurpose = package.DryIceIsForMedicalUse ? "Y" : "N";
            }

            // Add verbal confirmation if enabled
            if (package.VerbalConfirmationEnabled)
            {
                worldshipPackage.VerbalConfirmationOption = "Y";
                worldshipPackage.VerbalConfirmationContactName = package.VerbalConfirmationName;
                
                // WorldShip imports correctly when all non-numerics are removed from the concatenated phone number.
                Regex onlyNumbers = new Regex("[^.0-9]");
                string worldShipPhone = onlyNumbers.Replace(package.VerbalConfirmationPhone, string.Empty);
                worldShipPhone += onlyNumbers.Replace(package.VerbalConfirmationPhoneExtension, string.Empty);

                worldshipPackage.VerbalConfirmationTelephone = worldShipPhone;
            }

            adapter.SaveAndRefetch(worldshipPackage);
        }

        /// <summary>
        /// Set the package delivery confirmation options for the package
        /// </summary>
        private static void SetPackageDeliveryConfirmation(UpsShipmentEntity ups, WorldShipPackageEntity worldshipPackage)
        {
            worldshipPackage.DeliveryConfirmation = "N";
            worldshipPackage.DeliveryConfirmationAdult = "N";
            worldshipPackage.DeliveryConfirmationSignature = "N";
            worldshipPackage.MIDeliveryConfirmation = "N";

            if (!UpsUtility.IsUpsMiOrSurePostService((UpsServiceType)ups.Service))
            {
                // Delivery confirm 
                if (ups.DeliveryConfirmation != (int) UpsDeliveryConfirmationType.None)
                {
                    worldshipPackage.DeliveryConfirmation = "Y";
                    worldshipPackage.DeliveryConfirmationSignature = (ups.DeliveryConfirmation == (int) UpsDeliveryConfirmationType.NoSignature) ? "N" : "Y";
                    worldshipPackage.DeliveryConfirmationAdult = (ups.DeliveryConfirmation == (int) UpsDeliveryConfirmationType.AdultSignature) ? "Y" : "N";
                }
            }
            else if (UpsUtility.IsUpsMiService((UpsServiceType)ups.Service))
            {
                // Delivery confirm 
                if (ups.DeliveryConfirmation != (int)UpsDeliveryConfirmationType.None)
                {
                    worldshipPackage.MIDeliveryConfirmation = "Y";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns>
        /// 0 for Shipment, 
        /// 1 for Package</returns>
        private static UpsDeliveryConfirmationEntityLevel GetShipmentOrPackageForDeliveryConfirmation(ShipmentEntity shipment)
        {
            string fromCountry = shipment.AdjustedOriginCountryCode().ToUpperInvariant();
            string toCountry = shipment.AdjustedShipCountryCode().ToUpperInvariant();

            // See http://fogbugz.interapptive.com/default.asp?185745#1387154 for UPS Appendix defining the matrix
            switch (fromCountry)
            {
                case "US":
                case "PR":
                    switch (toCountry)
                    {
                        case "US":
                        case "PR":
                            return UpsDeliveryConfirmationEntityLevel.Package;
                        default:
                            return UpsDeliveryConfirmationEntityLevel.Shipment;
                    }
                case "CA":
                    switch (toCountry)
                    {
                        case "CA":
                            return UpsDeliveryConfirmationEntityLevel.Package;
                        default:
                            return UpsDeliveryConfirmationEntityLevel.Shipment;
                    }
                default:
                    return UpsDeliveryConfirmationEntityLevel.Shipment;
            }
        }

        /// <summary>
        /// Set the shipment delivery confirmation options for the shipment
        /// </summary>
        private static void SetShipmentDeliveryConfirmation(UpsShipmentEntity ups, WorldShipShipmentEntity worldshipShipment)
        {
            worldshipShipment.DeliveryConfirmation = "N";
            worldshipShipment.DeliveryConfirmationAdult = "N";
            
            if (!UpsUtility.IsUpsMiOrSurePostService((UpsServiceType)ups.Service))
            {
                // Delivery confirm (for domestic)
                if (ups.DeliveryConfirmation != (int)UpsDeliveryConfirmationType.None)
                {
                    worldshipShipment.DeliveryConfirmation = "Y";
                    worldshipShipment.DeliveryConfirmationAdult = (ups.DeliveryConfirmation == (int)UpsDeliveryConfirmationType.AdultSignature) ? "Y" : "N";
                }
            }
        }

        /// <summary>
        /// Save the given content item to the shipment goods table
        /// </summary>
        private static void SaveToGoodsTable(ShipmentCustomsItemEntity customsItem, WorldShipShipmentEntity worldship, SqlAdapter adapter)
        {
            WorldShipGoodsEntity goods = new WorldShipGoodsEntity
                {
                    ShipmentCustomsItemID = customsItem.ShipmentCustomsItemID,
                    ShipmentID = worldship.ShipmentID,
                    Description = customsItem.Description,
                    Units = (int) Math.Ceiling(customsItem.Quantity),
                    UnitPrice = customsItem.UnitValue,
                    UnitOfMeasure = worldship.ToCountryCode == "CA" ? "NMB" : "EA",
                    Weight = customsItem.Weight,
                    CountryOfOrigin = customsItem.CountryOfOrigin,
                    TariffCode = customsItem.HarmonizedCode,
                    InvoiceCurrencyCode = worldship.InvoiceCurrencyCode
                };

            adapter.SaveAndRefetch(goods);
        }

        /// <summary>
        /// Get the code to send to WorldShip for the given packaging type
        /// </summary>
        [NDependIgnoreComplexMethodAttribute]
        private static string GetPackageTypeCode(UpsPackagingType packagingType)
        {
            switch (packagingType)
            {
                case UpsPackagingType.Box10Kg: return "10";
                case UpsPackagingType.Box25Kg: return "25";
                case UpsPackagingType.BoxExpressLarge: return "28";
                case UpsPackagingType.BoxExpressMedium: return "27";
                case UpsPackagingType.BoxExpressSmall: return "26";
                case UpsPackagingType.BPM: return "BPM";
                case UpsPackagingType.BPMFlats: return "BPF";
                case UpsPackagingType.BPMParcels: return "BPP";
                case UpsPackagingType.Custom: return "CP";
                case UpsPackagingType.FirstClassMail: return "FCL";
                case UpsPackagingType.Flats: return "Flats";        // per bryan@ups.com
                case UpsPackagingType.Irregulars: return "IRR";
                case UpsPackagingType.Letter: return "EE";
                case UpsPackagingType.Machinables: return "MAC";
                case UpsPackagingType.MediaMail: return "MML";
                case UpsPackagingType.Pak: return "PK";
                case UpsPackagingType.ParcelPost: return "PPT";
                case UpsPackagingType.Parcels: return "Parcels";    // per bryan@ups.com
                case UpsPackagingType.PriorityMail: return "PTY";
                case UpsPackagingType.StandardFlats: return "SFL";
                case UpsPackagingType.Tube: return "TB";
                case UpsPackagingType.ExpressEnvelope: return "EE";
                case UpsPackagingType.BoxExpress: return "UPS Express Box";
            }

            return "CP";
        }

        /// <summary>
        /// Get the worldship code for the given export reason
        /// </summary>
        private static string GetReasonForExportCode(UpsExportReason reason)
        {
            switch (reason)
            {
                case UpsExportReason.Gift: return "05";
                case UpsExportReason.InterCompanyData: return "08";
                case UpsExportReason.Sale: return "02";
                case UpsExportReason.Sample: return "06";
                case UpsExportReason.Repair: return "03";
                case UpsExportReason.Return: return "07";
            }

            // Other
            return "09";
        }

        /// <summary>
        /// Get the full path to the worldship executable
        /// </summary>
        public static string GetWorldShipExe()
        {
            string subKeyPathAndName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WorldShipTD.exe";
            string exePath = GetWorldShipExe(subKeyPathAndName);
            if (string.IsNullOrWhiteSpace(exePath))
            {
                subKeyPathAndName = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\App Paths\WorldShipTD.exe";
                exePath = GetWorldShipExe(subKeyPathAndName);
            }

            return exePath;
        }

        /// <summary>
        /// Get the full path to the worldship executable
        /// </summary>
        public static string GetWorldShipExe(string subKeyPathAndName)
        {
            string path = string.Empty;

            using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(subKeyPathAndName))
            {
                if (regKey != null)
                {
                    path = (string) regKey.GetValue("Path", "");

                    if (!string.IsNullOrEmpty(path))
                    {
                        path = Path.Combine(path, "WorldShipTD.exe");

                        if (!File.Exists(path))
                        {
                            path = string.Empty;
                        }
                    }
                }
            }

            return path;
        }

        /// <summary>
        /// Get the full path to the worldship executable
        /// </summary>
        public static string GetWorldShipNetworkPath()
        {
            string regKeyPath = @"SOFTWARE\UPS\Installation\";
            string regKeyWow64Path = @"SOFTWARE\Wow6432Node\UPS\Installation\";

            string networkPath = GetWorldShipNetworkPath(regKeyPath);

            if (string.IsNullOrWhiteSpace(networkPath))
            {
                networkPath = GetWorldShipNetworkPath(regKeyWow64Path);
            }

            return networkPath;
        }
        /// <summary>
        /// Get the full path to the worldship executable
        /// </summary>
        private static string GetWorldShipNetworkPath(string regKeyPath)
        {
            string path = string.Empty;

            using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(regKeyPath))
            {
                if (regKey != null)
                {
                    path = (string)regKey.GetValue("NetworkShareDir", "");

                    if (!string.IsNullOrEmpty(path))
                    {
                        if (!Directory.Exists(path))
                        {
                            path = string.Empty;
                        }
                    }
                }
            }

            return path;
        }

        /// <summary>
        /// Get the directory that the WorldShip executable is in
        /// </summary>
        public static string GetWorldShipExePath()
        {
            string worldShipExePath = string.Empty;

            // Get the full path to the exe
            string worldShipExeFullPath = GetWorldShipExe();

            // Make sure it's not blank
            if (!string.IsNullOrWhiteSpace(worldShipExeFullPath))
            {
                // Get the directory name
                worldShipExePath = Path.GetDirectoryName(worldShipExeFullPath);
            }

            return worldShipExePath;
        }

        /// <summary>
        /// Get the directory that the WorldShip executable is in
        /// </summary>
        public static string GetWorldShipIniPath()
        {
            string worldShipIniPath = string.Empty;

            // Get the full path to the ini
            string worldShipIniFullPath = GetWorldShipIni();

            // Make sure it's not blank
            if (!string.IsNullOrWhiteSpace(worldShipIniFullPath))
            {
                // Get the directory name
                worldShipIniPath = Path.GetDirectoryName(worldShipIniFullPath);
            }

            return worldShipIniPath;
        }

        /// <summary>
        /// Get the full path to the worldship executable
        /// </summary>
        public static string GetWorldShipIni()
        {
            string subKeyPathAndName = @"SOFTWARE\UPS\Installation";
            string ini = GetWorldShipIni(subKeyPathAndName);
            if (string.IsNullOrWhiteSpace(ini))
            {
                subKeyPathAndName = @"SOFTWARE\Wow6432Node\UPS\Installation";
                ini = GetWorldShipIni(subKeyPathAndName);
            }

            return ini;
        }

        /// <summary>
        /// Get the full path to the worldship executable
        /// </summary>
        public static string GetWorldShipIni(string subKeyPathAndName)
        {
            string path = string.Empty;

            using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(subKeyPathAndName))
            {
                if (regKey != null)
                {
                    path = (string)regKey.GetValue("ShipMain", "");

                    if (!File.Exists(path))
                    {
                        path = string.Empty;
                    }
                }
            }

            return path;
        }

        /// <summary>
        /// Gets the major version of WorldShip that is installed
        /// </summary>
        /// <returns>The major version of shippingSoftwareVersion in wstdShipmain.ini.  If wstdShipmain.ini cannot be found or an error occurs reading it,
        /// worldShipDefaultMajorVersion (15) will be returned.</returns>
        public static int WorldShipMajorVersion
        {
            get
            {
                if (worldShipMajorVersion != int.MinValue)
                {
                    return worldShipMajorVersion;
                }

                // Get the path of the main WorldShip exe
                string worldShipPath = GetWorldShipExePath();
                if (string.IsNullOrWhiteSpace(worldShipPath))
                {
                    // We didn't find WorldShip on this computer, but it could be installed on another computer
                    // So we will default to 2012
                    worldShipMajorVersion = worldShipDefaultMajorVersion;
                    return worldShipMajorVersion;
                }

                // Define the path/filename for the wstdShipmain.ini
                string wstdShipmainPath = Path.Combine(worldShipPath, "wstdShipmain.ini");

                // Default to worldShipDefaultMajorVersion
                string worldShipVersion = worldShipDefaultMajorVersion.ToString();

                // See if the ini file exists.  If it doesn't, we'll default to worldShipDefaultMajorVersion below
                if (File.Exists(wstdShipmainPath))
                {
                    // Read the software version for this install of WorldShip
                    IniFile wsIni = new IniFile(wstdShipmainPath);
                    worldShipVersion = wsIni.ReadValue("configuration", "shippingSoftwareVersion");
                }

                if (string.IsNullOrWhiteSpace(worldShipVersion) || !int.TryParse(worldShipVersion.Substring(0, 2), out worldShipMajorVersion))
                {
                    // Default to version worldShipDefaultMajorVersion
                    worldShipMajorVersion = worldShipDefaultMajorVersion;
                }

                return worldShipMajorVersion;
            }
        }

        /// <summary>
        /// Luanch worldship.  If an error occurs the message will be parented to the given owner window
        /// </summary>
        public static void LaunchWorldShip(IWin32Window errorOwner)
        {
            string exe = GetWorldShipExe();

            if (string.IsNullOrEmpty(exe))
            {
                MessageHelper.ShowWarning(errorOwner, "ShipWorks could not launch WorldShip since it could not be found on this computer.");
                return;
            }

            try
            {
                Process.Start(exe);
            }
            catch (Win32Exception ex)
            {
                log.Warn("ShipWorks was unable to launch WorldShip:\n\n", ex);
                MessageHelper.ShowWarning(errorOwner, "ShipWorks was unable to launch WorldShip:\n\n" + ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                log.Warn("ShipWorks was unable to launch WorldShip:\n\n", ex);
                MessageHelper.ShowWarning(errorOwner, "ShipWorks was unable to launch WorldShip:\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Locates a shipment's secondary tracking number (for USPS) if it is applicable
        /// </summary>
        public static void DetermineAlternateTracking(ShipmentEntity shipment, AlternateTrackingLoaded alternativeTracking)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (alternativeTracking == null)
            {
                throw new ArgumentNullException("alternativeTracking");
            }

            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) shipment.ShipmentType;
            if (shipmentTypeCode == ShipmentTypeCode.UpsWorldShip || shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
            {
                // we need to inspect the UPS-specific values, get them loaded
                ShippingManager.EnsureShipmentLoaded(shipment);

                if (UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
                {
                    alternativeTracking(shipment.Ups.UspsTrackingNumber, (UpsServiceType) shipment.Ups.Service);
                }
            }
        }

        /// <summary>
        /// Attempts to determine ShipmentIDs for WorldShipProcessed entities that do not have ShipmentIDs
        /// </summary>
        /// <param name="worldShipProcessedEntries">List of WorldShipProcessed entities to fix.</param>
        public static void FixInvalidShipmentIDs(List<WorldShipProcessedEntity> worldShipProcessedEntries)
        {
            long testShipmentID;
            foreach (WorldShipProcessedEntity wsp in worldShipProcessedEntries)
            {
                if (string.IsNullOrWhiteSpace(wsp.ShipmentID) || !long.TryParse(wsp.ShipmentID, out testShipmentID))
                {
                    WorldShipProcessedEntity wspToCopy = worldShipProcessedEntries.FirstOrDefault(x => x.WorldShipShipmentID == wsp.WorldShipShipmentID
                        && !string.IsNullOrWhiteSpace(x.ShipmentID)
                        && long.TryParse(x.ShipmentID, out testShipmentID)
                        && x.WorldShipProcessedID != wsp.WorldShipProcessedID);

                    wsp.ShipmentID = wspToCopy != null ? wspToCopy.ShipmentID : null;
                }
            }
        }
    }
}
