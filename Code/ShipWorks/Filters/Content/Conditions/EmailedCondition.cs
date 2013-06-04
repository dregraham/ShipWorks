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
    /// Base condition for determining what emails have been sent as related to an object
    /// </summary>
    public abstract class EmailedCondition : TemplateCondition
    {
        // Boolean to apply to the condition
        EmailedQueryType queryType = EmailedQueryType.HasBeen;
        
        /// <summary>
        /// The type of query to do on the email outbound table
        /// </summary>
        public EmailedQueryType QueryType
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

            return new EmailedValueEditor(this);
        }

        /// <summary>
        /// Generate the SQL that will be the predicate filter
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            SqlGenerationScopeType scopeType = (QueryType == EmailedQueryType.HasBeen) ? SqlGenerationScopeType.AnyChild : SqlGenerationScopeType.NoChild;

            // We have to get from Table -> EmailOutbound and their is no builtin relation for it.
            using (SqlGenerationScope scope = CreateScope(context, scopeType))
            {
                return scope.Adorn(
                    string.Format("({0} = {1} AND {2} = {3})",
                        context.GetColumnReference(EmailOutboundFields.TemplateID),
                        TemplateID,
                        context.GetColumnReference(EmailOutboundFields.SendStatus),
                        (int) EmailOutboundStatus.Sent
                        )
                );
            }
        }

        /// <summary>
        /// Create the scope that will be used to get down to the child emails
        /// </summary>
        protected abstract SqlGenerationScope CreateScope(SqlGenerationContext context, SqlGenerationScopeType scopeType);
    }
}
