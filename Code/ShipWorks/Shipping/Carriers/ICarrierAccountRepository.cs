﻿using System.Collections.Generic;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Generic implementation of ICarrierAccountRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICarrierAccountRepository<T, TInterface> : ICarrierAccountRetriever<T>
        where T : TInterface where TInterface : ICarrierAccount
    {
        /// <summary>
        /// Force a check for changes
        /// </summary>
        void CheckForChangesNeeded();

        /// <summary>
        /// Returns the default account as defined by the primary profile
        /// </summary>
        T DefaultProfileAccount { get; }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        void Save(T account);

        /// <summary>
        /// Get a readonly version of the specified account
        /// </summary>
        TInterface GetAccountReadOnly(long accountID);

        /// <summary>
        /// Get a collection of readonly accounts
        /// </summary>
        IEnumerable<TInterface> AccountsReadOnly { get; }
    }
}
