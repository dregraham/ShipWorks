using System.Diagnostics;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// Creates a AmazonUspsNotLinkedFootnoteControl
    /// </summary>
    public class AmazonNotLinkedFootnoteFactory : IAmazonNotLinkedFootnoteFactory
    {
        private readonly ShipmentTypeCode shipmentTypeCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonNotLinkedFootnoteFactory"/> class.
        /// </summary>
        public AmazonNotLinkedFootnoteFactory(ShipmentTypeCode shipmentTypeCode)
        {
            Debug.Assert(shipmentTypeCode==ShipmentTypeCode.Usps || shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools);
            this.shipmentTypeCode = shipmentTypeCode;
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
            new AmazonNotLinkedFootnoteControl(shipmentTypeCode);

        /// <summary>
        /// Notes that this factory should or should not be used in BestRate
        /// For example, when using BestRate, we do not want Usps promo footnotes to display, so this will be set to false.
        /// </summary>
        public bool AllowedForBestRate => true;
    }
}