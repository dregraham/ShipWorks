using System;
using System.Collections.Generic;
using Interapptive.Shared.Extensions;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;
using Interapptive.Shared.Business;

namespace ShipWorks.Filters.Content.Conditions.QuickSearch
{
    /// <summary>
    /// Condition for a quick search
    /// </summary>
    public class QuickSearchCondition : Condition
    {
        private readonly string searchText;
        private readonly bool isNumeric;
        private readonly IEnumerable<IQuickSearchStoreSql> storeSqls;
        private SqlGenerationContext sqlGenerationContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuickSearchCondition(string searchText, IEnumerable<IQuickSearchStoreSql> storeSqls)
        {
            this.storeSqls = storeSqls;
            isNumeric = searchText.IsNumeric();
            this.searchText = searchText;
        }

        /// <summary>
        /// Generate the SQL that evaluates the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            sqlGenerationContext = context;

            string textParamName = context.RegisterParameter(OrderFields.OrderNumberComplete, $"{searchText}%");

            List<string> selectStatements = new List<string>()
            {
                $"SELECT OrderId FROM [Order] WHERE OrderNumberComplete LIKE {textParamName}",
                $"SELECT OrderId FROM [OrderSearch] WHERE OrderNumberComplete LIKE {textParamName}"
            };

            AddOrderNumberSearch(context, selectStatements);

            // If the search text isn't numeric, add name/email fields.
            if (!isNumeric)
            {
                // If there's a space in the search text, we assume they are wanting a to do a 
                // first name/last name search, so build that AND'd.
                if (searchText.Contains(" "))
                {
                    PersonName name = PersonName.Parse(searchText);

                    string firstNameParamName = context.RegisterParameter(OrderFields.ShipFirstName, $"{name.First}%");
                    string lastNameParamName = context.RegisterParameter(OrderFields.ShipLastName, $"{name.Last}%");

                    selectStatements.Add($"SELECT OrderId FROM [Order] WHERE (BillFirstName LIKE {firstNameParamName} OR ShipFirstName LIKE {firstNameParamName}) AND (BillLastName LIKE {lastNameParamName} OR ShipLastName LIKE {lastNameParamName})");
                }
                else
                {
                    // No space means it could be anything, OR each field.
                    selectStatements.Add($"SELECT OrderId FROM [Order] WHERE BillFirstName LIKE {textParamName} OR ShipFirstName LIKE {textParamName} OR BillLastName LIKE {textParamName} OR ShipLastName LIKE {textParamName} OR BillEmail LIKE {textParamName} OR ShipEmail LIKE {textParamName}");
                }
            }

            // Get each store's quick search SQL
            foreach (IQuickSearchStoreSql quickSearchStoreSql in storeSqls)
            {
                selectStatements.AddRange(quickSearchStoreSql.GenerateSql(sqlGenerationContext, $"{searchText}%"));
            }

            // Now create the SQL
            string searchSql = $@" o.OrderId in
            (
	            {
                    string.Join($" {Environment.NewLine} UNION {Environment.NewLine}", selectStatements)
                }
            )";

            return searchSql;
        }

        /// <summary>
        /// If searchText is numeric and starts with 0, search orders for the numeric value of the order number.
        /// </summary>
        private void AddOrderNumberSearch(SqlGenerationContext context, List<string> selectStatements)
        {
            if (isNumeric && long.Parse(searchText) > 0)
            {
                long numericSearchText = long.Parse(searchText);
                string numericParamName = context.RegisterParameter(OrderFields.OrderNumber, numericSearchText);
                
                selectStatements.Add($"SELECT OrderId FROM [Order] WHERE OrderNumber = {numericParamName}");
                selectStatements.Add($"SELECT OrderId FROM [OrderSearch] WHERE OrderNumber = {numericParamName}");
            }
        }

        /// <summary>
        /// No editor!
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return null;
        }
    }
}
