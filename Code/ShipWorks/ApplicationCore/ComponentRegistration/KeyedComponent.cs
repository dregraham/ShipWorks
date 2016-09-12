using System;
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
                    Attribute = GetAttribute(x),
                })
                .Where(x => x.Attribute != null);

            foreach (var item in keyedComponents)
            {
                builder.RegisterType(item.Component)
                    .Keyed(item.Attribute.Key, item.Attribute.Service);
            }
        }

        /// <summary>
        /// Get a component attribute from the type
        /// </summary>
        private static KeyedComponentAttribute GetAttribute(Type type) =>
            GetCustomAttribute(type, typeof(KeyedComponentAttribute)) as KeyedComponentAttribute;
    }
}