using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    [ConditionElement("The number of prints matching the following", "Customer.QuantityOfPrint")]
    public class ForQuantityOfPrintCondition : ChildQuantityCondition
    {
        /// <summary>
        /// Coming from a note entity
        /// </summary>
        protected override EntityType? EntityType
        {
            get
            {
                return ShipWorks.Data.Model.EntityType.PrintResultEntity;
            }
        }

        /// <summary>
        /// Using a custom predicate
        /// </summary>
        protected override string GetTargetScopeChildPredicate(SqlGenerationContext context)
        {
            return CustomerPrintedCondition.GetChildPredicate(context);
        }

        /// <summary>
        /// Target scope of this container
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.Printed;
        }
    }
}
