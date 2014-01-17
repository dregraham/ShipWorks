namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// A factory interface for being able to create footnote controls. 
    /// </summary>
    public interface IRateFootnoteFactory
    {
        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        ShipmentType ShipmentType { get; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <returns>A instance of a RateFoonoteControl.</returns>
        RateFootnoteControl CreateFootnote();
    }
}
