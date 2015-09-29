using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Represents the data necessary to send to FIMS to process a shipment
    /// </summary>
    public class FimsShipRequest : IFimsShipRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FimsShipRequest(ShipmentEntity shipment, string username, string password)
        {
            this.Shipment = shipment;
            this.Username = username;
            this.Password = password;
        }

        /// <summary>
        /// The shipment to process
        /// </summary>
        public ShipmentEntity Shipment { get; private set; }

        /// <summary>
        /// The FIMS username
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// The FIMS password
        /// </summary>
        public string Password { get; private set; }
    }
}
