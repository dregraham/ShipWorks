using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    public class InsureShipSubmitClaimResponse : IInsureShipResponse
    {
        private readonly InsureShipRequestBase request;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipSubmitClaimResponse" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="log">The log.</param>
        public InsureShipSubmitClaimResponse(InsureShipRequestBase request, ILog log)
        {
            this.request = request;
            this.log = log;
        }

        /// <summary>
        /// Process a response from InsureShip. This will inspect the raw response to ensure the
        /// request was successful. A response code of anything other than 204 will result in an
        /// InsureShipResponseException being thrown.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InsureShipResponseException"></exception>
        public InsureShipResponseCode Process()
        {
            HttpWebResponse rawResponse = request.RawResponse;

            InsureShipResponseCode responseCode = ValidateInsureShipResponse(rawResponse);
            SaveToShipment(rawResponse);

            return responseCode;
        }

        /// <summary>
        /// Validates the response making sure the claim was submitted correctly.
        /// </summary>
        /// <param name="rawResponse">The raw response.</param>
        /// <returns>The InsureShipResponseCode value based on the HttpWebResponse.</returns>
        private InsureShipResponseCode ValidateInsureShipResponse(HttpWebResponse rawResponse)
        {
            InsureShipResponseCode responseCode;

            try
            {
                responseCode = EnumHelper.GetEnumByApiValue<InsureShipResponseCode>(((int)rawResponse.StatusCode).ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
                int statusCode = request.RawResponse != null ? (int)rawResponse.StatusCode : -0;
                string message = string.Format("An unknown response code was received from the InsureShip API for shipment {0}: {1}", request.Shipment.ShipmentID, statusCode);

                log.Error(message);
                throw new InsureShipResponseException(InsureShipResponseCode.UnknownFailure, message);
            }

            // We have a recognizable response code
            if (responseCode != InsureShipResponseCode.OK)
            {
                // Response code indicates the claim did not go through
                int statusCode = request.RawResponse != null ? (int)rawResponse.StatusCode : -0;
                string message = string.Format("An error occurred trying to submit a claim for shipment {0} with the InsureShip API: {1}", request.Shipment.ShipmentID, statusCode);

                log.Error(message);
                throw new InsureShipResponseException(responseCode, message);
            }

            return responseCode;
        }

        /// <summary>
        /// Reads the response and assigns the claim identifier on the shipment's insurance policy.
        /// </summary>
        /// <param name="rawResponse">The raw response.</param>
        private void SaveToShipment(HttpWebResponse rawResponse)
        {
            try
            {
                string responseBody = string.Empty;
                using (Stream responseStream = rawResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }

                // Response comes back in the format of: {"claim_id":"312","message":"Claim created successfully with claim_id 312"}
                string claimID = (string) JObject.Parse(responseBody).SelectToken("claim_id");
                request.Shipment.InsurancePolicy.ClaimID = long.Parse(claimID);
            }
            catch (FormatException exception)
            {
                log.Error("The claim ID was not in a numeric format.");
                HandleExceptionFromReadingClaimID(exception);
            }
            catch (JsonReaderException exception)
            {
                HandleExceptionFromReadingClaimID(exception);
            }
            catch (ArgumentNullException exception)
            {
                HandleExceptionFromReadingClaimID(exception);
            }
        }

        private void HandleExceptionFromReadingClaimID(Exception exception)
        {
            log.Error("The claim could not be parsed out of the response from InsureShip.", exception);
            throw new InsureShipException("An error occurred trying to submit a claim. Please try again or contact InsureShip to submit a claim.");
        }
    }
}
