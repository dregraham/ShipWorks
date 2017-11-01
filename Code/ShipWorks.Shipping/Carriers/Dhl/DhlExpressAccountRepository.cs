using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// DhlExpress Account Repository
    /// </summary>
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.DhlExpress)]
    [KeyedComponent(typeof(ICarrierAccountRepository<ShipEngineAccountEntity, IShipEngineAccountEntity>), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressAccountRepository : IDhlExpressAccountRepository, ICarrierAccountRetriever
    {
        private readonly IShipEngineAccountRepository shipEngineAccountRepository;

        /// <summary>
        /// Create a DhlExpressAccountRepository
        /// </summary>
        public DhlExpressAccountRepository(IShipEngineAccountRepository shipEngineAccountRepository)
        {
            this.shipEngineAccountRepository = shipEngineAccountRepository;
        }

        /// <summary>
        /// Get Account
        /// </summary>
        public ShipEngineAccountEntity GetAccount(IShipmentEntity shipment) =>
            shipEngineAccountRepository.GetAccount(shipment.DhlExpress.ShipEngineAccountID);

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<ShipEngineAccountEntity> Accounts =>
            shipEngineAccountRepository.Accounts.Where(a=>a.ShipmentTypeCode == (int) ShipmentTypeCode.DhlExpress);

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public ShipEngineAccountEntity DefaultProfileAccount => Accounts.FirstOrDefault();

        /// <summary>
        /// Returns Readonly account
        /// </summary>
        public IShipEngineAccountEntity GetAccountReadOnly(IShipmentEntity shipment) => 
            GetAccount(shipment).AsReadOnly();

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<IShipEngineAccountEntity> AccountsReadOnly =>
            shipEngineAccountRepository.AccountsReadOnly.Where(a => a.ShipmentTypeCode == (int) ShipmentTypeCode.DhlExpress);

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public void CheckForChangesNeeded() =>
            shipEngineAccountRepository.CheckForChangesNeeded();

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public ShipEngineAccountEntity GetAccount(long accountID) =>
            shipEngineAccountRepository.GetAccount(accountID);

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public IShipEngineAccountEntity GetAccountReadOnly(long accountID) =>
            shipEngineAccountRepository.GetAccountReadOnly(accountID);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        public void Save(ShipEngineAccountEntity account) => shipEngineAccountRepository.Save(account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DeleteAccount(ShipEngineAccountEntity account) => shipEngineAccountRepository.DeleteAccount(account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public void Save<T>(T account) => Save(account as ShipEngineAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public void DeleteAccount<T>(T account) => DeleteAccount(account as ShipEngineAccountEntity);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected long? GetAccountIDFromShipment(IShipmentEntity shipment)
        {
            return shipment.DhlExpress.ShipEngineAccountID;
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        ICarrierAccount ICarrierAccountRetriever.GetAccountReadOnly(IShipmentEntity shipment) =>
            GetAccountReadOnly(shipment);

        /// <summary>
        /// Returns a list of accounts for the carrier.
        /// </summary>
        IEnumerable<ICarrierAccount> ICarrierAccountRetriever.AccountsReadOnly =>
            AccountsReadOnly.OfType<ICarrierAccount>();

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        IEnumerable<ICarrierAccount> ICarrierAccountRetriever.Accounts => Accounts.OfType<ICarrierAccount>();

        /// <summary>
        /// Gets a readonly acount
        /// </summary>
        ICarrierAccount ICarrierAccountRetriever.GetAccountReadOnly(long accountID)
        {
            return GetAccountReadOnly(accountID);
        }

    }
}
