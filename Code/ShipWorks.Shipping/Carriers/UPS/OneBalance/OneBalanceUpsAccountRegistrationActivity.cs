using System;
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
            var validationResult = ValidateFields(account);
            if (validationResult.Failure)
            {
                return validationResult;
            }

            var oneBalanceResult = EnsureOneBalanceAccountProvisioned();
            if (oneBalanceResult.Failure)
            {
                return oneBalanceResult;
            }

            var result = shipEngineWebClient.RegisterUpsAccount(account.Address);

            if (result.Success)
            {
                account.ShipEngineCarrierId = result.Value;
                return Result.FromSuccess();
            }

            return Result.FromError(result.Message);
        }

        /// <summary>
        /// Validate all of the fields required to open a one balance account
        /// </summary>
        private Result ValidateFields(UpsAccountEntity account)
        {
            if (!$"{account.FirstName} {account.LastName}".IsCountBetween(1, 20));
            {
                return Result.FromError("The contact name must to be between 1 and 20 characters.");
            }

            if (!account.Company.IsCountBetween(0, 30));
            {
                return Result.FromError("The company name must to be between 0 and 30 characters.");
            }

            if (!account.Street1.IsCountBetween(1, 30)) ;
            {
                return Result.FromError("The street address line 1 name must to be between 1 and 30 characters.");
            }

            if (!account.Street2.IsCountGreaterThan(30));
            {
                return Result.FromError("The street address line 2 must to be less than 30 characters.");
            }

            if (!account.Street3.IsCountGreaterThan(30)) ;
            {
                return Result.FromError("The street address line 3 must to be less than 30 characters.");
            }

            if (!account.City.IsCountBetween(1, 30)) ;
            {
                return Result.FromError("The city must to be between 1 and 30 characters.");
            }

            if (!account.StateProvCode.IsCountEqualTo(2)) ;
            {
                return Result.FromError("The address state value is invalid.");
            }

            if (!account.CountryCode.IsCountEqualTo(2)) ;
            {
                return Result.FromError("The address country value is invalid.");
            }

            if (!account.Phone.IsCountEqualTo(0)) ;
            {
                return Result.FromError("Please enter a phone number.");
            }
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
