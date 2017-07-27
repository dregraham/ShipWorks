using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.SqlServer.Filters.DirtyCounts;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// Class for generating the SQL that will be used to execute a filter definition
    /// </summary>
    public class SqlGenerationContext
    {
        FilterTarget target;
        SqlGenerationScope topLevelScope;

        List<SqlParameter> sqlParameters = new List<SqlParameter>();
        List<EntityField2> columnsUsed = new List<EntityField2>();
        List<FilterNodeJoinType> joinsUsed = new List<FilterNodeJoinType>();

        // The next parameter to create
        int nextParameter = 1;

        // For formatting
        int indentLevel;
        string indentString;

        // All registered table alias'
        List<string> registeredAlias = new List<string>();

        Stack<SqlGenerationScope> scopeStack = new Stack<SqlGenerationScope>();

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlGenerationContext(FilterTarget target)
        {
            this.target = target;

            IndentLevel = 1;

            // Initial scope
            topLevelScope = new SqlGenerationScope(this, FilterHelper.GetEntityType(target));
            scopeStack.Push(topLevelScope);
        }

        /// <summary>
        /// The target object the filter is being generated for
        /// </summary>
        public FilterTarget FilterTarget
        {
            get
            {
                return target;
            }
        }

        /// <summary>
        /// The parameters required by the context
        /// </summary>
        public ICollection<SqlParameter> Parameters
        {
            get
            {
                return sqlParameters;
            }
        }

        /// <summary>
        /// All of the columns used (and thus affected by changes of) for the filter
        /// </summary>
        public List<EntityField2> ColumnsUsed
        {
            get { return columnsUsed; }
        }

        /// <summary>
        /// Tracks all the relevant joins that get used.  Not all of them - just the ones that matter for tracking which filters will depend on what edited types.
        /// </summary>
        public List<FilterNodeJoinType> JoinsUsed
        {
            get { return joinsUsed; }
        }

        /// <summary>
        /// Returns the top level generation scope of the context.  Will correspond to the FilterTarget.
        /// </summary>
        public SqlGenerationScope TopLevelScope
        {
            get { return topLevelScope; }
        }

        /// <summary>
        /// Returns the scope that is current for the context.
        /// </summary>
        public SqlGenerationScope CurrentScope
        {
            get { return scopeStack.Peek(); }
        }

        /// <summary>
        /// For formatting
        /// </summary>
        public int IndentLevel
        {
            get
            {
                return indentLevel;
            }
            set
            {
                indentLevel = value;
                indentString = new string(' ', 3 + indentLevel * 3);
            }
        }

        /// <summary>
        /// For formatting
        /// </summary>
        public string IndentString
        {
            get { return indentString; }
        }

        /// <summary>
        /// Get the name of the physical column
        /// </summary>
        public string GetColumnName(EntityField2 field)
        {
            // Add this field as used by the filter
            columnsUsed.Add(field);

            return DataAccessAdapter.GetPersistenceInfo(field).SourceColumnName;
        }

        /// <summary>
        /// Get SQL representing the fully qualified reference to the given field.  This automatically registers the field as referenced by the filter, which is used to determine
        /// what filters to update when data changes.
        /// </summary>
        public string GetColumnReference(EntityField2 field)
        {
            if (CurrentScope.EntityType != EntityTypeProvider.GetEntityType(field.ContainingObjectName))
            {
                throw new InvalidOperationException(string.Format("Cannot reference field {0} when object {1} is in scope.", field.Name, SqlAdapter.GetTableName(scopeStack.Peek().EntityType)));
            }

            return string.Format("{0}.{1}", CurrentScope.TableAlias, GetColumnName(field));
        }

        /// <summary>
        /// Register a new alias for the given entity type
        /// </summary>
        public string RegisterTableAlias(EntityType entityType)
        {
            string tableName = SqlAdapter.GetTableName(entityType);

            return RegisterTableAlias(tableName);
        }

        /// <summary>
        /// Register a new alias for the given table name
        /// </summary>
        private string RegisterTableAlias(string tableName)
        {
            string aliasBase = "";
            foreach (char c in tableName)
            {
                if (char.IsUpper(c))
                {
                    aliasBase += c;
                }
            }

            aliasBase = aliasBase.ToLower();
            string alias = aliasBase;

            int next = 2;
            while (registeredAlias.Contains(alias))
            {
                alias = string.Format("{0}{1}", aliasBase, next++);
            }

            registeredAlias.Add(alias);

            return alias;
        }

        /// <summary>
        /// Register the value as a parameter in the sql
        /// </summary>
        public string RegisterParameter(object value)
        {
            string param = string.Format("@param{0}", nextParameter++);

            string text = value as string;
            if (text != null)
            {
                int length = Math.Max(columnsUsed[columnsUsed.Count - 1].MaxLength, 1);

                SqlParameter parameter;

                // We can't send a 0 length param, so if length is 0, we don't send the size param.
                // Also, if length is greater than int32's max value less 1, we don't send the size param.
                // If we sent 0  or max length - 1, SQL Server would yell at us.
                if (length == 0 || length >= Int32.MaxValue - 1)
                {
                    parameter = new SqlParameter(param, SqlDbType.NVarChar);
                }
                else
                {
                    parameter = new SqlParameter(param, SqlDbType.NVarChar, Math.Max(length, text.Length));
                }

                parameter.Value = value;

                sqlParameters.Add(parameter);
            }
            else
            {
                // If its an enum, we need to use its integer value
                if (value is Enum)
                {
                    value = (int) value;
                }

                sqlParameters.Add(new SqlParameter(param, value));
            }

            return param;
        }

        /// <summary>
        /// Transition to the scope of the EntityType of the target scope field, by joining on the current scope field and the target scope field.
        /// </summary>
        public SqlGenerationScope PushScope(EntityField2 currentScopeField, EntityField2 targetScopeField, SqlGenerationScopeType scopeType, Func<string, string> childQuantityAdorner = null)
        {
            EntityType fromType = EntityTypeProvider.GetEntityType(currentScopeField.ActualContainingObjectName);
            if (CurrentScope.EntityType != fromType)
            {
                throw new InvalidOperationException(string.Format("The field {0} is not valid at this point since it's entity is not in scope.", currentScopeField.Name));
            }

            EntityField2 primaryField;
            EntityField2 foreignField;

            // Depending on if we are going to parent or child determine which field must be the PK.  (note it doesn't have to be the PK - just the 1 side of the 1:n)
            if (scopeType == SqlGenerationScopeType.Parent)
            {
                primaryField = targetScopeField;
                foreignField = currentScopeField;
            }
            else
            {
                primaryField = currentScopeField;
                foreignField = targetScopeField;
            }

            EntityRelation relation = new EntityRelation(primaryField, foreignField, RelationType.OneToMany);
            relation.SetPKFieldPersistenceInfo(0, DataAccessAdapter.GetPersistenceInfo(primaryField));
            relation.SetFKFieldPersistenceInfo(0, DataAccessAdapter.GetPersistenceInfo(foreignField));

            return PushScope(relation, scopeType, childQuantityAdorner);
        }

        /// <summary>
        /// Transition to the scope of the given EntityType using the EntityRelation builtin and known by LLBLGen between the EntityType of the
        /// current scope and that of the given scope.
        /// </summary>
        public SqlGenerationScope PushScope(EntityType entityType, SqlGenerationScopeType scopeType, Func<string, string> childQuantityAdorner = null)
        {
            IEntityRelation relation;

            if (scopeType == SqlGenerationScopeType.Parent)
            {
                // Find a relation between the current scope table and what will be the new parent scope
                relation = FindRelation(CurrentScope.EntityType, entityType);
            }
            else
            {
                // Find the relation between the target child and the current scope, which is the parent
                relation = FindRelation(entityType, CurrentScope.EntityType);
            }

            if (relation == null)
            {
                throw new InvalidOperationException(string.Format("Cannot transition to scope; no such relation exists. {0} -> {1}", CurrentScope.EntityType, entityType));
            }

            return PushScope(relation, scopeType, childQuantityAdorner);
        }

        /// <summary>
        /// Transition to the scope of the given EntityType using the predicate specified.  The child predicate should contain
        /// a {0} placeholder for where the child table (of entityType) alias will be inserted.
        /// </summary>
        public SqlGenerationScope PushScope(EntityType entityType, string childPredicate, SqlGenerationScopeType scopeType, Func<string, string> childQuantityAdorner = null)
        {
            SqlGenerationScope newScope = new SqlGenerationScope(this, scopeType, CurrentScope, entityType, childPredicate, childQuantityAdorner);

            RegisterJoin(CurrentScope.EntityType, newScope.EntityType);

            // Push it and return it
            scopeStack.Push(newScope);
            return newScope;
        }

        /// <summary>
        /// Transition to a parent scope (going from n:1) or to a child scope (going from 1:n) using the given relation and scope type.
        /// </summary>
        private SqlGenerationScope PushScope(IEntityRelation relation, SqlGenerationScopeType scopeType, Func<string, string> childQuantityAdorner)
        {
            SqlGenerationScope newScope;
            SqlGenerationScope currentScope = CurrentScope;

            // Parent scope
            if (scopeType == SqlGenerationScopeType.Parent)
            {
                SqlGenerationScope parentScope = currentScope.GetParentScopeFromRelation(relation);

                newScope = parentScope;
            }
            else
            {
                newScope = new SqlGenerationScope(this, scopeType, currentScope, relation, childQuantityAdorner);
            }

            RegisterJoin(currentScope.EntityType, newScope.EntityType);

            // Push it and return it
            scopeStack.Push(newScope);
            return newScope;
        }

        /// <summary>
        /// Revert the entity scope to the previous
        /// </summary>
        public void PopScope(SqlGenerationScope sqlGeneratorScope)
        {
            if (CurrentScope != sqlGeneratorScope)
            {
                throw new InvalidOperationException("SqlGenerationScope popped out of order.");
            }

            scopeStack.Pop();
        }

        /// <summary>
        /// Registers a join from one entity to the other
        /// </summary>
        private void RegisterJoin(EntityType from, EntityType to)
        {
            if (from == EntityType.CustomerEntity)
            {
                if (to == EntityType.OrderEntity)
                {
                    joinsUsed.Add(FilterNodeJoinType.CustomerToOrder);
                }
            }

            if (from == EntityType.OrderEntity)
            {
                if (to == EntityType.CustomerEntity)
                {
                    joinsUsed.Add(FilterNodeJoinType.OrderToCustomer);
                }

                if (to == EntityType.OrderItemEntity)
                {
                    joinsUsed.Add(FilterNodeJoinType.OrderToItem);
                }

                if (to == EntityType.ShipmentEntity)
                {
                    joinsUsed.Add(FilterNodeJoinType.OrderToShipment);
                }
            }

            if (from == EntityType.OrderItemEntity)
            {
                if (to == EntityType.OrderEntity)
                {
                    joinsUsed.Add(FilterNodeJoinType.ItemToOrder);
                }
            }

            if (from == EntityType.ShipmentEntity)
            {
                if (to == EntityType.OrderEntity)
                {
                    joinsUsed.Add(FilterNodeJoinType.ShipmentToOrder);
                }
            }
        }

        /// <summary>
        /// Get the SQL that executes the specified aggregate operation on the given child field as related to the parent entity in scope
        /// </summary>
        public string GetChildAggregate(string aggregateFunction, EntityField2 fieldToAggregate)
        {
            SqlGenerationScope parentScope = scopeStack.Peek();

            // Get the relation representing this child\parent
            IEntityRelation relation = FindRelation(EntityTypeProvider.GetEntityType(fieldToAggregate.ContainingObjectName), parentScope.EntityType);
            if (relation == null)
            {
                throw new InvalidOperationException("Cannot create child aggregate; no child relation defined.");
            }

            // Register the field to be tracked - so if we are doing like a SUM and a value changes, we pick it up
            ColumnsUsed.Add(fieldToAggregate);

            // We also have to register the FK used to get from the child to the parent - in case the parent changes, we need to pick that up.
            ColumnsUsed.Add((EntityField2) relation.GetFKEntityFieldCore(0));

            // We also have to register the parent PK.  This is to track direct additions\deletions of parents without any changes to children
            ColumnsUsed.Add((EntityField2) relation.GetPKEntityFieldCore(0));

            // Have to make sure we register that we are joining down to this child table
            RegisterJoin(parentScope.EntityType, EntityTypeProvider.GetEntityType(fieldToAggregate.ContainingObjectName));

            string childPredicate = string.Format("{0}.{1} = {2}.{3}",
                "{0}",
                relation.GetFKFieldPersistenceInfo(0).SourceColumnName,
                parentScope.TableAlias,
                relation.GetPKFieldPersistenceInfo(0).SourceColumnName);

            return GetChildAggregate(aggregateFunction, fieldToAggregate, childPredicate);
        }

        /// <summary>
        /// Get the SQL that executes the specified aggregate operation on the given child field as related to the given parent scope using the specified relation.
        /// </summary>
        public string GetChildAggregate(string aggregateFunction, EntityField2 fieldToAggregate, string childPredicate)
        {
            IFieldPersistenceInfo info = DataAccessAdapter.GetPersistenceInfo(fieldToAggregate);

            // Need a new alias for the table
            string tableAlias = RegisterTableAlias(info.SourceObjectName);

            // Replace the placeholder with the alias
            childPredicate = string.Format(childPredicate, tableAlias);

            return string.Format("(SELECT {0}({1}.{2}) FROM [{3}] {4} WHERE {5})",
                aggregateFunction,
                tableAlias,
                info.SourceColumnName,
                info.SourceObjectName,
                tableAlias,
                childPredicate
                );
        }

        /// <summary>
        /// Find the relation that links the current entity type to the specified
        /// </summary>
        private IEntityRelation FindRelation(EntityType childType, EntityType parentType)
        {
            RelationCollection relations = EntityUtility.FindRelationChain(childType, parentType);
            if (relations.Count == 1)
            {
                return (IEntityRelation) relations[0];
            }

            return null;
        }
    }
}
