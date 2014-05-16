using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Condition that compares against the company name of an order
    /// </summary>
    [ConditionElement("Company Name", "Order.CompanyName")]
    public class OrderCompanyNameCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillCompany), context.GetColumnReference(OrderFields.ShipCompany), context);
        }
    }
}
