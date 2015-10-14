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
        /// <summary>
        /// Apply the logic to the predicate expression
        /// </summary>
        public void Apply(IPredicateExpression predicate)
        {
            DateTime validationThreshold = DateTime.UtcNow.AddDays(-7);

            // For order address validation status and OrderDate
            predicate.Add(OrderFields.ShipAddressValidationStatus == (int)AddressValidationStatusType.Error &
                          OrderFields.OrderDate > validationThreshold);

            // We need to get all oders from the first predicate above that have shipments that are not processed or voided
            FieldCompareSetPredicate shipmentsNotProcessedOrVoided = new FieldCompareSetPredicate(OrderFields.OrderID, null, ShipmentFields.OrderID, null, SetOperator.Exist,
                ShipmentFields.OrderID == OrderFields.OrderID & ShipmentFields.Processed == false & ShipmentFields.Voided == false, false);

            // We also need all of the matching orders that don't have any shipments
            FieldCompareSetPredicate ordersWithNoShipments = new FieldCompareSetPredicate(OrderFields.OrderID, null, ShipmentFields.OrderID, null, SetOperator.Exist,
                ShipmentFields.OrderID == OrderFields.OrderID, true);

            // Now add both of these to the container predicate so that we can get SQL like "... AND (shipmentsNotProcessedOrVoided OR ordersWithNoShipments) ..."
            IPredicateExpression containerPredicateExpression = new PredicateExpression();
            containerPredicateExpression.Add(shipmentsNotProcessedOrVoided);
            containerPredicateExpression.AddWithOr(ordersWithNoShipments);

            predicate.Add(containerPredicateExpression);
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