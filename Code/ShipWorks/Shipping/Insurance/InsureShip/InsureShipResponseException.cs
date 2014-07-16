using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Insurance.InsureShip.Enums;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// An InsureShip response specific exception 
    /// </summary>
    public class InsureShipResponseException : InsureShipException
    {
        private InsureShipResponseCode insureShipResponseCode;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipResponseException(InsureShipResponseCode insureShipResponseCode)
        {
            this.insureShipResponseCode = insureShipResponseCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipResponseException(InsureShipResponseCode insureShipResponseCode, string message)
            : base(message)
        {
            this.insureShipResponseCode = insureShipResponseCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipResponseException(InsureShipResponseCode insureShipResponseCode, string message, Exception inner)
            : base(message, inner)
        {
            this.insureShipResponseCode = insureShipResponseCode;
        }

        /// <summary>
        /// Property giving access to this exception's InsureShipResponseCode 
        /// </summary>
        public InsureShipResponseCode InsureShipResponseCode
        {
            get
            {
                return insureShipResponseCode;
            }
        }
    }
}
