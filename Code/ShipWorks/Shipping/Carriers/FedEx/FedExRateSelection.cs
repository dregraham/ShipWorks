using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Data class to hold FedEx rate information
    /// </summary>
    public class FedExRateSelection
    {
        FedExServiceType serviceType;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExRateSelection(FedExServiceType serviceType)
        {
            this.serviceType = serviceType;
        }

        /// <summary>
        /// The FedEx service type that was selected
        /// </summary>
        public FedExServiceType ServiceType
        {
            get { return serviceType; }
        }
    }
}
