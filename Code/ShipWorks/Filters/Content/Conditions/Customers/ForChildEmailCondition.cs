using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.SqlServer.Filters.DirtyCounts;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Base condition for entering a child email scope
    /// </summary>
    public class ForChildEmailedCondition : ContainerCondition
    {
        /// <summary>
        /// We introduce a new scope target
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.Email;
        }

        /// <summary>
        /// Generate the sql for the given child scope type
        /// </summary>
        protected string GenerateSql(SqlGenerationContext context, SqlGenerationScopeType scopeType)
        {
            using (SqlGenerationScope scope = context.PushScope(EntityType.EmailOutboundEntity, GetChildPredicate(context), scopeType))
            {
                return scope.Adorn(base.GenerateSql(context));
            }
        }

        /// <summary>
        /// Get the child predicate to use to push down into the child scope
        /// </summary>
        public static string GetChildPredicate(SqlGenerationContext context)
        {
            // Get the parent scope (which will be the current customer scope)
            SqlGenerationScope parentScope = context.CurrentScope;

            // We need an alias for the relation table
            string emailRelationAlias = context.RegisterTableAlias(EntityType.EmailOutboundRelationEntity);

            // This is the sql to match the emails for the cusotmer. For the customer email table alias, we use a placeholder {0}, which will
            // get replaced by the scope once the alias is created.
            string customerEmails = string.Format("{0}.{1} IN (SELECT {2}.{3} FROM [{4}] {2} WHERE {2}.{8} = {5} AND {2}.{9} = {6}.{7})",
                "{0}",
                context.GetColumnName(EmailOutboundFields.EmailOutboundID),
                emailRelationAlias,
                context.GetColumnName(EmailOutboundRelationFields.EmailOutboundID),
                SqlAdapter.GetTableName(EntityType.EmailOutboundRelationEntity),
                (int) EmailOutboundRelationType.RelatedObject,
                parentScope.TableAlias,
                context.GetColumnName(parentScope.PrimaryKey),
                context.GetColumnName(EmailOutboundRelationFields.RelationType),
                context.GetColumnName(EmailOutboundRelationFields.EntityID)
                );

            string orderAlias = context.RegisterTableAlias(EntityType.OrderEntity);

            // Sql to match the emails of every child order of the customer
            string orderEmails = string.Format("{0}.{1} IN (SELECT {2}.{3} FROM [{4}] {2} WHERE {2}.{12} = {5} AND {2}.{13} IN " +
                                                         " (SELECT {6}.{7} FROM [{8}] {6} WHERE {6}.{9} = {10}.{11}))",
                "{0}",
                context.GetColumnName(EmailOutboundFields.EmailOutboundID),
                emailRelationAlias,
                context.GetColumnName(EmailOutboundRelationFields.EmailOutboundID),
                SqlAdapter.GetTableName(EntityType.EmailOutboundRelationEntity),
                (int) EmailOutboundRelationType.RelatedObject,
                orderAlias,
                context.GetColumnName(OrderFields.OrderID),
                SqlAdapter.GetTableName(EntityType.OrderEntity),
                context.GetColumnName(OrderFields.CustomerID),
                parentScope.TableAlias,
                context.GetColumnName(CustomerFields.CustomerID),
                context.GetColumnName(EmailOutboundRelationFields.RelationType),
                context.GetColumnName(EmailOutboundRelationFields.EntityID)
               );

            // We need to manually record that we related down to orders
            context.JoinsUsed.Add(FilterNodeJoinType.CustomerToOrder);

            // This is the full predicate required to match all emails for the customer
            string childPredicate = string.Format("(({0}) OR ({1}))", customerEmails, orderEmails);

            return childPredicate;
        }
    }
}
