using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Content;

namespace ShipWorks.Filters.Content.Conditions.Notes
{
    [ConditionElement("Note Visibility", "Note.Visibility")]
    public class NoteVisibilityCondition : EnumCondition<NoteVisibility>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NoteVisibilityCondition()
        {
            Value = NoteVisibility.Internal;
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(NoteFields.Visibility), context);
        }
    }
}
