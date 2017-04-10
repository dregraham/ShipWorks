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
        /// Add a rates collection to the rate table
        /// </summary>
        void AddRates(IEnumerable<UpsPackageRateEntity> rates);

        /// <summary>
        /// Add a surcharge collection to the rate table
        /// </summary>
        void AddSurcharges(IEnumerable<UpsRateSurchargeEntity> surcharges);
    }
}
