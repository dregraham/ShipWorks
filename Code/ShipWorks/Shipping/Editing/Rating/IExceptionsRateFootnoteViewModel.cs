namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Exception footnote view model
    /// </summary>
    public interface IExceptionsRateFootnoteViewModel
    {
        /// <summary>
        /// Text to display in the footnote
        /// </summary>
        string ErrorText { get; set; }

        /// <summary>
        /// Text to display in the 'More info' link
        /// </summary>
        string DetailedMessage { get; set; }
    }
}
