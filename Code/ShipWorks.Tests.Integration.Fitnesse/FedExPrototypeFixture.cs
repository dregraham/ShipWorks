﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Win32;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Data;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Templates;
using ShipWorks.Users;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Stores;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Users.Audit;
using ShipWorks.Shipping;
using Interapptive.Shared.Business;
using ShipWorks.Users.Security;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Enums;

namespace ShipWorks.Tests.Integration.Fitnesse
{   
    public class FedExPrototypeFixture : fit.ColumnFixture
    {
        public FedExPrototypeFixture()
        {
            // Sleep to allow time to attach the debugger to runner.exe if needed
            //System.Threading.Thread.Sleep(30000);
            // Ctrl + Alt + Shift + Windows Key
            if (DebugKeysDown)
            {
                Debugger.Launch();
            }


            // Need to comment out Debug.Assert statements in ShipWorks.Data.Caching.EntityCacheChangeMonitor
            // to avoid errors resulting from an assertion that the MainForm is running

            Guid swInstance;
            switch (Environment.MachineName.ToLower())
            {
                case "tim-pc":
                    swInstance = Guid.Parse("{2D64FF9F-527F-47EF-BA24-ECBF526431EE}");
                    break;
                case "john-pc":
                    swInstance = Guid.Parse("{00000000-143F-4C2B-A80F-5CF0E121A909}");
                    break;
                case "kevin-pc":
                    swInstance = Guid.Parse("{7735A885-9127-44A9-BD87-2D06F6273945}");
                    break;
                case "fitnesse-vm":
                    swInstance = Guid.Parse("{3BAE47D1-6903-428B-BD9D-31864E614709}");
                    break;
                default:
                    throw new ApplicationException("Enter your machine and ShipWorks instance guid in FedExPrototypeFixture()");
            }

            ApplicationCore.ShipWorksSession.Initialize(swInstance);
            SqlSession.Initialize();

            Console.WriteLine(SqlSession.Current.Configuration.DatabaseName);
            Console.WriteLine(SqlSession.Current.Configuration.ServerInstance);

            DataProvider.InitializeForApplication();
            AuditProcessor.InitializeForApplication();

            FedExAccountManager.InitializeForCurrentUser();
            ShippingSettings.InitializeForCurrentDatabase();
            ShippingProfileManager.InitializeForCurrentUser();
            ShippingDefaultsRuleManager.InitializeForCurrentUser();
            ShippingProviderRuleManager.InitializeForCurrentUser();

            StoreManager.InitializeForCurrentUser();

            UserManager.InitializeForCurrentUser();

            UserSession.InitializeForCurrentDatabase();
            UserSession.Logon("shipworks", "shipworks", true);

            ShippingManager.InitializeForCurrentDatabase();
            LogSession.Initialize();

            TemplateManager.InitializeForCurrentUser();
        }

        public string FedExAccountNumber { get; set; }

        public string CustomerTransactionId { get; set; }
        public string ShipTimestamp { get; set; }
        public string DropoffType { get; set; }
        public string ShipmentServiceType { get; set; }
        public string PackagingType { get; set; }
        public string ShipmentWeightUnits { get; set; }
        public double ShipmentTotalWeightValue { get; set; }
        
        public string ShipperPersonName { get; set; }
        public string ShipperCompanyName { get; set; }
        public string ShipperPhoneNumber { get; set; }
        public string ShipperStreetLines { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperStateOrProvinceCode { get; set; }
        public string ShipperPostalCode { get; set; }
        public string ShipperCountryCode { get; set; }
        public string ShipperResidential { get; set; }

        public string RecipientCompanyName { get; set; }
        public string RecipientPersonName { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public string RecipientStreetLines { get; set; }
        public string RecipientCity { get; set; }
        public string RecipientStateOrProvinceCode { get; set; }
        public string RecipientPostalCode { get; set; }
        public string RecipientCountryCode { get; set; }
        public string RecipientResidential { get; set; }

        public string ResponsiblePartyPaymentType { get; set; }
        public string ResponsiblePartyAccountNumber { get; set; }
        public string ResponsiblePartyPersonName { get; set; }
        public string ResponsiblePartyCountryCode { get; set; }

        public string SpecialServiceType1 { get; set; }
        public string SpecialServiceType2 { get; set; }
        
        public string CodCollectionCurrency { get; set; }
        public string CodCollectionAmount { get; set; }
        public string CodDetailCollectionType { get; set; }
        public string CodTinType { get; set; }
        public string CodTinNumber { get; set; }
        public string CodPersonName { get; set; }
        public string CodTitle { get; set; }
        public string CodCompanyName { get; set; }
        public string CodPhoneNumber { get; set; }
        public string CodStreetLines { get; set; }
        public string CodCity { get; set; }
        public string CodStateOrProvinceCode { get; set; }
        public string CodPostalCode { get; set; }
        public string CodChargeBasis { get; set; }
        public string CodCountryCode { get; set; }
        public string CodResidential { get; set; }
        public string CodAccountNumber { get; set; }

        /// <summary>
        /// Hardcoded to invoice
        /// </summary>
        public string CodReferenceIndicator { get; set; }

        public string HoldPersonName { get; set; }
        public string HoldPhoneNumber { get; set; }
        public string HoldCompanyName { get; set; }
        public string HoldContactPhoneNumber { get; set; }
        public string HoldStreetLines { get; set; }
        public string HoldCity { get; set; }
        public string HoldStateOrProvinceCode { get; set; }
        public string HoldPostalCode { get; set; }
        public string HoldCountryCode { get; set; }
        public string HoldLocationType { get; set; }
        public string HoldDetailPhoneNumber { get; set; }

        public string EmailRecipientType { get; set; }
        public string EmailAddress { get; set; }
        public string EmailNotifyOnDelivery { get; set; }
        public string EmailFormat { get; set; }
        public string EmailLanguageCode { get; set; }

        public string ReturnType { get; set; }
        public string ReturnRmaNumber { get; set; }
        public string ReturnRmaReason { get; set; }
        public string EmailAllowedSpecialServices { get; set; }

        public string RateRequestTypes { get; set; }
        public int PackageCount { get; set; }

        public string PackageLineItemInsuredValueAmount { get; set; }
        public string PackageLineItemInsuredValueCurrency { get; set; }

        public string PackageLineItemWeightUnits { get; set; }
        public string PackageLineItemWeightValue { get; set; }
        public string PackageLineItemLength { get; set; }
        public string PackageLineItemWidth { get; set; }
        public string PackageLineItemHeight { get; set; }
        public string PackageLineItemUnits { get; set; }

        public string CustomerReferenceType { get; set; }
        public string CustomerReferenceValue { get; set; }

        public string PackageLineItemSpecialServiceType1 { get; set; }
        public string PackageLineItemSpecialServiceType2 { get; set; }
        public string PackageLineItemSpecialServiceType3 { get; set; }

        public string PackageLineItemPriorityEnhancementType { get; set; }
        public string PackageLineItemPriorityContent { get; set; }

        public string DryIceWeightUnits { get; set; }
        public double DryIceWeightValue { get; set; }

        public string DangerousGoodsAccessibility { get; set; }
        public string DangerousGoodsCargoAircraftOnly { get; set; }

        public string ShipmentSignatureOptionType { get; set; }
        public string ShipmentSignatureReleaseNumber { get; set; }

        public string FreightDetailPackingListEnclosed { get; set; }
        public string FreightDetailShippersLoadAndCount { get; set; }
        public string FreightDetailBookingConfirmationNumber { get; set; }

        public string LabelSpecificationLabelStockType { get; set; }

        public string SaveLabel { get; set; }


        /// <summary>
        /// Determines if a special key combination is active.  Can be used
        /// for enabling "hidden" (but not secure!) functionality.
        /// </summary>
        public static bool MagicKeysDown
        {
            get
            {
                return Control.ModifierKeys == (Keys.Control | Keys.Shift) &&
                    (NativeMethods.GetAsyncKeyState(Keys.LWin) & 0x8000) != 0;
            }
        }

        /// <summary>
        /// Determines if a special key combination is active.  Can be used
        /// for enabling "hidden" (but not secure!) functionality.
        /// </summary>
        public static bool DebugKeysDown
        {
            get
            {
                return Control.ModifierKeys == (Keys.Control | Keys.Shift | Keys.Alt) &&
                    (NativeMethods.GetAsyncKeyState(Keys.LWin) & 0x8000) != 0;
            }
        }

        public virtual bool Ship()
        {
            try
            {
                InterapptiveOnlyUtilities.UseListRates = RateRequestTypes == "LIST";
                
                ShipmentEntity shipment = CreateShipment();

                // If you want to create the shipments, but NOT process them, press the magic keys
                // This is helpful to get all the shipments into SW unprocessed so that you can process them with the UI
                if (!MagicKeysDown)
                {

                    FedExShippingClerk shippingClerk = new FedExShippingClerk();

                    shippingClerk.Ship(shipment);

                    shipment.ContentWeight = shipment.FedEx.Packages.Sum(p => p.DimsWeight);
                    shipment.Processed = true;
                    shipment.ProcessedDate = DateTime.UtcNow;
                    shipment.Voided = false;
                    shipment.CustomsGenerated = true;
                }

                shipment.CustomsGenerated = true;

                ShippingManager.SaveShipment(shipment);
                
                return true;
            }
            finally
            {

                CleanupLabel();

                // Reset values that don't appear in every test case since it appears that the 
                // value from the previous test doesn't carry over to the next test (the runner
                // doesn't create a new instance of the fixture for each run)
                ResetValues();
            }
        }

        private void CleanupLabel()
        {
            if (String.IsNullOrWhiteSpace(SaveLabel) || !SaveLabel.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                string certificationDirectory = LogSession.LogFolder + "\\FedExCertification\\";

                if (Directory.Exists(certificationDirectory))
                {
                    string[] filesToDelete = Directory.GetFiles(certificationDirectory, CustomerTransactionId + "*.png");
                    foreach (string fileToDelete in filesToDelete)
                    {
                        Console.WriteLine(string.Format("Deleting image file {0}", fileToDelete));
                        File.Delete(fileToDelete);
                    }
                }
            }
        }

        /// <summary>
        /// Grab a US ship to order id from the db
        /// </summary>
        /// <returns></returns>
        private long GetOrderId(string countryCode)
        {
            long orderId;
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandTimeout = 15;
                command.CommandType = CommandType.Text;
                using (SqlConnection connection = new SqlConnection(SqlSession.Current.Configuration.GetConnectionString()))
                {
                    command.Connection = connection;
                    command.Connection.Open();
                    command.CommandText = string.Format("select top 1 OrderId from [Order] where ShipCountryCode = '{0}'", countryCode);
                    orderId = (long) command.ExecuteScalar();
                    command.Connection.Close();
                }
            }
            return orderId;
        }

        /// <summary>
        /// Gets a ShipWorks FedExAccountID for a FedEx account number
        /// </summary>
        /// <param name="fedExAccountNumber"></param>
        protected long GetFedExAccountId(string fedExAccountNumber)
        {
            FedExAccountEntity accountEntity = FedExAccountManager.Accounts.FirstOrDefault(x => x.AccountNumber == fedExAccountNumber);
            if (accountEntity == null)
            {
                throw new InvalidDataException("Test account " + fedExAccountNumber + " not found in DB");
            }

            return accountEntity.FedExAccountID;
        }

        /// <summary>
        /// Gets a ShipWorks FedExAccountID for a FedEx account number AND account country code
        /// </summary>
        protected long GetFedExAccountId(string fedExAccountNumber, string countryCode)
        {
            FedExAccountEntity accountEntity = FedExAccountManager.Accounts
                .FirstOrDefault(x => x.AccountNumber == fedExAccountNumber && x.CountryCode.ToUpperInvariant() == countryCode.ToUpperInvariant());

            if (accountEntity == null)
            {
                throw new InvalidDataException(string.Format("Test account {0} with country code {1} not found in DB", fedExAccountNumber, countryCode));
            }

            return accountEntity.FedExAccountID;
        }

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public virtual ShipmentEntity CreateShipment()
        {
            OrderEntity orderEntity = (OrderEntity)ShipWorksDataMethods.GetEntity(GetOrderId("US"));
            ShipmentEntity shipment =
                ShipWorksDataMethods.InternalCreateShipment(orderEntity, ShipmentTypeCode.FedEx, PackageCount, ShipmentTotalWeightValue / PackageCount, PackageLineItemWeightUnits);

            // remove the default package that gets created in the ConfigureNewShipment method
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                FedExPackageEntity fedExPackageEntity = shipment.FedEx.Packages[0];
                shipment.FedEx.Packages.RemoveAt(0);
                adapter.DeleteEntity(fedExPackageEntity);

                // Save the shipment
                adapter.SaveAndRefetch(shipment);
                adapter.Commit();
            }

            shipment.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Pounds;

            shipment.FedEx.ReferencePO = CustomerTransactionId;
            shipment.FedEx.Service = (int)GetServiceType();

            SetPaymentInfo(shipment);

            shipment.FedEx.DropoffType = GetDropoffType();

            if (string.IsNullOrWhiteSpace(FedExAccountNumber))
            {
                FedExAccountNumber = ShipperCountryCode == "CA" ? "602611841" : "602344126";
            }

            // Default to the shipper country code
            shipment.FedEx.FedExAccountID = GetFedExAccountId(FedExAccountNumber, ShipperCountryCode); 

            // Set the ship date to now if the timestamp is not specified otherwise find the date of the day specified by the timestamp
            shipment.ShipDate = GetShipTimestamp();

            shipment.TotalWeight = ShipmentTotalWeightValue;

            shipment.OriginFirstName = ShipperPersonName;
            shipment.OriginMiddleName = string.Empty;
            shipment.OriginLastName = ShipperPersonName;
            shipment.OriginCompany = ShipperCompanyName;
            shipment.OriginNameParseStatus = 2;
            shipment.OriginPhone = ShipperPhoneNumber;
            shipment.OriginStreet1 = ShipperStreetLines;
            shipment.OriginStreet2 = string.Empty;
            shipment.OriginStreet3 = string.Empty;
            shipment.OriginCity = ShipperCity;
            shipment.OriginStateProvCode = ShipperStateOrProvinceCode;
            shipment.OriginPostalCode = ShipperPostalCode;
            shipment.OriginCountryCode = ShipperCountryCode;
            shipment.OriginEmail = string.Empty;
            shipment.OriginWebsite = string.Empty;
            shipment.OriginFax = string.Empty;
            shipment.OriginUnparsedName = ShipperPersonName;
            shipment.OriginOriginID = 1;

            shipment.ShipFirstName = RecipientPersonName;
            shipment.ShipMiddleName = string.Empty;
            shipment.ShipLastName = RecipientPersonName;
            shipment.ShipCompany = RecipientCompanyName;
            shipment.ShipNameParseStatus = 2;
            shipment.ShipUnparsedName = RecipientPersonName;
            shipment.ShipPhone = RecipientPhoneNumber;
            shipment.ShipStreet1 = RecipientStreetLines;
            shipment.ShipStreet2 = string.Empty;
            shipment.ShipStreet3 = string.Empty;
            shipment.ShipCity = RecipientCity;
            shipment.ShipStateProvCode = RecipientStateOrProvinceCode;
            shipment.ShipPostalCode = RecipientPostalCode;
            shipment.ShipCountryCode = RecipientCountryCode;
            shipment.ShipEmail = string.Empty;

            shipment.FedEx.HomeDeliveryPhone = string.Empty;
            shipment.FedEx.HomeDeliveryInstructions = string.Empty;
            shipment.FedEx.HomeDeliveryDate = DateTime.Today;

            shipment.ResidentialResult = !string.IsNullOrEmpty(RecipientResidential) && RecipientResidential.ToLower() == "true";
            if (shipment.ResidentialResult)
            {
                shipment.ResidentialDetermination = (int) ResidentialDeterminationType.Residential;
            }
            else
            {
                shipment.ResidentialDetermination = (int)ResidentialDeterminationType.Commercial;
            }

            string[] name = ShipperPersonName.Split(new char[] { ' ' }, 0);
            if (name.Length > 0)
            {
                shipment.OriginFirstName = name[0];
            }

            if (name.Length > 1)
            {
                shipment.OriginLastName = name[1];
            }

            if (RecipientPersonName == null)
            {
                RecipientPersonName = string.Empty;
            }

            name = RecipientPersonName.Split(new char[] { ' ' }, 0);
            if (name.Length > 0)
            {
                shipment.ShipFirstName = name[0];
            }

            if (name.Length > 1)
            {
                shipment.ShipLastName = name[1];
            }

            shipment.FedEx.PackagingType = (int)GetPackagingType();

            SetLinearUnits(shipment);
            SetShipmentSpecialServiceType(shipment, SpecialServiceType1);
            SetShipmentSpecialServiceType(shipment, SpecialServiceType2);

            SetEmailOptions(shipment);

            SetCodData(shipment);
            SetPackageData(shipment);

            AddCustomerReferences(shipment);
            SetSignatureOption(shipment);

            SetNonStandardContainer(shipment, PackageLineItemSpecialServiceType1);
            SetNonStandardContainer(shipment, PackageLineItemSpecialServiceType2);
            SetNonStandardContainer(shipment, PackageLineItemSpecialServiceType3);

            SetPriortyAlertData(shipment);
            SetDryIceData(shipment);
            SetDangerousGoodsData(shipment);
            SetHoldLocationData(shipment);

            SetAlcoholData(shipment);
            SetReturnShipmentData(shipment);

            SetupFreight(shipment);

            if (!string.IsNullOrEmpty(FreightDetailBookingConfirmationNumber))
            {
                shipment.FedEx.FreightBookingNumber = FreightDetailBookingConfirmationNumber;
            }

            // Save the record
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Save the shipment
                adapter.SaveAndRefetch(shipment);
                adapter.Commit();
            }

            return shipment;
        }

        private DateTime GetShipTimestamp()
        {
            DateTime shipTimestamp;

            if (!DateTime.TryParse(ShipTimestamp, out shipTimestamp))
            {
                shipTimestamp = string.IsNullOrEmpty(ShipTimestamp) ? DateTime.Now : GetNext(DateTime.Now, (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ShipTimestamp));    
            }

            return shipTimestamp;
        }

        private void SetReturnShipmentData(ShipmentEntity shipment)
        {
            if (!string.IsNullOrEmpty(ReturnType))
            {
                shipment.ReturnShipment = true;

                // The spreadsheet has return shipment addresses already swapped; since the manipulators swap the
                // addresses, we need to "re-swap" the addresses here, so the manipulators swap them back to
                // the addresses in the spreadsheet
                SwapAddressForReturns(shipment);

                if (ReturnType.ToLower() == "print_return_label")
                {
                    shipment.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;
                }
                else
                {
                    shipment.FedEx.ReturnType = (int) FedExReturnType.EmailReturnLabel;
                }
            }

            if (!string.IsNullOrWhiteSpace(ReturnRmaReason))
            {
                shipment.FedEx.RmaReason = ReturnRmaReason;
            }

            if (!string.IsNullOrWhiteSpace(ReturnRmaNumber))
            {
                shipment.FedEx.RmaNumber = ReturnRmaNumber;
            }
        }


        private void SwapAddressForReturns(ShipmentEntity shipment)
        {
            if (shipment.ReturnShipment)
            {
                shipment.ShipFirstName = ShipperPersonName;
                shipment.ShipMiddleName = string.Empty;
                shipment.ShipLastName = ShipperPersonName;
                shipment.ShipCompany = ShipperCompanyName;
                shipment.ShipNameParseStatus = 2;
                shipment.ShipPhone = ShipperPhoneNumber;
                shipment.ShipStreet1 = ShipperStreetLines;
                shipment.ShipStreet2 = string.Empty;
                shipment.ShipStreet3 = string.Empty;
                shipment.ShipCity = ShipperCity;
                shipment.ShipStateProvCode = ShipperStateOrProvinceCode;
                shipment.ShipPostalCode = ShipperPostalCode;
                shipment.ShipCountryCode = ShipperCountryCode;
                shipment.ShipEmail = string.Empty;
                shipment.ShipUnparsedName = ShipperPersonName;


                shipment.OriginFirstName = RecipientPersonName;
                shipment.OriginMiddleName = string.Empty;
                shipment.OriginLastName = RecipientPersonName;
                shipment.OriginCompany = RecipientCompanyName;
                shipment.OriginNameParseStatus = 2;
                shipment.OriginUnparsedName = RecipientPersonName;
                shipment.OriginPhone = RecipientPhoneNumber;
                shipment.OriginStreet1 = RecipientStreetLines;
                shipment.OriginStreet2 = string.Empty;
                shipment.OriginStreet3 = string.Empty;
                shipment.OriginCity = RecipientCity;
                shipment.OriginStateProvCode = RecipientStateOrProvinceCode;
                shipment.OriginPostalCode = RecipientPostalCode;
                shipment.OriginCountryCode = RecipientCountryCode;
                shipment.OriginEmail = string.Empty;
                shipment.OriginWebsite = string.Empty;
                shipment.OriginFax = string.Empty;
            }
        }

        /// <summary>
        /// Iterates through each property, in our ShipWorks fitnesse objects, setting each to null
        /// </summary>
        public virtual void ResetValues()
        {
            Type type = this.GetType();
            PropertyInfo[] properties = (from c in type.GetProperties()
                                         where c.DeclaringType.FullName.ToUpperInvariant().Contains("SHIPWORKS")
                                            && c.Name.ToUpperInvariant() != "MagicKeysDown".ToUpperInvariant()
                                            && c.Name.ToUpperInvariant() != "DebugKeysDown".ToUpperInvariant()
                                         select c).ToArray();

            foreach (PropertyInfo item in properties)
            {
                try
                {
                    item.SetValue(this, null, null);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    throw;
                }
            }
        }

        protected virtual void SetLinearUnits(ShipmentEntity shipment)
        {
            shipment.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.IN;
        }

        protected virtual void SetHoldLocationData(ShipmentEntity shipment)
        {
            if (!string.IsNullOrEmpty(HoldLocationType))
            {
                shipment.FedEx.HoldLocationType = (int) GetLocationType();
                
                shipment.FedEx.HoldCity = HoldCity;
                shipment.FedEx.HoldCompanyName = HoldCompanyName;
                shipment.FedEx.HoldCountryCode = HoldCountryCode;
                shipment.FedEx.HoldEmailAddress = null;
                shipment.FedEx.HoldFaxNumber = null;
                shipment.FedEx.HoldLocationId = string.Empty;
                shipment.FedEx.HoldPagerNumber = null;
                shipment.FedEx.HoldPersonName = HoldPersonName;
                shipment.FedEx.HoldPhoneExtension = null;
                shipment.FedEx.HoldPhoneNumber = HoldPhoneNumber;
                shipment.FedEx.HoldPostalCode = HoldPostalCode;
                shipment.FedEx.HoldResidential = null;
                shipment.FedEx.HoldStateOrProvinceCode = HoldStateOrProvinceCode;
                shipment.FedEx.HoldStreet1 = HoldStreetLines;
                shipment.FedEx.HoldStreet2 = string.Empty;
                shipment.FedEx.HoldStreet3 = string.Empty;
                shipment.FedEx.HoldTitle = string.Empty;
                shipment.FedEx.HoldUrbanizationCode = null;                

                shipment.FedEx.FedExHoldAtLocationEnabled = true;
            }
        }

        private FedExLocationType GetLocationType()
        {
            // TODO: Add additional types as test cases dictate
            switch (HoldLocationType.ToLower())
            {
                case "fedex_express_station": return FedExLocationType.FedExExpressStation;
            }

            throw new InvalidOperationException("Need to add another case to the GetLocationType switch statement in the test fixture");
        }

        private void InitializePackage(FedExPackageEntity package)
        {
            if (package.Weight <= 0)
            {
                package.Weight = package.DimsWeight;
            }

            package.DimsProfileID = 0;
            package.SkidPieces = 0;
            package.InsurancePennyOne = false;
            package.TrackingNumber = string.Empty;

            package.PriorityAlertEnhancementType = 0;
            package.PriorityAlertDetailContent = string.Empty;
            package.PriorityAlert = false;

            package.DryIceWeight = 0;
            package.DeclaredValue = 0M;

            package.DimsLength = 0;
            package.DimsHeight = 0;
            package.DimsWidth = 0;
            package.DimsWeight = 0;
            package.DimsAddWeight = false;

            package.Insurance = false;
            package.InsuranceValue = 0M;

            package.DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.LithiumBatteries;
            package.DangerousGoodsPackagingCount = 0;
            package.DangerousGoodsOfferor = string.Empty;
            package.DangerousGoodsEnabled = false;
            package.DangerousGoodsEmergencyContactPhone = string.Empty;
            package.DangerousGoodsCargoAircraftOnly = false;
            package.DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Accessible;

            package.HazardousMaterialClass = string.Empty;
            package.HazardousMaterialNumber = string.Empty;
            package.HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.Default;
            package.HazardousMaterialProperName = string.Empty;
            package.HazardousMaterialQuanityUnits = (int) FedExHazardousMaterialsQuantityUnits.Kilogram;
            package.HazardousMaterialQuantityValue = 0;
        }

        /// <summary>
        /// Gets the formatted responsible party number.
        /// </summary>
        /// <returns></returns>
        private string GetFormattedResponsiblePartyNumber()
        {
            string accountNumber = ResponsiblePartyAccountNumber;

            if (accountNumber.ToLower() == "please use shipper account number")
            {
                // Return the fedex account number of our account
                accountNumber = "224813333";
            }

            return accountNumber;
        }

        /// <summary>
        /// Sets the payment info.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetPaymentInfo(ShipmentEntity shipment)
        {
            FedExPayorType payorType = GetPaymentType(ResponsiblePartyPaymentType);
            shipment.FedEx.PayorTransportType = (int) payorType;

            if (payorType == FedExPayorType.Sender)
            {
                shipment.FedEx.PayorDutiesAccount = GetFormattedResponsiblePartyNumber();
                shipment.FedEx.PayorDutiesName = ResponsiblePartyPersonName;
            }
            else
            {
                // TODO: Use account number from spreadsheet
                // The account numbers provided in the spreadsheet for recipient are invalid; need to
                // get updated account numbers from FedEx; until then just send a hard coded account
                // number that we know works
                shipment.FedEx.PayorTransportAccount = GetFormattedResponsiblePartyNumber();
            }
            
            
        }
        /// <summary>
        /// Sets the alcohol data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetAlcoholData(ShipmentEntity shipment)
        {
            foreach (FedExPackageEntity package in shipment.FedEx.Packages)
            {
                // TODO: populate based on test data
                package.ContainsAlcohol = false;
            }
        }

        /// <summary>
        /// Gets the type of the dropoff.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Unrecognized drop off type in test fixture.</exception>
        private int GetDropoffType()
        {
            switch (DropoffType.ToLower())
            {
                case "regular_pickup":
                    return (int) FedExDropoffType.RegularPickup;
                case "station":
                    return (int) FedExDropoffType.Station;
                case "request_courier":
                    return (int) FedExDropoffType.RequestCourier;
                case "business_service_center":
                    return (int) FedExDropoffType.BusinessServiceCenter;
            }

            throw new InvalidOperationException("Unrecognized drop off type in test fixture.");
        }

        /// <summary>
        /// Sets the priorty alert data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetPriortyAlertData(ShipmentEntity shipment)
        {
            if (PackageLineItemSpecialServiceType1 == "PRIORITY_ALERT" || PackageLineItemSpecialServiceType2 == "PRIORITY_ALERT" || PackageLineItemSpecialServiceType3 == "PRIORITY_ALERT")
            {
                foreach (FedExPackageEntity package in shipment.FedEx.Packages)
                {
                    package.PriorityAlert = true;
                    package.PriorityAlertDetailContent = PackageLineItemPriorityContent;
                    
                    switch (PackageLineItemPriorityEnhancementType)
                    {
                        case null:
                        case "":
                            package.PriorityAlertEnhancementType = (int) Shipping.Carriers.FedEx.Api.v2013.Enums.FedExPriorityAlertEnhancementType.None;
                            break;
                        default:
                            package.PriorityAlertEnhancementType = (int)Shipping.Carriers.FedEx.Api.v2013.Enums.FedExPriorityAlertEnhancementType.PriorityAlertPlus;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the dry ice data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetDryIceData(ShipmentEntity shipment)
        {
            if (DryIceWeightValue > 0)
            {
                foreach (FedExPackageEntity package in shipment.FedEx.Packages)
                {
                    // convert KG to LBS
                    package.DryIceWeight = DryIceWeightValue*2.2046;
                }
            }
        }

        /// <summary>
        /// Sets the dangerous goods data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetDangerousGoodsData(ShipmentEntity shipment)
        {
            if ((SpecialServiceType1 != null && SpecialServiceType1.ToLower() == "dangerous_goods") || 
                (SpecialServiceType2 != null && SpecialServiceType2.ToLower() == "dangerous_goods") ||
                (PackageLineItemSpecialServiceType1 != null && PackageLineItemSpecialServiceType1.ToLower() == "dangerous_goods") ||
                (PackageLineItemSpecialServiceType2 != null && PackageLineItemSpecialServiceType2.ToLower() == "dangerous_goods") ||
                (PackageLineItemSpecialServiceType3 != null && PackageLineItemSpecialServiceType3.ToLower() == "dangerous_goods"))
                
            {
                bool cargoAircraftOnly = DangerousGoodsCargoAircraftOnly == "1";
                FedExDangerousGoodsAccessibilityType accessibility = GetFedExDangerousGoodsAccessibilityType();

                foreach (FedExPackageEntity package in shipment.FedEx.Packages)
                {
                    package.DangerousGoodsEnabled = true;
                    package.DangerousGoodsAccessibilityType = (int) accessibility;
                    package.DangerousGoodsCargoAircraftOnly = cargoAircraftOnly;

                    // default the material type to not applicable
                    package.DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.NotApplicable;
                }
            }
        }

        /// <summary>
        /// Gets the type of the fed ex dangerous goods accessibility.
        /// </summary>
        /// <returns></returns>
        private FedExDangerousGoodsAccessibilityType GetFedExDangerousGoodsAccessibilityType()
        {
            FedExDangerousGoodsAccessibilityType accessibility = FedExDangerousGoodsAccessibilityType.NotApplicable;

            if (!string.IsNullOrEmpty(DangerousGoodsAccessibility))
            {
                accessibility = FedExDangerousGoodsAccessibilityType.Accessible;
                if (DangerousGoodsAccessibility.ToLower() == "inaccessible")
                {
                    accessibility = FedExDangerousGoodsAccessibilityType.Inaccessible;
                }
            }

            return accessibility;
        }

        /// <summary>
        /// Sets the package data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetPackageData(ShipmentEntity shipment)
        {
            for (int i = 0; i < PackageCount; i++)
            {
                FedExPackageEntity package = new FedExPackageEntity();
                InitializePackage(package);

                if (!string.IsNullOrWhiteSpace(PackageLineItemInsuredValueAmount))
                {
                    decimal amount = decimal.Parse(PackageLineItemInsuredValueAmount);
                    package.DeclaredValue = amount;
                    package.Insurance = true;
                }
                
                package.Weight = string.IsNullOrEmpty(PackageLineItemWeightValue) ? 0 : double.Parse(PackageLineItemWeightValue);
                
                if (!string.IsNullOrEmpty(PackageLineItemLength))
                {
                    package.DimsLength = double.Parse(PackageLineItemLength);
                }

                if (!string.IsNullOrEmpty(PackageLineItemHeight))
                {
                    package.DimsHeight = double.Parse(PackageLineItemHeight);
                }

                if (!string.IsNullOrEmpty(PackageLineItemWidth))
                {
                    package.DimsWidth = double.Parse(PackageLineItemWidth);
                }

                shipment.FedEx.Packages.Add(package);
            }

        }

        /// <summary>
        /// Sets the signature option.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetSignatureOption(ShipmentEntity shipment)
        {
            if (!string.IsNullOrEmpty(ShipmentSignatureOptionType))
            {
                switch (ShipmentSignatureOptionType.ToLower())
                {
                    case "direct":
                        shipment.FedEx.Signature = (int) FedExSignatureType.Direct;
                        break;
                    case "indirect":
                        shipment.FedEx.Signature = (int) FedExSignatureType.Direct;
                        break;
                   case "adult":
                        shipment.FedEx.Signature = (int) FedExSignatureType.Adult;
                        break;
                    case "no_signature_required":
                        shipment.FedEx.Signature = (int) FedExSignatureType.NoSignature;
                        break;
                    default:
                        shipment.FedEx.Signature = (int) FedExSignatureType.ServiceDefault;
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the non standard container.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="specialServiceType">Type of the special service.</param>
        private void SetNonStandardContainer(ShipmentEntity shipment, string specialServiceType)
        {
            if (!string.IsNullOrEmpty(specialServiceType))
            {
                if (specialServiceType.ToLower() == "non_standard_container")
                {
                    shipment.FedEx.NonStandardContainer = true;
                }
            }
        }

        /// <summary>
        /// Adds the customer references.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void AddCustomerReferences(ShipmentEntity shipment)
        {
            if (!string.IsNullOrWhiteSpace(CustomerReferenceType))
            {
                if (CustomerReferenceType.ToLower() == "customer_reference")
                {
                    shipment.FedEx.ReferenceCustomer = CustomerReferenceValue ?? string.Empty;
                }

                else if (CustomerReferenceType.ToLower() == "p_o_number")
                {
                    shipment.FedEx.ReferencePO = CustomerReferenceValue ?? string.Empty;
                }

                else if (CustomerReferenceType.ToLower() == "invoice_number")
                {
                    shipment.FedEx.ReferenceInvoice = CustomerReferenceValue ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// Sets the type of the shipment special service.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="serviceType">Type of the service.</param>
        private void SetShipmentSpecialServiceType(ShipmentEntity shipment, string serviceType)
        {
            if (!string.IsNullOrEmpty(serviceType))
            switch (serviceType.ToLower())
            {
                case "cod":
                case "cod each package":
                    shipment.FedEx.CodEnabled = true;
                    break;
                case "inside_delivery":
                    shipment.FedEx.FreightInsideDelivery = true;
                    break;
                case "saturday_delivery":
                    shipment.FedEx.SaturdayDelivery = true;
                    break;
            }
        }

        /// <summary>
        /// Sets the email options.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetEmailOptions(ShipmentEntity shipment)
        {
            // Reset all the values
            shipment.FedEx.EmailNotifyBroker = 0;
            shipment.FedEx.EmailNotifyMessage = null;
            shipment.FedEx.EmailNotifyOther = 0;
            shipment.FedEx.EmailNotifyOtherAddress = null;
            shipment.FedEx.EmailNotifyRecipient = 0;
            shipment.FedEx.EmailNotifySender = 0;
            shipment.FedEx.BrokerEmail = null;

            FedExEmailNotificationType notificationType = FedExEmailNotificationType.Ship;
            if (!string.IsNullOrEmpty(EmailNotifyOnDelivery) && EmailNotifyOnDelivery.ToLower() == "true")
            {
                notificationType = FedExEmailNotificationType.Deliver;
            }

            if (!string.IsNullOrEmpty(EmailRecipientType))
            {
                switch (EmailRecipientType.ToLower())
                {
                    case "shipper":
                        shipment.FedEx.EmailNotifySender = (int) notificationType;
                        shipment.OriginEmail = EmailAddress;
                        shipment.FedEx.EmailNotifyOtherAddress = EmailAddress;
                        break;
                    case "recipient":
                        shipment.FedEx.EmailNotifyRecipient = (int) notificationType;
                        shipment.ShipEmail = EmailAddress;
                        shipment.FedEx.EmailNotifyOtherAddress = EmailAddress;
                        break;
                    case "broker":
                        shipment.FedEx.EmailNotifyBroker = (int) notificationType;
                        shipment.FedEx.BrokerEmail = EmailAddress;
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the cod data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetCodData(ShipmentEntity shipment)
        {
            if ((PackageLineItemSpecialServiceType1 != null && PackageLineItemSpecialServiceType1.ToLower() == "cod") || 
                (PackageLineItemSpecialServiceType2 != null && PackageLineItemSpecialServiceType2.ToLower() == "cod") || 
                (PackageLineItemSpecialServiceType3 != null && PackageLineItemSpecialServiceType3.ToLower() == "cod"))
            {
                shipment.FedEx.CodEnabled = true;
            }

            if (shipment.FedEx.CodEnabled)
            {
                if (!string.IsNullOrEmpty(this.CodCollectionAmount))
                {
                    shipment.FedEx.CodAmount = decimal.Parse(this.CodCollectionAmount);
                }

                shipment.FedEx.CodPaymentType = (int) GetCodPaymentType();
                shipment.FedEx.CodCity = this.CodCity;
                shipment.FedEx.CodCompany = this.CodCompanyName;
                string[] personName = this.CodPersonName.Split(new char[] {' '}, 0);
                if (personName.Length > 0)
                {
                    shipment.FedEx.CodFirstName = personName[0];
                }

                if (personName.Length > 1)
                {
                    shipment.FedEx.CodLastName = personName[1];
                }
                
                shipment.FedEx.CodPhone = CodPhoneNumber;
                shipment.FedEx.CodPostalCode = CodPostalCode;
                shipment.FedEx.CodStateProvCode = CodStateOrProvinceCode;
                shipment.FedEx.CodStreet1 = CodStreetLines;
                shipment.FedEx.CodCountryCode = CodCountryCode;

                shipment.FedEx.CodAccountNumber = CodAccountNumber;
                shipment.FedEx.CodTIN = CodTinNumber;

                shipment.FedEx.CodChargeBasis = (int) GetCodChargeBasis();
            }
        }

        /// <summary>
        /// Setups the freight.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected void SetupFreight(ShipmentEntity shipment)
        {
            if (!string.IsNullOrWhiteSpace(FreightDetailBookingConfirmationNumber))
            {
                shipment.FedEx.FreightBookingNumber = FreightDetailBookingConfirmationNumber;
            }

            if (!string.IsNullOrEmpty(FreightDetailShippersLoadAndCount))
            {
                shipment.FedEx.FreightLoadAndCount = int.Parse(FreightDetailShippersLoadAndCount);
            }

            // TODO: Set the packing list enclosed on the shipment after it's been added to the entity
            if (!string.IsNullOrEmpty(FreightDetailPackingListEnclosed))
            {
                // shipment.FedEx.FreightPackingListEnclosed = FreightDetailPackingListEnclosed == "1";
            }
        }

        /// <summary>
        /// Gets the charge basis of the cod payment.
        /// </summary>
        private FedExCodAddTransportationChargeBasisType GetCodChargeBasis()
        {
            FedExCodAddTransportationChargeBasisType codChargeBasis = FedExCodAddTransportationChargeBasisType.NetCharge;
            if (!string.IsNullOrWhiteSpace(CodChargeBasis))
            {
                CodChargeBasis = CodChargeBasis.ToUpperInvariant();
                if (CodChargeBasis == "COD_SURCHARGE")
                {
                    codChargeBasis = FedExCodAddTransportationChargeBasisType.CodSurcharge;
                }
                else if (CodChargeBasis == "NET_FREIGHT")
                {
                    codChargeBasis = FedExCodAddTransportationChargeBasisType.NetFreight;
                }
                else if (CodChargeBasis == "TOTAL_CUSTOMER_CHARGE")
                {
                    codChargeBasis = FedExCodAddTransportationChargeBasisType.TotalCustomerCharge;
                }
            }

            return codChargeBasis;
        }

        /// <summary>
        /// Gets the type of the cod payment.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unrecognized COD collection type</exception>
        private FedExCodPaymentType GetCodPaymentType()
        {
            // Do contains to account for "Each Package" text in the tests
            if (CodDetailCollectionType.ToLower().Contains("any"))
            {
                return FedExCodPaymentType.Any;
            }

            if (CodDetailCollectionType.ToLower().Contains("guaranteed_funds"))
            {
                return FedExCodPaymentType.Secured;
            }

            if (CodDetailCollectionType.ToLower().Contains("cash"))
            {
                return FedExCodPaymentType.Unsecured;
            }
            
            throw new Exception("Unrecognized COD collection type");
        }

        /// <summary>
        /// Gets the next.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns></returns>
        private DateTime GetNext(DateTime from, DayOfWeek dayOfWeek)
        {
            DateTime date = new DateTime(from.Ticks);

            if (!string.IsNullOrWhiteSpace(SpecialServiceType1) && SpecialServiceType1.ToUpperInvariant() == "FUTURE_DAY_SHIPMENT")
            {
                if (date.DayOfWeek == dayOfWeek)
                {
                    date = date.AddDays(1);
                }
            }

            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
            }

            return date;
        }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unrecognized service type.</exception>
        private FedExServiceType GetServiceType()
        {
            switch (ShipmentServiceType)
            {
                case "PRIORITY_OVERNIGHT": return FedExServiceType.PriorityOvernight;
                case "STANDARD_OVERNIGHT": return FedExServiceType.StandardOvernight;
                case "FIRST_OVERNIGHT": return FedExServiceType.FirstOvernight;
                case "FEDEX_2_DAY": return FedExServiceType.FedEx2Day;
                case "FEDEX_EXPRESS_SAVER": return FedExServiceType.FedExExpressSaver;
                case "INTERNATIONAL_PRIORITY": return FedExServiceType.InternationalPriority;
                case "INTERNATIONAL_ECONOMY": return FedExServiceType.InternationalEconomy;
                case "INTERNATIONAL_FIRST": return FedExServiceType.InternationalFirst;
                case "FEDEX_1_DAY_FREIGHT": return FedExServiceType.FedEx1DayFreight;
                case "FEDEX_2_DAY_FREIGHT": return FedExServiceType.FedEx2DayFreight;
                case "FEDEX_3_DAY_FREIGHT": return FedExServiceType.FedEx3DayFreight;
                case "FEDEX_GROUND": return FedExServiceType.FedExGround;
                case "GROUND_HOME_DELIVERY": return FedExServiceType.GroundHomeDelivery;
                case "INTERNATIONAL_PRIORITY_FREIGHT": return FedExServiceType.InternationalPriorityFreight;
                case "INTERNATIONAL_ECONOMY_FREIGHT": return FedExServiceType.InternationalEconomyFreight;
                case "SMART_POST": return FedExServiceType.SmartPost;
                case "FEDEX_2_DAY_AM": return FedExServiceType.FedEx2DayAM;
            }

            throw new Exception("Unrecognized service type.");
        }

        /// <summary>
        /// Gets the type of the packaging.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unrecognized packaging type</exception>
        private FedExPackagingType GetPackagingType()
        {
            switch (PackagingType)
            {
                case "FEDEX_BOX": return FedExPackagingType.Box;
                case "FEDEX_10KG_BOX": return FedExPackagingType.Box10Kg;
                case "FEDEX_25KG_BOX": return FedExPackagingType.Box25Kg;
                case "YOUR_PACKAGING": return FedExPackagingType.Custom;
                case "FEDEX_ENVELOPE": return FedExPackagingType.Envelope;
                case "FEDEX_PAK": return FedExPackagingType.Pak;
                case "FEDEX_TUBE": return FedExPackagingType.Tube;
            }

            throw new Exception("Unrecognized packaging type");
        }

        /// <summary>
        /// Gets the type of the payment.
        /// </summary>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unrecognized payment type</exception>
        protected FedExPayorType GetPaymentType(string paymentType)
        {
            switch (paymentType.ToLower())
            {
                case "sender": return FedExPayorType.Sender;
                case "third_party": return FedExPayorType.ThirdParty;
                case "recipient": return FedExPayorType.Recipient;
                case "collect": return FedExPayorType.Collect;
            }

            throw new Exception("Unrecognized payment type");
        }
    }
}
