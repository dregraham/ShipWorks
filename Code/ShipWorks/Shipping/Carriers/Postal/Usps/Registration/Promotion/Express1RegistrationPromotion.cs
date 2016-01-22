namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    /// <summary>
    /// Promo needed to convert or create an Express1 account
    /// </summary>
    public class Express1RegistrationPromotion : IRegistrationPromotion
    {
        /// <summary>
        /// The is monthly fee waived
        /// </summary>
        public bool IsMonthlyFeeWaived => false;

        /// <summary>
        /// Gets the promo code.
        /// </summary>
        public string GetPromoCode() => "ShipWorks667";
    }
}
