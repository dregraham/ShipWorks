using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Email;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Emails
{
    [ConditionElement("Status", "Email.Status")]
    public class EmailStatusCondition : EnumCondition<EmailOutboundStatus>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EmailStatusCondition()
        {
            Value = EmailOutboundStatus.Sent;
            SelectedValues = new[] { Value };
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(EmailOutboundFields.SendStatus), context);
        }
    }
}
