using System;
using System.Globalization;
using System.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    /// <summary>
    /// Encapsulates the logic for inspecting/processing the response from InsureShip when checking the status of a claim.
    /// </summary>
    public class InsureShipClaimStatusResponse : IInsureShipResponse
    {
        private readonly InsureShipRequestBase request;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipClaimStatusResponse" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="log">The log.</param>
        public InsureShipClaimStatusResponse(InsureShipRequestBase request, ILog log)
        {
            this.request = request;
            this.log = log;
        }

        /// <summary>
        /// Process a response from InsureShip. This will inspect the raw response to ensure the
        /// request was successful. A response code of anything other than 204 will result in an
        /// InsureShipResponseException being thrown.
        /// </summary>
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
        /// <param name="responseStatusCode">The response status code.</param>
        /// <returns>The InsureShipResponseCode value based on the HttpWebResponse.</returns>
        /// <exception cref="InsureShipResponseException"></exception>
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
                string message = string.Format("An error occurred trying to check the claim status for shipment {0} with the InsureShip API: {1}", request.Shipment.ShipmentID, (int) responseStatusCode);

                log.Error(message);
                throw new InsureShipResponseException(responseCode, message);
            }

            return responseCode;
        }

        /// <summary>
        /// Reads the response and assigns the claim status on the shipment's insurance policy.
        /// </summary>
        /// <param name="responseContent">The content of the response from InsureShip.</param>
        private void SaveToShipment(string responseContent)
        {
            try
            {
                // Response comes back in the format of: {"status":"Created"}
                string status = (string) JObject.Parse(responseContent).SelectToken("status");
                if (status == null)
                {
                    log.Error("The claim status was not found in the InsureShip response. Check the request/response logs for more details.");
                }

                request.Shipment.InsurancePolicy.ClaimStatus = status ?? "No status information available.";
            }
            catch (JsonReaderException exception)
            {
                HandleExceptionFromReadingClaimStastus(exception);
            }
            catch (ArgumentNullException exception)
            {
                HandleExceptionFromReadingClaimStastus(exception);
            }
        }

        /// <summary>
        /// Handles the exception from reading the claim status.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <exception cref="InsureShipException">An error occurred trying to check the status of a claim. Please try again or contact InsureShip to check the claim status.</exception>
        private void HandleExceptionFromReadingClaimStastus(Exception exception)
        {
            log.Error("The status could not be parsed out of the response from InsureShip.", exception);
            throw new InsureShipException("An error occurred trying to check the status of a claim. Please try again or contact InsureShip to check the claim status.");
        }
    }
}
