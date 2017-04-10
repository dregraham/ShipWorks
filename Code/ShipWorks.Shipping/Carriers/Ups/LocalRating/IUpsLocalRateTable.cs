using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;
using System.IO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Interface that represents a Ups local rate table
    /// </summary>
    public interface IUpsLocalRateTable
    {
        /// <summary>
        /// Load the rate table from a stream
        /// </summary>
        void Load(Stream stream);

        /// <summary>
        /// Save the rate table
        /// </summary>
        void Save(UpsAccountEntity accountEntity);

        /// <summary>
        /// Add a package rates collection to UpsLocalRateTable
        /// </summary>
        void AddPackageRates(IEnumerable<UpsPackageRateEntity> rates);

        /// <summary>
        /// Add a letter rates collection to UpsLocalRateTable
        /// </summary>
        void AddLetterRates(IEnumerable<UpsLetterRateEntity> rates);

        /// <summary>
        /// Add price per pound collection to UpsLocalRateTable
        /// </summary>
        void AddPricesPerPound(IEnumerable<UpsPricePerPoundEntity> rates);

        /// <summary>
        /// Add a surcharge collection to the rate table
        /// </summary>
        void AddSurcharges(IEnumerable<UpsRateSurchargeEntity> surcharges);
    }
}
