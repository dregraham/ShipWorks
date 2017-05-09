﻿using System;
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
        /// Get all of the UpsLocalServiceRates applicable to the shipment
        /// </summary>
        public IEnumerable<UpsLocalServiceRate> GetServiceRates(UpsShipmentEntity shipment)
        {
            string origin = shipment.Shipment.OriginPostalCode;
            string destination = shipment.Shipment.ShipPostalCode;

            int originPostalCode;
            int destinationPostalCode;
            
            if (origin?.Length < 5 || !int.TryParse(origin?.Substring(0, 5), out originPostalCode))
            {
                throw new UpsLocalRatingException($"Unable to find zone using origin postal code {origin}.");
            }

            if (destination?.Length < 5 || !int.TryParse(destination?.Substring(0, 5), out destinationPostalCode))
            {
                throw new UpsLocalRatingException($"Unable to find zone using destination postal code {destination}.");
            }

            UpsLocalRatingZoneFileEntity zoneFile = GetLatestZoneFile();
            // TODO: cache the zones so we dont have to reload every time
            FetchCollection(zoneFile.UpsLocalRatingZone);
            
            IEnumerable<string> zones = zoneFile.UpsLocalRatingZone.Where(z => z.OriginZipFloor <= originPostalCode &&
                                                        z.OriginZipCeiling >= originPostalCode &&
                                                        z.DestinationZipFloor <= destinationPostalCode &&
                                                        z.DestinationZipCeiling >= destinationPostalCode).Select(z => z.Zone).ToArray();
            
            // We don't have zone info for the given origin/destination combo
            if (zones == null || zones.None())
            {
                throw new UpsLocalRatingException($"Unable to find zone using origin postal code {origin} and destination postal code {destination}.");
            }

            UpsRateTableEntity rateTable = Get(accountRepository.GetAccount(shipment.Shipment));

            // TODO: cache rates so that we dont have to load this data every time which is super slow
            FetchCollection(rateTable.UpsPackageRate);
            FetchCollection(rateTable.UpsLetterRate);
            FetchCollection(rateTable.UpsPricePerPound);

            // If Package = Letter and Billable Weight ≤ 8 oz. use Letter rate.
            // If Package = Letter and Billable Weight > 8 oz.OR Package ≠ Letter use rate by weight.
            Dictionary<UpsServiceType, decimal> result = new Dictionary<UpsServiceType, decimal>();

            foreach (UpsPackageEntity package in shipment.Packages)
            {
                Dictionary<UpsServiceType, decimal> rates;
                
                if (package?.PackagingType == (int) UpsPackagingType.Letter && package.TotalWeight <= .5)
                {
                    rates = GetLetterRates(rateTable, zones);
                }
                else
                {
                    rates = GetPackageRates(rateTable, zones, package);
                }

                AddPackageRateToResult(result, rates);
            }

            return result.Select(rate => new UpsLocalServiceRate(rate.Key, rate.Value, true, null));
        }

        /// <summary>
        /// Add the set or package rates to the rate result
        /// </summary>
        private static void AddPackageRateToResult(Dictionary<UpsServiceType, decimal> rateResult, Dictionary<UpsServiceType, decimal> packageRates)
        {
            foreach (KeyValuePair<UpsServiceType, decimal> rate in packageRates)
            {
                if (rateResult.ContainsKey(rate.Key))
                {
                    rateResult[rate.Key] = rateResult[rate.Key] + rate.Value;
                }
                else
                {
                    rateResult.Add(rate.Key, rate.Value);
                }
            }
        }

        /// <summary>
        /// Get all of the price per pound rates for the given package/zone/servicetypes
        /// </summary>
        private Dictionary<UpsServiceType, decimal> GetPackageRates(UpsRateTableEntity rateTable, IEnumerable<string> zones, UpsPackageEntity package)
        {
            if (package.TotalWeight >= 150)
            {
                // Get the base rate at 150
                IEnumerable<UpsPackageRateEntity> baseRates = rateTable.UpsPackageRate
                    .Where(r => zones.Contains(r.Zone) && r.WeightInPounds == 150).ToList();

                // Add the price per pound for anything over 150
                Dictionary<UpsServiceType, decimal> result = new Dictionary<UpsServiceType, decimal>();
                foreach (UpsPackageRateEntity baseRate in baseRates)
                {
                    UpsPricePerPoundEntity pricePerPoundRate =
                        rateTable.UpsPricePerPound.FirstOrDefault(r => r.Service == baseRate.Service && r.Zone == baseRate.Zone);

                    decimal rate = baseRate.Rate;
                    if (pricePerPoundRate != null)
                    {
                        rate = rate + pricePerPoundRate.Rate;
                    }

                    result.Add((UpsServiceType)baseRate.Service, rate);
                }
                return result;
            }
            IEnumerable<UpsPackageRateEntity> rates = rateTable.UpsPackageRate
                .Where(r => zones.Contains(r.Zone) && r.WeightInPounds == package.BillableWeight).ToList();
            return rates.Select(GetRate).ToDictionary(r => r.Key, r => r.Value);
        }

        /// <summary>
        /// Get all the letter rates from the rate table matching the service/zone
        /// </summary>
        private static Dictionary<UpsServiceType, decimal> GetLetterRates(UpsRateTableEntity rateTable, IEnumerable<string> zones)
        {
            IEnumerable<UpsLetterRateEntity> rates = rateTable.UpsLetterRate.Where(r => zones.Contains(r.Zone));
            return rates.Select(GetRate).ToDictionary(r => r.Key, r => r.Value);
        }

        /// <summary>
        /// Get a key/val rate from the rate entity
        /// </summary>
        private static KeyValuePair<UpsServiceType, decimal> GetRate(object rate)
        {
            UpsPackageRateEntity packageRate = rate as UpsPackageRateEntity;
            if (packageRate != null)
            {
                return new KeyValuePair<UpsServiceType, decimal>((UpsServiceType)packageRate.Service, packageRate.Rate);
            }

            UpsLetterRateEntity letterRate = rate as UpsLetterRateEntity;
            if (letterRate != null)
            {
                return new KeyValuePair<UpsServiceType, decimal>((UpsServiceType)letterRate.Service, letterRate.Rate);
            }

            UpsPricePerPoundEntity perPoundRate = rate as UpsPricePerPoundEntity;
            if (perPoundRate != null)
            {
                return new KeyValuePair<UpsServiceType, decimal>((UpsServiceType)perPoundRate.Service, perPoundRate.Rate);
            }

            throw new UpsLocalRatingException("Unknown RateType");
        }
        
        /// <summary>
        /// Get the surcharges for the given account
        /// </summary>
        public IDictionary<UpsSurchargeType, double> GetSurcharges(long accountId)
        {
            Dictionary<UpsSurchargeType, double> result =
                new Dictionary<UpsSurchargeType, double>();

            UpsAccountEntity account = accountRepository.GetAccount(accountId);
            UpsRateTableEntity rateTable = Get(account);

            if (rateTable != null)
            {
                FetchCollection(rateTable.UpsRateSurcharge);

                foreach (UpsRateSurchargeEntity surcharge in rateTable.UpsRateSurcharge)
                {
                    if (result.ContainsKey((UpsSurchargeType)surcharge.SurchargeType))
                    {
                        result[(UpsSurchargeType)surcharge.SurchargeType] = surcharge.Amount;
                    }
                    else
                    {
                        result.Add((UpsSurchargeType)surcharge.SurchargeType, surcharge.Amount);
                    }
                }
            }

            return result;
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
