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
        bool hidden = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskAttribute(string displayName, string identifier, ActionTaskCategory category)
        {
            this.displayName = displayName;
            this.identifier = identifier;
            this.category = category;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskAttribute(string displayName, string identifier, ActionTaskCategory category, bool hidden)
        {
            this.displayName = displayName;
            this.identifier = identifier;
            this.category = category;
            this.hidden = hidden;
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
        /// Used to determine if this ActionTask should be hidden in the UI
        /// </summary>
        public bool Hidden
        {
            get
            {
                return hidden;
            }
        }
    }
}
