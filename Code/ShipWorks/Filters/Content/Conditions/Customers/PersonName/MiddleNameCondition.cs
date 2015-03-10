using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers.PersonName
{
    /// <summary>
    /// Condition that compares against the customer's middle name
    /// </summary>
    [ConditionElement("Middle Name", "Customer.Name.Middle")] 
    public class CustomerMiddleNameCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillMiddleName), context.GetColumnReference(CustomerFields.ShipMiddleName), context);
        }
    }
}
