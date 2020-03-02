﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Security;
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
    [Component]
    public class OneBalanceUpsAccountRegistrationActivity : IOneBalanceUpsAccountRegistrationActivity
    {
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceUpsAccountRegistrationActivity(IShipEngineWebClient shipEngineWebClient,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository,
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.shipEngineWebClient = shipEngineWebClient;
            this.uspsAccountRepository = uspsAccountRepository;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Execute the activity on the given account
        /// </summary>
        public async Task<Result> Execute(UpsAccountEntity account)
        {
            var validationResult = ValidateFields(account);
            if (validationResult.Failure)
            {
                return validationResult;
            }

            var oneBalanceResult = await EnsureOneBalanceAccountProvisioned().ConfigureAwait(false);
            if (oneBalanceResult.Failure)
            {
                return oneBalanceResult;
            }

            var result = await shipEngineWebClient.RegisterUpsAccount(account.Address).ConfigureAwait(false);
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
            var results = new List<Result>()
            {
                account.Address.UnparsedName.ValidateLength(20, 1, "The contact name must be between 1 and 20 characters."),
                account.Company.ValidateLength(30, 0, "The company name must be less than 30 characters."),
                account.Street1.ValidateLength(30, 1, "The street address line 1 must be between 1 and 30 characters."),
                account.Street2.ValidateLength(30, 0, "The street address line 2 must be less than 30 characters."),
                account.Street3.ValidateLength(30, 0, "The street address line 3 must be less than 30 characters."),
                account.City.ValidateLength(30, 1, "The city must to be between 1 and 30 characters."),
                account.StateProvCode.ValidateLength(2, 2, "The address state code must be 2 characters."),
                account.PostalCode.ValidateLength(5, 5, "The postal code must be 5 characters."),
                account.Phone.ValidateLength(null, 10, "Please enter a phone number that is at least 10 digits."),
                account.Email.ValidateLength(50, 1, "Please enter an email address that is less than 50 characters.")
            };

            if (!account.CountryCode.Equals("US", StringComparison.OrdinalIgnoreCase))
            {
                results.Add(Result.FromError("ShipWorks can only create US accounts. To create an account for another country, please register your new account on the UPS website."));
            }

            if (results.Any(r => r.Failure))
            {
                return Result.FromError(string.Join(Environment.NewLine, results.Where(r => r.Failure).Select(r => r.Message)));
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Ensure that a OneBalance account has been provisioned
        /// </summary>
        private async Task<Result> EnsureOneBalanceAccountProvisioned()
        {
            if (uspsAccountRepository.Accounts.IsCountEqualTo(1))
            {
                UspsAccountEntity uspsAccount = uspsAccountRepository.Accounts.Single();

                if (uspsAccount.ShipEngineCarrierId == null)
                {
                    return await CreateOneBalanceAccount(uspsAccount).ConfigureAwait(false);
                }
                else
                {
                    return Result.FromSuccess();
                }
            }

            // future stories will handle multiple or no usps accounts
            return Result.FromError("The number of USPS accounts is not one");
        }

        /// <summary>
        /// Create a one balance account by adding Stamps.com to ShipEngine
        /// </summary>
        private async Task<Result> CreateOneBalanceAccount(UspsAccountEntity uspsAccount)
        {
            var encryptionProvider = encryptionProviderFactory.CreateSecureTextEncryptionProvider(uspsAccount.Username);

            var carrierId = await shipEngineWebClient.ConnectStampsAccount(
                    uspsAccount.Username, encryptionProvider.Decrypt(uspsAccount.Password)).ConfigureAwait(false);

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
