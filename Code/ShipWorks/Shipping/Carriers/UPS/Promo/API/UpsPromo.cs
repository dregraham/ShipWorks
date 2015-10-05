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
        public readonly IPromoClientFactory PromoClientFactory;
        public PromoAcceptanceTerms Terms;
        private readonly ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository;
        private readonly UpsAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromo(int accountId, string licenseNumber, ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository, IPromoClientFactory promoFactory)
        {
            UpsAccountEntity upsAccount = upsAccountRepository.GetAccount(accountId);

            AccountNumber = upsAccount.AccountNumber;
            Username = upsAccount.UserID;
            Password = upsAccount.Password;
            AccessLicenseNumber = licenseNumber;
            CountryCode = upsAccount.CountryCode == "CA" ? "CA" : "US";
            PromoClientFactory = promoFactory;
            this.upsAccountRepository = upsAccountRepository;
            account = upsAccount;
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromo(UpsAccountEntity upsAccount, string licenseNumber, ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository, IPromoClientFactory promoFactory)
        {
            AccountNumber = upsAccount.AccountNumber;
            Username = upsAccount.UserID;
            Password = upsAccount.Password;
            AccessLicenseNumber = licenseNumber;
            CountryCode = upsAccount.CountryCode == "CA" ? "CA" : "US";
            PromoClientFactory = promoFactory;
            this.upsAccountRepository = upsAccountRepository;
            account = upsAccount;
        }

        /// <summary>
        /// Activates the Promo Code
        /// </summary>
        public PromoActivation Apply()
        {
            // Check to see if the terms have been accepted
            if (Terms.IsAccepted == false)
            {
                throw new UpsPromoException("You must first accept the Terms and Conditions");
            }

            IUpsApiPromoClient client = PromoClientFactory.CreatePromoClient(this);
            PromoActivation promoActivation = client.Activate(Terms.AcceptanceCode);

            // If the activation was successful save it to the UpsAccount Entity
            if (promoActivation.IsSuccessful)
            {
                account.PromoStatus = (int)UpsPromoStatus.Applied;
                upsAccountRepository.Save(account);
            }

            return promoActivation;
        }

        /// <summary>
        /// Gets the PromoAcceptanceTerms from UpsApiPromoClient
        /// </summary>
        /// <returns></returns>
        public PromoAcceptanceTerms GetAgreementTerms()
        {
            IUpsApiPromoClient client = PromoClientFactory.CreatePromoClient(this);
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