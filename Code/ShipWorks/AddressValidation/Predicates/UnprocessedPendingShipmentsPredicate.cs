using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.AddressValidation.Predicates
{
    /// <summary>
    /// Get pending shipments
    /// </summary>
    public class UnprocessedPendingShipmentsPredicate : IPredicateProvider, ILimitResultRows
    {
        /// <summary>
        /// Apply the logic to the predicate expression
        /// </summary>
        public void Apply(IPredicateExpression predicate)
        {
            predicate.Add(ShipmentFields.ShipAddressValidationStatus == (int) AddressValidationStatusType.Pending)
                .AddWithAnd(ShipmentFields.Processed == false);
        }

        /// <summary>
        /// Maximum rows that this predicate should return; 0 returns all rows
        /// </summary>
        public int MaximumRows => AddressValidationQueue.GetBatchSize();
    }
}