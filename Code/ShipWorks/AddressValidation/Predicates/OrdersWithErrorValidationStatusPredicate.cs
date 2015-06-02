using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.AddressValidation.Predicates
{
    /// <summary>
    /// Get orders with a specified ship address validation status
    /// </summary>
    public class OrdersWithErrorValidationStatusPredicate : IPredicateProvider, ILimitResultRows
    {
        /// <summary>
        /// Apply the logic to the predicate expression
        /// </summary>
        public void Apply(IPredicateExpression predicate)
        {
            DateTime validationThreshold = DateTime.UtcNow.AddDays(-7);

            predicate.Add(OrderFields.ShipAddressValidationStatus == (int)AddressValidationStatusType.Error &
                          OrderFields.OrderDate > validationThreshold);
            predicate.Add(new FieldCompareSetPredicate(OrderFields.OrderID, null, ShipmentFields.OrderID, null, SetOperator.In,
                ShipmentFields.Voided == true | ShipmentFields.Processed == true, true));
        }

        /// <summary>
        /// Maximum rows that this predicate should return; 0 returns all rows
        /// </summary>
        public int MaximumRows
        {
            get
            {
                return 50;
            }
        }
    }
}