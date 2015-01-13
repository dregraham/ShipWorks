namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Factory to create an InvalidPackageDimensionsRateFootnoteControl
    /// </summary>
    class InvalidPackageDimensionsRateFootnoteFactory : IRateFootnoteFactory
    {
        private readonly string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPackageDimensionsRateFootnoteFactory"/> class.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="errorMessage">The details about the error (i.e. why this footnote factory is needed).</param>
        public InvalidPackageDimensionsRateFootnoteFactory(ShipmentType shipmentType, string errorMessage)
        {
            ShipmentType = shipmentType;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        /// <value>The type of the shipment.</value>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <param name="parameters">Parameters that allow footnotes to interact with the rates grid</param>
        /// <returns>A instance of a RateFootnoteControl.</returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new InvalidPackageDimensionsRateFootnoteControl(errorMessage);
        }
    }
}
