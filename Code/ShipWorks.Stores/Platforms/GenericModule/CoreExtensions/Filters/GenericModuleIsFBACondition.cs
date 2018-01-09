using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters
{
    [ConditionElement("Generic Module Is FBA", "GenericModule.IsFBA")]
    [GenericModuleCondition]
    public class GenericModuleIsFBACondition : BooleanCondition
    {
        public GenericModuleIsFBACondition() 
            : base("Yes", "No")
        { }

        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from OrderItem -> ChannelAdvisorOrderItem
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, GenericModuleOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(GenericModuleOrderFields.IsFBA), context));
            }
        }
    }
}