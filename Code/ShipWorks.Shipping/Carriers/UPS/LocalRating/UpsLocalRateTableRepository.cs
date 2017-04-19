using System;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Repository for UpsLocalRateTable
    /// </summary>
    [Component(SingleInstance = true)]
    public class UpsLocalRateTableRepository : IUpsLocalRateTableRepository
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLocalRateTableRepository(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Remove rate tables that are not associated with a UpsAccountEntity
        /// </summary>
        public void CleanupRates()
        {
            // bucket is used to get all rate tables that have a RateTableID not associated with a UPS Account
            // SELECT [dbo].[UpsRateTable].[UpsRateTableID],
            //       [dbo].[UpsRateTable].[UploadDate]
            //        FROM[dbo].[UpsRateTable]
            // WHERE(NOT EXISTS
            //            (SELECT[dbo].[UpsAccount].[UpsRateTableID]
            //             FROM   [dbo].[UpsAccount]
            //             WHERE  [dbo].[UpsRateTable].[UpsRateTableID] = [dbo].[UpsAccount].[UpsRateTableID])) 
            RelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.PredicateExpression.Add(
                new FieldCompareSetPredicate(
                    UpsRateTableFields.UpsRateTableID,
                    null,
                    UpsAccountFields.UpsRateTableID,
                    null,
                    SetOperator.Exist,
                    (UpsRateTableFields.UpsRateTableID == UpsAccountFields.UpsRateTableID),
                    true));

            IEntityCollection2 rateTables = new EntityCollection(new UpsRateTableEntityFactory());

            try
            {
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    adapter.FetchEntityCollection(rateTables, bucket);
                    adapter.DeleteEntityCollection(rateTables);
                }
            }
            catch (Exception ex) when (ex is ORMException || ex is SqlException)
            {
                throw new UpsLocalRatingException($"Error cleaning up old rates:\r\n\r\n{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the UpsRateTable for the given account
        /// </summary>  
        /// <returns>
        /// Returns associated rate table.  If no rate table exists for the account, null is returned.
        /// </returns>
        public UpsRateTableEntity Get(UpsAccountEntity accountEntity)
        {
            UpsRateTableEntity rateTable = null;

            if (accountEntity.UpsRateTable != null)
            {
                rateTable = accountEntity.UpsRateTable;
            }
            else if (accountEntity.UpsRateTableID != null)
            {
                rateTable = new UpsRateTableEntity(accountEntity.UpsRateTableID.Value);
                try
                {
                    using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                    {
                        adapter.FetchEntity(rateTable);
                        return rateTable;
                    }
                }
                catch (Exception ex) when (ex is ORMException || ex is SqlException)
                {
                    throw new UpsLocalRatingException($"Error retrieving rate:\r\n\r\n{ex.Message}", ex);
                }
            }
            
            return rateTable;
        }
        
        /// <summary>
        /// Save the rate table and update the account to use the given rate table
        /// </summary>
        public void Save(UpsRateTableEntity rateTable, UpsAccountEntity account)
        {
            try
            {
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    // save and refetch the rate table only. Not all the rates.
                    adapter.SaveEntity(rateTable, true, false);

                    // save all the rates but don't refetch all the rates. 
                    // doing a saved and refetch was causing us to make an extra
                    // 10,000 fetches.
                    adapter.SaveEntity(rateTable, false, true);

                    // update account with new rate table
                    account.UpsRateTable = rateTable;

                    // save and refetch the account, but not all the rates.
                    adapter.SaveEntity(account, true, false);
                }
            }
            catch (Exception ex) when (ex is ORMException || ex is SqlException)
            {
                throw new UpsLocalRatingException($"Error saving rates:\r\n\r\n{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get the newest zone file
        /// </summary>
        /// <returns></returns>
        public UpsLocalRatingZoneFileEntity GetLatestZoneFile()
        {
            try
            {
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    UpsLocalRatingZoneFileCollection zoneFiles = new UpsLocalRatingZoneFileCollection();
                    adapter.FetchEntityCollection(zoneFiles, null);

                    return zoneFiles.Items.OrderByDescending(f => f.UploadDate).FirstOrDefault();
                }
            }
            catch (Exception ex) when (ex is ORMException || ex is SqlException)
            {
                throw new UpsLocalRatingException($"Error retrieving zones:\r\n\r\n{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Save the zone file
        /// </summary>
        public void Save(UpsLocalRatingZoneFileEntity zoneFile)
        {
            try
            {
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    adapter.SaveEntity(zoneFile, false, true);
                }
            }
            catch (Exception ex) when (ex is ORMException || ex is SqlException)
            {
                throw new UpsLocalRatingException($"Error saving zones:\r\n\r\n{ex.Message}", ex);
            }
        }
    }
}
