using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Repository for UpsLocalRateTable
    /// </summary>
    public class UpsLocalRateTableRepo : IUpsLocalRateTableRepo
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLocalRateTableRepo(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Remove rate tables that are not associated with a UpsAccountEntity
        /// </summary>
        public void CleanupRates()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the UpsRateTable for the given account
        /// </summary>
        public UpsRateTableEntity Get(UpsAccountEntity accountEntity)
        {
            if (accountEntity.UpsRateTable != null)
            {
                return accountEntity.UpsRateTable;
            }

            if (accountEntity.UpsRateTableID != null)
            {
                UpsRateTableEntity rateTable = new UpsRateTableEntity(accountEntity.UpsRateTableID.Value);

                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    adapter.FetchEntity(rateTable);
                    return rateTable;
                }
            }

            return null;
        }

        /// <summary>
        /// Save the rate table and update the account to use the given rate table
        /// </summary>
        public void Save(UpsRateTableEntity rateTable, UpsAccountEntity account)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                adapter.SaveAndRefetch(rateTable);

                account.UpsRateTable = rateTable;
                account.UpsRateTableID = rateTable.UpsRateTableID;

                adapter.SaveAndRefetch(account);
            }
        }
    }
}
