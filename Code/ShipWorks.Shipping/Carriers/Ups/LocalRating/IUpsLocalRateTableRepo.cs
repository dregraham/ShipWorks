using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Represents a repository for LocalRateTable
    /// </summary>
    public interface IUpsLocalRateTableRepo
    {
        /// <summary>
        /// Save the rate table and update the account to use the given rate table
        /// </summary>
        void Save(UpsRateTableEntity rateTable, UpsAccountEntity account);

        /// <summary>
        /// Remove rate tables that are not associated with a UpsAccountEntity
        /// </summary>
        void CleanupRates();

        /// <summary>
        /// Gets the UpsRateTable for the given account
        /// </summary>
        UpsRateTableEntity Get(UpsAccountEntity accountEntity);
    }
}
