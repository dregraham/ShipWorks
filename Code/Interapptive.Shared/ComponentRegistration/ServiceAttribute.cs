﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Interapptive.Shared.ComponentRegistration.Ordering;

namespace Interapptive.Shared.ComponentRegistration
{

    /// <summary>
    /// Register a component for implemented interfaces
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class ServiceAttribute : Attribute
    {
        /// <summary>
        /// Register all components that use this attribute
        /// </summary>
        public static void Register(ContainerBuilder builder,
            IDictionary<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registrationCache,
            params Assembly[] assemblies)
        {
            List<Type> services = assemblies.SelectMany(x => x.GetTypes())
                .Where(IsService)
                .ToList();

            foreach (Type component in assemblies.SelectMany(x => x.GetTypes()).Where(x => !x.IsInterface))
            {
                foreach (Type service in services.Where(ShouldRegister(component)))
                {
                    var registrationBuilder = ComponentAttribute.GetRegistrationBuilder(component, builder, registrationCache);

                    registrationBuilder.As(service).OrderByMetadata(service);

                    if (GetAttributes(service).Any(s => s.SingleInstance))
                    {
                        registrationBuilder.SingleInstance();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it should be registered as a single instance.
        /// </summary>
        public bool SingleInstance { get; set; } = false;

        /// <summary>
        /// Should the component be registered for the given service
        /// </summary>
        private static Func<Type, bool> ShouldRegister(Type component)
        {
            return service => service != component && service.IsAssignableFrom(component);
        }

        /// <summary>
        /// Is the given type marked as a component
        /// </summary>
        private static bool IsService(Type type)
        {
            return GetCustomAttribute(type, typeof(ServiceAttribute)) != null;
        }

        /// <summary>
        /// Get a component attribute from the type
        /// </summary>
        private static IEnumerable<ServiceAttribute> GetAttributes(Type type) =>
            GetCustomAttributes(type, typeof(ServiceAttribute)).OfType<ServiceAttribute>();
    }
}
