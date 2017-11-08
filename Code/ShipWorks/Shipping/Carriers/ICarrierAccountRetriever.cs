using System.Collections.Generic;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Default carrier account retriever interface
    /// </summary>
    public interface ICarrierAccountRetriever
    {
        /// <summary>
        /// Get a read only version of the specified account
        /// </summary>
        ICarrierAccount GetAccountReadOnly(long accountID);

        /// <summary>
        /// Get a read only version of the account on the given shipment
        /// </summary>
        ICarrierAccount GetAccountReadOnly(IShipmentEntity shipment);

        /// <summary>
        /// Get a collection of read only accounts
        /// </summary>
        IEnumerable<ICarrierAccount> AccountsReadOnly { get; }
    }

    /// <summary>
    /// Generic carrier account retriever
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICarrierAccountRetriever<T, TInterface> : IReadOnlyCarrierAccountRetriever<TInterface>
        where T : TInterface
        where TInterface : ICarrierAccount
    {
        /// <summary>
        /// Get the specified account
        /// </summary>
        T GetAccount(long accountID);

        /// <summary>
        /// Get the account on the given shipment
        /// </summary>
        T GetAccount(IShipmentEntity shipment);

        /// <summary>
        /// Get a collection of accounts
        /// </summary>
        IEnumerable<T> Accounts { get; }
    }

    /// <summary>
    /// Generic carrier account retriever
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyCarrierAccountRetriever<TInterface>
        where TInterface : ICarrierAccount
    {
        /// <summary>
        /// Get a read only version of the specified account
        /// </summary>
        TInterface GetAccountReadOnly(long accountID);

        /// <summary>
        /// Get a read only version of the account on the given shipment
        /// </summary>
        TInterface GetAccountReadOnly(IShipmentEntity shipment);

        /// <summary>
        /// Get a collection of read only accounts
        /// </summary>
        IEnumerable<TInterface> AccountsReadOnly { get; }
    }
}