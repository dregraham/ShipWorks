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
        /// Add a surcharge collection to the surcharge table
        /// </summary>
        void AddSurcharges(IEnumerable<UpsRateSurchargeEntity> surcharges);

        /// <summary>
        /// Adds the rates to the rate tables
        /// </summary>
        void AddRates(IEnumerable<UpsPackageRateEntity> packageRates,
            IEnumerable<UpsLetterRateEntity> letterRates,
            IEnumerable<UpsPricePerPoundEntity> pricesPerPound);
    }
}
