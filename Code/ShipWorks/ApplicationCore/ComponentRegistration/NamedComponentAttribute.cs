using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace ShipWorks.ApplicationCore.ComponentRegistration
{
    /// <summary>
    /// Register a component named for specific service
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NamedComponentAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NamedComponentAttribute(string name, Type type)
        {
            ComponentName = name;
            ComponentType = type;
        }

        /// <summary>
        /// The name of the component
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// The type of the base class or interface
        /// </summary>
        public Type ComponentType { get; set; }

        /// <summary>
        /// Register all components that use this attribute
        /// </summary>
        internal static void Register(ContainerBuilder builder,
            IDictionary<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registrationCache,
            params Assembly[] assemblies)
        {
            var namedComponents = assemblies.SelectMany(x => x.GetTypes())
                .Select(x => new
                {
                    Component = x,
                    Attributes = GetAttributes(x),
                })
                .Where(x => x.Attributes.Any());

            foreach (var item in namedComponents)
            {
                foreach (NamedComponentAttribute attribute in item.Attributes)
                {
                    ComponentAttribute.GetRegistrationBuilder(item.Component, builder, registrationCache)
                        .Named(attribute.ComponentName, attribute.ComponentType);
                }
            }
        }

        /// <summary>
        /// Get a component attribute from the type
        /// </summary>
        private static IEnumerable<NamedComponentAttribute> GetAttributes(Type type) =>
            GetCustomAttributes(type, typeof(NamedComponentAttribute)).OfType<NamedComponentAttribute>();
    }
}
