using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.PayPal.WebServices;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Used to activate UPS Promo on UPS accounts
    /// </summary>
    public class UpsPromo
    {
        private string AccountNumber;
        private string UserId;
        private string Password;
        private string AccessLicenseNumber;
        private CountryCodeType CountryCode;
        public PromoAcceptanceTerms Terms;

        public UpsPromo(int accountId, string licenseNumber, ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository, IPromoClientFactory promoFactory)
        {
            
        }

        public UpsPromo(UpsAccountEntity upsAccount, string licenseNumber, ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository, IPromoClientFactory promoFactory)
        {
            
        }

        /// <summary>
        /// Activates the Promo Code
        /// </summary>
        public void Apply()
        {
            
        }

        /// <summary>
        /// Gets the PromoAcceptanceTerms from UpsApiPromoClient
        /// </summary>
        /// <returns></returns>
        public PromoAcceptanceTerms GetAgreementTerms()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the PromoStatus for the UpsAccountEntity
        /// </summary>
        /// <returns></returns>
        public UpsPromoStatus GetStatus()
        {
            throw new NotImplementedException();
        }
    }
}