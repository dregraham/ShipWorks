using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Generic implementation of ICarrierAccountRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICarrierAccountRepository<T> : ICarrierAccountRetriever<T> where T : ICarrierAccount
    {
        /// <summary>
        /// Returns the default account as defined by the primary profile
        /// </summary>
        T DefaultProfileAccount { get; }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        void Save(T account);
    }
}
