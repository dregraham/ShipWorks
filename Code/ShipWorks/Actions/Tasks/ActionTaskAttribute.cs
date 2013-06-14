using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        bool allowedForScheduledTask;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskAttribute(string displayName, string identifier)
        {
            this.displayName = displayName;
            this.identifier = identifier;
            allowedForScheduledTask = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskAttribute(string displayName, string identifier, bool allowedForScheduledTask)
        {
            this.displayName = displayName;
            this.identifier = identifier;
            this.allowedForScheduledTask = allowedForScheduledTask;
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
        /// If this task is allowed to be added to an Action that is of Scheduled type.  For example, "Play a Sound" is not allowed
        /// as the sound will not be heard when run as a service.
        /// </summary>
        public bool AllowedForScheduledTask
        {
            get
            {
                return allowedForScheduledTask;
            }
        }  
    }
}
