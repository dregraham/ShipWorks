using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers.Address
{
    /// <summary>
    /// Condition that compares against the Street2 of an customer.
    /// </summary>
    [ConditionElement("Street2", "Customer.Address.Street2")]
    public class CustomerAddressStreet2Condition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillStreet2), context.GetColumnReference(CustomerFields.ShipStreet2), context);
        }
    }
}
