namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Factory to create an ExceptionsRateFootnote
    /// </summary>
    public class ExceptionsRateFootnoteFactory : IRateFootnoteFactory
    {
        private readonly string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionsRateFootnoteFactory"/> class.
        /// </summary>
        public ExceptionsRateFootnoteFactory(ShipmentType shipmentType, string errorMessage)
        {
            ShipmentType = shipmentType;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new ExceptionsRateFootnoteControl(errorMessage);
        }
    }
}
