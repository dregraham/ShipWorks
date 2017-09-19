using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace Interapptive.Shared.ComponentRegistration
{
    /// <summary>
    /// Register a component for implemented interfaces
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ComponentAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="registerAs"></param>
        public ComponentAttribute(RegistrationType registerAs = RegistrationType.ImplementedInterfaces)
        {
            RegisterAs = registerAs;
        }

        /// <summary>
        /// What service should this component be registered as
        /// </summary>
        public RegistrationType RegisterAs { get; set; }

        /// <summary>
        /// Service for which this component should be registered
        /// </summary>
        public Type Service { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the component should be registered as a single instance.
        /// </summary>
        public bool SingleInstance { get; set; } = false;

        /// <summary>
        /// Get a registration, either from cache or by creating a new one
        /// </summary>
        public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> GetRegistrationBuilder(
            Type component,
            ContainerBuilder builder,
            IDictionary<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registrationCache)
        {
            if (registrationCache.ContainsKey(component))
            {
                return registrationCache[component];
            }

            var registration = builder.RegisterType(component);
            registrationCache.Add(component, registration);
            return registration;
        }

        /// <summary>
        /// Register all components that use this attribute
        /// </summary>
        public static void Register(ContainerBuilder builder,
            IDictionary<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registrationCache,
            params Assembly[] assemblies)
        {
            var components = assemblies.SelectMany(x => x.GetTypes())
                .Select(x => new
                {
                    Component = x,
                    Attributes = GetAttributes(x),
                })
                .Where(x => x.Attributes.Any());

            foreach (var item in components)
            {
                var registration = GetRegistrationBuilder(item.Component, builder, registrationCache);

                if (item.Attributes.Any(x => x.RegisterAs.HasFlag(RegistrationType.ImplementedInterfaces)))
                {
                    registration.AsImplementedInterfaces();
                }

                if (item.Attributes.Any(x => x.RegisterAs.HasFlag(RegistrationType.Self)))
                {
                    registration.AsSelf();
                }

                Type[] specificServices = item.Attributes
                    .Where(x => x.RegisterAs.HasFlag(RegistrationType.SpecificService))
                    .Select(x => x.Service)
                    .ToArray();
                if (specificServices.Any())
                {
                    registration.As(specificServices);
                }

                if (item.Attributes.Any(x => x.SingleInstance))
                {
                    registration.SingleInstance();
                }
            }
        }

        /// <summary>
        /// Get a component attribute from the type
        /// </summary>
        private static IEnumerable<ComponentAttribute> GetAttributes(Type type) =>
            type.GetCustomAttributes(false).OfType<ComponentAttribute>();
    }
}
