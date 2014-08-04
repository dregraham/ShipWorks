using System;
using System.Globalization;
using System.IO;
using System.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using log4net;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    /// <summary>
    /// Encapsulates the logic for inspeting/processing the response from InsureShip when a claim is submitted.
    /// </summary>
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
            InsureShipResponseCode responseCode = ValidateInsureShipResponse(request.ResponseStatusCode);
            SaveToShipment(request.ResponseContent);

            return responseCode;
        }

        /// <summary>
        /// Validates the response making sure the claim was submitted correctly.
        /// </summary>
        /// <returns>The InsureShipResponseCode value based on the HttpWebResponse.</returns>
        private InsureShipResponseCode ValidateInsureShipResponse(HttpStatusCode responseStatusCode)
        {
            InsureShipResponseCode responseCode;

            try
            {
                responseCode = EnumHelper.GetEnumByApiValue<InsureShipResponseCode>(((int) responseStatusCode).ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
                string message = string.Format("An unknown response code was received from the InsureShip API for shipment {0}: {1}", request.Shipment.ShipmentID, (int) responseStatusCode);

                log.Error(message);
                throw new InsureShipResponseException(InsureShipResponseCode.UnknownFailure, message);
            }

            // We have a recognizable response code
            if (responseCode != InsureShipResponseCode.OK)
            {
                // Response code indicates the claim did not go through
                string message = string.Format("An error occurred trying to submit a claim for shipment {0} with the InsureShip API: {1}", request.Shipment.ShipmentID, (int) responseStatusCode);

                log.Error(message);
                throw new InsureShipResponseException(responseCode, message);
            }

            return responseCode;
        }

        /// <summary>
        /// Reads the response and assigns the claim identifier on the shipment's insurance policy.
        /// </summary>
        /// <param name="responseContent">The raw response.</param>
        private void SaveToShipment(string responseContent)
        {
            try
            {
                // Response comes back in the format of: {"claim_id":"312","message":"Claim created successfully with claim_id 312"}
                string claimId = (string)JObject.Parse(responseContent).SelectToken("claim_id");
                request.Shipment.InsurancePolicy.ClaimID = long.Parse(claimId);
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

        /// <summary>
        /// Handles the exception from reading the claim identifier.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <exception cref="InsureShipException">An error occurred trying to submit a claim. Please try again or contact InsureShip to submit a claim.</exception>
        private void HandleExceptionFromReadingClaimID(Exception exception)
        {
            log.Error("The claim could not be parsed out of the response from InsureShip.", exception);
            throw new InsureShipException("An error occurred trying to submit a claim. Please try again or contact InsureShip to submit a claim.");
        }
    }
}
