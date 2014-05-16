using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Condition that compares against the company name of a customer
    /// </summary>
    [ConditionElement("Company Name", "Customer.CompanyName")]
    public class CustomerCompanyNameCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillCompany), context.GetColumnReference(CustomerFields.ShipCompany), context);
        }
    }
}
