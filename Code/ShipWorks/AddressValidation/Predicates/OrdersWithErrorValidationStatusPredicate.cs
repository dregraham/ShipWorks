using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.AddressValidation.Predicates
{
    /// <summary>
    /// Get orders with a specified ship address validation status
    /// </summary>
    public class OrdersWithErrorValidationStatusPredicate : IPredicateProvider, ILimitResultRows
    {
        private const int DefaultConcurrency = 50;
        private const int MaximumConcurrency = 1000;
        private const string ValidationConcurrencyRegistryKey = "errorOrders";

        /// <summary>
        /// Apply the logic to the predicate expression
        /// </summary>
        public void Apply(IPredicateExpression predicate)
        {
            DateTime validationThreshold = DateTime.UtcNow.AddDays(-7);

            // For order address validation status and OrderDate
            predicate.Add(OrderFields.ShipAddressValidationStatus == (int) AddressValidationStatusType.Error &
                          OrderFields.OrderDate > validationThreshold);

            // We only want orders that don't have any processod or voided shipments, even if there ARE still unprocessed shipments
            // This is to reduce unnecessary validation retries to Stamps servers
            predicate.Add(new FieldCompareSetPredicate(OrderFields.OrderID, null, ShipmentFields.OrderID, null, SetOperator.In,
                ShipmentFields.Voided == true | ShipmentFields.Processed == true, true));
        }

        /// <summary>
        /// Maximum rows that this predicate should return; 0 returns all rows
        /// </summary>
        public int MaximumRows => AddressValidationQueue.GetConcurrencyCount(ValidationConcurrencyRegistryKey,
            DefaultConcurrency, MaximumConcurrency);
    }
}