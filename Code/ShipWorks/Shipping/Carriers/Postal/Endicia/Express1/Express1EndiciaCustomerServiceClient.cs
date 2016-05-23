using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Business;
using System.Xml.Linq;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Security;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Interacts with the Express 1 Customer Service web service
    /// </summary>
    public static class Express1EndiciaCustomerServiceClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Express1EndiciaCustomerServiceClient));

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

                new XElement("Test", (Express1EndiciaUtility.UseTestServer || account.TestAccount) ? "Y" : "N"),

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
        private static Postal.Express1.WebServices.LabelService.EwsLabelService CreateExpress1LabelService(string logName)
        {
            Postal.Express1.WebServices.LabelService.EwsLabelService refundService = new Postal.Express1.WebServices.LabelService.EwsLabelService(new ApiLogEntry(ApiLogSource.UspsExpress1Endicia, logName));

            if (Express1EndiciaUtility.UseTestServer)
            {
                refundService.Url = Express1EndiciaUtility.Express1DevelopmentUrl;
            }
            else
            {
                refundService.Url = Express1EndiciaUtility.Express1ProductionUrl;
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
                using (Postal.Express1.WebServices.LabelService.EwsLabelService service = CreateExpress1LabelService("Void"))
                {
                    string rawResponse = service.RefundRequest(request);

                    // process the response
                    XDocument xDocument = ExtractSuccessResponse(rawResponse);

                    XElement result = xDocument.Descendants("PICNumber").SingleOrDefault();
                    if (result == null)
                    {
                        throw new ShippingException("The response from Express1 does not appear to be correctly formatted.");
                    }

                    if (string.Compare((string)result.Element("IsApproved"), "YES", StringComparison.OrdinalIgnoreCase) != 0)
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

                new XElement("Test", (Express1EndiciaUtility.UseTestServer || account.TestAccount) ? "Y" : "N"));

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
                using (Postal.Express1.WebServices.LabelService.EwsLabelService service = CreateExpress1LabelService("SCANForm"))
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
