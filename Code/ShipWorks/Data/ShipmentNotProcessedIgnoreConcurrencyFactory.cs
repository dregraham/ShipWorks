using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    /// <summary>
    /// Factory that generates the predicate expression that tests for concurrency
    /// </summary>
    [DependencyInjectionInfo(typeof(ShipmentEntity), "ConcurrencyPredicateFactoryToUse")]
    [Serializable]
    public class ShipmentNotProcessedIgnoreConcurrencyFactory : IConcurrencyPredicateFactory
    {
        /// <summary>
        /// Create the expression used to test for concurrency violations
        /// </summary>
	    public IPredicateExpression CreatePredicate(ConcurrencyPredicateType predicateTypeToCreate, object containingEntity)
	    {
		    IPredicateExpression expression = new PredicateExpression();
            CommonEntityBase entity = (CommonEntityBase) containingEntity;

            ShipmentEntity shipment = (ShipmentEntity) entity;
            if (shipment != null)
            {
                EntityField2 processed = (EntityField2) shipment.Fields[(int) ShipmentFieldIndex.Processed];

                if ((object) processed != null)
                {
                    // The processed value has to match what it was when we retrieved it
                    expression.Add(processed == processed.DbValue);
                }

                entity.IgnoreConcurrency = true;
            }

		    return expression;
	    }
    }
}
