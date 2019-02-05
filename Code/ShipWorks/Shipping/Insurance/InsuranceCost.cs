namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Cost of insurance
    /// </summary>
    public class InsuranceCost
    {
        private string infoMessage;

        /// <summary>
        /// Total cost of ShipWorks insurance
        /// </summary>
        public decimal? ShipWorks { get; set; }

        /// <summary>
        /// Rate of ShipWorks insurance per $100
        /// </summary>
        public decimal? ShipWorksRate { get; set; }

        /// <summary>
        /// Carrier
        /// </summary>
        public decimal? Carrier { get; set; }

        /// <summary>
        /// Info message
        /// </summary>
        public string InfoMessage { get { return infoMessage; } }

        /// <summary>
        /// Should PennyOne be advertised
        /// </summary>
        public bool AdvertisePennyOne { get; set; }

        /// <summary>
        /// Add the given message to be displayed to the user
        /// </summary>
        public void AddInfoMessage(string message)
        {
            if (infoMessage != null)
            {
                infoMessage += "\n\n" + message;
            }
            else
            {
                infoMessage = message;
            }
        }
    }
}
