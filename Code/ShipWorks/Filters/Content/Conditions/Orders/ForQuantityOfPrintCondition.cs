using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [ConditionElement("The number of prints matching the following", "Order.QuantityOfPrint")]
    public class ForQuantityOfPrintCondition : ChildQuantityCondition
    {
        /// <summary>
        /// The field used in the current scope for relations (PK)
        /// </summary>
        protected override SD.LLBLGen.Pro.ORMSupportClasses.EntityField2 CurrentScopeField
        {
            get
            {
                return OrderFields.OrderID;
            }
        }

        /// <summary>
        /// FK field used to get to our target scope
        /// </summary>
        protected override EntityField2 TargetScopeField
        {
            get
            {
                return PrintResultFields.RelatedObjectID;
            }
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
