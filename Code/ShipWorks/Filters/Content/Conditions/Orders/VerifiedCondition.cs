using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Order verified condition
    /// </summary>
    [ConditionElement("Verified", "Order.Verified")]
    public class VerifiedCondition : BooleanCondition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VerifiedCondition()
            : base("Yes", "No")
        {
            Value = false;
        }

        /// <summary>
        /// Generate Sql 
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.Verified), context);
        }
    }
}
