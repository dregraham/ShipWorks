using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using ShipWorks.ApplicationCore.ComponentRegistration.Ordering;

namespace ShipWorks.ApplicationCore.ComponentRegistration
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
        public static void Register(ContainerBuilder builder, params Assembly[] assemblies)
        {
            List<Type> services = assemblies.SelectMany(x => x.GetTypes())
                .Where(IsService)
                .ToList();

            foreach (Type component in assemblies.SelectMany(x => x.GetTypes()))
            {
                foreach (Type service in services.Where(ShouldRegister(component)))
                {
                    builder.RegisterType(component)
                        .As(service)
                        .OrderByMetadata()
                        .PreserveExistingDefaults();
                }
            }
        }

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
    }
}
