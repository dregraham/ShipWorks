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
        private const int DefaultConcurrency = 50;
        private const int MaximumConcurrency = 1000;
        private const string ValidationConcurrencyRegistryKey = "pendingShipments";

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
        public int MaximumRows => AddressValidationQueue.GetConcurrencyCount(ValidationConcurrencyRegistryKey,
            DefaultConcurrency, MaximumConcurrency);
    }
}