using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Used to activate UPS Promo on UPS accounts
    /// </summary>
    public class UpsPromo
    {
        private readonly IPromoClientFactory promoClientFactory;
        private readonly ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository;
        private readonly UpsAccountEntity account;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromo(UpsAccountEntity upsAccount, ICarrierSettingsRepository upsSettingsRepository, ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository, IPromoClientFactory promoFactory)
        {
            AccountNumber = upsAccount.AccountNumber;
            Username = upsAccount.UserID;
            Password = upsAccount.Password;
            AccessLicenseNumber = SecureText.Decrypt(upsSettingsRepository.GetShippingSettings().UpsAccessKey, "UPS");
            CountryCode = upsAccount.CountryCode == "CA" ? "CA" : "US";
            promoClientFactory = promoFactory;
            this.upsAccountRepository = upsAccountRepository;
            account = upsAccount;
        }

        /// <summary>
        /// The UPS Account Number
        /// </summary>
        public string AccountNumber { get; }

        /// <summary>
        /// The UPS Accounts UserId
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// The UPS Accounts Password
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// The Access License Number
        /// </summary>
        public string AccessLicenseNumber { get; }

        /// <summary>
        /// The Country Code of the UPS Account
        /// </summary>
        public string CountryCode { get; }

        /// <summary>
        /// The Promo Terms and Conditions
        /// </summary>
        public PromoAcceptanceTerms Terms { get; set; }

        /// <summary>
        /// Activates the Promo Code
        /// </summary>
        public void Apply()
        {
            // Check to see if the terms have been accepted
            if (Terms.IsAccepted == false)
            {
                throw new UpsPromoException("You must first accept the Terms and Conditions");
            }

            IUpsApiPromoClient client = promoClientFactory.CreatePromoClient(this);
            PromoActivation promoActivation = client.Activate(Terms.AcceptanceCode);

            // If the activation was successful save it to the UpsAccount Entity
            // Otherwise throw exception containing the info about the failure 
            if (promoActivation.IsSuccessful)
            {
                account.PromoStatus = (int)UpsPromoStatus.Applied;
                upsAccountRepository.Save(account);
            }
            else
            {
                throw new UpsPromoException(promoActivation.Info);
            }
        }

        /// <summary>
        /// Sets the PromoStatus of the UpsAccount to Declined
        /// </summary>
        public void Decline()
        {
            account.PromoStatus = (int)UpsPromoStatus.Declined;
            upsAccountRepository.Save(account);
        }

        /// <summary>
        /// Gets the PromoAcceptanceTerms from UpsApiPromoClient
        /// </summary>
        /// <returns></returns>
        public PromoAcceptanceTerms GetAgreementTerms()
        {
            IUpsApiPromoClient client = promoClientFactory.CreatePromoClient(this);
            Terms = client.GetAgreement();
            return Terms;
        }

        /// <summary>
        /// Gets the PromoStatus for the UpsAccountEntity
        /// </summary>
        /// <returns></returns>
        public UpsPromoStatus GetStatus()
        {
            return (UpsPromoStatus)account.PromoStatus;
        }
    }
}