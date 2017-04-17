using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;
using System;
using System.Web.Services.Protocols;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// Used to activate UPS Promo on UPS accounts
    /// </summary>
    public class UpsPromo : IUpsPromo
    {
        private readonly IPromoClientFactory promoClientFactory;
        private readonly IUpsPromoPolicy promoPolicy;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository;
        private readonly UpsAccountEntity account;
        private readonly ICarrierSettingsRepository upsSettingsRepository;
        private PromoAcceptanceTerms terms;

        private const string TestPromoCode = "BVOGIGNA7";
        private const string LivePromoCode = "BXA4S3YB9";

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromo(UpsAccountEntity upsAccount, ICarrierSettingsRepository upsSettingsRepository, ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository, IPromoClientFactory promoFactory, IUpsPromoPolicy promoPolicy)
        {
            AccountId = upsAccount.UpsAccountID;
            AccountNumber = upsAccount.AccountNumber;
            Username = upsAccount.UserID;
            Password = upsAccount.Password;
            AccessLicenseNumber = SecureText.Decrypt(upsSettingsRepository.GetShippingSettings().UpsAccessKey, "UPS");
            CountryCode = upsAccount.CountryCode == "CA" ? "CA" : "US";
            promoClientFactory = promoFactory;
            this.promoPolicy = promoPolicy;
            this.upsAccountRepository = upsAccountRepository;
            account = upsAccount;
            this.upsSettingsRepository = upsSettingsRepository;
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
        /// Gets the account identifier.
        /// </summary>
        public long AccountId { get; }

        /// <summary>
        /// The Promo Terms and Conditions
        /// </summary>
        public PromoAcceptanceTerms Terms => terms ?? (terms = GetAgreementTerms());

        /// <summary>
        /// Gets the promo code to send, based on the state from the UPS account's address
        /// </summary>
        public string PromoCode
        {
            get
            {
                if (!CountryCode.Equals("US", StringComparison.InvariantCultureIgnoreCase))
                {
                    return string.Empty;
                }

                return upsSettingsRepository.UseTestServer ? TestPromoCode : LivePromoCode;
            }
        }

        /// <summary>
        /// Applies the Promo Code
        /// </summary>
        public void Apply()
        {
            // Check to see if the terms have been accepted
            if (Terms.IsAccepted == false)
            {
                throw new UpsPromoException("You must first accept the Terms and Conditions");
            }
            
            try
            {
                IUpsApiPromoClient client = promoClientFactory.CreatePromoClient(this);
                PromoActivation promoActivation = client.Activate(Terms.AcceptanceCode, account.AccountNumber);

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
            catch (UpsPromoException ex)
            {
                SoapException soapEx = ex.InnerException as SoapException;

                string errCode = soapEx?.Detail?.SelectSingleNode("//*[local-name()='Code']")?.InnerText;
                if (errCode == "9560008")
                {
                    // "Account is already signed up for this promotion"
                    // Set the promo status on the account as applied because our promo has already been applied
                    account.PromoStatus = (int)UpsPromoStatus.Applied;
                    upsAccountRepository.Save(account);
                }
                else if (errCode == "9560010")
                {
                    // Account is on a bid that cannot be overridden
                    // Set the promo status to declined because our promo cannot be applied
                    Decline();
                    throw  new UpsPromoException("Your UPS Account is on a promo that cannot be overridden.");
                }
                else
                {
                    // This is an error we dont know about, could be an outage so remind me later
                    RemindMe();
                    throw;
                }
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
        /// Remind the user about the promo later
        /// </summary>
        public void RemindMe()
        {
            promoPolicy.RemindLater(this);
        }

        /// <summary>
        /// Gets the PromoAcceptanceTerms from UpsApiPromoClient
        /// </summary>
        private PromoAcceptanceTerms GetAgreementTerms()
        {
            IUpsApiPromoClient client = promoClientFactory.CreatePromoClient(this);
            return client.GetAgreement();
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