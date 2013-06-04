using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters
{
    [ConditionElement("ChannelAdvisor Flag Type", "ChannelAdvisorOrder.FlagType")]
    [ConditionStoreType(StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorFlagTypeCondition : EnumCondition<ChannelAdvisorFlagType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelAdvisorFlagTypeCondition"/> class.
        /// </summary>
        public ChannelAdvisorFlagTypeCondition() 
            : base()
        { }

        /// <summary>
        /// Generate the SQL for the condition element
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override string GenerateSql(ShipWorks.Filters.Content.SqlGeneration.SqlGenerationContext context)
        {
            // We have to get from Order -> ChannelAdivsorOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ChannelAdvisorOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(ChannelAdvisorOrderFields.FlagType), context));
            }
        }
    }
}
