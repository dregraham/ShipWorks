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
        /// <param name="parameters">Parameters that allow footnotes to interact with the rates grid</param>
        /// <returns>A instance of a RateFoonoteControl.</returns>
        RateFootnoteControl CreateFootnote(FootnoteParameters parameters);
    }
}
