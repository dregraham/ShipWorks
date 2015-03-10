using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.AddressValidation.Predicates
{
    /// <summary>
    /// Get orders with a specified ship address validation status
    /// </summary>
    public class UnprocessedShipmentsWithShipValidationStatusPredicate : IPredicateProvider, ILimitResultRows
    {
        private readonly AddressValidationStatusType statusType;

        /// <summary>
        /// Constructor
        /// </summary>
        public UnprocessedShipmentsWithShipValidationStatusPredicate(AddressValidationStatusType statusType)
        {
            this.statusType = statusType;
        }

        /// <summary>
        /// Apply the logic to the predicate expression
        /// </summary>
        public void Apply(IPredicateExpression predicate)
        {
            predicate.Add(ShipmentFields.ShipAddressValidationStatus == (int)statusType)
                .AddWithAnd(ShipmentFields.Processed == false);
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