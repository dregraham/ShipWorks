using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.AddressValidation.Predicates
{
    /// <summary>
    /// Get unprocessed shipments for a given order
    /// </summary>
    public class UnprocessedShipmentsForOrderPredicate : IPredicateProvider
    {
        private readonly long orderId;

        /// <summary>
        /// Constructor
        /// </summary>
        public UnprocessedShipmentsForOrderPredicate(long orderId)
        {
            this.orderId = orderId;
        }

        /// <summary>
        /// Apply the logic to the predicate expression
        /// </summary>
        public void Apply(IPredicateExpression predicate)
        {
            predicate.Add(ShipmentFields.OrderID == orderId)
                .AddWithAnd(ShipmentFields.Processed == false);
        }
    }
}