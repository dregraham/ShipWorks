namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    /// <summary>
    /// Need to use Express1 instead of IntuiShip when converting accounts
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
