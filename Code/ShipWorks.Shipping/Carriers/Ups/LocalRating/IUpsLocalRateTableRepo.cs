using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
