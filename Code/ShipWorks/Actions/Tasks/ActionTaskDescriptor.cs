using System;
using System.Diagnostics;
using Autofac;

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
        ActionTaskCategory category;
        bool hidden;

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
                type.Namespace.EndsWith("CoreExtensions.Actions") ||
                type.Namespace.StartsWith("ShipWorks.Tests.Actions");

            Debug.Assert(validNamespace,
                @"When obfuscated, only types in the above namespace formats will work.  This is due to the xr option we use with demeanor.  If a type truly
                  should go in another namespace, then ensure it will work under obfuscation, and adjust this assert.");

#endif

            this.type = type;

            // Load the name
            ActionTaskAttribute attribute = (ActionTaskAttribute) Attribute.GetCustomAttribute(type, typeof(ActionTaskAttribute));
            this.baseName = attribute.DisplayName;
            this.identifier = attribute.Identifier;
            this.category = attribute.Category;
            this.hidden = attribute.Hidden;
        }

        /// <summary>
        /// String representation of the object.
        /// </summary>
        public override string ToString() => BaseName;

        /// <summary>
        /// This is the user-visible name of the task as defined by the original attribute.  If its a store-type specific task, this does
        /// not include the store type description.  Use DisplayName to include the Store Type information.
        /// </summary>
        public string BaseName => baseName;

        /// <summary>
        /// An identifier that uniquely identifies the action.  Once applied, this identifier cannot be changed,
        /// as it is used in a key in the serialization process.
        /// </summary>
        public string Identifier => identifier;

        /// <summary>
        /// Get the task category - used for UI display purposes
        /// </summary>
        public ActionTaskCategory Category => category;

        /// <summary>
        /// The CLR type of the action.
        /// </summary>
        public Type SystemType => type;

        /// <summary>
        /// Used to determine if this ActionTask should be hidden in the UI
        /// </summary>
        public bool Hidden => hidden;

        ///// <summary>
        ///// Create a new instance of the ActionTask for this type.
        ///// </summary>
        //public ActionTask CreateInstance() =>
        //    (ActionTask) SystemType.GetConstructor(Type.EmptyTypes).Invoke(null);

        /// <summary>
        /// Create a new instance of the ActionTask for this type.
        /// </summary>
        /// <remarks>
        /// This version will instantiate the entire dependency tree
        /// </remarks>
        public ActionTask CreateInstance(ILifetimeScope lifetimeScope) =>
            lifetimeScope.Resolve(SystemType) as ActionTask;
    }
}
