using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Templates;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Email;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base condition for determining what print results have been done as related to an object
    /// </summary>
    public abstract class PrintedCondition : TemplateCondition
    {
        // Boolean to apply to the condition
        PrintedQueryType queryType = PrintedQueryType.HasBeen;
        
        /// <summary>
        /// The type of query to do on the print result table
        /// </summary>
        public PrintedQueryType QueryType
        {
            get { return queryType; }
            set { queryType = value; }
        }

        /// <summary>
        /// Create the editor for the condition
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            EnsureDefaultTemplateID();

            return new PrintedValueEditor(this);
        }

        /// <summary>
        /// Generate the SQL that will be the predicate filter
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            SqlGenerationScopeType scopeType = (QueryType == PrintedQueryType.HasBeen) ? SqlGenerationScopeType.AnyChild : SqlGenerationScopeType.NoChild;

            // We have to get from Table -> PrintResult and their is no builtin relation for it.
            using (SqlGenerationScope scope = CreateScope(context, scopeType))
            {
                return scope.Adorn(
                    string.Format("{0} = {1}",
                        context.GetColumnReference(PrintResultFields.TemplateID),
                        TemplateID
                        )
                );
            }
        }

        /// <summary>
        /// Create the scope that will be used to get down to the child print outs
        /// </summary>
        protected abstract SqlGenerationScope CreateScope(SqlGenerationContext context, SqlGenerationScopeType scopeType);
    }
}
