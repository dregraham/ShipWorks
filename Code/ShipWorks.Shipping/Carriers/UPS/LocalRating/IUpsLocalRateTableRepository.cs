using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Represents a repository for LocalRateTable
    /// </summary>
    public interface IUpsLocalRateTableRepository
    {
        /// <summary>
        /// Save the rate table and update the account to use the given rate table
        /// </summary>
        void Save(UpsRateTableEntity rateTable, UpsAccountEntity account);

        /// <summary>
        /// Saves the given zone file
        /// </summary>
        void Save(UpsLocalRatingZoneFileEntity zoneFile);

        /// <summary>
        /// Gets the latest zone file
        /// </summary>
        UpsLocalRatingZoneFileEntity GetLatestZoneFile();

        /// <summary>
        /// Remove rate tables that are not associated with a UpsAccountEntity
        /// </summary>
        void CleanupRates();

        /// <summary>
        /// Gets the UpsRateTable for the given account
        /// </summary>
        UpsRateTableEntity Get(UpsAccountEntity accountEntity);

        /// <summary>
        /// Removes Zone Files that are not the newest zone file
        /// </summary>
        void CleanupZones();
    }
}
