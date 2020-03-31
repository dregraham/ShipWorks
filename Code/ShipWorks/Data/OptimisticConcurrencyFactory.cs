using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    /// <summary>
    /// Factory that generates the predicate expression that tests for concurrency
    /// </summary>
    [DependencyInjectionInfo(typeof(FilterEntity), "ConcurrencyPredicateFactoryToUse")]
    [DependencyInjectionInfo(typeof(FilterSequenceEntity), "ConcurrencyPredicateFactoryToUse")]
    [DependencyInjectionInfo(typeof(FilterNodeEntity), "ConcurrencyPredicateFactoryToUse")]
    [DependencyInjectionInfo(typeof(TemplateEntity), "ConcurrencyPredicateFactoryToUse")]
    [DependencyInjectionInfo(typeof(TemplateFolderEntity), "ConcurrencyPredicateFactoryToUse")]
    [DependencyInjectionInfo(typeof(ActionEntity), "ConcurrencyPredicateFactoryToUse")]
    [DependencyInjectionInfo(typeof(ShipmentEntity), "ConcurrencyPredicateFactoryToUse")]
    [Serializable]
    public class OptimisticConcurrencyFactory : IConcurrencyPredicateFactory
    {
        /// <summary>
        /// Create the expression used to test for concurrency violations
        /// </summary>
        public IPredicateExpression CreatePredicate(ConcurrencyPredicateType predicateTypeToCreate, object containingEntity)
        {
            IPredicateExpression expression = new PredicateExpression();
            CommonEntityBase entity = (CommonEntityBase) containingEntity;

            if (!entity.IgnoreConcurrency)
            {
                EntityField2 rowVersion = (EntityField2) entity.Fields["RowVersion"];

                if ((object) rowVersion != null)
                {
                    // The row version has to match what it was when we retrieved it
                    expression.Add(rowVersion == rowVersion.DbValue);
                }
            }

            return expression;
        }
    }
}
