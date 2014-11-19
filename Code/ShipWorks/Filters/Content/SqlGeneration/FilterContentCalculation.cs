using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions.Special;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using System.Data;
using ShipWorks.Data.Connection;
using System.Linq;
using ShipWorks.SqlServer.Filters.DirtyCounts;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// Responsible for generating the calculation sql for calculating filter counts for a given node
    /// </summary>
    public class FilterContentCalculation : IFilterContentSqlGenerator
    {
        FilterNodeEntity node;

        // For filters, this is the effective definition which takes into consideration
        // all parent restrictions.  Will be null for folders
        FilterDefinition filterDefinition;
        SqlGenerationContext filterSqlContext;
        string filterSqlPredicate;

        /// <summary>
        /// Creates a new instance of the calculation generator for the given node
        /// </summary>
        public FilterContentCalculation(FilterNodeEntity node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            this.node = node;

            // For filters, go ahead and generate the sql predicate that evaluates the condition
            if (!node.Filter.IsFolder)
            {
                filterDefinition = GetEffectiveFilterDefinition(node);
                filterSqlContext = new SqlGenerationContext(filterDefinition.FilterTarget);

                if (filterDefinition.IsEmpty() || node.Filter.State == (byte)FilterState.Disabled)
                {
                    filterSqlPredicate = "WHERE 1 = 2";
                }
                else
                {
                    // Generate the sql represented by the element
                    filterSqlPredicate = filterDefinition.RootContainer.GenerateSql(filterSqlContext);

                    // Since we already tested the definition for empty, the predicate should not turn out empty
                    Debug.Assert(!string.IsNullOrEmpty(filterSqlPredicate));
                    if (!string.IsNullOrEmpty(filterSqlPredicate))
                    {
                        filterSqlPredicate = "WHERE\n" + filterSqlPredicate;
                    }
                }

                Debug.Assert(filterSqlContext.IndentLevel == 1, "IndentLevel should be back to 1");
            }
        }

        /// <summary>
        /// Generate the SQL.  Either for the initial calculation, or update calculation.
        /// </summary>
        public FilterSqlResult GenerateSql(long countID)
        {
            if (node.Filter.IsFolder)
            {
                var masks = GenerateFolderMasks();

                return new FilterSqlResult(countID,
                    GenerateFolderSql(true),
                    GenerateFolderSql(false),
                    new List<SqlParameter>(),
                    masks.Item1,
                    masks.Item2);
            }
            else
            {
                return new FilterSqlResult(
                    countID, 
                    GetFilterSql(true), 
                    GetFilterSql(false), 
                    filterSqlContext.Parameters, 
                    filterSqlContext.ColumnsUsed,
                    filterSqlContext.JoinsUsed);
            }
        }

        /// <summary>
        /// Get the SQL to execute for a filter node.
        /// </summary>
        private string GetFilterSql(bool initial)
        {
            SqlGenerationScope scope = filterSqlContext.TopLevelScope;

            return string.Format(initial ? FilterSqlTemplates.FilterInitial : FilterSqlTemplates.FilterUpdate,
                scope.TableAlias,
                filterSqlContext.GetColumnName(scope.PrimaryKey),
                scope.GetFromClause(),
                filterSqlPredicate,
                (int) FilterCountStatus.Ready);
        }

        /// <summary>
        /// Generate the SQL for a folder
        /// </summary>
        private string GenerateFolderSql(bool initial)
        {
            List<long> childContentKeys = GetChildContentKeys(node);

            if (childContentKeys.Count == 0)
            {
                return string.Format(FilterSqlTemplates.FolderEmpty,
                    (int) FilterCountStatus.Ready);
            }

            StringBuilder childInList = new StringBuilder();
            foreach (long childCountID in childContentKeys)
            {
                if (childInList.Length > 0)
                {
                    childInList.Append(", ");
                }

                childInList.AppendFormat("{0}", childCountID);
            }

            return string.Format(initial ? FilterSqlTemplates.FolderInitial : FilterSqlTemplates.FolderUpdate,
                childInList,
                (int) FilterCountStatus.Ready);
        }

        /// <summary>
        /// Get the ID of every content ID that is associated with a child node
        /// </summary>
        private List<long> GetChildContentKeys(FilterNodeEntity parent)
        {
            if (!node.Filter.IsFolder)
            {
                throw new InvalidOperationException("Cannot get child count IDs for a filter.");
            }

            List<long> contentKeys = new List<long>();

            foreach (FilterNodeEntity child in parent.ChildNodes)
            {
                if (child.Filter.IsFolder)
                {
                    contentKeys.AddRange(GetChildContentKeys(child));
                }
                else
                {
                    contentKeys.Add(child.FilterNodeContentID);
                }
            }

            return contentKeys;
        }

        /// <summary>
        /// Get the effective defintion for the specified node that represents a filter
        /// </summary>
        private FilterDefinition GetEffectiveFilterDefinition(FilterNodeEntity node)
        {
            if (node.Filter.IsFolder)
            {
                throw new InvalidOperationException("Cannot generate filter definition for a folder.");
            }

            List<FilterDefinition> definitions = new List<FilterDefinition>();

            // Has to pass the definition of this filter
            FilterDefinition nodeDefinition = (node.Filter.Definition != null) ? new FilterDefinition(node.Filter.Definition) : new FilterDefinition((FilterTarget) node.Filter.FilterTarget);
            
            // If the node definition itself is empty, then we want the entire definition to be empty.  If we didn't do this, then the node definition would become equivalent
            // to the folder definition, which is not what we want.  An empty node definition should yeild zero results.
            if (nodeDefinition.IsEmpty())
            {
                return nodeDefinition;
            }

            // Add in the definition for the node, which will be added on to by the parent folders
            definitions.Add(nodeDefinition);

            // Also has to pass any parent folder definitions
            FilterNodeEntity parentFolder = node.ParentNode;
            while (parentFolder != null)
            {
                if (parentFolder.Filter.Definition != null)
                {
                    definitions.Add(new FilterDefinition(parentFolder.Filter.Definition));
                }

                parentFolder = parentFolder.ParentNode;
            }

            FilterDefinition definitionToUse;

            // No need to merge if there is only one definition
            if (definitions.Count == 1)
            {
                definitionToUse = definitions[0];
            }
            else
            {
                // Now we create a new definition that will be the merged result of all the other definitions
                FilterDefinition mergedDefinition = new FilterDefinition((FilterTarget) node.Filter.FilterTarget);
                ConditionGroup group = mergedDefinition.RootContainer.FirstGroup;

                // They all have to pass
                group.JoinType = ConditionJoinType.All;

                // Add them all in
                foreach (FilterDefinition definition in definitions)
                {
                    CombinedResultCondition condition = new CombinedResultCondition();
                    condition.Container = definition.RootContainer;
                    group.Conditions.Add(condition);
                }

                definitionToUse = mergedDefinition;
            }

            return definitionToUse;
        }

        /// <summary>
        /// Create the column mask and join mask for the folder, which is the mask of all fields affected by the child filters
        /// </summary>
        private Tuple<byte[], int> GenerateFolderMasks()
        {
            ResultsetFields resultFields = new ResultsetFields(2);
            resultFields.DefineField(FilterNodeContentFields.ColumnMask, 0, "ColumnMask", "");
            resultFields.DefineField(FilterNodeContentFields.JoinMask, 1, "JoinMask", "");

            var childContentKeys = GetChildContentKeys(node);
            if (childContentKeys.Count == 0)
            {
                return Tuple.Create(new byte[0], 0);
            }

            RelationPredicateBucket bucket = new RelationPredicateBucket(
                FilterNodeContentFields.FilterNodeContentID == childContentKeys);

            List<byte[]> childMasks = new List<byte[]>();
            int joinMasks = 0;

            // Do the fetch
            ExistingConnectionScope.ExecuteWithAdapter(adapter =>
            {
                using (SqlDataReader reader = (SqlDataReader) adapter.FetchDataReader(resultFields, bucket, CommandBehavior.Default, 0, true))
                {
                    while (reader.Read())
                    {
                        childMasks.Add((byte[]) reader.GetValue(0));
                        joinMasks |= reader.GetInt32(1);
                    }
                }
            });

            return Tuple.Create(FilterNodeColumnMaskUtility.CreateUnionedBitmask(childMasks), joinMasks);
        }
    }
}
