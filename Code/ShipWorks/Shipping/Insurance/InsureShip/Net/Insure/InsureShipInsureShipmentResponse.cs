using System;
using System.Globalization;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Insure
{
    public class InsureShipInsureShipmentResponse : IInsureShipResponse
    {
        private readonly InsureShipRequestBase request;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipInsureShipmentResponse" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="log">The log.</param>
        public InsureShipInsureShipmentResponse(InsureShipRequestBase request, ILog log)
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
            
            try
            {
                responseCode = EnumHelper.GetEnumByApiValue<InsureShipResponseCode>(((int)request.RawResponse.StatusCode).ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
                int statusCode = request.RawResponse != null ? (int) request.RawResponse.StatusCode : -0;
                string message = string.Format("An unknown response code was received from the InsureShip API for shipment {0}: {1}", request.Shipment.ShipmentID, statusCode);
                log.Error(message);

                throw new InsureShipResponseException(InsureShipResponseCode.UnknownFailure, message);
            }

            // We have a recognizable response status code
            if (responseCode != InsureShipResponseCode.Success)
            {
                int statusCode = request.RawResponse != null ? (int)request.RawResponse.StatusCode : -0;
                string message = string.Format("An error occurred trying to insure shipment {0} with the InsureShip API: {1}", request.Shipment.ShipmentID, statusCode);
                log.Error(message);

                throw new InsureShipResponseException(responseCode, message);
            }

            return responseCode;
        }
    }
}
