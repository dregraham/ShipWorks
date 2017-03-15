using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Create instances of ConditionElement based on identifider
    /// </summary>
    public static class ConditionElementFactory
    {
        static Dictionary<string, ConditionElementDescriptor> elementDescriptors;

        /// <summary>
        /// Static constructor
        /// </summary>
        static ConditionElementFactory()
        {
            LoadDescriptors();
        }

        /// <summary>
        /// Create an instance of a ConditionElement based on its attribute assigned identifier
        /// </summary>
        public static ConditionElement CreateElement(string identifier)
        {
            // We need the descriptor for this identifier
            ConditionElementDescriptor descriptor = GetDescriptor(identifier);

            // Use the descriptor to create the instance
            return descriptor.CreateInstance();
        }

        /// <summary>
        /// Get the descriptor that has the given identifier
        /// </summary>
        public static ConditionElementDescriptor GetDescriptor(string identifier)
        {
            ConditionElementDescriptor descriptor = null;
            if (!elementDescriptors.TryGetValue(identifier, out descriptor))
            {
                throw new FilterConditionException(string.Format("Could not find descriptor for identifier '{0}'.", identifier));
            }

            return descriptor;
        }

        /// <summary>
        /// Get the descriptor that matches the given system type
        /// </summary>
        public static ConditionElementDescriptor GetDescriptor(Type type)
        {
            foreach (ConditionElementDescriptor descriptor in elementDescriptors.Values)
            {
                if (descriptor.SystemType == type)
                {
                    return descriptor;
                }
            }

            throw new FilterConditionException(string.Format("Could not find descriptor for type '{0}'.", type.FullName));
        }

        /// <summary>
        /// Load condition descriptors from core and stores assemblies
        /// </summary>
        private static void LoadDescriptors()
        {
            elementDescriptors = new Dictionary<string, ConditionElementDescriptor>();

            LoadDescriptorsFromAssembly(Assembly.GetExecutingAssembly());
            LoadDescriptorsFromAssembly(Assembly.Load("ShipWorks.Stores"));

        }

        /// <summary>
        /// Load all the condition element descriptors present in the assembly
        /// </summary>
        private static void LoadDescriptorsFromAssembly(Assembly assembly)
        {
            // Look for the ConditionAttribute on each type in the assembly
            foreach (Type type in assembly.GetTypes())
            {
                if (Attribute.IsDefined(type, typeof(ConditionElementAttribute)))
                {
                    ConditionElementDescriptor descriptor = new ConditionElementDescriptor(type);

                    // Each key must be unique
                    if (elementDescriptors.ContainsKey(descriptor.Identifier))
                    {
                        throw new InvalidOperationException(string.Format("Multiple condition elements with the same identifier were found. ({0})", descriptor.Identifier));
                    }

                    // Cache the descriptor
                    elementDescriptors[descriptor.Identifier] = descriptor;
                }
            }
        }
    }
}
