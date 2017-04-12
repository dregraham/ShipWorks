using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Repository for UpsLocalRateTable
    /// </summary>
    [Component]
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
            // bucket is used to get all rate tables that have a RateTableID not in any UPSAccounts
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
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                ((SqlAdapter) adapter).FetchEntityCollection(rateTables, bucket);
                adapter.DeleteEntityCollection(rateTables);
            }
        }

        /// <summary>
        /// Gets the UpsRateTable for the given account
        /// </summary>  
        public UpsRateTableEntity Get(UpsAccountEntity accountEntity)
        {
            UpsRateTableEntity rateTable = null;

            if (accountEntity.UpsRateTable != null)
            {
                rateTable = accountEntity.UpsRateTable;
            }

            if (accountEntity.UpsRateTableID != null)
            {
                rateTable = new UpsRateTableEntity(accountEntity.UpsRateTableID.Value);

                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    adapter.FetchEntity(rateTable);
                    return rateTable;
                }
            }
            
            return rateTable;
        }
        
        /// <summary>
        /// Save the rate table and update the account to use the given rate table
        /// </summary>
        public void Save(UpsRateTableEntity rateTable, UpsAccountEntity account)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                // save and refetch the rate table only. Not all the rates.
                adapter.SaveEntity(rateTable, true, false);

                // save all the rates but don't
                adapter.SaveEntity(rateTable, false, true);

                // update account with new rate table
                account.UpsRateTable = rateTable;

                // save and refetch the account, but not all the rates.
                adapter.SaveEntity(account, true, false);
            }
        }
    }
}
