﻿using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Account repository for Express1 Endicia accounts
    /// </summary>
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.Express1Endicia)]
    [KeyedComponent(typeof(ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>), ShipmentTypeCode.Express1Endicia)]
    public class Express1EndiciaAccountRepository : CarrierAccountRepositoryBase<EndiciaAccountEntity, IEndiciaAccountEntity>
    {
        /// <summary>
        /// Gets all the Express1 Endicia accounts in the system
        /// </summary>
        public override IEnumerable<EndiciaAccountEntity> Accounts => EndiciaAccountManager.Express1Accounts;

        /// <summary>
        /// Gets all the Express1 Endicia accounts in the system
        /// </summary>
        public override IEnumerable<IEndiciaAccountEntity> AccountsReadOnly => EndiciaAccountManager.Express1AccountsReadOnly;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() => EndiciaAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            EndiciaAccountManager.InitializeForCurrentSession();
        }

        /// <summary>
        /// Gets the Endicia account with the specified id.
        /// </summary>
        /// <param name="accountID">Id of the account to retrieve</param>
        public override EndiciaAccountEntity GetAccount(long accountID)
        {
            EndiciaAccountEntity endiciaAccountEntity = EndiciaAccountManager.GetAccount(accountID);

            // Return null if found account is not Express1
            return (endiciaAccountEntity != null && endiciaAccountEntity.EndiciaReseller == (int) EndiciaReseller.Express1) ? endiciaAccountEntity : null;
        }

        /// <summary>
        /// Gets the Endicia account with the specified id.
        /// </summary>
        /// <param name="accountID">Id of the account to retrieve</param>
        public override IEndiciaAccountEntity GetAccountReadOnly(long accountID)
        {
            IEndiciaAccountEntity endiciaAccountEntity = EndiciaAccountManager.GetAccountReadOnly(accountID);

            // Return null if found account is not Express1
            return (endiciaAccountEntity != null && endiciaAccountEntity.EndiciaReseller == (int) EndiciaReseller.Express1) ? endiciaAccountEntity : null;
        }

        /// <summary>
        /// Gets the account associated withe the default profile. A null value is returned
        /// if there is not an account associated with the default profile.
        /// </summary>
        /// <exception cref="ShippingException">An account that no longer exists is associated with the default Express1 profile.</exception>
        public override EndiciaAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.Express1Endicia).Postal.Endicia.EndiciaAccountID;
                return GetProfileAccount(ShipmentTypeCode.Express1Endicia, accountID);
            }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(EndiciaAccountEntity account) => EndiciaAccountManager.SaveAccount(account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void DeleteAccount(EndiciaAccountEntity account) =>
            EndiciaAccountManager.DeleteAccount(account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void Save<T>(T account) => Save(account as EndiciaAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void DeleteAccount<T>(T account) => Save(account as EndiciaAccountEntity);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment) =>
            shipment?.Postal?.Endicia?.EndiciaAccountID;
    }
}
