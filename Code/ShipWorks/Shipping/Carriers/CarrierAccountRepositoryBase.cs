using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// An abstract base class that implements the ICarrierAccountRepository interface with common method
    /// implementations that could be used by other carrier account repositories.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CarrierAccountRepositoryBase<T, TInterface> :
        ICarrierAccountRepository<T, TInterface>, ICarrierAccountRetriever
        where T : TInterface where TInterface : ICarrierAccount
    {
        /// <summary>
        /// Force a check for changes
        /// </summary>
        public abstract void CheckForChangesNeeded();

        /// <summary>
        /// Returns a list of accounts for the carrier.
        /// </summary>
        public abstract IEnumerable<T> Accounts { get; }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public abstract T GetAccount(long accountID);

        /// <summary>
        /// Returns a carrier account associated with the specified shipment
        /// </summary>
        public T GetAccount(IShipmentEntity shipment) => GetAccount(GetAccountIDFromShipment(shipment) ?? -1);

        /// <summary>
        /// Returns a carrier account associated with the specified shipment
        /// </summary>
        public TInterface GetAccountReadOnly(IShipmentEntity shipment) =>
            GetAccountReadOnly(GetAccountIDFromShipment(shipment) ?? -1);

        /// <summary>
        ///  Returns the default account as defined by the primary profile
        ///  </summary>
        public abstract T DefaultProfileAccount { get; }
        
        /// <summary>
        /// Readonly list of accounts
        /// </summary>
        public abstract IEnumerable<TInterface> AccountsReadOnly { get; }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public abstract void Save(T account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        public abstract void DeleteAccount(T account);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public abstract void Save<TAccount>(TAccount account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        public abstract void DeleteAccount<TAccount>(TAccount account);

        /// <summary>
        /// A helper method to gets the account associated withe the default profile. A null value is returned
        /// if there is not an account associated with the default profile.
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code.</param>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException">An account that no longer exists is associated with the default FedEx profile.</exception>
        protected T GetProfileAccount(ShipmentTypeCode shipmentTypeCode, long? accountID)
        {
            // Start with a null account
            T account = default(T);

            if (accountID.HasValue)
            {
                account = GetAccount(accountID.Value);
                if (object.Equals(account, default(T)))
                {
                    throw new ShippingException(string.Format("An account that no longer exists is associated with the default {0} profile.", EnumHelper.GetDescription(shipmentTypeCode)));
                }
            }

            return account;
        }

        /// <summary>
        /// Get the primary profile
        /// </summary>
        protected ShippingProfileEntity GetPrimaryProfile(ShipmentTypeCode shipmentTypeCode)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ShipmentType shipmentType = lifetimeScope.ResolveKeyed<ShipmentType>(shipmentTypeCode);
                IShippingProfileManager shippingProfileManager = lifetimeScope.Resolve<IShippingProfileManager>();

                return shippingProfileManager.GetOrCreatePrimaryProfile(shipmentType);
            }
        }

        /// <summary>
        /// Get the account id from a given shipment
        /// </summary>
        protected abstract long? GetAccountIDFromShipment(IShipmentEntity shipment);

        /// <summary>
        /// Get a readonly account with the given id
        /// </summary>
        public abstract TInterface GetAccountReadOnly(long accountID);

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        ICarrierAccount ICarrierAccountRetriever.GetAccountReadOnly(long accountID) =>
            GetAccountReadOnly(accountID);

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
    }
}
