using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Model.Custom
{
    /// <summary>
    /// Our customer inheritance provider that knows how to exlude order and item tables that are not active
    /// </summary>
    internal class ActiveTableInheritanceInfoProvider : InheritanceInfoProviderCore, IInheritanceInfoProvider
    {
        class NullExpression : DbFunctionCall
        {
            public NullExpression()
                : base("NULL", null)
            {

            }

            public override string ToQueryText(bool inHavingClause)
            {
                // Hav to call the base even though we don't use it b\c it sets up the parameters collection
                base.ToQueryText(inHavingClause);

                return "NULL";
            }
        }

        /// <summary>
        /// Gets the hierarchy fields for the entity as LLGLGen does, but then for any fields that are in non-active tables it just uses a NULL value rather
        /// than referencing the field.
        /// </summary>
        IEntityFieldCore[] IInheritanceInfoProvider.GetHierarchyFields(string entityName)
        {
            IEntityFieldCore[] fields = base.GetHierarchyFields(entityName);

            string[] activeDerived = ActiveTableInheritanceManager.GetActiveTables(entityName);
            if (activeDerived != null)
            {
                foreach (IEntityFieldCore field in fields.Where(f => f.ContainingObjectName != entityName && !activeDerived.Contains(f.ContainingObjectName)))
                {
                    field.ExpressionToApply = new NullExpression();
                }
            }

            return fields;
        }

        /// <summary>
        /// Gets the hierarchy relations for the given entity like LLGLGen does, but then strips out all the LEFT JOINs to non-active tables.
        /// </summary>
        RelationCollection IInheritanceInfoProvider.GetHierarchyRelations(string entityName, string objectAlias)
        {
            IInheritanceInfoProvider explicitInterface = (ActiveTableInheritanceInfoProvider) this;

            return explicitInterface.GetHierarchyRelations(entityName, objectAlias, true);
        }

        /// <summary>
        /// Gets the hierarchy relations for the given entity like LLGLGen does, but then strips out all the LEFT JOINs to non-active tables.
        /// </summary>
        RelationCollection IInheritanceInfoProvider.GetHierarchyRelations(string entityName, string objectAlias, bool includePathsToReachableLeafs)
        {
            RelationCollection relations = base.GetHierarchyRelations(entityName, objectAlias, includePathsToReachableLeafs);

            string[] activeDerived = ActiveTableInheritanceManager.GetActiveTables(entityName);
            if (activeDerived != null)
            {
                foreach (EntityRelation relation in relations.Cast<EntityRelation>().ToList())
                {
                    if (relation.IsHierarchyRelation && !activeDerived.Contains(relation.GetFKEntityFieldCore(0).ContainingObjectName))
                    {
                        relations.Remove(relation);
                    }
                }
            }

            return relations;
        }
    }
}
