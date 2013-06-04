using System;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    /// <summary>
    /// An exception to indicate that the pickup address was not accepted when opening a UPS account.
    /// </summary>
    public class UpsOpenAccountPickupAddressException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountBusinessAddressException" /> class.
        /// </summary>
        /// <param name="addressCandidate">The address candidate.</param>
        public UpsOpenAccountPickupAddressException(AddressKeyCandidateType addressCandidate)
            : this(addressCandidate, string.Empty, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountBusinessAddressException" /> class.
        /// </summary>
        /// <param name="addressCandidate">The address candidate.</param>
        /// <param name="message">The message.</param>
        public UpsOpenAccountPickupAddressException(AddressKeyCandidateType addressCandidate, string message)
            : this(addressCandidate, message, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountBusinessAddressException" /> class.
        /// </summary>
        /// <param name="addressCandidate">The address candidate.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpsOpenAccountPickupAddressException(AddressKeyCandidateType addressCandidate, string message, Exception innerException)
            : base(message, innerException)
        {
            SuggestedAddress = addressCandidate;
        }

        /// <summary>
        /// Gets the address suggested by UPS.
        /// </summary>
        /// <value>The address candidate.</value>
        public AddressKeyCandidateType SuggestedAddress { get; private set; }
    }
}
