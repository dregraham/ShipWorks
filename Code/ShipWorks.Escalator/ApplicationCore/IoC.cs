using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Interapptive.Shared.ComponentRegistration;
using log4net;

namespace ShipWorks.Escalator.ApplicationCore
{
    public static class IoC
    {
        /// <summary>
        /// Get the current IoC container
        /// </summary>
        private static IContainer current;

        /// <summary>
        /// Get the current global lifetime scope
        /// </summary>
        /// <remarks>This should ONLY be used in situations where a new lifetime scope cannot be created or disposed.
        /// Any dependency resolved through this will NEVER be released, which could cause a memory leak if the dependency
        /// is not marked as ExternallyOwned or SingleInstance</remarks>
        public static ILifetimeScope UnsafeGlobalLifetimeScope => current;

        /// <summary>
        /// All ShipWorks assemblies
        /// </summary>
        public static Assembly[] AllAssemblies { get; private set; }

        /// <summary>
        /// Begin a lifetime scope from which dependencies can be resolved
        /// </summary>
        public static ILifetimeScope BeginLifetimeScope() => current.BeginLifetimeScope();

        /// <summary>
        /// Initialize the IoC container
        /// </summary>
        public static IContainer Initialize(Action<ContainerBuilder> addExtraRegistrations, params Assembly[] assemblies) =>
            current = Build(addExtraRegistrations, assemblies);

        /// <summary>
        /// Build the registrations
        /// </summary>
        /// <remarks>
        /// This should be used for tests since the Initialize method sets the current container, which is not thread safe
        /// </remarks>
        public static IContainer InitializeForUnitTests(IContainer container) =>
            current = container;


        /// <summary>
        /// Build the IoC container
        /// </summary>
        public static IContainer Build(Action<ContainerBuilder> addExtraRegistrations, params Assembly[] assemblies)
        {
            AllAssemblies = assemblies;
            var builder = new ContainerBuilder();

            AddRegistrations(builder, AllAssemblies);

            addExtraRegistrations(builder);

            return builder.Build();
        }

        private static void AddRegistrations(ContainerBuilder builder, Assembly[] allAssemblies)
        {
            IDictionary<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registrationCache =
                new Dictionary<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>();

            ComponentAttribute.Register(builder, registrationCache, allAssemblies);

            // Pass "Func<Type, ILog> logFactory" as a dependency and get your log with:
            // log = logFactory(typeof (type))
            builder.Register((_, parameters) => LogManager.GetLogger(parameters.TypedAs<Type>()));
        }
    }
}
