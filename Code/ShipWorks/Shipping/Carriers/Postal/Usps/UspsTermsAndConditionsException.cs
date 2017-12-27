namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Exception thrown when terms and conditions need to be accepted before processing
    /// </summary>
    public class UspsTermsAndConditionsException : UspsException, ITermsAndConditionsException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsTermsAndConditionsException(string message, IUspsTermsAndConditions termsAndConditions) :
            base(message)
        {
            TermsAndConditions = termsAndConditions;
        }

        /// <summary>
        /// Associated Usps TermsAndConditions
        /// </summary>
        public IUspsTermsAndConditions TermsAndConditions { get; }
    }
}
