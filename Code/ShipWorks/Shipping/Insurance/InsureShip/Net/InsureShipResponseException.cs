using System;
using System.Net;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// An InsureShip response specific exception
    /// </summary>
    public class InsureShipResponseException : InsureShipException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipResponseException(HttpStatusCode insureShipResponseCode)
        {
            HttpStatusCode = insureShipResponseCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipResponseException(HttpStatusCode insureShipResponseCode, string message)
            : base(message)
        {
            HttpStatusCode = insureShipResponseCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipResponseException(HttpStatusCode insureShipResponseCode, string message, Exception inner)
            : base(message, inner)
        {
            HttpStatusCode = insureShipResponseCode;
        }

        /// <summary>
        /// Property giving access to this exception's HttpStatusCode
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; private set; }
    }
}
