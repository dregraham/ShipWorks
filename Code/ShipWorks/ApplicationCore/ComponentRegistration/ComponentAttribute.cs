using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Register all components that use this attribute
        /// </summary>
        internal static void Register(ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterAssemblyTypes(assemblies)
                .Where(IsComponent(RegistrationType.ImplementedInterfaces))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(IsComponent(RegistrationType.Self))
                .AsSelf();
        }

        /// <summary>
        /// Is the given type marked as a component
        /// </summary>
        private static Func<Type, bool> IsComponent(RegistrationType registerAs)
        {
            return type => GetAttribute(type)?.Any(x => x.RegisterAs.HasFlag(registerAs)) ?? false;
        }

        /// <summary>
        /// Get a component attribute from the type
        /// </summary>
        private static IEnumerable<ComponentAttribute> GetAttribute(Type type) =>
            type.GetCustomAttributes(false).OfType<ComponentAttribute>();
    }
}
