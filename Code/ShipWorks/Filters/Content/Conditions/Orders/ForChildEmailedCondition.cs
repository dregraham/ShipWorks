using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
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
            // Get the parent scope (which will be the current order scope)
            SqlGenerationScope parentScope = context.CurrentScope;

            // We need an alias for the relation table
            string emailRelationAlias = context.RegisterTableAlias(EntityType.EmailOutboundRelationEntity);

            // This is the sql to match the emails for the order. For the order email table alias, we use a placeholder {0}, which will
            // get replaced by the scope once the alias is created.
            string orderEmails = string.Format("{0}.{1} IN (SELECT {2}.{3} FROM [{4}] {2} WHERE {2}.{5} = {6} AND {2}.{7} = {8}.{9})",
                "{0}",
                context.GetColumnName(EmailOutboundFields.EmailOutboundID),
                emailRelationAlias,
                context.GetColumnName(EmailOutboundRelationFields.EmailOutboundID),
                SqlAdapter.GetTableName(EntityType.EmailOutboundRelationEntity),
                context.GetColumnName(EmailOutboundRelationFields.RelationType),
                (int) EmailOutboundRelationType.RelatedObject,
                context.GetColumnName(EmailOutboundRelationFields.EntityID),
                parentScope.TableAlias,
                context.GetColumnName(parentScope.PrimaryKey)
                );

            // We also need to make sure to only return publicly visible email. For example, this filters out emails sent by Yahoo! for status updates
            // that are internal only.
            orderEmails += string.Format(" AND {0}.{1} = {2}",
                "{0}",
                context.GetColumnName(EmailOutboundFields.Visibility),
                (int) EmailOutboundVisibility.Visible);

            return orderEmails;
        }
    }
}
