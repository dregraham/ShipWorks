using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Interface that represents a Ups local rate table
    /// </summary>
    public interface IUpsLocalRateTable
    {
        /// <summary>
        /// Load the rate table froma stream
        /// </summary>
        void Load(Stream stream);

        /// <summary>
        /// Save the rate table
        /// </summary>
        void Save(UpsAccountEntity entity);

        /// <summary>
        /// Add a rates collection to the rate table
        /// </summary>
        void AddRates(IEnumerable<UpsLocalRateEntity> rates);

        /// <summary>
        /// Add a surcharge collection to the rate table
        /// </summary>
        void AddSurcharges(IEnumerable<UpsLocalRateSurchargeEntity> surcharges);
    }
}
