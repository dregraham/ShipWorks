namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// The result of a validation
    /// </summary>
    public interface ILocalRateValidationResult
    {
        /// <summary>
        /// Gets the validation message.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Shows the validation message.
        /// </summary>
        void ShowMessage();
    }
}