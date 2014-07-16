using System;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Shipping.Insurance.InsureShip.Enums;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    public class InsureShipmentResponse : IInsureShipResponse
    {
        private readonly InsureShipRequestBase request;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipmentResponse" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="log">The log.</param>
        public InsureShipmentResponse(InsureShipRequestBase request, ILog log)
        {
            this.request = request;
            this.log = log;
        }

        /// <summary>
        /// Process a response from InsureShip. This will inspect the raw response to ensure the 
        /// request was successful. A response code of anything other than 204 will result in an
        /// InsureShipResponseException being thrown.
        /// </summary>
        /// <exception cref="ShipWorks.Shipping.Insurance.InsureShip.InsureShipResponseException">
        /// </exception>
        public void Process()
        {            
            InsureShipResponseCode responseCode;
            
            try
            {
                responseCode = EnumHelper.GetEnumByApiValue<InsureShipResponseCode>(request.RawResponse.StatusCode.ToString());
            }
            catch (Exception)
            {
                string message = string.Format("An unknown response code was received from the InsureShip API for shipment {0}: {1}", request.Shipment.ShipmentID, request.RawResponse.StatusCode);
                log.Error(message);

                throw new InsureShipResponseException(InsureShipResponseCode.UnknownFailure, message);
            }

            // We have a recognizable response status code
            if (responseCode != InsureShipResponseCode.Success)
            {
                string message = string.Format("An error occurred trying to insure shipment {0} with the InsureShip API: {1}", request.Shipment.ShipmentID, request.RawResponse.StatusCode);
                log.Error(message);

                throw new InsureShipResponseException(responseCode, message);
            }
        }
    }
}
