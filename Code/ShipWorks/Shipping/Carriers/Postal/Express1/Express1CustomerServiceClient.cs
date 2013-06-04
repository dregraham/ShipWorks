using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService;
using System.Web;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Business;
using System.Net;
using System.Web.Services.Protocols;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using System.Xml;
using System.Xml.Linq;
using Express1LabelService = ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.LabelService;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore;
using System.Text.RegularExpressions;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Interacts with the Express 1 Customer Service web service
    /// </summary>
    public static class Express1CustomerServiceClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Express1CustomerServiceClient));

        /// <summary>
        /// Encrypts a string based on Express1's requirements 
        /// </summary>
        private static string EncryptData(string dataString)
        {
            using (Rijndael rijndael = Rijndael.Create())
            {
                // per Express1
                rijndael.BlockSize = 128;
                rijndael.KeySize = 256;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;

                // our secret stuff
                string key = Express1Utility.CertificateID.Substring(0, 32);
                string iv = "F9sl139fakvN#1kl";

                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                byte[] ivBytes = Encoding.ASCII.GetBytes(iv);
                byte[] dataBytes = Encoding.ASCII.GetBytes(dataString);

                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, rijndael.CreateEncryptor(keyBytes, ivBytes), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                        cryptoStream.Close();

                        // return the encrypted data, base64 encoded
                        return Convert.ToBase64String(memStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Get the encrypted customerInfo string
        /// </summary>
        public static string GetCustomerInfoString(string customerID)
        {
            // create the tokenized string to encrypt
            string info = string.Format("{0}|{1}|Account", Express1Utility.FranchiseID, customerID);

            // encrypt per Express1's requirements
            return EncryptData(info);
        }

        /// <summary>
        /// Get the signup url for the web-based signup method
        /// </summary>
        public static string GetSignupUrl(string customerInfo)
        {
            string urlBase = "https://service.express1.com/Customer/Gateway.aspx";
            if (Express1Utility.UseTestServer)
            {
                urlBase = "http://www.express1dev.com/CustomerWeb/Gateway.aspx";
            }

            // format the url
            string url = string.Format("{0}?app={1}&info={2}", urlBase, Express1Utility.ApiKey, HttpUtility.UrlEncode(customerInfo));

            // that's it
            return url;
        }

        /// <summary>
        /// Customer Service creation
        /// </summary>
        private static CustomerService CreateCustomerService(string logName)
        {
            CustomerService service = new CustomerService(new ApiLogEntry(ApiLogSource.UspsExpress1, logName));

            // configure the endpoint
            if (Express1Utility.UseTestServer)
            {
                service.Url = "http://www.express1dev.com/Services/CustomerService.svc";
            }
            else
            {
                service.Url = "https://service.express1.com/Services/CustomerService.svc";
            }

            return service;
        }

        /// <summary>
        /// Populates an AddressInfo with data from an Endicia Account
        /// </summary>
        private static AddressInfo CreateAddressInfo(PersonAdapter fromAddress)
        {
            AddressInfo address = new AddressInfo();

            address.Address1 = fromAddress.Street1;
            address.Address2 = fromAddress.Street2;
            address.City = fromAddress.City;
            address.PostalCode = fromAddress.PostalCode;
            address.State = fromAddress.StateProvCode;

            return address;
        }

        /// <summary>
        /// Create and execute the Signup web request to Express1.
        /// </summary>
        public static void Signup(EndiciaAccountEntity account, PersonAdapter address, EndiciaPaymentInfo paymentInfo)
        {
            #region Validation

            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            if (paymentInfo == null)
            {
                throw new ArgumentNullException("paymentInfo");
            }

            #endregion

            try
            {
                CustomerRegistrationData customerData = new CustomerRegistrationData();

                // TOS
                customerData.TermsAcceptedSpecified = true;
                customerData.TermsAccepted = true;

                // Contact Info
                customerData.Email = address.Email;
                customerData.Phone = address.Phone10Digits;
                customerData.ContactName = new PersonName(address.FirstName, address.MiddleName, address.LastName).FullName;
                customerData.CompanyName = address.Company;

                // Mailing address
                customerData.MailingAddress = CreateAddressInfo(address);

                // Payment for postage
                if (paymentInfo.UseCheckingForPostage)
                {
                    customerData.PaymentType = PaymentType.ACH;
                    customerData.PaymentTypeSpecified = true;

                    customerData.BankAccount = new BankAccountInfo();
                    customerData.BankAccount.BankName = "None"; // paymentInfo.BankName;
                    customerData.BankAccount.AccountName = address.Company;
                    customerData.BankAccount.AccountNumber = paymentInfo.CheckingAccount;
                    customerData.BankAccount.RoutingNumber = paymentInfo.CheckingRouting;

                    // address
                    customerData.BankAccount.BillingAddress = CreateAddressInfo(address);
                    customerData.BankAccount.AccountType = BankAccountTypeEnum.Checking;
                    customerData.BankAccount.AccountTypeSpecified = true;
                }
                else
                {
                    customerData.PaymentType = PaymentType.CreditCard;
                    customerData.PaymentTypeSpecified = true;

                    customerData.CreditCard = new CreditCardInfo();

                    // card detials
                    customerData.CreditCard.CardNumber = paymentInfo.CardNumber;
                    customerData.CreditCard.CardType = Express1Utility.GetExpress1CardType(paymentInfo.CardType); // enum values align, we're ok 
                    customerData.CreditCard.NameOnCard = customerData.ContactName;
                    customerData.CreditCard.CardTypeSpecified = true;
                    customerData.CreditCard.ExpirationMonth = paymentInfo.CardExpirationMonth.ToString("00");
                    customerData.CreditCard.ExpirationYear = paymentInfo.CardExpirationYear.ToString();

                    // card billing address
                    customerData.CreditCard.BillingAddress = CreateAddressInfo(paymentInfo.CardBillingAddress);
                }

                // populate security information
                customerData.SecurityInfo = GetSecurityInfo();

                using (CustomerService service = CreateCustomerService("Signup"))
                {
                    CustomerCredentials returnedCredentials = service.RegisterCustomer(Express1Utility.ApiKey, GetCustomerInfoString(Guid.NewGuid().ToString()), customerData);

                    // Save the address 
                    PersonAdapter.Copy(address, new PersonAdapter(account, ""));
                    account.MailingPostalCode = account.PostalCode;

                    // Account type and passwords
                    account.ApiUserPassword = SecureText.Encrypt(returnedCredentials.PassPhrase, "Endicia");
                    account.AccountNumber = returnedCredentials.AccountID;

                    // record if this is a Test account
                    account.TestAccount = Express1Utility.UseTestServer;

                    // other data
                    account.CreatedByShipWorks = true;
                    account.AccountType = (int)EndiciaAccountType.Standard;
                    account.ApiInitialPassword = "";
                    account.SignupConfirmation = "";
                    account.WebPassword = "";
                    account.ScanFormAddressSource = (int) EndiciaScanFormAddressSource.Provider;

                    // Deafult description.  Blank until we get an account number
                    account.Description = EndiciaAccountManager.GetDefaultDescription(account);

                    // Save the account
                    EndiciaAccountManager.SaveAccount(account);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(Express1Exception));
            }
        }

        /// <summary>
        /// Creates the SecurityInfo data to send to Express1 along with registration
        /// for fraud detection purposes
        /// </summary>
        private static SecurityInfo GetSecurityInfo()
        {
            SecurityInfo security = new SecurityInfo();
            security.RemoteIPAddress = GetPublicIP();
            security.SessionID = ShipWorksSession.InstanceID.ToString();
            security.UserAgent = "ShipWorks";

            return security;
        }

        /// <summary>
        /// Gets the public ip address of this computer
        /// </summary>
        private static string GetPublicIP()
        {
            try
            {
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org");

                // if we don't get a near-immediate response, don't wait
                request.Timeout = 1000;

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string text = reader.ReadToEnd();

                        Regex addressRegex = new Regex(@"Current IP Address: (\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})");
                        string ip = addressRegex.Match(text).Groups[1].Value;

                        if (!String.IsNullOrEmpty(ip))
                        {
                            return ip;
                        }
                        else
                        {
                            return "Unknown";
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "Unknown";
            }
        }

       
        #region Refunds

        /// <summary>
        /// Creates a shipment refund request to be sent to Endicia on the shipper's behalf
        /// </summary>
        private static string CreateRefundRequest(ShipmentEntity shipment)
        {
            EndiciaAccountEntity account = EndiciaAccountManager.GetAccount(shipment.Postal.Endicia.EndiciaAccountID);
            if (account == null)
            {
                throw new EndiciaException("The Express1 account associated with the shipment has been removed from ShipWorks.");
            }

            XElement xRoot = new XElement("RefundRequest",

                new XElement("AccountID", account.AccountNumber),
                new XElement("PassPhrase", SecureText.Decrypt(account.ApiUserPassword, "Endicia")),

                new XElement("Test", (Express1Utility.UseTestServer || account.TestAccount) ? "Y" : "N"),

                new XElement("RefundList", 
                    new XElement("PICNumber", shipment.TrackingNumber)));
            
            // return the generated request
            return xRoot.ToString();
        }

        /// <summary>
        /// Creates the Express1 Web Proxy that has the Refund method on it.
        /// 
        /// Express1 has the Refund functionality attached to the LabelService, unlike Endicia who
        /// has it as a part of the CustomerService.
        /// </summary>
        private static Express1LabelService.EwsLabelService CreateExpress1LabelService(string logName)
        {
            Express1LabelService.EwsLabelService refundService = new Express1LabelService.EwsLabelService(new ApiLogEntry(ApiLogSource.UspsExpress1, logName));

            if (Express1Utility.UseTestServer)
            {
                refundService.Url = Express1Utility.Express1DevelopmentUrl;
            }
            else
            {
                refundService.Url = Express1Utility.Express1ProductionUrl;
            }

            return refundService;
        }

        /// <summary>
        /// Requests a postage refund (Voiding in SW).
        /// </summary>
        public static void RequestRefund(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            string request = CreateRefundRequest(shipment);
            try
            {
                // create the request
                using (Express1LabelService.EwsLabelService service = CreateExpress1LabelService("Void"))
                {
                    string rawResponse = service.RefundRequest(request);

                    // process the response
                    XDocument xDocument = ExtractSuccessResponse(rawResponse);

                    XElement result = xDocument.Descendants("PICNumber").SingleOrDefault();
                    if (result == null)
                    {
                        throw new ShippingException("The response from Express1 does not appear to be correctly formatted.");
                    }

                    if (string.Compare((string)result.Element("IsApproved"), "YES", true) != 0)
                    {
                        throw new EndiciaException((string)result.Element("ErrorMsg"));
                    }

                    // Save the Refund Form ID.  
                    // Express1 says they currently aren't using this so it's expected to be blank.  Will save anyway if it's there.
                    XElement formNumberElement = xDocument.Descendants("FormNumber").FirstOrDefault();
                    if (formNumberElement != null)
                    {
                        int value = 0;
                        if (int.TryParse(formNumberElement.Value, out value))
                        {
                            shipment.Postal.Endicia.RefundFormID = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Extracts the return document from a successful response, or throws an error of a response contains an error.
        /// </summary>
        private static XDocument ExtractSuccessResponse(object rawResponse)
        {
            return ExtractSuccessResponse(rawResponse, null);
        }

        /// <summary>
        /// Extracts the return document from a successful response, or throws an error of a response contains an error.
        /// </summary>
        private static XDocument ExtractSuccessResponse(object rawResponse, Func<string, string> errorFormatter)
        {
            string response = rawResponse as string;
            if (response == null)
            {
                log.ErrorFormat("Unexpected result type returned from call: {0}", rawResponse == null ? "NULL" : rawResponse.GetType().FullName);

                throw new InvalidOperationException("Express1 returned results in an unexpected format.");
            }

            XDocument xDocument = XDocument.Parse(response);

            // Check for errors
            string errorMessage = (string)xDocument.Descendants().FirstOrDefault(e => e.Name.LocalName.StartsWith("Error", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(errorMessage))
            {
                if (errorFormatter != null)
                {
                    errorMessage = errorFormatter(errorMessage);
                }

                if (!errorMessage.EndsWith(".", StringComparison.OrdinalIgnoreCase))
                {
                    errorMessage += ".";
                }

                throw new EndiciaException(errorMessage);
            }

            return xDocument;
        }

        #endregion


        #region SCAN Forms        
        /// <summary>
        /// Create a SCAN form for the given shipments. All of the shipments should have been created using the same account.
        /// 
        /// Express1 SCAN Form functionality has been merged into their wrapper of Endicia's Label Service.  Whereas Endicia has this functionality
        /// as a part of their Account Service.
        /// </summary>
        public static XDocument CreateScanForm(IEnumerable<ShipmentEntity> shipments)
        {
            if (shipments == null || shipments.Count() == 0)
            {
                throw new EndiciaException("No shipments were selected for the SCAN form.");
            }

            EndiciaAccountEntity account = EndiciaAccountManager.GetAccount(shipments.First().Postal.Endicia.EndiciaAccountID);
            if (account == null)
            {
                throw new EndiciaException("The Express1 account associated with the shipments has been removed from ShipWorks.");
            }

            PersonAdapter person = new PersonAdapter(account, "");

            XElement xRoot = new XElement("SCANRequest",

                new XElement("AccountID", account.AccountNumber),
                new XElement("PassPhrase", SecureText.Decrypt(account.ApiUserPassword, "Endicia")),

                new XElement("Test", (Express1Utility.UseTestServer || account.TestAccount) ? "Y" : "N"));

            // Optionally add in the address to use as the return address. The user may not want to send this, as it then forces validation that all shipments on the form must have the same from zip code.
            if (account.ScanFormAddressSource == (int) EndiciaScanFormAddressSource.ShipWorks)
            {
                xRoot.Add(
                    new XElement("FromName", new PersonName(person).FullName),
                    new XElement("FromAddress", person.StreetAll),
                    new XElement("FromCity", person.City),
                    new XElement("FromState", person.StateProvCode),
                    new XElement("FromZipCode", person.PostalCode));
            }

            // Add in the image format details
            xRoot.Add(
                new XElement("ImageFormat", "PNG"),
                new XElement("DPI", "300"));

            XElement xScanList = new XElement("SCANList");
            xRoot.Add(xScanList);

            // Add each shipment to the SCAN creation list
            foreach (ShipmentEntity shipment in shipments)
            {
                EndiciaShipmentEntity endicia = shipment.Postal.Endicia;

                if (endicia.EndiciaAccountID != account.EndiciaAccountID)
                {
                    throw new EndiciaException("The shipments were not all processed from the same Express1 account.");
                }

                xScanList.Add(new XElement("PICNumber", shipment.TrackingNumber));
            }

            try
            {
                using (Express1LabelService.EwsLabelService service = CreateExpress1LabelService("SCANForm"))
                {
                    object rawResponse = service.SCANRequest(xRoot.ToString());
                    XDocument xDocument = ExtractSuccessResponse(rawResponse,
                        message => message.Contains("102") ? "The shipments are already on another SCAN form, do not qualify for SCAN, or the postal code of your account in ShipWorks does not match the return address of all of the shipments." : message);
                    
                    return xDocument;                    
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        #endregion 
    }
}
