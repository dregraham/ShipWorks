using System;
using System.Collections.Generic;
using System.Text;
using Interapptive.Shared;
using ShipWorks.Data;
using ShipWorks.Data.Model.FactoryClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Adapter.Custom;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// Represents a table scope that is currently in progress during SqlGeneration
    /// </summary>
    public class SqlGenerationScope : IDisposable
    {
        SqlGenerationContext context;
        EntityType entityType;

        string table;
        string alias;

        SqlGenerationScopeType scopeType;

        // The scope we transitioned from to get to this scope
        SqlGenerationScope scopeFrom;

        // The relation used to get from the "scopeFrom" to this scope
        IEntityRelation scopeRelation;

        // All registered parent scopes
        List<SqlGenerationScope> parentScopes = new List<SqlGenerationScope>();

        // If this scope is a child scope, this is the predicate used to select children
        string childPredicate;

        // Used and required for transitioning into QuantityOfChild scopes
        Func<string, string> childQuantityAdorner;

        // Helpful for preventing bugs if a dev forgets to call Adorn
        bool adornCalled = false;

        /// <summary>
        /// Used to create the top-level of a SqlGenerationScope stack
        /// </summary>
        public SqlGenerationScope(SqlGenerationContext context, EntityType topLevelType)
        {
            this.context = context;

            this.scopeType = SqlGenerationScopeType.Parent;
            this.scopeFrom = null;
            this.scopeRelation = null;

            this.entityType = topLevelType;

            this.table = SqlAdapter.GetTableName(entityType);
            this.alias = context.RegisterTableAlias(entityType);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlGenerationScope(SqlGenerationContext context, SqlGenerationScopeType scopeType, SqlGenerationScope scopeFrom, IEntityRelation scopeRelation, Func<string, string> childQuantityAdorner)
        {
            if (scopeFrom == null)
            {
                throw new ArgumentNullException("scopeFrom");
            }

            ValidateScopeType(scopeType, childQuantityAdorner);

            if (scopeType != SqlGenerationScopeType.Parent)
            {
                context.IndentLevel++;
            }

            this.context = context;

            this.scopeType = scopeType;
            this.scopeFrom = scopeFrom;
            this.scopeRelation = scopeRelation;
            this.childQuantityAdorner = childQuantityAdorner;

            // OneToMany and Hierarchy relations have the derived type on the FK side.
            if (scopeType == SqlGenerationScopeType.Parent && !scopeRelation.IsHierarchyRelation)
            {
                this.entityType = EntityTypeProvider.GetEntityType(scopeRelation.GetPKEntityFieldCore(0).ContainingObjectName);
            }
            else
            {
                this.entityType = EntityTypeProvider.GetEntityType(scopeRelation.GetFKEntityFieldCore(0).ContainingObjectName);
            }

            this.table = SqlAdapter.GetTableName(entityType);
            this.alias = context.RegisterTableAlias(entityType);

            // We always need to register the FK field to be tracked.  For instance, if moving from Order -> Customer scope... they may end up
            // filtering on Customer.BillFirstName, and that would get tracked.  But we also need to track if a) this order is deleted (or a new one created) and
            // b) if the customer changes.  Tracking the FK (Order.CustomerID) handles this.  Or going the other direction Order -> OrderCharge, we would add
            // OrderCharge.OrderID for the same reason
            context.ColumnsUsed.Add((EntityField2) scopeRelation.GetFKEntityFieldCore(0));

            // For child scopes we need to build the child predicate
            if (scopeType != SqlGenerationScopeType.Parent)
            {
                // Going down to a child we need to track the PK side's PK.  So if we were going from Order -> OrderCharge, we still need to know when order's
                // are added or deleted, and tracking the PK for the Order table would do that.
                context.ColumnsUsed.Add((EntityField2) scopeRelation.GetPKEntityFieldCore(0));

                // Get the fk and pk column names
                IFieldPersistenceInfo pkInfo = scopeRelation.GetPKFieldPersistenceInfo(0);
                IFieldPersistenceInfo fkInfo = scopeRelation.GetFKFieldPersistenceInfo(0);

                // Depending on how we use the predicate (directly, or in an aggregate), the table alias may be different. So it gets a placeholder {0}
                this.childPredicate = string.Format("{0}.{1} = {2}.{3}", "{0}", fkInfo.SourceColumnName, this.scopeFrom.TableAlias, pkInfo.SourceColumnName);
            }
        }

        /// <summary>
        /// Constructor.  The child predicate should contain a {0} placeholder for where the child table (of entityType) alias will be inserted.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public SqlGenerationScope(SqlGenerationContext context, SqlGenerationScopeType scopeType, SqlGenerationScope scopeFrom, EntityType entityType, string childPredicate, Func<string, string> childQuantityAdorner)
        {
            if (scopeType == SqlGenerationScopeType.Parent)
            {
                throw new InvalidOperationException("Cannot change to parent scope using constructor that takes a child predicate.");
            }

            if (scopeFrom == null)
            {
                throw new ArgumentNullException("scopeFrom");
            }

            ValidateScopeType(scopeType, childQuantityAdorner);

            context.IndentLevel++;

            this.context = context;

            this.scopeType = scopeType;
            this.scopeFrom = scopeFrom;
            this.entityType = entityType;
            this.childQuantityAdorner = childQuantityAdorner;

            this.table = SqlAdapter.GetTableName(entityType);
            this.alias = context.RegisterTableAlias(entityType);

            this.childPredicate = childPredicate;

            // Going down to a child we need to track the PK side's PK.  So if we were going from Order -> OrderCharge, we still need to know when order's
            // are added or deleted, and tracking the PK for the Order table would do that.
            context.ColumnsUsed.Add((EntityField2) scopeFrom.PrimaryKey);
        }

        /// <summary>
        /// Validate the scope type against the given adorner
        /// </summary>
        private static void ValidateScopeType(SqlGenerationScopeType scopeType, Func<string, string> childQuantityAdorner)
        {
            if (scopeType == SqlGenerationScopeType.QuantityOfChild && childQuantityAdorner == null)
            {
                throw new ArgumentException("childQuantityAdorner cannot be null for QuantityOfChild scope", "childQuantityAdorner");
            }

            if (scopeType != SqlGenerationScopeType.QuantityOfChild && childQuantityAdorner != null)
            {
                throw new ArgumentException("childQuantityAdorner cannot be set for non QuantityOfChild scope", "childQuantityAdorner");
            }
        }

        /// <summary>
        /// This scope goes out of scope
        /// </summary>
        public void Dispose()
        {
            if (!adornCalled)
            {
                throw new InvalidOperationException("The Adorn method of the SqlGenerationScope was not called.");
            }

            context.PopScope(this);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The EntityType that the scope represents
        /// </summary>
        public EntityType EntityType
        {
            get
            {
                return entityType;
            }
        }

        /// <summary>
        /// Get the primary key field for the EntityType represented by this scope
        /// </summary>
        public EntityField2 PrimaryKey
        {
            get
            {
                return EntityUtility.GetPrimaryKeyField(entityType);
            }
        }

        /// <summary>
        /// The declartion of the table and its alias.  Such as "[Order] o";
        /// </summary>
        public string TableDeclaration
        {
            get
            {
                return string.Format("[{0}] {1}", table, alias);
            }
        }

        /// <summary>
        /// The alias of the table that this scope brings in.  Such as "o" for the Orders table.
        /// </summary>
        public string TableAlias
        {
            get
            {
                return alias;
            }
        }

        /// <summary>
        /// The relation used to get from this scope to the scope that transitioned to us
        /// </summary>
        public IEntityRelation Relation
        {
            get
            {
                return scopeRelation;
            }
        }

        /// <summary>
        /// Get the parent scope for the given entity type
        /// </summary>
        public SqlGenerationScope GetParentScopeFromRelation(IEntityRelation relation)
        {
            foreach (SqlGenerationScope scope in parentScopes)
            {
                IEntityRelation scopeRelation = scope.Relation;

                if (EntityUtility.IsSameField(scopeRelation.GetPKEntityFieldCore(0), relation.GetPKEntityFieldCore(0)) &&
                    EntityUtility.IsSameField(scopeRelation.GetFKEntityFieldCore(0), relation.GetFKEntityFieldCore(0)))
                {
                    return scope;
                }
            }

            SqlGenerationScope sqlScope = new SqlGenerationScope(
                context,
                SqlGenerationScopeType.Parent,
                this,
                relation,
                null);

            parentScopes.Add(sqlScope);

            return sqlScope;
        }

        /// <summary>
        /// Get the from clause for the current scope
        /// </summary>
        public string GetFromClause()
        {
            // Start with our own declaration
            StringBuilder fromBuilder = new StringBuilder(TableDeclaration);

            // Now we have to inner join all parents
            foreach (SqlGenerationScope parentScope in parentScopes)
            {
                IEntityRelation relation = parentScope.Relation;

                fromBuilder.AppendFormat(" INNER JOIN {0} ON {1}.{2} = {3}.{4}",
                    parentScope.GetFromClause(),
                    TableAlias, relation.GetFKFieldPersistenceInfo(0).SourceColumnName,
                    parentScope.TableAlias, relation.GetPKFieldPersistenceInfo(0).SourceColumnName);
            }

            if (parentScopes.Count > 0)
            {
                fromBuilder.Insert(0, "(");
                fromBuilder.Append(")");
            }

            return fromBuilder.ToString();
        }

        /// <summary>
        /// Adorns the specified where condition with the appropriate SQL depending on the scope type.
        /// </summary>
        [NDependIgnoreLongMethod]
        public string Adorn(string where)
        {
            adornCalled = true;

            // The parent scope adds it contents to the FROM join clauses
            if (scopeType == SqlGenerationScopeType.Parent)
            {
                return where;
            }

            if (string.IsNullOrEmpty(where))
            {
                // This one is the counter-part to the one we did in the constructor
                context.IndentLevel--;
                
                // Don't match anything if there is no filter conditions specified
                return string.Format("\n{0}0 = 1\n", context.IndentString);
            }

            StringBuilder sb = new StringBuilder();

            // Any\No child can be true
            if (scopeType == SqlGenerationScopeType.AnyChild || scopeType == SqlGenerationScopeType.NoChild)
            {
                string notPrefix = (scopeType == SqlGenerationScopeType.NoChild) ? "NOT " : "";

                sb.AppendFormat("\n{0}{1}EXISTS\n", context.IndentString, notPrefix);
                sb.AppendFormat("{0}(\n", context.IndentString);

                context.IndentLevel++;

                sb.AppendFormat("{0}SELECT {1}.{2}\n", context.IndentString, TableAlias, context.GetColumnName(PrimaryKey));
                sb.AppendFormat("{0}FROM {1}\n", context.IndentString, GetFromClause());
                sb.AppendFormat("{0}WHERE {1} AND ", context.IndentString, string.Format(childPredicate, TableAlias));

                sb.Append(where);

                context.IndentLevel--;

                // This one is the counter-part to the one we did in the constructor
                context.IndentLevel--;

                sb.AppendFormat(")\n{0}", context.IndentString);
            }

            StringBuilder childCount = new StringBuilder();

            childCount.AppendFormat("{0}(\n", context.IndentString);

            context.IndentLevel++;

            if (scopeType == SqlGenerationScopeType.EveryChild)
            {
                // The CASE is used to output a -1 inplace of zero.  This prevents returning rows where thare are no rows, because we would have found the parent has
                // zero total children, and the number of children mactching the query wuold be zero, so the parent would be returned.  Insted when that happens, zero will be
                // compared to -1, and it wont be a match.
                childCount.AppendFormat("{0}SELECT CASE COUNT({1}.{2}) WHEN 0 THEN -1 ELSE COUNT({1}.{2}) END\n", context.IndentString, TableAlias, context.GetColumnName(PrimaryKey));
            }
            else
            {
                // For QuantityOfChild - we want the actual count
                childCount.AppendFormat("{0}SELECT COUNT({1}.{2})\n", context.IndentString, TableAlias, context.GetColumnName(PrimaryKey));
            }

            childCount.AppendFormat("{0}FROM {1}\n", context.IndentString, GetFromClause());
            childCount.AppendFormat("{0}WHERE {1} AND ", context.IndentString, string.Format(childPredicate, TableAlias));

            childCount.Append(where);

            context.IndentLevel--;

            childCount.AppendFormat(")\n{0}", context.IndentString);

            // Every child must be true
            if (scopeType == SqlGenerationScopeType.EveryChild)
            {
                sb.AppendFormat("\n{0}{1} = \n", context.IndentString, context.GetChildAggregate("COUNT", PrimaryKey, childPredicate));

                sb.Append(childCount);

                // This one is the counter-part to the one we did in the constructor
                context.IndentLevel--;
            }

            // Quantity of child
            if (scopeType == SqlGenerationScopeType.QuantityOfChild)
            {
                sb.Append(childQuantityAdorner(childCount.ToString()));

                // This one is the counter-part to the one we did in the constructor
                context.IndentLevel--;
            }

            return sb.ToString();
        }
    }
}
