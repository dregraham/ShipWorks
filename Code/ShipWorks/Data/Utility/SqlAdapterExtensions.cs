using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Extension methods for the SqlAdapter class
    /// </summary>
    public static class SqlAdapterExtensions
    {
        /// <summary>
        /// Get a collection of entities using the given predicate configuration
        /// </summary>
        public static EntityCollection<T> GetCollectionFromPredicate<T>(this ISqlAdapter sqlAdapter, IPredicateProvider predicateProvider) where T : EntityBase2
        {
            int limitToRows = (predicateProvider as ILimitResultRows)?.MaximumRows ?? 0;

            return GetCollectionFromPredicate<T>(sqlAdapter, predicateProvider.Apply, limitToRows);
        }

        /// <summary>
        /// Get a collection of entities using the given predicate configuration
        /// </summary>
        public static EntityCollection<T> GetCollectionFromPredicate<T>(this ISqlAdapter sqlAdapter, Action<IPredicateExpression> predicateConfigurator) where T : EntityBase2
        {
            return GetCollectionFromPredicate<T>(sqlAdapter, predicateConfigurator, 0);
        }

        /// <summary>
        /// Get a collection of entities using the given predicate configuration
        /// </summary>
        public static EntityCollection<T> GetCollectionFromPredicate<T>(this ISqlAdapter sqlAdapter, Action<IPredicateExpression> predicateConfigurator, int maxRowsToReturn = 0) where T : EntityBase2
        {
            if (sqlAdapter == null)
            {
                throw new ArgumentNullException("sqlAdapter");
            }

            RelationPredicateBucket bucket = new RelationPredicateBucket();

            predicateConfigurator?.Invoke(bucket.PredicateExpression);

            EntityCollection<T> entities = new EntityCollection<T>();
            sqlAdapter.FetchEntityCollection(entities, bucket, maxRowsToReturn);

            return entities;
        }
    }
}
