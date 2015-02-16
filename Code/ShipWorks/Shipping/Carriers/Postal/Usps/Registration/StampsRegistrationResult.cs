using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// A data transfer object that encapsulates the registration status and values of the output parameters
    /// from the Stamps API registration request.
    /// </summary>
    public class UspsRegistrationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRegistrationResult"/> class.
        /// </summary>
        /// <param name="registrationStatus">The registration status.</param>
        /// <param name="suggestedUsername">The suggested username.</param>
        /// <param name="promoUrl">The promo URL.</param>
        public UspsRegistrationResult(RegistrationStatus registrationStatus, string suggestedUsername, string promoUrl)
        {
            IsSuccessful = registrationStatus == RegistrationStatus.Success;
            SuggestedUsername = suggestedUsername;
            PromoUrl = promoUrl;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is successful.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is successful; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// Gets the suggested username.
        /// </summary>
        public string SuggestedUsername { get; private set; }

        /// <summary>
        /// Gets the promo URL.
        /// </summary>
        public string PromoUrl { get; private set; }
    }
}
