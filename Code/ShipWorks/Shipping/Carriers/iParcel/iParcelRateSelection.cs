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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        public override bool Equals(object obj)
        {
            bool isEqual = false;

            iParcelRateSelection rateSelection = obj as iParcelRateSelection;
            if (rateSelection != null)
            {
                isEqual = rateSelection.ServiceType == this.ServiceType;
            }

            return isEqual;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return serviceType.GetHashCode();
        }
    }
}
