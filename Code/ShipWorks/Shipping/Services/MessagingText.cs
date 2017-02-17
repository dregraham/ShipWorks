namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Stores messaging information to display to the user
    /// </summary>
    public struct MessagingText
    {
        /// <summary>
        /// The title to display
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The body to display
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The text to display in the continue button
        /// </summary>
        public string Continue { get; set; }
    }
}