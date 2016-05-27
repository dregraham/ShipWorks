using System;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    /// <summary>
    /// An exception to indicate that the business address was not accepted when opening a UPS account.
    /// </summary>
    [Serializable]
    public class UpsOpenAccountBusinessAddressException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountBusinessAddressException" /> class.
        /// </summary>
        /// <param name="addressCandidate">The address candidate.</param>
        public UpsOpenAccountBusinessAddressException(AddressKeyCandidateType addressCandidate)
            : this(addressCandidate, string.Empty, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountBusinessAddressException" /> class.
        /// </summary>
        /// <param name="addressCandidate">The address candidate.</param>
        /// <param name="message">The message.</param>
        public UpsOpenAccountBusinessAddressException(AddressKeyCandidateType addressCandidate, string message)
            : this (addressCandidate, message, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountBusinessAddressException" /> class.
        /// </summary>
        /// <param name="addressCandidate">The address candidate.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpsOpenAccountBusinessAddressException(AddressKeyCandidateType addressCandidate, string message, Exception innerException)
            : base(message, innerException)
        {
            SuggestedAddress = addressCandidate;
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected UpsOpenAccountBusinessAddressException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }

        /// <summary>
        /// Gets the address suggested by UPS.
        /// </summary>
        /// <value>The address candidate.</value>
        public AddressKeyCandidateType SuggestedAddress { get; private set; }
    }
}
