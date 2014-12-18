using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Templates;
using ShipWorks.Tests.Integration.MSTest.Utilities;
using ShipWorks.Users;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Stores;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Users.Audit;
using ShipWorks.Shipping;
using log4net;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Tests.Integration.MSTest.Fixtures
{
    public abstract class ShipWorksFixtureBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksFixtureBase));

        protected ShipWorksFixtureBase()
        {
            // Need to comment out Debug.Assert statements in ShipWorks.Data.Caching.EntityCacheChangeMonitor
            // to avoid errors resulting from an assertion that the MainForm is running

            Guid swInstance = ShipWorksInitializer.GetShipWorksInstance();

            if (ApplicationCore.ShipWorksSession.ComputerID == Guid.Empty)
            {

                ApplicationCore.ShipWorksSession.Initialize(swInstance);
                SqlSession.Initialize();

                Console.WriteLine(SqlSession.Current.Configuration.DatabaseName);
                Console.WriteLine(SqlSession.Current.Configuration.ServerInstance);

                DataProvider.InitializeForApplication();
                AuditProcessor.InitializeForApplication();

                AccountManagerInitializeForCurrentUser();

                ShippingSettings.InitializeForCurrentDatabase();
                ShippingProfileManager.InitializeForCurrentSession();
                ShippingDefaultsRuleManager.InitializeForCurrentSession();
                ShippingProviderRuleManager.InitializeForCurrentSession();

                StoreManager.InitializeForCurrentSession();

                UserManager.InitializeForCurrentUser();

                UserSession.InitializeForCurrentDatabase();

                if (!UserSession.Logon("shipworks", "shipworks", true))
                {
                    throw new Exception("A 'shipworks' account with password 'shipworks' needs to be created.");
                }
                ;

                ShippingManager.InitializeForCurrentDatabase();
                LogSession.Initialize();

                TemplateManager.InitializeForCurrentSession();
            }
        }

        #region Properties
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

        public string UspsEndorsement { get; set; }

        #endregion Properties

        /// <summary>
        /// Gets a ShipWorks AccountID for an account number
        /// </summary>
        /// <param name="accountNumber"></param>
        protected abstract long GetAccountId(string accountNumber);

        /// <summary>
        /// Gets a ShipWorks AccountID for a account number AND account country code
        /// </summary>
        protected abstract long GetFedExAccountId(string accountNumber, string countryCode);

        /// <summary>
        /// Initialize AccountManager for the current user
        /// </summary>
        protected abstract void AccountManagerInitializeForCurrentUser();

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

        /// <summary>
        /// Initialize any specific shipment fields
        /// </summary>
        /// <param name="shipment"></param>
        protected abstract void InitializeShipmentFields(ShipmentEntity shipment);

        protected abstract void ShipShipment(ShipmentEntity shipment);

        protected abstract decimal RateShipment(ShipmentEntity shipment);

        public virtual decimal GetRates()
        {
            decimal totalRates = -2;
            
            InterapptiveOnlyUtilities.UseListRates = RateRequestTypes == "LIST";

            ShipmentEntity shipment = CreateShipment();

            // If you want to create the shipments, but NOT process them, press the magic keys
            if (!MagicKeysDown)
            {
                totalRates = RateShipment(shipment);
            }

            shipment.CustomsGenerated = true;

            ShippingManager.SaveShipment(shipment);

            return totalRates;
        }

        public virtual bool Ship()
        {
            InterapptiveOnlyUtilities.UseListRates = RateRequestTypes == "LIST";

            ShipmentEntity shipment = CreateShipment();

            // If you want to create the shipments, but NOT process them, press the magic keys
            // This is helpful to get all the shipments into SW unprocessed so that you can process them with the UI
            if (!MagicKeysDown)
            {
                ShipShipment(shipment);

                shipment.Processed = true;
                shipment.ProcessedDate = DateTime.UtcNow;
                shipment.Voided = false;
                shipment.CustomsGenerated = true;
            }

            shipment.CustomsGenerated = true;

            ShippingManager.SaveShipment(shipment);

            return true;
        }

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
                    orderId = (long)command.ExecuteScalar();
                    command.Connection.Close();
                }
            }
            return orderId;
        }

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public virtual ShipmentEntity CreateShipment()
        {
            OrderEntity orderEntity = (OrderEntity)ShipWorksDataMethods.GetEntity(GetOrderId("US"));
            ShipmentEntity shipment =
                ShipWorksDataMethods.InternalCreateShipment(orderEntity, ShipmentTypeCode, PackageCount, ShipmentTotalWeight / PackageCount, PackageLineItemWeightUnits);

            InitializeShipmentFields(shipment);

            if (LabelType.ToUpperInvariant() == "IMAGE")
            {
                shipment.ActualLabelFormat = null;
            }
            else if (LabelType.ToUpperInvariant() == "ZPL")
            {
                shipment.ActualLabelFormat = (int)ThermalLanguage.ZPL;
            }
            else if (LabelType.ToUpperInvariant() == "EPL")
            {
                shipment.ActualLabelFormat = (int)ThermalLanguage.EPL;
            }

            // Set the ship date to now if the timestamp is not specified otherwise find the date of the day specified by the timestamp
            shipment.ShipDate = GetShipTimestamp();

            shipment.TotalWeight = ShipmentTotalWeight;

            shipment.BilledWeight = shipment.TotalWeight;
            shipment.BilledType = (int)BilledType.Unknown;

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

            SetPackageData(shipment);

            AddCustomerReferences(shipment);

            SetReturnShipmentData(shipment);

            // Save the record
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Save the shipment
                adapter.SaveAndRefetch(shipment);
                adapter.Commit();
            }

            return shipment;
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
    }
}
