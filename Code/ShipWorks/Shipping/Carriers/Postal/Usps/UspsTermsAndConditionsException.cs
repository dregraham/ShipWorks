namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Exception thrown when terms and conditions need to be accepted before processing
    /// </summary>
    public class UspsTermsAndConditionsException : UspsException, ITermsAndConditionsException
    {
        public IUspsTermsAndConditions TermsAndConditions { get; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsTermsAndConditionsException(string message, IUspsTermsAndConditions termsAndConditions) :
            base(message)
        {
            TermsAndConditions = termsAndConditions;
        }
    }
}
