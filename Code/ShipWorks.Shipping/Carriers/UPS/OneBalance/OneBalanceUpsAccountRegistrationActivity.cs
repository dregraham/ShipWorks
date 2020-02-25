using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.OneBalance;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Ups.OneBalance
{
    /// <summary>
    /// Activity for registering a Ups ShipWorks OneBalance account
    /// </summary>
    public class OneBalanceUpsAccountRegistrationActivity : IOneBalanceUpsAccountRegistrationActivity
    {
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceUpsAccountRegistrationActivity(IShipEngineWebClient shipEngineWebClient, 
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository)
        {
            this.shipEngineWebClient = shipEngineWebClient;
            this.uspsAccountRepository = uspsAccountRepository;
        }

        /// <summary>
        /// Execute the activity on the given account
        /// </summary>
        public Result Execute(UpsAccountEntity account)
        {
            EnsureOneBalanceAccountProvisioned();

            var result = shipEngineWebClient.RegisterUpsAccount(new PersonAdapter(account, string.Empty));

            if (result.Success)
            {
                account.ShipEngineCarrierId = result.Value;
                return Result.FromSuccess();
            }

            return Result.FromError(result.Message);
        }

        /// <summary>
        /// Ensure that a OneBalance account has been provisioned
        /// </summary>
        private Result EnsureOneBalanceAccountProvisioned()
        {
            if(uspsAccountRepository.Accounts.IsCountEqualTo(1))
            {
                UspsAccountEntity uspsAccount = uspsAccountRepository.Accounts.Single();

                if (uspsAccount.ShipEngineCarrierId == null)
                {
                    return CreateOneBalanceAccount(uspsAccount);
                }
                else
                {
                    return Result.FromSuccess();
                }
            }

            // future stories will handle multiple or no usps accounts
            return Result.FromError("The number of USPS accounts is not One!");
        }

        /// <summary>
        /// Create a one balance account by adding Stamps.com to ShipEngine
        /// </summary>
        private Result CreateOneBalanceAccount(UspsAccountEntity uspsAccount)
        {
            var carrierId = shipEngineWebClient.ConnectStampsAccount(uspsAccount.Username, uspsAccount.Password);

            if (carrierId.Success)
            {

                uspsAccount.ShipEngineCarrierId = carrierId.Value;
                uspsAccountRepository.Save(uspsAccount);
                return Result.FromSuccess();
            }
            
            return Result.FromError(carrierId.Message);
        }
    }
}
