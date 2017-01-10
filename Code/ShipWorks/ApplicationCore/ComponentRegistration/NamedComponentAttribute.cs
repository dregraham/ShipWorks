using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace ShipWorks.ApplicationCore.ComponentRegistration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NamedComponentAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NamedComponentAttribute(string name, Type type)
        {
            ServiceName = name;
            ServiceType = type;
        }

        /// <summary>
        /// The name of the component
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// The type of the base class or interface
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// Gets a value indicating whether [externally owned].
        /// </summary>
        public bool ExternallyOwned { get; set; }

        /// <summary>
        /// Register all components that use this attribute
        /// </summary>
        internal static void Register(ContainerBuilder builder, params Assembly[] assemblies)
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
                    IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration =
                        builder.RegisterType(item.Component).Named(attribute.ServiceName, attribute.ServiceType);

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
        private static IEnumerable<NamedComponentAttribute> GetAttributes(Type type) =>
            GetCustomAttributes(type, typeof(NamedComponentAttribute)).OfType<NamedComponentAttribute>();
    }
}
