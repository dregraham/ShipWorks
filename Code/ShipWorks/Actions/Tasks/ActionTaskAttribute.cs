using System;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Attribute that is applied to ActionTask instances to register them in the system.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ActionTaskAttribute : Attribute
    {
        string displayName;
        string identifier;
        ActionTaskCategory category;
        ActionTriggerClassifications allowedActionTriggerClassifications;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskAttribute(string displayName, string identifier, ActionTaskCategory category) :
            this(displayName, identifier, category, ActionTriggerClassifications.Scheduled | ActionTriggerClassifications.Nonscheduled)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskAttribute(string displayName, string identifier, ActionTaskCategory category, ActionTriggerClassifications allowedActionTriggerClassifications)
        {
            this.displayName = displayName;
            this.identifier = identifier;
            this.category = category;
            this.allowedActionTriggerClassifications = allowedActionTriggerClassifications;
        }

        /// <summary>
        /// The name of the task as displayed to the user.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        /// <summary>
        /// An identifier that uniquely identifies the task.  Once applied, this identifier cannot be changed,
        /// as it is used in a key in the serialization process.
        /// </summary>
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }

        /// <summary>
        /// Get the task category - used for UI display purposes
        /// </summary>
        public ActionTaskCategory Category
        {
            get
            {
                return category;
            }
        }

        /// <summary>
        /// If this task is allowed to be added to an Action that is of Scheduled type.
        /// </summary>
        public ActionTriggerClassifications AllowedActionTriggerClassifications
        {
            get
            {
                return allowedActionTriggerClassifications;
            }
        }  
    }
}
