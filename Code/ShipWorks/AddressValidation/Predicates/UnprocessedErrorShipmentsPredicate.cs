using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;

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
                .Add(new FieldCompareSetPredicate(ShipmentFields.OrderID, null, OrderFields.OrderID, null, SetOperator.In, OrderFields.OrderDate > validationThreshold));
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
