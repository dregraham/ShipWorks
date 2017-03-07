using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace ShipWorks.ApplicationCore.ComponentRegistration
{
    /// <summary>
    /// Register a component keyed for specific service
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class KeyedComponentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedComponentAttribute"/> class.
        /// </summary>
        public KeyedComponentAttribute(Type service, object key)
        {
            Service = service;
            Key = key;
        }

        /// <summary>
        /// Service for which this component should be registered
        /// </summary>
        public Type Service { get; set; }

        /// <summary>
        /// Key that will be used for the service registration
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// Gets a value indicating whether [externally owned].
        /// </summary>
        public bool ExternallyOwned { get; set; }

        /// <summary>
        /// Register all components that use this attribute
        /// </summary>
        internal static void Register(ContainerBuilder builder,
            IDictionary<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registrationCache,
            params Assembly[] assemblies)
        {
            var keyedComponents = assemblies.SelectMany(x => x.GetTypes())
                .Select(x => new
                {
                    Component = x,
                    Attributes = GetAttributes(x),
                })
                .Where(x => x.Attributes.Any());

            foreach (var item in keyedComponents)
            {
                foreach (KeyedComponentAttribute attribute in item.Attributes)
                {
                    var registration = ComponentAttribute.GetRegistrationBuilder(item.Component, builder, registrationCache)
                        .Keyed(attribute.Key, attribute.Service);

                    if (attribute.ExternallyOwned)
                    {
                        registration.ExternallyOwned();
                    }
                }
            }
        }

        /// <summary>
        /// Get a component attribute from the type
        /// </summary>
        private static IEnumerable<KeyedComponentAttribute> GetAttributes(Type type) =>
            type.GetCustomAttributes(false).OfType<KeyedComponentAttribute>();
    }
}