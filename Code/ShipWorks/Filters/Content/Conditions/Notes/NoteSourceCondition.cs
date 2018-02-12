using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Content;

namespace ShipWorks.Filters.Content.Conditions.Notes
{
    [ConditionElement("Note Source", "Note.Source")]
    public class NoteSourceCondition : EnumCondition<NoteSource>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NoteSourceCondition()
        {
            Value = NoteSource.ShipWorksUser;
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(NoteFields.Source), context);
        }
    }
}
