using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Triggers.Editors;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Actions.Triggers
{
    /// <summary>
    /// Base class for all triggers
    /// </summary>
    public abstract class ActionTrigger : SerializableObject
    {
        /// <summary>
        /// Raised then the TriggeringEntityType property changes
        /// </summary>
        public event EventHandler TriggeringEntityTypeChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ActionTrigger(string xmlSettings)
        {
            if (string.IsNullOrEmpty(xmlSettings))
            {
                return;
            }

            DeserializeXml(xmlSettings);
        }

        /// <summary>
        /// Overridden to provide the trigger type
        /// </summary>
        public abstract ActionTriggerType TriggerType { get; }

        /// <summary>
        /// Creates the editor that is used to edit the condition.
        /// </summary>
        public abstract ActionTriggerEditor CreateEditor();

        /// <summary>
        /// The type of entity that causes the trigger to fire
        /// </summary>
        public abstract EntityType? TriggeringEntityType { get; }

        /// <summary>
        /// Raise the TriggeringEntityTypeChanged event
        /// </summary>
        protected void RaiseTriggeringEntityTypeChanged()
        {
            if (TriggeringEntityTypeChanged != null)
            {
                TriggeringEntityTypeChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Get the XML serialized representation of the trigger
        /// </summary>
        public string GetXml()
        {
            return SerializeXml("Settings");
        }

        /// <summary>
        /// Some triggers may have extra database state they need to save in addition to what get's persisted in the XML settings.
        /// Such as the sound file resource for the PlaySound task.  This gives the trigger a chance to save it.
        /// </summary>
        public virtual void SaveExtraState(ActionEntity action, SqlAdapter adapter)
        {

        }

        /// <summary>
        /// The trigger is being deleted.  Any database extra state saved by the trigger in SaveExtraState should be cleaned up.
        /// </summary>
        public virtual void DeleteExtraState(ActionEntity action, SqlAdapter adapter)
        {

        }
    }
}
