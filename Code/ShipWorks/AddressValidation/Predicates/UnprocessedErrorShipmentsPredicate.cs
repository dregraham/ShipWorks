using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.AddressValidation.Predicates
{
    /// <summary>
    /// Get unprocessed shipments with orders within 7 days and a status of error
    /// </summary>
    public class UnprocessedErrorShipmentsPredicate : IPredicateProvider, ILimitResultRows
    {
        /// <summary>
        /// Apply the logic to the predicate expression
        /// </summary>
        public void Apply(IPredicateExpression predicate)
        {
            DateTime validationThreshold = DateTime.UtcNow.AddDays(-7);

            predicate.Add(ShipmentFields.ShipAddressValidationStatus == (int) AddressValidationStatusType.Error)
                .Add(ShipmentFields.Processed == false)
                .Add(ShipmentFields.Voided == false)
                .Add(new FieldCompareSetPredicate(ShipmentFields.OrderID, null, OrderFields.OrderID, null, SetOperator.In, OrderFields.OrderDate > validationThreshold));
        }

        /// <summary>
        /// Maximum rows that this predicate should return; 0 returns all rows
        /// </summary>
        public int MaximumRows => AddressValidationQueue.GetBatchSize();
    }
}
