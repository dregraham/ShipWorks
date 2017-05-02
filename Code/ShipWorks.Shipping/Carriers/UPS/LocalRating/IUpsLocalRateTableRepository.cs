using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

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
        /// Get all of the UpsLocalServiceRates applicable to the shipment/servicetypes
        /// </summary>
        IEnumerable<UpsLocalServiceRate> GetServiceRates(UpsShipmentEntity shipment, IEnumerable<UpsServiceType> serviceTypes);

        /// <summary>
        /// Get the surcharges for the given account
        /// </summary>
        IDictionary<UpsSurchargeType, double> GetSurcharges(long accountId);

        /// <summary>
        /// Removes Zone Files that are not the newest zone file
        /// </summary>
        void CleanupZones();
    }
}
