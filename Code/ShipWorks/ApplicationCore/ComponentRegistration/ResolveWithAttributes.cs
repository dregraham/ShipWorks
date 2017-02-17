using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extras.Attributed;

namespace ShipWorks.ApplicationCore.ComponentRegistration
{
    /// <summary>
    /// Use attributes when resolving the associated service
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ResolveWithAttributesAttribute : Attribute
    {
        /// <summary>
        /// Register all components that use this attribute
        /// </summary>
        internal static void Register(ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterAssemblyTypes(assemblies)
                .Where(ShouldUseAtttributes)
                .WithAttributeFilter();
        }

        /// <summary>
        /// Should the given type be resolved with attributes
        /// </summary>
        private static bool ShouldUseAtttributes(Type type) =>
            type.GetCustomAttributes(false).OfType<ComponentAttribute>().Any();
    }
}
