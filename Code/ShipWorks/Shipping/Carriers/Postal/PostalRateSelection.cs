using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Simple class that is used to convey the chosen rate from the rate grid to the service control
    /// </summary>
    public class PostalRateSelection
    {
        PostalServiceType serviceType;
        PostalConfirmationType confirmationType;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalRateSelection(PostalServiceType serviceType, PostalConfirmationType confirmationType)
        {
            this.serviceType = serviceType;
            this.confirmationType = confirmationType;
        }

        /// <summary>
        /// The selected service type
        /// </summary>
        public PostalServiceType ServiceType
        {
            get { return serviceType; }
        }

        /// <summary>
        /// The selected confirmation type
        /// </summary>
        public PostalConfirmationType ConfirmationType
        {
            get { return confirmationType; }
        }
    }
}
