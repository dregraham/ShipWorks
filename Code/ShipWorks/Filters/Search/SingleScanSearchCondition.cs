using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Searches OrderNumberComplete, and if searchText is numeric, also searches orderNumber
    /// </summary>
    public class SingleScanSearchCondition : Condition
    {
        private readonly string searchText;

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleScanSearchCondition(string searchText)
        {
            this.searchText = searchText;
        }

        /// <summary>
        /// Searches OrderNumberComplete, and if searchText is numeric, also searches orderNumber
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string textParamName = context.RegisterParameter(OrderFields.OrderNumberComplete, $"{searchText}%");

            List<string> selectStatements = new List<string>
            {
                $"SELECT OrderId FROM [Order] WHERE OrderNumberComplete LIKE {textParamName}",
                $"SELECT OrderId FROM [OrderSearch] WHERE OrderNumberComplete LIKE {textParamName}"
            };

            long searchNumber;
            bool isNumeric = long.TryParse(searchText, out searchNumber);

            if (isNumeric && searchNumber > 0)
            {
                string numericParamName = context.RegisterParameter(OrderFields.OrderNumber, searchNumber);
                selectStatements.Add($"SELECT OrderId FROM [Order] WHERE OrderNumber = {numericParamName}");
                selectStatements.Add($"SELECT OrderId FROM [OrderSearch] WHERE OrderNumber = {numericParamName}");
            }

            // Now create the SQL predicate.
            // This will ultimately be part of a where clause created by SearchSqlGenerator.GenerateSql. [Order] o is pulled in from
            // SqlGenerationScope.GetFromClause()
            string searchSql = $@" o.OrderId in
            (
	            {string.Join($" {Environment.NewLine} UNION {Environment.NewLine}", selectStatements)}
            )";

            return searchSql;
        }

        /// <summary>
        /// No editor
        /// </summary>
        public override ValueEditor CreateEditor() => null;
    }
}