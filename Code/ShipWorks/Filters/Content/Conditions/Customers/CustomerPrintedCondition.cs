using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model;
using ShipWorks.SqlServer.Filters.DirtyCounts;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Condition for determining what templates have been used to send emails for a customer
    /// </summary>
    [ConditionElement("Printed", "Customer.Printed")]
    public class CustomerPrintedCondition : PrintedCondition
    {
        /// <summary>
        /// Get the child predicate to use to push down into the child scope
        /// </summary>
        protected override SqlGenerationScope CreateScope(SqlGenerationContext context, SqlGenerationScopeType scopeType)
        {
            return CreateChildScope(context, scopeType);
        }

        /// <summary>
        /// Creat a scope that goes from the current customer to the customer's email objects
        /// </summary>
        public static SqlGenerationScope CreateChildScope(SqlGenerationContext context, SqlGenerationScopeType scopeType)
        {
            // Get the parent scope (which will be the current customer scope)
            SqlGenerationScope parentScope = context.CurrentScope;

            // This is the sql to match the prints for the customer. For the Customer Print table alias, we use a placeholder {0}, which will
            // get replaces by the scope once the alias is created.
            string customerEmail = string.Format("{0}.{1} = {2}.{3}",
                "{0}",
                context.GetColumnName(PrintResultFields.RelatedObjectID),
                parentScope.TableAlias,
                context.GetColumnName(CustomerFields.CustomerID));

            string orderAlias = context.RegisterTableAlias(EntityType.OrderEntity);

            // This is the sql to match the prints for the child orders. For the Order Print table alias, we use a placeholder {0}, which will
            // get replaces by the scope once the alias is created.
            string orderEmail = string.Format("{0}.{1} IN (SELECT {2}.{3} FROM [Order] {2} WHERE {2}.{4} = {5}.{4})",
                "{0}",
                context.GetColumnName(PrintResultFields.RelatedObjectID),
                orderAlias,
                context.GetColumnName(OrderFields.OrderID),
                context.GetColumnName(OrderFields.CustomerID),
                parentScope.TableAlias);

            // We need to manually record that we related down to orders
            context.JoinsUsed.Add(FilterNodeJoinType.CustomerToOrder);

            // This is the full predicate required to match all notes for the order
            string childPredicate = string.Format("(({0}) OR ({1}))", customerEmail, orderEmail);

            return context.PushScope(EntityType.PrintResultEntity, childPredicate, scopeType);
        }
    }
}
