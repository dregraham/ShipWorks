using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Condition that compares against the Customer ID of a customer
    /// </summary>
    [ConditionElement("Customer ID", "Customer.ID")]
    public class CustomerIDCondition : NumericCondition<long>
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.CustomerID), context);
        }
    }
}
