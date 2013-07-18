using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using ShipWorks.Actions.Triggers;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Metadata about an action task that can be instantiated and used by the user.
    /// </summary>
    public class ActionTaskDescriptor
    {        
        Type type;
        string baseName;
        string identifier;
        ActionTriggerClassifications allowedActionTriggerClassifications;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskDescriptor(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!Attribute.IsDefined(type, typeof(ActionTaskAttribute)))
            {
                throw new InvalidOperationException("Cannot create ActionTaskDescriptor instance for class without ActionTaskAttribute.");
            }

            if (!type.IsClass)
            {
                throw new InvalidOperationException("Cannot create ActionTaskDescriptor instance for non-class.");
            }

            if (!type.IsSubclassOf(typeof(ActionTask)))
            {
                throw new InvalidOperationException("Cannot create ActionTaskDescriptor instance for class not derived from ActionTask.");
            }

            #if DEBUG

            bool validNamespace =
                type.Namespace.StartsWith("ShipWorks.Actions.Tasks.Common") ||
                type.Namespace.EndsWith("CoreExtensions.Actions");

            Debug.Assert(validNamespace,
                @"When obfuscated, only types in the above namespace formats will work.  This is due to the xr option we use with demeanor.  If a type truly
                  should go in another namespace, then ensure it will work under obfuscation, and adjust this assert.");

            #endif

            this.type = type;

            // Load the name
            ActionTaskAttribute attribute = (ActionTaskAttribute) Attribute.GetCustomAttribute(type, typeof(ActionTaskAttribute));
            this.baseName = attribute.DisplayName;
            this.identifier = attribute.Identifier;
            this.allowedActionTriggerClassifications = attribute.AllowedActionTriggerClassifications;
        }

        /// <summary>
        /// String representation of the object.
        /// </summary>
        public override string ToString()
        {
            return BaseName;
        }

        /// <summary>
        /// This is the user-visible name of the task as defined by the original attribute.  If its a store-type specific task, this does 
        /// not include the store type description.  Use DisplayName to include the Store Type information.
        /// </summary>
        public string BaseName
        {
            get
            {
                return baseName;
            }
        }

        /// <summary>
        /// An identifier that uniquely identifies the action.  Once applied, this identifier cannot be changed,
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
        /// The CLR type of the action.
        /// </summary>
        public Type SystemType
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// If this task is allowed to be added to an Action that is of Scheduled type.  For example, "Play a Sound" is not allowed
        /// as the sound will not be heard when run as a service.
        /// </summary>
        public ActionTriggerClassifications TriggerClassifications
        {
            get
            {
                return allowedActionTriggerClassifications;
            }
        }

        /// <summary>
        /// Is the task allowed to be run using the specified trigger type?
        /// </summary>
        /// <param name="trigger">Trigger that should be tested</param>
        /// <returns></returns>
        public bool IsAllowedForTrigger(ActionTrigger trigger)
        {
            ActionTriggerClassifications classification = (trigger.TriggerType == ActionTriggerType.Scheduled) ? 
                ActionTriggerClassifications.Scheduled : 
                ActionTriggerClassifications.NonScheduled;
            return (allowedActionTriggerClassifications & classification) != ActionTriggerClassifications.None;
        }

        /// <summary>
        /// Create a new instance of the ActionTask for this type.
        /// </summary>
        public ActionTask CreateInstance()
        {
            return (ActionTask) SystemType.GetConstructor(Type.EmptyTypes).Invoke(null);
        }
    }
}
