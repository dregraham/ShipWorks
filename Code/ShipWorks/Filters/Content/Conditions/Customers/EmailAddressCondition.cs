using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Condition that compares against the email address of a customer
    /// </summary>
    [ConditionElement("Email Address", "Customer.EmailAddress")]
    public class CustomerEmailAddressCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillEmail), context.GetColumnReference(CustomerFields.ShipEmail), context);
        }
    }
}
