using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    public interface IUpsPromo
    {
        /// <summary>
        /// The UPS Account Number
        /// </summary>
        string AccountNumber { get; }

        /// <summary>
        /// The UPS Accounts UserId
        /// </summary>
        string Username { get; }

        /// <summary>
        /// The UPS Accounts Password
        /// </summary>
        string Password { get; }

        /// <summary>
        /// The Access License Number
        /// </summary>
        string AccessLicenseNumber { get; }

        /// <summary>
        /// The Country Code of the UPS Account
        /// </summary>
        string CountryCode { get; }

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        long AccountId { get; }

        /// <summary>
        /// The Promo Terms and Conditions
        /// </summary>
        PromoAcceptanceTerms Terms { get; }

        /// <summary>
        /// Gets the promo code
        /// </summary>
        string PromoCode { get; }

        /// <summary>
        /// Activates the Promo Code
        /// </summary>
        PromoActivation Apply();

        /// <summary>
        /// Sets the PromoStatus of the UpsAccount to Declined
        /// </summary>
        void Decline();

        /// <summary>
        /// Gets the PromoStatus for the UpsAccountEntity
        /// </summary>
        /// <returns></returns>
        UpsPromoStatus GetStatus();

        /// <summary>
        /// Remind the user of the paromo later
        /// </summary>
        void RemindMe();
    }
}