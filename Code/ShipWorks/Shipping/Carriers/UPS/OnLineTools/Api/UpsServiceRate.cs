using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Data structure for storing rating information for a UPS service
    /// </summary>
    public class UpsServiceRate
    {
        UpsServiceType service;
        decimal amount;
        bool negotiated;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsServiceRate(UpsServiceType service, decimal amount, bool negotiated)
        {
            this.service = service;
            this.amount = amount;
            this.negotiated = negotiated;
        }

        /// <summary>
        /// The service the rate is for
        /// </summary>
        public UpsServiceType Service
        {
            get { return service; }
        }

        /// <summary>
        /// The cost to ship with the service
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
        }

        /// <summary>
        /// Indicates if the rate is a negotiated "Account Based Rate" (ABR)
        /// </summary>
        public bool Negotiated
        {
            get { return negotiated; }
        }
    }
}
