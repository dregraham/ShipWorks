using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace ShipWorks.ApplicationCore.ComponentRegistration
{
    /// <summary>
    /// Register a component keyed for specific service
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class KeyedComponentAttribute : Attribute
    {
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
        /// Register all components that use this attribute
        /// </summary>
        internal static void Register(ContainerBuilder builder, params Assembly[] assemblies)
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
                foreach (var attribute in item.Attributes)
                {
                    builder.RegisterType(item.Component)
                        .Keyed(attribute.Key, attribute.Service);
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