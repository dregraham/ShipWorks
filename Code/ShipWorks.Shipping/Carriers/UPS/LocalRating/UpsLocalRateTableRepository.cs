using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Repository for UpsLocalRateTable
    /// </summary>
    [Component(SingleInstance = true)]
    public class UpsLocalRateTableRepository : IUpsLocalRateTableRepository
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLocalRateTableRepository(ISqlAdapterFactory sqlAdapterFactory, ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.accountRepository = accountRepository;
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
        public UpsRateTableEntity GetRateTable(UpsAccountEntity accountEntity)
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
        /// Get Zones for origin and destination zip codes.
        /// </summary>
        public IEnumerable<string> GetZones(int originZip, int destinationZip)
        {
            UpsLocalRatingZoneFileEntity zoneFile = GetLatestZoneFile();

            RelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.PredicateExpression.Add(UpsLocalRatingZoneFields.OriginZipFloor <= originZip);
            bucket.PredicateExpression.AddWithAnd(UpsLocalRatingZoneFields.OriginZipCeiling >= originZip);
            bucket.PredicateExpression.AddWithAnd(UpsLocalRatingZoneFields.DestinationZipFloor <= destinationZip);
            bucket.PredicateExpression.AddWithAnd(UpsLocalRatingZoneFields.DestinationZipCeiling >= destinationZip);
            bucket.PredicateExpression.AddWithAnd(UpsLocalRatingZoneFields.ZoneFileID == zoneFile.ZoneFileID);

            FetchCollection(zoneFile.UpsLocalRatingZone, bucket);

            return zoneFile.UpsLocalRatingZone.Select(x => x.Zone);
        }

        /// <summary>
        /// Populate the package rates
        /// </summary>
        public IEnumerable<UpsLocalServiceRate> GetPackageRates(long accountID, IEnumerable<string> zones, int weight)
        {
            long rateTableID = GetRateTableIdForAccount(accountID);

            RelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.PredicateExpression.Add(UpsPackageRateFields.WeightInPounds == weight);
            bucket.PredicateExpression.AddWithAnd(new FieldCompareRangePredicate(UpsPackageRateFields.Zone, null, zones));
            bucket.PredicateExpression.AddWithAnd(UpsPackageRateFields.UpsRateTableID == rateTableID);

            UpsPackageRateCollection packageRates = new UpsPackageRateCollection();
            FetchCollection(packageRates, bucket);

            return packageRates.Select(r => new UpsLocalServiceRate((UpsServiceType) r.Service, r.Zone, r.Rate, weight.ToString()));
        }

        /// <summary>
        /// Populate the price per pound collection
        /// </summary>
        /// <remarks>
        /// Rate amount is price per pound * weight
        /// </remarks>
        public IEnumerable<UpsLocalServiceRate> GetPricePerPoundRates(long accountID, IEnumerable<string> zones, int billableWeight)
        {
            long rateTableID = GetRateTableIdForAccount(accountID);

            RelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.PredicateExpression.Add(new FieldCompareRangePredicate(UpsPricePerPoundFields.Zone, null, zones));
            bucket.PredicateExpression.AddWithAnd(UpsPricePerPoundFields.UpsRateTableID == rateTableID);

            UpsPricePerPoundCollection pricePerPoundRates = new UpsPricePerPoundCollection();
            FetchCollection(pricePerPoundRates, bucket);

            return pricePerPoundRates.Select(r => new UpsLocalServiceRate((UpsServiceType) r.Service, r.Zone, r.Rate * billableWeight, billableWeight.ToString()));
        }

        public IEnumerable<UpsLocalServiceRate> GetLetterRates(long accountID, IEnumerable<string> zones)
        {
            long rateTableID = GetRateTableIdForAccount(accountID);

            RelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.PredicateExpression.Add(new FieldCompareRangePredicate(UpsPackageRateFields.Zone, null, zones));
            bucket.PredicateExpression.AddWithAnd(UpsLetterRateFields.UpsRateTableID == rateTableID);

            UpsLetterRateCollection letterRates = new UpsLetterRateCollection();
            FetchCollection(letterRates, bucket);

            return letterRates.Select(r => new UpsLocalServiceRate((UpsServiceType) r.Service, r.Zone, r.Rate, "Letter"));
        }

        /// <summary>
        /// Gets the rate table identifier for account.
        /// </summary>
        private long GetRateTableIdForAccount(long accountID)
        {
            UpsAccountEntity account = accountRepository.GetAccount(accountID);

            if (account == null)
            {
                throw new UpsLocalRatingException("Cannot find account associated with accountID.");
            }

            if (account.UpsRateTableID == null)
            {
                throw new UpsLocalRatingException("Account not associated with rate table.");
            }

            return account.UpsRateTableID.Value;
        }

        /// <summary>
        /// Get the surcharges for the given account
        /// </summary>
        public IDictionary<UpsSurchargeType, double> GetSurcharges(long accountId)
        {
            long rateTableID = GetRateTableIdForAccount(accountId);

            UpsRateSurchargeCollection surcharges = new UpsRateSurchargeCollection();

            FetchCollection(surcharges,
                new RelationPredicateBucket(UpsRateSurchargeFields.UpsRateTableID == rateTableID));

            return surcharges.ToDictionary(s => (UpsSurchargeType) s.SurchargeType, s => s.Amount);
        }

        /// <summary>
        /// Fetch a collection
        /// </summary>
        private void FetchCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket = null)
        {
            try
            {
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    adapter.FetchEntityCollection(collectionToFill, filterBucket);
                }
            }
            catch (Exception ex) when (ex is ORMException || ex is SqlException)
            {
                throw new UpsLocalRatingException($"Error retrieving collection:\r\n\r\n{ex.Message}", ex);
            }
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
        /// Removes Zone Files that are not the newest zone file
        /// </summary>
        public void CleanupZones()
        {
            try
            {
                ISortExpression sort = new SortExpression(new SortClause(UpsLocalRatingZoneFileFields.UploadDate, null, SortOperator.Descending));
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    UpsLocalRatingZoneFileCollection zoneFiles = new UpsLocalRatingZoneFileCollection();
                    adapter.FetchEntityCollection(zoneFiles, null, 0, sort);
                    if (zoneFiles.Count > 1)
                    {
                        zoneFiles.RemoveAt(0);
                        adapter.DeleteEntityCollection(zoneFiles);
                    }
                }
            }
            catch (Exception ex) when (ex is ORMException || ex is SqlException)
            {
                throw new UpsLocalRatingException($"Error cleaning up old zones:\r\n\r\n{ex.Message}", ex);
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
                    // save all the zones, but don't refetch all the zones.
                    adapter.SaveEntity(zoneFile, false, true);

                    // refetch the zone table only, not all the zones
                    adapter.FetchEntity(zoneFile);
                }
            }
            catch (Exception ex) when (ex is ORMException || ex is SqlException)
            {
                throw new UpsLocalRatingException($"Error saving zones:\r\n\r\n{ex.Message}", ex);
            }
        }
    }
}
