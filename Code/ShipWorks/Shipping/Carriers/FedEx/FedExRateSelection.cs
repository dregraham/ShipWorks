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
        private FedExServiceType serviceType;

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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        public override bool Equals(object obj)
        {
            bool isEqual = false;

            FedExRateSelection rateSelection = obj as FedExRateSelection;
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
