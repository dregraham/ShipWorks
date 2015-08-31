using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.AccountService;
using ShipWorks.ApplicationCore.Logging;
using log4net;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using System.IO;
using Interapptive.Shared.Business;
using System.Net;
using System.Web.Services.Protocols;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Account
{
    /// <summary>
    /// Special account level endicia services proviced from a seperate web service
    /// </summary>
    public static class EndiciaApiAccount
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EndiciaApiAccount));

        /// <summary>
        /// Create the webservice object
        /// </summary>
        private static ELSServicesService CreateService(string logName)
        {
            ELSServicesService webService = new ELSServicesService(new ApiLogEntry(ApiLogSource.UspsEndicia, logName));
            webService.Url = "https://www.endicia.com/ELS/ELSServices.cfc";

            return webService;
        }

        /// <summary>
        /// Signup a new endicia label server account given the specified account information
        /// </summary>
        public static EndiciaAccountEntity Signup(
            EndiciaAccountEntity account,
            EndiciaAccountType accountType, 
            PersonAdapter address, 
            string webPassword, 
            string apiPassword, 
            string challengeQuestion, 
            string challengeAnswer, 
            EndiciaPaymentInfo paymentInfo)
        {
            string apiInitialPassword = apiPassword + "_initial";

            XElement xRoot = new XElement("UserSignupRequest",

                new XElement("Test", EndiciaApiClient.UseTestServer ? "Y" : "N"),
                new XElement("OverrideEmailCheck", "Y"),

                new XElement("CompanyName", address.Company),
                new XElement("FirstName", address.FirstName),
                new XElement("LastName", address.LastName),
                new XElement("EmailAddress", address.Email),
                new XElement("EmailConfirm", address.Email),
                new XElement("PhoneNumber", address.Phone),
                new XElement("ICertify", "Y"),
                new XElement("FaxNumber", address.Fax),
                new XElement("PhysicalAddress", address.StreetAll),
                new XElement("PhysicalCity", address.City),
                new XElement("PhysicalState", address.StateProvCode),
                new XElement("PhysicalZipCode", address.PostalCode),

                new XElement("WebPassword", webPassword),
                new XElement("PassPhrase", apiInitialPassword),
                new XElement("ChallengeQuestion", challengeQuestion),
                new XElement("ChallengeAnswer", challengeAnswer),

                new XElement("BillingType", (accountType == EndiciaAccountType.Premium) ? "T8" : (accountType == EndiciaAccountType.Standard) ? "T7" : "TP"),
                new XElement("PartnerId", EndiciaApiClient.GetInterapptivePartnerID(accountType)),
                new XElement("ProductType", "LABELSERVER"),

                new XElement("CreditCardNumber", paymentInfo.CardNumber),
                new XElement("CreditCardAddress", paymentInfo.CardBillingAddress.StreetAll),
                new XElement("CreditCardCity", paymentInfo.CardBillingAddress.City),
                new XElement("CreditCardState", paymentInfo.CardBillingAddress.StateProvCode),
                new XElement("CreditCardZipCode", paymentInfo.CardBillingAddress.PostalCode),
                new XElement("CreditCardType", GetCreditCardTypeApiCode(paymentInfo.CardType)),
                new XElement("CreditCardExpMonth", string.Format("{0:00}", paymentInfo.CardExpirationMonth)),
                new XElement("CreditCardExpYear", paymentInfo.CardExpirationYear.ToString()),

                new XElement("PaymentType", paymentInfo.UseCheckingForPostage ? "ACH" : "CC"));

            if (paymentInfo.UseCheckingForPostage)
            {
                xRoot.Add(
                    new XElement("CheckingAccountNumber", paymentInfo.CheckingAccount),
                    new XElement("CheckingAccountRoutingNumber", paymentInfo.CheckingRouting));
            }

            try
            {
                using (ELSServicesService service = CreateService("Signup"))
                {
                    object rawResponse = service.UserSignup(xRoot.ToString());
                    XDocument xDocument = ExtractSuccessResponse(rawResponse);

                    // Extract returned confirmation
                    string confirmation = (string) xDocument.Root.Element("ConfirmationNumber");
                    if (string.IsNullOrEmpty(confirmation))
                    {
                        throw new EndiciaException("The Endicia server did not return a confirmation number.");
                    }

                    if (confirmation == "0")
                    {
                        throw new EndiciaException("The Endicia server returned an invalid confirmation number.");
                    }

                    // Save the address 
                    PersonAdapter.Copy(address, new PersonAdapter(account, ""));
                    account.MailingPostalCode = account.PostalCode;

                    // Account type and passwords
                    account.SignupConfirmation = confirmation;
                    account.WebPassword = SecureText.Encrypt(webPassword, "Endicia");
                    account.ApiInitialPassword = SecureText.Encrypt(apiInitialPassword, "Endicia");
                    account.ApiUserPassword = SecureText.Encrypt(apiPassword, "Endicia");

                    account.AccountType = (int) accountType;
                    account.CreatedByShipWorks = true;
                    account.ScanFormAddressSource = (int) EndiciaScanFormAddressSource.Provider;

                    // Store whether its a test account
                    account.TestAccount = EndiciaApiClient.UseTestServer;

                    // Deafult description.  Blank until we get an account number
                    account.Description = "";

                    // Save the account
                    EndiciaAccountManager.SaveAccount(account);

                    return account;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Request a refund from endicia for the given shipment
        /// </summary>
        public static void RequestRefund(ShipmentEntity shipment)
        {
            EndiciaAccountEntity account = EndiciaAccountManager.GetAccount(shipment.Postal.Endicia.EndiciaAccountID);
            if (account == null)
            {
                throw new EndiciaException("The Endicia account associated with the shipment has been removed from ShipWorks.");
            }

            if (ShipmentTypeManager.IsEndiciaDhl((PostalServiceType) shipment.Postal.Service))
            {
                log.InfoFormat("DHL shipments do not support refunds. {0}", shipment.ShipmentID);
                return;
            }

            // Nothing to refund
            if (shipment.Postal.NoPostage && shipment.Postal.Endicia.TransactionID == 0)
            {
                log.WarnFormat("No refund being required from Endicia for non-postage shipment. {0}", shipment.ShipmentID);
                return;
            }

            XElement xRoot = new XElement("RefundRequest",

                new XElement("AccountID", account.AccountNumber),
                new XElement("PassPhrase", SecureText.Decrypt(account.ApiUserPassword, "Endicia")),

                new XElement("Test", (EndiciaApiClient.UseTestServer || account.TestAccount) ? "Y" : "N"),

                new XElement("RefundList", 
                    new XElement("PieceID", shipment.Postal.Endicia.TransactionID)));

            try
            {
                using (ELSServicesService service = CreateService("RefundRequest"))
                {
                    object rawResponse = service.RefundRequest(xRoot.ToString());
                    XDocument xDocument = ExtractSuccessResponse(rawResponse);

                    XElement result = xDocument.Descendants("PieceID").SingleOrDefault();
                    if (result == null)
                    {
                        throw new ShippingException("The response from Endicia does not appear to be correctly formatted.");
                    }

                    if (string.Compare((string) result.Element("IsApproved"), "YES", true) != 0)
                    {
                        string message = (string) result.Element("ErrorMsg");

                        if (message == "Denied - Invalid" && shipment.Postal.Service == (int) PostalServiceType.InternationalFirst)
                        {
                            message = "The Postal Service requires that International First-Class mail be voided directly through Endicia.";
                        }

                        throw new EndiciaException(message);
                    }

                    // Save the reufnd from id
                    shipment.Postal.Endicia.RefundFormID = (int?) xDocument.Descendants("FormNumber").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }
               
        /// <summary>
        /// Create a SCAN form for the given shipments. All of the shipments should have been created using the same account.
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
                throw new EndiciaException("The Endicia account associated with the shipments has been removed from ShipWorks.");
            }

            PersonAdapter person = new PersonAdapter(account, "");

            XElement xRoot = new XElement("SCANRequest",

                new XElement("AccountID", account.AccountNumber),
                new XElement("PassPhrase", SecureText.Decrypt(account.ApiUserPassword, "Endicia")),

                new XElement("Test", (EndiciaApiClient.UseTestServer || account.TestAccount) ? "Y" : "N"));

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
                    throw new EndiciaException("The shipments were not all processed from the same Endicia account.");
                }

                xScanList.Add(new XElement("PICNumber", shipment.TrackingNumber));
            }

            try
            {
                using (ELSServicesService service = CreateService("ScanForm"))
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
            XmlNode[] resultNodes = rawResponse as XmlNode[];

            // Has to be of the expected type
            if (resultNodes == null)
            {
                log.ErrorFormat("Unexpected result type returned from call: {0}", rawResponse == null ? "NULL" : rawResponse.GetType().FullName);

                throw new InvalidOperationException("Endicia returned results in an unexpected format.");
            }

            // Find the one that holds the actual response
            XmlNode responseNode = resultNodes.SingleOrDefault(n => n.NodeType == XmlNodeType.Element);
            if (responseNode == null)
            {
                log.ErrorFormat("Could not find response node in returned node set.");

                throw new InvalidOperationException("Could not find response node in returned node set.");
            }

            XDocument xDocument = XDocument.Parse(responseNode.OuterXml);

            // Check for errors
            string errorMessage = (string) xDocument.Root.Elements().FirstOrDefault(e => e.Name.LocalName.StartsWith("Error"));
            if (!string.IsNullOrEmpty(errorMessage))
            {
                if (errorFormatter != null)
                {
                    errorMessage = errorFormatter(errorMessage);
                }

                if (!errorMessage.EndsWith("."))
                {
                    errorMessage += ".";
                }

                throw new EndiciaException(errorMessage);
            }

            return xDocument;
        }

        /// <summary>
        /// Get the API code value to use for the given credit card type
        /// </summary>
        private static string GetCreditCardTypeApiCode(EndiciaCreditCardType cardType)
        {
            switch (cardType)
            {
                case EndiciaCreditCardType.Visa: return "V";
                case EndiciaCreditCardType.MasterCard: return "M";
                case EndiciaCreditCardType.AmericanExpress: return "A";
                case EndiciaCreditCardType.CarteBlanche: return "B";
                case EndiciaCreditCardType.Discover: return "N";
                case EndiciaCreditCardType.DinersClub: return "D";
            }

            throw new InvalidOperationException("Invalid endicia card type: " + cardType);
        }
    }
}
