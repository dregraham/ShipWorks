using System;
using System.Reflection;
using Autofac;

namespace ShipWorks.ApplicationCore.ComponentRegistration
{
    /// <summary>
    /// Register a component for implemented interfaces
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ComponentAttribute : Attribute
    {
        /// <summary>
        /// Register all components that use this attribute
        /// </summary>
        internal static void Register(ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterAssemblyTypes(assemblies)
                .Where(IsComponent)
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Is the given type marked as a component
        /// </summary>
        private static bool IsComponent(Type type)
        {
            return GetCustomAttribute(type, typeof(ComponentAttribute)) != null;
        }
    }
}
