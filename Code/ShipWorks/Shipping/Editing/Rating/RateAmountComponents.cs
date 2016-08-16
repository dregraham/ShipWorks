namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Extra rate amounts
    /// </summary>
    public struct RateAmountComponents
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RateAmountComponents(decimal? taxes, decimal? duties, decimal? shipping)
        {
            Taxes = taxes;
            Duties = duties;
            Shipping = shipping;
        }

        /// <summary>
        /// The amount of taxes included in the rate
        /// </summary>
        public decimal? Taxes { get; }

        /// <summary>
        /// The amount of duties included in the rate
        /// </summary>
        public decimal? Duties { get; }

        /// <summary>
        /// The portion of the amount that goes for shipping
        /// </summary>
        public decimal? Shipping { get; }
    }
}
