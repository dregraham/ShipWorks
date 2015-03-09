using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers.PersonName
{
    /// <summary>
    /// Condition that compares against the customer's last name
    /// </summary>
    [ConditionElement("Last Name", "Customer.Name.Last")] 
    public class CustomerLastNameCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillLastName), context.GetColumnReference(CustomerFields.ShipLastName), context);
        }
    }
}
