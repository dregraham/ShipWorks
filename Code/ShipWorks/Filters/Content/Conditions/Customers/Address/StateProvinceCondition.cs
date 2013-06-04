using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers.Address
{
    /// <summary>
    /// Condition that compares against the state\province of an customer.
    /// </summary>
    [ConditionElement("State\\Province", "Customer.Address.StateProvince")]
    public class CustomerAddressStateProvinceCondition : BillShipStateProvinceCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.BillStateProvCode), context.GetColumnReference(CustomerFields.ShipStateProvCode), context);
        }
    }
}
