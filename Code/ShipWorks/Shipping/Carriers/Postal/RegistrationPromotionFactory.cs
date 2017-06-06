using System;
using System.Linq;
using Autofac.Features.AttributeFilters;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Factory that creates registration promotion classes based on which types of accounts a user has
    /// </summary>
    [Component(RegistrationType.Self)]
    public class RegistrationPromotionFactory
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly Lazy<bool> uspsAccountsExist;
        private readonly Lazy<bool> endiciaAccountsExist;
        private readonly Lazy<bool> express1AccountsExist;

        /// <summary>
        /// Constructor that allows easier testing of the factory
        /// </summary>
        public RegistrationPromotionFactory(
            [KeyFilter(ShipmentTypeCode.Usps)] ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository,
            [KeyFilter(ShipmentTypeCode.Express1Usps)] ICarrierAccountRetriever uspsExpress1AccountRepository,
            [KeyFilter(ShipmentTypeCode.Endicia)] ICarrierAccountRetriever endiciaAccountRepository,
            [KeyFilter(ShipmentTypeCode.Express1Endicia)] ICarrierAccountRetriever endiciaExpress1AccountRepository)
        {
            this.uspsAccountRepository = uspsAccountRepository;

            // Use lazy values so that the constructor does not throw in tests that don't have the entire account system set up
            uspsAccountsExist = new Lazy<bool>(() => uspsAccountRepository.AccountsReadOnly.Any());
            endiciaAccountsExist = new Lazy<bool>(() => endiciaAccountRepository.AccountsReadOnly.Any());
            express1AccountsExist = new Lazy<bool>(() =>
                uspsExpress1AccountRepository.AccountsReadOnly.Any() ||
                endiciaExpress1AccountRepository.AccountsReadOnly.Any());
        }

        /// <summary>
        /// Creates the registration promotion based on the Postal accounts in ShipWorks.
        /// </summary>
        /// <returns>An instance of IRegistrationPromotion.</returns>
        public IRegistrationPromotion CreateRegistrationPromotion()
        {
            if (!PostalAccountsExist() || OnlyExpress1AccountsExist())
            {
                return new Express1RegistrationPromotion();
            }

            if (endiciaAccountsExist.Value)
            {
                return AnyUspsResellerAccountsExist() ?
                    (IRegistrationPromotion) new EndiciaCbpRegistrationPromotion() :
                    new Express1RegistrationPromotion();
            }

            return AnyUspsResellerAccountsExist() ?
                (IRegistrationPromotion) new UspsCbpRegistrationPromotion() :
                new Express1RegistrationPromotion();
        }

        /// <summary>
        /// Are there any postal accounts in the system?
        /// </summary>
        private bool PostalAccountsExist()
        {
            return (endiciaAccountsExist.Value || uspsAccountsExist.Value || express1AccountsExist.Value);
        }

        /// <summary>
        /// Are there only Express1 accounts in the system?
        /// </summary>
        private bool OnlyExpress1AccountsExist()
        {
            return express1AccountsExist.Value && !uspsAccountsExist.Value && !endiciaAccountsExist.Value;
        }

        /// <summary>
        /// Is any Usps account a reseller account?
        /// </summary>
        /// <returns></returns>
        private bool AnyUspsResellerAccountsExist()
        {
            return uspsAccountRepository.AccountsReadOnly.Any(x => x.ContractType == (int) UspsAccountContractType.Reseller);
        }
    }
}
