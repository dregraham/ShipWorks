using System;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data.Model;

namespace ShipWorks.Actions.Triggers
{
    /// <summary>
    /// Empty trigger that does nothing
    /// </summary>
    public class EmptyTrigger : ActionTrigger
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EmptyTrigger() : base(null)
        {
        }

        /// <summary>
        /// This does not apply to a specific entity
        /// </summary>
        public override EntityType? TriggeringEntityType => null;

        /// <summary>
        /// Specify the trigger type
        /// </summary>
        public override ActionTriggerType TriggerType => ActionTriggerType.None;

        /// <summary>
        /// Create the editor
        /// </summary>
        public override ActionTriggerEditor CreateEditor()
        {
            throw new NotImplementedException("This trigger cannot be edited");
        }
    }
}
