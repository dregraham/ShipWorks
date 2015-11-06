using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Creates a AmazonUspsNotLinkedFootnoteControl
    /// </summary>
    public class AmazonNotLinkedFootnoteFactory : IRateFootnoteFactory
    {
        private readonly string accountTypeToDisplay;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonNotLinkedFootnoteFactory"/> class.
        /// </summary>
        /// <param name="accountTypeToDisplay">The name of the shipping service account to display in the user facing messaging</param>
        public AmazonNotLinkedFootnoteFactory(string accountTypeToDisplay)
        {
            this.accountTypeToDisplay = accountTypeToDisplay;
        }

        /// <summary>
        /// Returns Amazon ShipmentTypeCode
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode => 
            ShipmentTypeCode.Amazon;

        /// <summary>
        /// Creates the footnote.
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters) => 
            new AmazonNotLinkedFootnoteControl(accountTypeToDisplay);

        /// <summary>
        /// Notes that this factory should or should not be used in BestRate
        /// For example, when using BestRate, we do not want Usps promo footnotes to display, so this will be set to false.
        /// </summary>
        public bool AllowedForBestRate => true;
    }
}