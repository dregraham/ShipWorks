using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Basic repository for retrieving postal (w/o postage) accounts
    /// </summary>
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.Amazon)]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.BestRate)]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.None)]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.Other)]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.PostalWebTools)]
    [KeyedComponent(typeof(ICarrierAccountRepository<NullCarrierAccount, ICarrierAccount>), ShipmentTypeCode.Amazon)]
    [KeyedComponent(typeof(ICarrierAccountRepository<NullCarrierAccount, ICarrierAccount>), ShipmentTypeCode.BestRate)]
    [KeyedComponent(typeof(ICarrierAccountRepository<NullCarrierAccount, ICarrierAccount>), ShipmentTypeCode.None)]
    [KeyedComponent(typeof(ICarrierAccountRepository<NullCarrierAccount, ICarrierAccount>), ShipmentTypeCode.Other)]
    [KeyedComponent(typeof(ICarrierAccountRepository<NullCarrierAccount, ICarrierAccount>), ShipmentTypeCode.PostalWebTools)]
    public class NullAccountRepository :
        ICarrierAccountRepository<NullCarrierAccount, ICarrierAccount>, ICarrierAccountRetriever
    {
        /// <summary>
        /// Returns a list of postal (w/o postage) accounts.
        /// </summary>
        public IEnumerable<NullCarrierAccount> Accounts => new List<NullCarrierAccount> { new NullCarrierAccount() };

        /// <summary>
        /// Returns a list of postal (w/o postage) accounts.
        /// </summary>
        public IEnumerable<ICarrierAccount> AccountsReadOnly => new List<NullCarrierAccount> { new NullCarrierAccount() };


        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        /// v
        public void Save<T>(T account) => Save(account as NullCarrierAccount);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public void DeleteAccount<T>(T account) => DeleteAccount(account as NullCarrierAccount);

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public void CheckForChangesNeeded()
        {
            // Since we have no accounts, this doesn't need to do anything
        }

        /// <summary>
        /// Returns a postal (w/o postage) account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>Null entity.</returns>
        public NullCarrierAccount GetAccount(long accountID) => new NullCarrierAccount();

        /// <summary>
        /// Returns a carrier account associated with the specified shipment
        /// </summary>
        public NullCarrierAccount GetAccount(IShipmentEntity shipment) => new NullCarrierAccount();

        /// <summary>
        /// Returns a carrier account associated with the specified shipment
        /// </summary>
        public ICarrierAccount GetAccountReadOnly(IShipmentEntity shipment) => new NullCarrierAccount();

        /// <summary>
        /// Returns a postal (w/o postage) account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>Null entity.</returns>
        public ICarrierAccount GetAccountReadOnly(long accountID) => new NullCarrierAccount();

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public NullCarrierAccount DefaultProfileAccount => new NullCarrierAccount();

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void Save(NullCarrierAccount account)
        {
            // Nothing to save
        }

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void DeleteAccount(NullCarrierAccount account)
        {
            //Nothing to delete for null account.
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        ICarrierAccount ICarrierAccountRetriever.GetAccountReadOnly(long accountID) =>
            GetAccountReadOnly(accountID);

        /// <summary>
        /// Returns a list of accounts for the carrier.
        /// </summary>
        IEnumerable<ICarrierAccount> ICarrierAccountRetriever.AccountsReadOnly =>
            AccountsReadOnly.OfType<ICarrierAccount>();
    }
}
