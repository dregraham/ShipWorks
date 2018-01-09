using System;
using System.Linq;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders
{
    [ConditionElement("Generic Module Is FBA", "GenericModule.IsFBA")]
    [ConditionStoreType(StoreTypeCode.GenericModule)]
    public class GenericModuleIsFBACondition : BooleanCondition
    {
        public GenericModuleIsFBACondition() 
            : base("Yes", "No")
        { }

        /// <summary>
        /// Generate Sql that filters orders based on StoreType and FBA status
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string storeSql = GenerateStoreTypeSql(context);
            string fbaSql = GenerateFbaSql(context);

            // AND the two together.
            return $"{storeSql} AND {fbaSql}";
        }

        /// <summary>
        /// Generate SQL to filter out FBA orders
        /// </summary>
        private string GenerateFbaSql(SqlGenerationContext context)
        {
            string fbaSql;
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, GenericModuleOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                fbaSql = scope.Adorn(base.GenerateSql(context.GetColumnReference(GenericModuleOrderFields.IsFBA), context));
            }

            return fbaSql;
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