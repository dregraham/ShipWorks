namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Simple class that is used to convey the chosen rate from the rate grid to the service control
    /// </summary>
    public class PostalRateSelection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalRateSelection(PostalServiceType serviceType)
        {
            ServiceType = serviceType;
        }

        /// <summary>
        /// The selected service type
        /// </summary>
        public PostalServiceType ServiceType { get; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        public override bool Equals(object obj)
        {
            bool isEqual = false;
            PostalRateSelection postalRateSelection = obj as PostalRateSelection;

            if (postalRateSelection != null)
            {
                isEqual = this.ServiceType == postalRateSelection.ServiceType;
            }

            return isEqual;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => base.GetHashCode();
    }
}
