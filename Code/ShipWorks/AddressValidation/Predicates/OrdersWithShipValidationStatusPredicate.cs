using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.AddressValidation.Predicates
{
    /// <summary>
    /// Get orders with a specified ship address validation status
    /// </summary>
    public class OrdersWithShipValidationStatusPredicate : IPredicateProvider, ILimitResultRows
    {
        private readonly AddressValidationStatusType statusType;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrdersWithShipValidationStatusPredicate(AddressValidationStatusType statusType)
        {
            this.statusType = statusType;
        }

        /// <summary>
        /// Apply the logic to the predicate expression
        /// </summary>
        public void Apply(IPredicateExpression predicate)
        {
            predicate.Add(OrderFields.ShipAddressValidationStatus == (int) statusType);
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