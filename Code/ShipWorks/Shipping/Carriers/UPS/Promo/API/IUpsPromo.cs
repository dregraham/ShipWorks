using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
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
        PromoAcceptanceTerms Terms { get; set; }

        /// <summary>
        /// Activates the Promo Code
        /// </summary>
        void Apply();

        /// <summary>
        /// Sets the PromoStatus of the UpsAccount to Declined
        /// </summary>
        void Decline();

        /// <summary>
        /// Gets the PromoAcceptanceTerms from UpsApiPromoClient
        /// </summary>
        /// <returns></returns>
        PromoAcceptanceTerms GetAgreementTerms();

        /// <summary>
        /// Gets the PromoStatus for the UpsAccountEntity
        /// </summary>
        /// <returns></returns>
        UpsPromoStatus GetStatus();
    }
}