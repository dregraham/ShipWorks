using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [ConditionElement("The number of emails matching the following", "Order.QuantityOfEmail")]
    public class ForQuantityOfEmailCondition : ChildQuantityCondition
    {
        /// <summary>
        /// Coming from a note entity
        /// </summary>
        protected override EntityType? EntityType
        {
            get
            {
                return ShipWorks.Data.Model.EntityType.EmailOutboundEntity;
            }
        }

        /// <summary>
        /// Using a custom predicate
        /// </summary>
        protected override string GetTargetScopeChildPredicate(SqlGenerationContext context)
        {
            return ForChildEmailedCondition.GetChildPredicate(context);
        }

        /// <summary>
        /// Target scope of this container
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.Email;
        }
    }
}
