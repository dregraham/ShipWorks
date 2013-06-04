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

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskAttribute(string displayName, string identifier)
        {
            this.displayName = displayName;
            this.identifier = identifier;
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
    }
}
