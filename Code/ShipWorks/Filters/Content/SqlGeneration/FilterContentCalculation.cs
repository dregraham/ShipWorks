using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Special;
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

                if (filterDefinition.IsEmpty() || node.Filter.State == (byte) FilterState.Disabled)
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
                    GenerateFolderSql(FilterQueryType.Initial, Enumerable.Empty<string>()),
                    GenerateFolderSql(FilterQueryType.Update, Enumerable.Empty<string>()),
                    GenerateFolderSql(FilterQueryType.Exists, masks.ExistsSqls),
                    masks.Item1,
                    masks.Item2);
            }
            else
            {
                return new FilterSqlResult(
                    countID,
                    GetFilterSql(FilterQueryType.Initial),
                    GetFilterSql(FilterQueryType.Update),
                    GetFilterSql(FilterQueryType.Exists),
                    filterSqlContext.ColumnsUsed,
                    filterSqlContext.JoinsUsed);
            }
        }

        /// <summary>
        /// Get the SQL to execute for a filter node.
        /// </summary>
        private string GetFilterSql(FilterQueryType filterQueryType)
        {
            SqlGenerationScope scope = filterSqlContext.TopLevelScope;

            string fromClause = scope.GetFromClause();
            string queryFormat;
            if (filterQueryType == FilterQueryType.Initial)
            {
                queryFormat = FilterSqlTemplates.FilterInitial;
            }
            else if (filterQueryType == FilterQueryType.Update)
            {
                queryFormat = FilterSqlTemplates.FilterUpdate;
            }
            else
            {
                queryFormat = FilterSqlTemplates.ExistsQuery;

                if (scope.ExistsWhereClauses.Any())
                {
                    string existsWhereClause = string.Empty;
                    foreach (string scopeExistsWhereClause in scope.ExistsWhereClauses)
                    {
                        existsWhereClause += $" {scopeExistsWhereClause} {Environment.NewLine} AND ";
                    }

                    var regex = new Regex(Regex.Escape("WHERE"));
                    filterSqlPredicate = regex.Replace(filterSqlPredicate, $"WHERE {existsWhereClause} {Environment.NewLine} ", 1);
                }
            }

            return string.Format(queryFormat,
                scope.TableAlias,
                filterSqlContext.GetColumnName(scope.PrimaryKey),
                fromClause,
                filterSqlPredicate,
                (int) FilterCountStatus.Ready);
        }

        /// <summary>
        /// Generate the SQL for a folder
        /// </summary>
        private string GenerateFolderSql(FilterQueryType filterQueryType, IEnumerable<string> existsSqls)
        {
            List<(long FilterNodeContentID, string EntityExistsQuery)> childContentKeys = GetChildContentKeys(node);

            if (childContentKeys.None())
            {
                if (filterQueryType != FilterQueryType.Exists)
                {
                    return string.Format(FilterSqlTemplates.FolderEmpty,
                        (int) FilterCountStatus.Ready);
                }
                else
                {
                    // Don't return anything.
                    return FilterSqlTemplates.ExistsQueryEmpty;
                }
            }

            StringBuilder childInList = new StringBuilder();
            foreach (long childCountID in childContentKeys.Select(x => x.FilterNodeContentID))
            {
                if (childInList.Length > 0)
                {
                    childInList.Append(", ");
                }

                childInList.AppendFormat("{0}", childCountID);
            }

            string folderSql = string.Empty;
            switch (filterQueryType)
            {
                case FilterQueryType.Initial:
                    folderSql = string.Format(FilterSqlTemplates.FolderInitial, childInList, (int) FilterCountStatus.Ready);
                    break;
                case FilterQueryType.Update:
                    folderSql = string.Format(FilterSqlTemplates.FolderUpdate, childInList, (int) FilterCountStatus.Ready);
                    break;
                case FilterQueryType.Exists:
                    folderSql = String.Join($"{Environment.NewLine} UNION {Environment.NewLine} ", existsSqls);
                    folderSql = string.Format(FilterSqlTemplates.ExistsQueryFolder, folderSql);
                    break;
            }

            return folderSql;
        }

        /// <summary>
        /// Get the ID of every content ID that is associated with a child node
        /// </summary>
        private List<(long FilterNodeContentID, string EntityExistsQuery)> GetChildContentKeys(FilterNodeEntity parent)
        {
            if (!node.Filter.IsFolder)
            {
                throw new InvalidOperationException("Cannot get child count IDs for a filter.");
            }

            List<(long FilterNodeContentID, string EntityExistsQuery)> contentKeys = new List<(long FilterNodeContentID, string EntityExistsQuery)>();

            foreach (FilterNodeEntity child in parent.ChildNodes)
            {
                if (child.Filter.IsFolder)
                {
                    contentKeys.AddRange(GetChildContentKeys(child));
                    //support this so that sub folders get their filternodeid returned if they have subs that apply
                }
                else
                {
                    if (child.FilterNodeContent == null)
                    {
                        child.FilterNodeContent = new FilterNodeContentEntity(child.FilterNodeContentID);
                        SqlAdapter.Default.FetchEntity(child.FilterNodeContent);
                    }

                    contentKeys.Add((child.FilterNodeContentID, child.FilterNodeContent.EntityExistsQuery));
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
        private (byte[] ChildMasks, int JoinMasks, List<string> ExistsSqls) GenerateFolderMasks()
        {
            ResultsetFields resultFields = new ResultsetFields(2);
            resultFields.DefineField(FilterNodeContentFields.ColumnMask, 0, "ColumnMask", "");
            resultFields.DefineField(FilterNodeContentFields.JoinMask, 1, "JoinMask", "");

            List<(long FilterNodeContentID, string EntityExistsQuery)> childContentKeys = GetChildContentKeys(node);
            if (childContentKeys.Count == 0)
            {
                return (new byte[0], 0, Enumerable.Empty<string>().ToList());
            }

            RelationPredicateBucket bucket = new RelationPredicateBucket(
                FilterNodeContentFields.FilterNodeContentID == childContentKeys.Select(x => x.FilterNodeContentID).ToList());

            List<byte[]> childMasks = new List<byte[]>();
            int joinMasks = 0;

            // Do the fetch
            ExistingConnectionScope.ExecuteWithAdapter(adapter =>
            {
                using (IDataReader reader = adapter.FetchDataReader(resultFields, bucket, CommandBehavior.Default, 0, true))
                {
                    while (reader.Read())
                    {
                        childMasks.Add((byte[]) reader.GetValue(0));
                        joinMasks |= reader.GetInt32(1);
                    }
                }
            });

            return (FilterNodeColumnMaskUtility.CreateUnionedBitmask(childMasks), joinMasks, childContentKeys.Select(x => x.EntityExistsQuery).ToList());
        }
    }
}
