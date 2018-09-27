using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Content;
using System.Data.SqlClient;
using ShipWorks.Filters;
using System.Diagnostics;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.SqlServer.Filters.DirtyCounts;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Provides the SQL for searching
    /// </summary>
    public class SearchSqlGenerator : IFilterContentSqlGenerator
    {
        FilterDefinition definition;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchSqlGenerator(FilterDefinition definition)
        {
            this.definition = definition;
        }

        /// <summary>
        /// Generate the SQL.  Either for the initial calculation, or update calculation.
        /// </summary>
        public FilterSqlResult GenerateSql(long countID)
        {
            string initialSql;
            string updateSql;
            string existsSql;
            ICollection<EntityField2> columnsUsed;
            ICollection<FilterNodeJoinType> joinsUsed;

            if (definition == null)
            {
                initialSql = string.Format(FilterSqlTemplates.FolderEmpty, (int) FilterCountStatus.Ready);
                updateSql = initialSql;
                existsSql = initialSql;

                columnsUsed = new List<EntityField2>();
                joinsUsed = new List<FilterNodeJoinType>();
            }
            else
            {
                SqlGenerationContext generationContext = new SqlGenerationContext(definition.FilterTarget);

                // Generate the sql represented by the element
                string predicate = definition.RootContainer.GenerateSql(generationContext);

                if (!string.IsNullOrEmpty(predicate))
                {
                    predicate = "WHERE\n" + predicate;
                }

                SqlGenerationScope scope = generationContext.TopLevelScope;

                initialSql = string.Format(FilterSqlTemplates.FilterInitial,
                    scope.TableAlias,
                    generationContext.GetColumnName(scope.PrimaryKey),
                    scope.GetFromClause(),
                    predicate,
                    (int) FilterCountStatus.Ready);

                updateSql = string.Format(FilterSqlTemplates.FilterUpdate,
                    scope.TableAlias,
                    generationContext.GetColumnName(scope.PrimaryKey),
                    scope.GetFromClause(),
                    predicate,
                    (int) FilterCountStatus.Ready);

                existsSql = string.Format(FilterSqlTemplates.ExistsQuery,
                    scope.TableAlias,
                    generationContext.GetColumnName(scope.PrimaryKey),
                    scope.GetFromClause(),
                    predicate,
                    (int) FilterCountStatus.Ready);

                columnsUsed = generationContext.ColumnsUsed;
                joinsUsed = generationContext.JoinsUsed;

                Debug.Assert(generationContext.IndentLevel == 1, "IndentLevel should be back to 1");
            }

            return new FilterSqlResult(countID, initialSql, updateSql, existsSql, columnsUsed, joinsUsed);
        }
    }
}
