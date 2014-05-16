using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers.Address
{
    /// <summary>
    /// Condition that compares against the PostalCode of an customer.
    /// </summary>
    [ConditionElement("Postal Code", "Customer.Address.PostalCode")]
    public class CustomerAddressPostalCodeCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillPostalCode), context.GetColumnReference(CustomerFields.ShipPostalCode), context);
        }
    }
}
