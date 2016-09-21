using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.SqlServer.Filters.DirtyCounts;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Base condition for entering a child note scope
    /// </summary>
    public abstract class ForChildNoteCondition : ContainerCondition
    {
        /// <summary>
        /// We introduce a new scope target
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.Note;
        }

        /// <summary>
        /// Generate the sql for the given child scope type
        /// </summary>
        protected string GenerateSql(SqlGenerationContext context, SqlGenerationScopeType scopeType)
        {
            using (SqlGenerationScope scope = context.PushScope(EntityType.NoteEntity, GetChildPredicate(context), scopeType))
            {
                return scope.Adorn(base.GenerateSql(context));
            }
        }

        /// <summary>
        /// Get the child predicate to use to push down into the child scope
        /// </summary>
        public static string GetChildPredicate(SqlGenerationContext context)
        {
            // Get the parent scope (which will be the current Order scope)
            SqlGenerationScope parentScope = context.CurrentScope;

            // This is the sql to match the notes for the order. For the OrderNote table alias, we use a placeholder {0}, which will
            // get replaces by the scope once the alias is created.
            string orderNotes = string.Format("{0}.{1} = {2}.{3}",
                "{0}",
                context.GetColumnName(NoteFields.EntityID),
                parentScope.TableAlias,
                context.GetColumnName(OrderFields.OrderID));

            // This is the sql to match the notes for the customer. For the OrderNote table alias, we use a placeholder {0}, which will
            // get replaces by the scope once the alias is created.
            string customerNotes = string.Format("{0}.{1} = {2}.{3}",
                "{0}",
                context.GetColumnName(NoteFields.EntityID),
                parentScope.TableAlias,
                context.GetColumnName(OrderFields.CustomerID));

            // We need to manually record that we related up to customers
            context.JoinsUsed.Add(FilterNodeJoinType.OrderToCustomer);

            // This is the full predicate required to match all notes for the order
            string childPredicate = string.Format("(({0}) OR ({1}))", orderNotes, customerNotes);

            return childPredicate;
        }
    }
}
