using System;
using System.Globalization;
using System.Net;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Void
{
    /// <summary>
    /// Processes a response from InsureShip for voiding a policy.
    /// </summary>
    public class InsureShipVoidPolicyResponse : IInsureShipResponse
    {
        private readonly InsureShipRequestBase request;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipVoidPolicyResponse" /> class.
        /// </summary>
        public InsureShipVoidPolicyResponse(InsureShipRequestBase request, ILog log)
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
            InsureShipResponseCode responseCode;

            HttpStatusCode statusCode = request.ResponseStatusCode;

            try
            {
                // It looks like the void request returns a 200 on success instead of a 204
                responseCode = statusCode == HttpStatusCode.OK ? 
                    InsureShipResponseCode.Success :
                    EnumHelper.GetEnumByApiValue<InsureShipResponseCode>(((int)statusCode).ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
                string message = string.Format("An unknown response code was received from the InsureShip API while attempting to void shipment {0}: {1}", request.Shipment.ShipmentID, (int) statusCode);
                log.Error(message);

                throw new InsureShipResponseException(InsureShipResponseCode.UnknownFailure, message);
            }

            // We have a recognizable response status code
            if (responseCode != InsureShipResponseCode.Success)
            {
                string message = string.Format("An error occurred trying to void a policy for shipment {0} with the InsureShip API: {1}", request.Shipment.ShipmentID, (int) statusCode);
                log.Error(message);

                throw new InsureShipResponseException(responseCode, message);
            }

            return responseCode;
        }
    }
}
