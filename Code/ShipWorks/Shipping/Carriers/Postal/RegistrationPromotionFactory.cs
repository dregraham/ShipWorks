﻿using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Factory that creates registration promotion classes based on which types of accounts a user has
    /// </summary>
    public class RegistrationPromotionFactory
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly bool uspsAccountsExist;
        private readonly bool endiciaAccountsExist;
        private readonly bool express1AccountsExist;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationPromotionFactory"/> class.
        /// </summary>
        public RegistrationPromotionFactory() :
            this(new UspsAccountRepository(), new Express1UspsAccountRepository(),
                new EndiciaAccountRepository(), new Express1EndiciaAccountRepository())
        {
        }

        /// <summary>
        /// Constructor that allows easier testing of the factory
        /// </summary>
        public RegistrationPromotionFactory(ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsExpress1AccountRepository,
            ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> endiciaAccountRepository,
            ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> endiciaExpress1AccountRepository)
        {
            this.uspsAccountRepository = uspsAccountRepository;

            uspsAccountsExist = uspsAccountRepository.Accounts.Any();
            endiciaAccountsExist = endiciaAccountRepository.Accounts.Any();
            express1AccountsExist = uspsExpress1AccountRepository.Accounts.Any() || endiciaExpress1AccountRepository.Accounts.Any();
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

            if (endiciaAccountsExist)
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
            return (endiciaAccountsExist || uspsAccountsExist || express1AccountsExist);
        }

        /// <summary>
        /// Are there only Express1 accounts in the system?
        /// </summary>
        private bool OnlyExpress1AccountsExist()
        {
            return express1AccountsExist && !uspsAccountsExist && !endiciaAccountsExist;
        }

        /// <summary>
        /// Is any Usps account a reseller account?
        /// </summary>
        /// <returns></returns>
        private bool AnyUspsResellerAccountsExist()
        {
            return uspsAccountRepository.Accounts.Any(x => x.ContractType == (int) UspsAccountContractType.Reseller);
        }
    }
}
