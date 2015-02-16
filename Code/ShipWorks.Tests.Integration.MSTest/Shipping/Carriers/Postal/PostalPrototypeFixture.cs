using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Win32;
using System.Xml.Linq;
using log4net;
using log4net.Core;
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
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Postal
{   
    public abstract class PostalPrototypeFixture
    {
        private readonly Mock<ExecutionMode> executionMode;

        public PostalPrototypeFixture()
        {
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Error;
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);

            // Sleep to allow time to attach the debugger to runner.exe if needed
            //System.Threading.Thread.Sleep(30000);
            // Ctrl + Alt + Shift + Windows Key
            if (DebugKeysDown)
            {
                Debugger.Launch();
            }

            // Need to comment out Debug.Assert statements in ShipWorks.Data.Caching.EntityCacheChangeMonitor
            // to avoid errors resulting from an assertion that the MainForm is running

            Guid swInstance = ShipWorksInitializer.GetShipWorksInstance();

            // Mock an execution mode for the various dependencies that use the execution
            // mode to determine at whether the UI is running
            executionMode = new Mock<ExecutionMode>();
            executionMode.Setup(m => m.IsUIDisplayed).Returns(false);
            executionMode.Setup(m => m.IsUISupported).Returns(true);

            if (ApplicationCore.ShipWorksSession.ComputerID == Guid.Empty)
            {
                ApplicationCore.ShipWorksSession.Initialize(swInstance);
                SqlSession.Initialize();

                Console.WriteLine(SqlSession.Current.Configuration.DatabaseName);
                Console.WriteLine(SqlSession.Current.Configuration.ServerInstance);

                DataProvider.InitializeForApplication(executionMode.Object);
                AuditProcessor.InitializeForApplication();

                StampsAccountManager.InitializeForCurrentSession();
                ShippingSettings.InitializeForCurrentDatabase();
                ShippingProfileManager.InitializeForCurrentSession();
                ShippingDefaultsRuleManager.InitializeForCurrentSession();
                ShippingProviderRuleManager.InitializeForCurrentSession();

                StoreManager.InitializeForCurrentSession();

                UserManager.InitializeForCurrentUser();

                UserSession.InitializeForCurrentDatabase(executionMode.Object);
                UserSession.Logon("shipworks", "shipworks", true);

                ShippingManager.InitializeForCurrentDatabase();
                LogSession.Initialize();

                TemplateManager.InitializeForCurrentSession();

                ShippingOriginManager.InitializeForCurrentSession();
            }
        }

        public string TestID { get; set; }

        // Shipment fields
        public string ShipmentTypeCode { get; set; }
        public string TotalWeight { get; set; }
        public string Service { get; set; }
        public string ShipDate { get; set; }
        public string OriginFirstName { get; set; }
        public string OriginMiddleName { get; set; }
        public string OriginLastName { get; set; }
        public string OriginCompany { get; set; }
        public string OriginNameParseStatus { get; set; }
        public string OriginPhone { get; set; }
        public string OriginStreet1 { get; set; }
        public string OriginStreet2 { get; set; }
        public string OriginStreet3 { get; set; }
        public string OriginCity { get; set; }
        public string OriginStateProvCode { get; set; }
        public string OriginPostalCode { get; set; }
        public string OriginCountryCode { get; set; }
        public string OriginFax { get; set; }
        public string OriginEmail { get; set; }
        public string OriginWebsite { get; set; }
        public string OriginUnparsedName { get; set; }
        public string ShipFirstName { get; set; }
        public string ShipMiddleName { get; set; }
        public string ShipLastName { get; set; }
        public string ShipCompany { get; set; }
        public string ShipNameParseStatus { get; set; }
        public string ShipUnparsedName { get; set; }
        public string ShipPhone { get; set; }
        public string ShipStreet1 { get; set; }
        public string ShipStreet2 { get; set; }
        public string ShipStreet3 { get; set; }
        public string ShipCity { get; set; }
        public string ShipStateProvCode { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountryCode { get; set; }
        public string ShipEmail { get; set; }
        public string ResidentialDetermination { get; set; }
        public string ResidentialResult { get; set; }
        public string OriginOriginID { get; set; }
        public string CustomsItems { get; set; }

        // Postal fields
        public string ShipmentType { get; set; }
        public string ContentWeight { get; set; }
        public string CustomsValue { get; set; }
        public string RequestedLabelFormat { get; set; }
        public string ReturnShipment { get; set; }
        public string Insurance { get; set; }
        public string InsuranceProvider { get; set; }
        public string Voided { get; set; }
        public string Confirmation { get; set; }
        public string PackagingType { get; set; }
        public string DimsLength { get; set; }
        public string DimsWidth { get; set; }
        public string DimsHeight { get; set; }
        public string EntryFacility { get; set; }
        public string SortType { get; set; }
        public string ExpressSignatureWaiver { get; set; }
        public string InsuranceValue { get; set; }
        public string CustomsContentDescription { get; set; }
        public string CustomsContentType { get; set; }
        public string NonMachinable { get; set; }
        public string NonRectangular { get; set; }
        public string DimsAddWeight { get; set; }
        public string DimsWeight { get; set; }

        public string LabelSpecificationLabelStockType { get; set; }

        public string SaveLabel { get; set; }

        /// <summary>
        /// Gets a value indicating whether save label is "true"
        /// </summary>
        public bool IsSaveLabel
        {
            get
            {
                return (String.IsNullOrWhiteSpace(SaveLabel) || SaveLabel.Equals("true", StringComparison.InvariantCultureIgnoreCase));
            }
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

        public abstract bool Ship(UspsResellerType stampsResellerType);

        protected abstract void CleanupLabel();

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
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public virtual ShipmentEntity CreateShipment()
        {
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) Convert.ToInt16(ShipmentType);
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipmentTypeCode);

            OrderEntity orderEntity = (OrderEntity)ShipWorksDataMethods.GetEntity(GetOrderId("US"));
            ShipmentEntity shipment = ShipWorksDataMethods.InternalCreateShipment(orderEntity, shipmentTypeCode, 2, Convert.ToDouble(TotalWeight), "LB");

            shipment.Postal.Service = Convert.ToInt16(Service);

            // Set the ship date to now if the timestamp is not specified otherwise find the date of the day specified by the timestamp
            shipment.ShipDate = DateTime.Now.AddHours(2).ToUniversalTime();

            shipment.TotalWeight = Convert.ToDouble(TotalWeight);

            shipment.BilledWeight = shipment.TotalWeight;
            shipment.BilledType = (int) BilledType.Unknown;

            shipment.ContentWeight = Convert.ToDouble(ContentWeight);
            shipment.CustomsValue = Convert.ToDecimal(CustomsValue);
            shipment.RequestedLabelFormat = Convert.ToInt16(RequestedLabelFormat);
            shipment.ReturnShipment = Convert.ToInt16(ReturnShipment) == 1;
            shipment.Insurance = Convert.ToInt16(Insurance) == 1;
            shipment.InsuranceProvider = Convert.ToInt16(InsuranceProvider);
            shipment.Voided = Convert.ToInt16(Voided) == 1;
            shipment.OriginFirstName = OriginFirstName;
            shipment.OriginMiddleName = OriginMiddleName;
            shipment.OriginLastName = OriginLastName;
            shipment.OriginCompany = OriginCompany;
            shipment.OriginNameParseStatus = Convert.ToInt16(OriginNameParseStatus);
            shipment.OriginPhone = OriginPhone;
            shipment.OriginStreet1 = OriginStreet1;
            shipment.OriginStreet2 = OriginStreet2;
            shipment.OriginStreet3 = OriginStreet3;
            shipment.OriginCity = OriginCity;
            shipment.OriginStateProvCode = OriginStateProvCode;
            shipment.OriginPostalCode = OriginPostalCode;
            shipment.OriginCountryCode = OriginCountryCode;
            shipment.OriginEmail = OriginEmail;
            shipment.OriginWebsite = OriginWebsite;
            shipment.OriginFax = OriginFax;
            shipment.OriginUnparsedName = OriginUnparsedName;
            shipment.OriginOriginID = Convert.ToInt64(OriginOriginID);

            shipment.ShipFirstName = ShipFirstName + " " + TestID;
            shipment.ShipMiddleName = ShipMiddleName;
            shipment.ShipLastName = ShipLastName;
            shipment.ShipCompany = ShipCompany;
            shipment.ShipNameParseStatus = Convert.ToInt16(ShipNameParseStatus);
            shipment.ShipUnparsedName = ShipUnparsedName;
            shipment.ShipPhone = ShipPhone;
            shipment.ShipStreet1 = ShipStreet1;
            shipment.ShipStreet2 = ShipStreet2;
            shipment.ShipStreet3 = ShipStreet3;
            shipment.ShipCity = ShipCity;
            shipment.ShipStateProvCode = ShipStateProvCode;
            shipment.ShipPostalCode = ShipPostalCode;
            shipment.ShipCountryCode = ShipCountryCode;
            shipment.ShipEmail = ShipEmail;

            shipment.ResidentialResult = Convert.ToInt16(ResidentialResult) == 1;
            shipment.ResidentialDetermination = Convert.ToInt16(ResidentialDetermination);

            shipment.Postal.Confirmation = Convert.ToInt16(Confirmation);
            shipment.Postal.PackagingType = Convert.ToInt16(PackagingType);
            shipment.Postal.DimsLength = Convert.ToInt16(DimsLength);
            shipment.Postal.DimsWidth = Convert.ToInt16(DimsWidth);
            shipment.Postal.DimsHeight = Convert.ToInt16(DimsHeight);
            shipment.Postal.EntryFacility = Convert.ToInt16(EntryFacility);
            shipment.Postal.SortType = Convert.ToInt16(SortType);
            shipment.Postal.ExpressSignatureWaiver = Convert.ToInt16(ExpressSignatureWaiver) == 1;
            shipment.Postal.InsuranceValue = Convert.ToDecimal(InsuranceValue);
            shipment.Postal.CustomsContentDescription = CustomsContentDescription;
            shipment.Postal.CustomsContentType = Convert.ToInt16(CustomsContentType);
            shipment.Postal.NonMachinable = Convert.ToInt16(NonMachinable) == 1;
            shipment.Postal.NonRectangular = Convert.ToInt16(NonRectangular) == 1;
            shipment.Postal.DimsAddWeight = Convert.ToInt16(DimsAddWeight) == 1;
            shipment.Postal.DimsWeight = Convert.ToDouble(DimsWeight);

            if (shipmentType.IsCustomsRequired(shipment) && !string.IsNullOrWhiteSpace(CustomsItems) && CustomsItems.ToUpperInvariant() != "NULL".ToUpperInvariant())
            {
                XElement shipmentCustomsItemsElements = XElement.Parse("<ShipmentCustomsItems>" + CustomsItems + "</ShipmentCustomsItems>");
                foreach (XElement sci in shipmentCustomsItemsElements.Descendants())
                {
                    ShipmentCustomsItemEntity shipmentCustomsItem = new ShipmentCustomsItemEntity();
                    shipmentCustomsItem.Description = Trim(sci.Attribute("Description").Value, 60);
                    shipmentCustomsItem.Quantity = Convert.ToDouble(sci.Attribute("Quantity").Value);
                    shipmentCustomsItem.Weight = Convert.ToDouble(sci.Attribute("Weight").Value);
                    shipmentCustomsItem.UnitValue = Convert.ToDecimal(sci.Attribute("UnitValue").Value);
                    shipmentCustomsItem.CountryOfOrigin = Trim(sci.Attribute("CountryOfOrigin").Value, 2);
                    shipmentCustomsItem.HarmonizedCode = Trim(sci.Attribute("HarmonizedCode").Value, 6);
                    shipmentCustomsItem.NumberOfPieces = Convert.ToInt16(sci.Attribute("NumberOfPieces").Value);
                    shipmentCustomsItem.UnitPriceAmount = Convert.ToDecimal(sci.Attribute("UnitPriceAmount").Value);
                    
                    shipment.CustomsItems.Add(shipmentCustomsItem);
                }
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

        /// <summary>
        /// Trim a string to a max length.
        /// </summary>
        /// <param name="textToTrim"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public string Trim(string textToTrim, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(textToTrim))
            {
                return string.Empty;
            }

            textToTrim = textToTrim.Trim();
            if (textToTrim.Length <= maxLength)
            {
                return textToTrim;
            }

            return textToTrim.Substring(0, maxLength);
        }

    }
}
