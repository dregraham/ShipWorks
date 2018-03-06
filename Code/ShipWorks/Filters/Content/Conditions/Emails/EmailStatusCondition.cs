using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.Filters.Content.SqlGeneration;

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
