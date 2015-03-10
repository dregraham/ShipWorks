using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers.PersonName
{
    /// <summary>
    /// Condition that compares against the customer's first name
    /// </summary>
    [ConditionElement("First Name", "Customer.Name.First")] 
    public class CustomerFirstNameCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillFirstName), context.GetColumnReference(CustomerFields.ShipFirstName), context);
        }
    }
}
