using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers.Address
{
    /// <summary>
    /// Condition that compares against the Street1 of an customer.
    /// </summary>
    [ConditionElement("Street1", "Customer.Address.Street1")]
    public class CustomerAddressStreet1Condition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillStreet1), context.GetColumnReference(CustomerFields.ShipStreet1), context);
        }
    }
}
