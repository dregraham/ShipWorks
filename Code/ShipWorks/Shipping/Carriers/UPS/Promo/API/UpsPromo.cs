using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Used to activate UPS Promo on UPS accounts
    /// </summary>
    public class UpsPromo
    {
        public readonly string AccountNumber;
        public readonly string Username;
        public readonly string Password;
        public readonly string AccessLicenseNumber;
        public readonly string CountryCode;
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