using System;
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
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
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



namespace ShipWorks.Tests.Integration.Fitnesse
{
    public abstract class ShipWorksFixtureBase : fit.ColumnFixture
    {
        public delegate void ShipShipment(ShipmentEntity shipment);

        public delegate decimal RateShipment(ShipmentEntity shipment);

        public ShipShipment RunShipShipment;

        public RateShipment RunRateShipment;

        public ShipWorksFixtureBase()
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
                    swInstance = Guid.Parse("{B88D0435-14A2-4CDD-BFF4-7765393480DD}");
                    break;
                case "fitnesse-vm":
                    swInstance = Guid.Parse("{3BAE47D1-6903-428B-BD9D-31864E614709}");
                    break;
                default:
                    throw new ApplicationException("Enter your machine and ShipWorks instance guid in iParcelPrototypeFixture()");
            }

            ApplicationCore.ShipWorksSession.Initialize(swInstance);
            SqlSession.Initialize();

            Console.WriteLine(SqlSession.Current.Configuration.DatabaseName);
            Console.WriteLine(SqlSession.Current.Configuration.ServerInstance);

            DataProvider.InitializeForApplication();
            AuditProcessor.InitializeForApplication();

            AccountManagerInitializeForCurrentUser();

            ShippingSettings.InitializeForCurrentDatabase();
            ShippingProfileManager.InitializeForCurrentUser();
            ShippingDefaultsRuleManager.InitializeForCurrentUser();
            ShippingProviderRuleManager.InitializeForCurrentUser();

            StoreManager.InitializeForCurrentUser();

            UserManager.InitializeForCurrentUser();

            UserSession.InitializeForCurrentDatabase();
            
            if(!UserSession.Logon("shipworks", "shipworks", true))
            {
                throw new Exception("A 'shipworks' account with password 'shipworks' needs to be created.");
            };

            ShippingManager.InitializeForCurrentDatabase();
            LogSession.Initialize();

            TemplateManager.InitializeForCurrentUser();
        }

        public virtual decimal GetRates()
        {
            decimal totalRates = -2;
            try
            {
                InterapptiveOnlyUtilities.UseListRates = RateRequestTypes == "LIST";

                ShipmentEntity shipment = CreateShipment();

                // If you want to create the shipments, but NOT process them, press the magic keys
                if (!MagicKeysDown)
                {
                    totalRates = RunRateShipment(shipment);
                }

                shipment.CustomsGenerated = true;

                ShippingManager.SaveShipment(shipment);

                return totalRates;
            }
            finally
            {
                // Reset values that don't appear in every test case since it appears that the 
                // value from the previous test doesn't carry over to the next test (the runner
                // doesn't create a new instance of the fixture for each run)
                ResetValues();
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
                    //iParcelShipmentType iParcelShipmentType = new iParcelShipmentType();

                    //iParcelShipmentType.ProcessShipment(shipment);

                    RunShipShipment(shipment);

                    shipment.ContentWeight = shipment.IParcel.Packages.Sum(p => p.DimsWeight);
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

        public string AccountID { get; set; }
        public string SaveLabel { get; set; }
        public string RateRequestTypes { get; set; }
        public string CarrierName { get; set; }
        public string ShipmentTransactionId { get; set; }
        public string NumberOfPackages { get; set; }
        public string TotalWeight { get; set; }
        public string PackageLineItemWeightUnits { get; set; }
        public double ShipmentTotalWeight 
        {
            get { return double.Parse(TotalWeight); }
        }
        public int PackageCount 
        {
            get { return int.Parse(NumberOfPackages); }
        }

        public ShipmentTypeCode ShipmentTypeCode { get; set; }
        public string ShipOnDay { get; set; }
        public string Service { get; set; }
        public string Voided { get; set; }
        public string OriginFirstName { get; set; }
        public string OriginLastName { get; set; }
        public string OriginCompany { get; set; }
        public string OriginStreet1 { get; set; }
        public string OriginStreet2 { get; set; }
        public string OriginStreet3 { get; set; }
        public string OriginCity { get; set; }
        public string OriginStateProvCode { get; set; }
        public string OriginPostalCode { get; set; }
        public string OriginCountryCode { get; set; }
        public string OriginPhone { get; set; }
        public string OriginEmail { get; set; }
        public string ShipFirstName { get; set; }
        public string ShipLastName { get; set; }
        public string ShipCompany { get; set; }
        public string ShipStreet1 { get; set; }
        public string ShipStreet2 { get; set; }
        public string ShipStreet3 { get; set; }
        public string ShipCity { get; set; }
        public string ShipStateProvCode { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountryCode { get; set; }
        public string ShipPhone { get; set; }
        public string ShipEmail { get; set; }
        public string ReturnShipment { get; set; }
        public string Insurance { get; set; }
        public string InsuranceProvider { get; set; }
        public string ReferenceCustomer { get; set; }
        public string WeightPerPackage { get; set; }
        public string HeightPerPackage { get; set; }
        public string LengthPerPackage { get; set; }
        public string WidthPerPackage { get; set; }
        public string InsuranceValuePerPackage { get; set; }
        public string InsurancePennyOne { get; set; }
        public string DeclaredValue { get; set; }
        public string ExpectedInsuranceValue { get; set; }
        public string ReturnType { get; set; }
        public string LabelType { get; set; }

        public string SkuAndQuantity { get; set; }


        public abstract void AccountManagerInitializeForCurrentUser();

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

        private void CleanupLabel()
        {
            if (String.IsNullOrWhiteSpace(SaveLabel) || !SaveLabel.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                string certificationDirectory = LogSession.LogFolder + string.Format("\\{0}Certification\\", CarrierName);

                if (Directory.Exists(certificationDirectory))
                {
                    string[] filesToDelete = Directory.GetFiles(certificationDirectory, ShipmentTransactionId + "*.png");
                    foreach (string fileToDelete in filesToDelete)
                    {
                        Console.WriteLine(string.Format("Deleting image file {0}", fileToDelete));
                        File.Delete(fileToDelete);
                    }
                }
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
                                            && c.Name.ToUpperInvariant() != "ShipmentTotalWeight".ToUpperInvariant()
                                            && c.Name.ToUpperInvariant() != "PackageCount".ToUpperInvariant()
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
                    orderId = (long)command.ExecuteScalar();
                    command.Connection.Close();
                }
            }
            return orderId;
        }

        /// <summary>
        /// Gets a ShipWorks AccountID for an account number
        /// </summary>
        /// <param name="fedExAccountNumber"></param>
        protected abstract long GetAccountId(string accountNumber);

        /// <summary>
        /// Gets a ShipWorks AccountID for a account number AND account country code
        /// </summary>
        protected abstract long GetFedExAccountId(string accountNumber, string countryCode);

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public virtual ShipmentEntity CreateShipment()
        {
            OrderEntity orderEntity = (OrderEntity)ShipWorksDataMethods.GetEntity(GetOrderId("US"));
            ShipmentEntity shipment =
                ShipWorksDataMethods.InternalCreateShipment(orderEntity, ShipmentTypeCode, PackageCount, ShipmentTotalWeight / PackageCount, PackageLineItemWeightUnits);

            // remove the default package that gets created in the ConfigureNewShipment method
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                IParcelPackageEntity fedExPackageEntity = shipment.IParcel.Packages[0];
                shipment.IParcel.Packages.RemoveAt(0);
                adapter.DeleteEntity(fedExPackageEntity);

                // Save the shipment
                adapter.SaveAndRefetch(shipment);
                adapter.Commit();
            }

            shipment.IParcel.Service = (int)GetServiceType();

            if (LabelType.ToUpperInvariant() == "IMAGE")
            {
                shipment.ThermalType = null;
            }
            else if (LabelType.ToUpperInvariant() == "ZPL")
            {
                shipment.ThermalType = (int) ThermalLabelType.ZPL;
            }
            else if (LabelType.ToUpperInvariant() == "EPL")
            {
                shipment.ThermalType = (int) ThermalLabelType.EPL;
            }
            
            //if (string.IsNullOrWhiteSpace(AccountID))
            //{
            //    AccountID = OriginCountryCode == "CA" ? "602611841" : "602344126";
            //}

            // Default to the Origin country code
            shipment.IParcel.IParcelAccountID = GetFedExAccountId(AccountID, ShipCountryCode);

            // Set the ship date to now if the timestamp is not specified otherwise find the date of the day specified by the timestamp
            shipment.ShipDate = GetShipTimestamp();

            shipment.TotalWeight = ShipmentTotalWeight;

            shipment.OriginFirstName = OriginFirstName;
            shipment.OriginMiddleName = string.Empty;
            shipment.OriginLastName = OriginLastName;
            shipment.OriginCompany = OriginCompany;
            shipment.OriginNameParseStatus = 2;
            shipment.OriginPhone = OriginPhone;
            shipment.OriginStreet1 = ShipStreet1;
            shipment.OriginStreet2 = ShipStreet2;
            shipment.OriginStreet3 = ShipStreet3;
            shipment.OriginCity = OriginCity;
            shipment.OriginStateProvCode = OriginStateProvCode;
            shipment.OriginPostalCode = OriginPostalCode;
            shipment.OriginCountryCode = OriginCountryCode;
            shipment.OriginEmail = OriginEmail;
            shipment.OriginWebsite = string.Empty;
            shipment.OriginFax = string.Empty;
            shipment.OriginUnparsedName = OriginFirstName + " " + OriginLastName;
            shipment.OriginOriginID = 1;

            shipment.ShipFirstName = ShipFirstName;
            shipment.ShipMiddleName = string.Empty;
            shipment.ShipLastName = ShipLastName;
            shipment.ShipCompany = ShipCompany;
            shipment.ShipNameParseStatus = 2;
            shipment.ShipUnparsedName = ShipFirstName + " " + ShipLastName;
            shipment.ShipPhone = ShipPhone;
            shipment.ShipStreet1 = ShipStreet1;
            shipment.ShipStreet2 = ShipStreet2;
            shipment.ShipStreet3 = ShipStreet3;
            shipment.ShipCity = ShipCity;
            shipment.ShipStateProvCode = ShipStateProvCode;
            shipment.ShipPostalCode = ShipPostalCode;
            shipment.ShipCountryCode = ShipCountryCode;
            shipment.ShipEmail = ShipEmail;

            //shipment.ResidentialResult = !string.IsNullOrEmpty(RecipientResidential) && RecipientResidential.ToLower() == "true";
            //if (shipment.ResidentialResult)
            //{
            //    shipment.ResidentialDetermination = (int)ResidentialDeterminationType.Residential;
            //}
            //else
            //{
            //    shipment.ResidentialDetermination = (int)ResidentialDeterminationType.Commercial;
            //}

            //string[] name = OriginPersonName.Split(new char[] { ' ' }, 0);
            //if (name.Length > 0)
            //{
            //    shipment.OriginFirstName = name[0];
            //}

            //if (name.Length > 1)
            //{
            //    shipment.OriginLastName = name[1];
            //}

            //if (RecipientPersonName == null)
            //{
            //    RecipientPersonName = string.Empty;
            //}

            //name = RecipientPersonName.Split(new char[] { ' ' }, 0);
            //if (name.Length > 0)
            //{
            //    shipment.ShipFirstName = name[0];
            //}

            //if (name.Length > 1)
            //{
            //    shipment.ShipLastName = name[1];
            //}

            //shipment.IParcel.PackagingType = (int)GetPackagingType();

            //SetLinearUnits(shipment);
            //SetShipmentSpecialServiceType(shipment, SpecialServiceType1);
            //SetShipmentSpecialServiceType(shipment, SpecialServiceType2);

            //SetEmailOptions(shipment);

            //SetCodData(shipment);
            SetPackageData(shipment);

            AddCustomerReferences(shipment);
            //SetSignatureOption(shipment);

            //SetNonStandardContainer(shipment, PackageLineItemSpecialServiceType1);
            //SetNonStandardContainer(shipment, PackageLineItemSpecialServiceType2);
            //SetNonStandardContainer(shipment, PackageLineItemSpecialServiceType3);

            //SetPriortyAlertData(shipment);
            //SetDryIceData(shipment);
            //SetDangerousGoodsData(shipment);
            //SetHoldLocationData(shipment);

            //SetAlcoholData(shipment);
            SetReturnShipmentData(shipment);

            //SetupFreight(shipment);

            //if (!string.IsNullOrEmpty(FreightDetailBookingConfirmationNumber))
            //{
            //    shipment.IParcel.FreightBookingNumber = FreightDetailBookingConfirmationNumber;
            //}

            // Save the record
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Save the shipment
                adapter.SaveAndRefetch(shipment);
                adapter.Commit();
            }

            return shipment;
        }

        /// <summary>
        /// Sets the package data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected abstract void SetPackageData(ShipmentEntity shipment);

        /// <summary>
        /// Adds the customer references.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected abstract void AddCustomerReferences(ShipmentEntity shipment);

        private void SetReturnShipmentData(ShipmentEntity shipment)
        {
            if (!string.IsNullOrEmpty(ReturnType))
            {
                shipment.ReturnShipment = true;

                // The spreadsheet has return shipment addresses already swapped; since the manipulators swap the
                // addresses, we need to "re-swap" the addresses here, so the manipulators swap them back to
                // the addresses in the spreadsheet
                SwapAddressForReturns(shipment);
            }
        }


        private void SwapAddressForReturns(ShipmentEntity shipment)
        {
            if (shipment.ReturnShipment)
            {
                shipment.ShipFirstName = OriginFirstName;
                shipment.ShipMiddleName = string.Empty;
                shipment.ShipLastName = OriginLastName;
                shipment.ShipCompany = OriginCompany;
                shipment.ShipNameParseStatus = 2;
                shipment.ShipPhone = OriginPhone;
                shipment.ShipStreet1 = OriginStreet1;
                shipment.ShipStreet2 = OriginStreet2;
                shipment.ShipStreet3 = OriginStreet3;
                shipment.ShipCity = OriginCity;
                shipment.ShipStateProvCode = OriginStateProvCode;
                shipment.ShipPostalCode = OriginPostalCode;
                shipment.ShipCountryCode = OriginCountryCode;
                shipment.ShipEmail = OriginEmail;
                shipment.ShipUnparsedName = OriginFirstName + " " + OriginLastName;


                shipment.OriginFirstName = ShipFirstName;
                shipment.OriginMiddleName = string.Empty;
                shipment.OriginLastName = ShipLastName;
                shipment.OriginCompany = ShipCompany;
                shipment.OriginNameParseStatus = 2;
                shipment.OriginUnparsedName = ShipFirstName + " " + ShipLastName;
                shipment.OriginPhone = ShipPhone;
                shipment.OriginStreet1 = ShipStreet1;
                shipment.OriginStreet2 = ShipStreet2;
                shipment.OriginStreet3 = ShipStreet3;
                shipment.OriginCity = ShipCity;
                shipment.OriginStateProvCode = ShipStateProvCode;
                shipment.OriginPostalCode = ShipPostalCode;
                shipment.OriginCountryCode = ShipCountryCode;
                shipment.OriginEmail = ShipEmail;
                shipment.OriginWebsite = string.Empty;
                shipment.OriginFax = string.Empty;
            }
        }

        private DateTime GetShipTimestamp()
        {
            DateTime shipTimestamp;

            if (!DateTime.TryParse(ShipOnDay, out shipTimestamp))
            {
                shipTimestamp = string.IsNullOrEmpty(ShipOnDay) ? DateTime.Now : GetNext(DateTime.Now, (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ShipOnDay));
            }

            return shipTimestamp;
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
        private iParcelServiceType GetServiceType()
        {
            switch (Service)
            {
                case "112": 
                case "0":
                    return iParcelServiceType.Immediate;
                case "115": return iParcelServiceType.Preferred;
                case "1": return iParcelServiceType.Saver;
                case "2": return iParcelServiceType.SaverDeferred;
            }

            throw new Exception("Unrecognized service type.");
        }
    }
}
