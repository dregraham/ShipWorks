﻿using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.ShipEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    [Component]
    [KeyedComponent(typeof(ICarrierAccountRetriever), ShipmentTypeCode.Asendia)]
    [KeyedComponent(typeof(ICarrierAccountRepository<ShipEngineAccountEntity, IShipEngineAccountEntity>), ShipmentTypeCode.Asendia)]
    public class AsendiaAccountRepository : CarrierAccountRepositoryBase<ShipEngineAccountEntity, IShipEngineAccountEntity>, IAsendiaAccountRepository
    {
        private IShipEngineAccountRepository shipEngineAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaAccountRepository(IShipEngineAccountRepository shipEngineAccountRepository)
        {
            this.shipEngineAccountRepository = shipEngineAccountRepository;
        }

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<ShipEngineAccountEntity> Accounts =>
            shipEngineAccountRepository.Accounts.Where(a => a.ShipmentTypeCode == (int) ShipmentTypeCode.Asendia);

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public override ShipEngineAccountEntity DefaultProfileAccount => null;

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public override IEnumerable<IShipEngineAccountEntity> AccountsReadOnly =>
            shipEngineAccountRepository.AccountsReadOnly.Where(a => a.ShipmentTypeCode == (int)ShipmentTypeCode.Asendia);

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() =>
            shipEngineAccountRepository.CheckForChangesNeeded();

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override ShipEngineAccountEntity GetAccount(long accountID) =>
            shipEngineAccountRepository.GetAccount(accountID);

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        public override IShipEngineAccountEntity GetAccountReadOnly(long accountID) =>
            shipEngineAccountRepository.GetAccountReadOnly(accountID);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        public override void Save(ShipEngineAccountEntity account) => shipEngineAccountRepository.Save(account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void DeleteAccount(ShipEngineAccountEntity account) => shipEngineAccountRepository.DeleteAccount(account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void Save<T>(T account) => Save(account as ShipEngineAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public override void DeleteAccount<T>(T account) => DeleteAccount(account as ShipEngineAccountEntity);

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected override long? GetAccountIDFromShipment(IShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }
    }
}
