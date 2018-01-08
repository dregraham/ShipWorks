using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.GenericModule.Enums;

namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters
{
    /// <summary>
    /// Filter condition for Is Prime
    /// </summary>
    [ConditionElement("Generic Module Is Amazon Prime", "GenericModule.IsPrime")]
    [ConditionStoreType(StoreTypeCode.GenericModule)]
    public class GenericModuleIsPrimeCondition : EnumCondition<GenericModuleIsAmazonPrime>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleIsPrimeCondition()
        {
            Value = GenericModuleIsAmazonPrime.Yes;
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, GenericModuleOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(GenericModuleOrderFields.IsPrime), context));
            }
        }
    }
}
