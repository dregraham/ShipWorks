using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.iParcel.Enums;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    public class iParcelRateSelection
    {
        iParcelServiceType serviceType;

        /// <summary>
        /// Constructor
        /// </summary>
        public iParcelRateSelection(iParcelServiceType serviceType)
        {
            this.serviceType = serviceType;
        }

        /// <summary>
        /// The iParcel service type that was selected
        /// </summary>
        public iParcelServiceType ServiceType
        {
            get { return serviceType; }
        }
    }
}
