using System;
using System.Linq;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Prime
    /// </summary>
    [ConditionElement("Generic Module Is Amazon Prime", "GenericModule.IsPrime")]
    [ConditionStoreType(StoreTypeCode.GenericModule)]
    public class GenericModuleIsPrimeCondition : EnumCondition<AmazonIsPrime>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleIsPrimeCondition()
        {
            Value = AmazonIsPrime.Yes;
        }

        /// <summary>
        /// Generate Sql that filters orders based on StoreType and IsPrime status
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string storeSql = GenerateStoreTypeSql(context);
            string isPrimeSql = GenerateIsPrimeSql(context);

            // AND the two together.
            return $"{storeSql} AND {isPrimeSql}";
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public string GenerateIsPrimeSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, GenericModuleOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(GenericModuleOrderFields.IsPrime), context));
            }
        }

        /// <summary>
        /// Generate a SQL statement for the given column
        /// </summary>
        private string GenerateSql(string valueExpression, SqlGenerationContext context, int value)
        {
            // Register the parameter
            string parm = context.RegisterParameter(value);

            return $"{valueExpression} = {parm}";
        }

        /// <summary>
        /// Generate SQL to filter out stores based on StoreType defined in the ConditionStoreType attribute
        /// </summary>
        private string GenerateStoreTypeSql(SqlGenerationContext context)
        {
            Attribute[] conditionStoreTypeAttributes = Attribute.GetCustomAttributes(GetType(), typeof(ConditionStoreTypeAttribute));
            if (conditionStoreTypeAttributes.Length != 1)
            {
                throw new InvalidOperationException("Condition must have one ConditionStoreType");
            }

            ConditionStoreTypeAttribute conditionStoreTypeAttribute = (ConditionStoreTypeAttribute) conditionStoreTypeAttributes.Single();
            var storeTypeCode = conditionStoreTypeAttribute.StoreType;
            string storeSql;
            using (SqlGenerationScope scope = context.PushScope(EntityType.StoreEntity, SqlGenerationScopeType.Parent))
            {
                storeSql = scope.Adorn(GenerateSql(context.GetColumnReference(StoreFields.TypeCode), context, (int) storeTypeCode));
            }

            return storeSql;
        }
    }
}
