using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Condition for determining what templates have been used to send emails for a customer
    /// </summary>
    [ConditionElement("Email", "Customer.Emailed")]
    public class CustomerEmailedCondition : EmailedCondition
    {
        /// <summary>
        /// Create the scope used to get the the child emails
        /// </summary>
        protected override SqlGenerationScope CreateScope(SqlGenerationContext context, SqlGenerationScopeType scopeType)
        {
            return context.PushScope(EntityType.EmailOutboundEntity, ForChildEmailedCondition.GetChildPredicate(context), scopeType);
        }
    }
}
