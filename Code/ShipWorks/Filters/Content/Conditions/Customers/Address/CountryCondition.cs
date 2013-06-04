using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers.Address
{
    /// <summary>
    /// Condition that compares against the country of an customer.
    /// </summary>
    [ConditionElement("Country", "Customer.Address.Country")]
    public class CustomerAddressCountryCondition : BillShipCountryCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillCountryCode), context.GetColumnReference(CustomerFields.ShipCountryCode), context);
        }
    }
}
