using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the Order# of an Order
    /// </summary>
    [ConditionElement("Order Number", "Order.Number")]
    public class OrderNumberCondition : NumericStringCondition<long>
    {
        /// <summary>
        /// Generate the SQL that evaluates the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderNumberSql = string.Empty;
            string orderSearchSql = string.Empty;
            string storeCombinedOrderSearchSql = string.Empty;

            if (IsNumeric)
            {
                orderNumberSql = GenerateSql(context.GetColumnReference(OrderFields.OrderNumber), context);
            }
            else
            {
                orderNumberSql = StringCondition.GenerateSql(StringValue, StringOperator, context.GetColumnReference(OrderFields.OrderNumberComplete), context);
            }

            orderSearchSql = GenerateCombinedOrderSearchSql(context);

            storeCombinedOrderSearchSql = GenerateStoreCombinedOrderSearchSql(context);

            return $"{orderNumberSql} OR {orderSearchSql} OR {storeCombinedOrderSearchSql}";
        }

        /// <summary>
        /// Get the SQL for searching order specific combined order numbers
        /// </summary>
        private string GenerateCombinedOrderSearchSql(SqlGenerationContext context)
        {
            EntityField2 searchField = IsNumeric ? OrderSearchFields.OrderNumber : OrderSearchFields.OrderNumberComplete;

            CombinedOrderNumberCondition combinedOrderNumberCondition = new CombinedOrderNumberCondition(searchField);
            combinedOrderNumberCondition.IsNumeric = IsNumeric;

            if (IsNumeric)
            {
                combinedOrderNumberCondition.Operator = Operator;
                combinedOrderNumberCondition.Value1 = Value1;
                combinedOrderNumberCondition.Value2 = Value2;
            }
            else
            {
                combinedOrderNumberCondition.StringOperator = StringOperator;
                combinedOrderNumberCondition.StringValue = StringValue;
            }

            return combinedOrderNumberCondition.GenerateSql(context);
        }

        /// <summary>
        /// Get the SQL for searching store specific combined order numbers
        /// </summary>
        private string GenerateStoreCombinedOrderSearchSql(SqlGenerationContext context)
        {
            List<string> storeConditionSqls = new List<string>();

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IStoreManager storeManager = lifetimeScope.Resolve<IStoreManager>();
                List<StoreTypeCode> existingStoreTypeCodes = storeManager.GetUniqueStoreTypes().Select(s => s.TypeCode).ToList();

                foreach (ICombinedOrderCondition combinedOrderCondition in lifetimeScope.Resolve<IEnumerable<ICombinedOrderCondition>>())
                {
                    ConditionStoreTypeAttribute attribute = (ConditionStoreTypeAttribute) Attribute.GetCustomAttribute(combinedOrderCondition.GetType(), typeof(ConditionStoreTypeAttribute));

                    if (existingStoreTypeCodes.Any(st => st == attribute.StoreType))
                    {
                        combinedOrderCondition.IsNumeric = IsNumeric;

                        if (IsNumeric)
                        {
                            combinedOrderCondition.Operator = Operator;
                            combinedOrderCondition.Value1 = Value1;
                            combinedOrderCondition.Value2 = Value2;
                        }
                        else
                        {
                            combinedOrderCondition.StringOperator = StringOperator;
                            combinedOrderCondition.StringValue = StringValue;
                        }

                        storeConditionSqls.Add(combinedOrderCondition.GenerateSql(context));
                    }
                }
            }

            return string.Join(" OR ", storeConditionSqls);
        }
    }
}
