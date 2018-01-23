using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Prime
    /// </summary>
    [ConditionElement("Generic Module Amazon Order ID", "GenericModule.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.GenericModule)]
    public class GenericModuleAmazonOrderIDCondition : StringCondition
    {
        /// <summary>
        /// Generate Sql that filters orders based on StoreType and AmazonOrderID
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string storeSql = GenerateStoreTypeSql(context);
            string amazonOrderIDSql = GenerateAmazonOrderIDSql(context);

            // AND the two together.
            return $"{storeSql} AND {amazonOrderIDSql}";
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public string GenerateAmazonOrderIDSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, GenericModuleOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(GenericModuleOrderFields.AmazonOrderID), context));
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
