using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers.Address
{
    /// <summary>
    /// Condition that compares against the Street3 of an customer.
    /// </summary>
    [ConditionElement("Street3", "Customer.Address.Street3")]
    public class CustomerAddressStreet3Condition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillStreet3), context.GetColumnReference(CustomerFields.ShipStreet3), context);
        }
    }
}
