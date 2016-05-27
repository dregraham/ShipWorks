#if DEBUG
using Autofac;
using Autofac.Core;
using System;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Write IoC resolution calls to console
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class IoCLogRequestModule : Module
    {
        private int depth;

        /// <summary>
        /// Override to attach module-specific functionality to a
        /// component registration.
        /// </summary>
        /// <param name="componentRegistry">The component registry.</param>
        /// <param name="registration">The registration to attach functionality to.</param>
        /// <remarks>
        /// This method will be called for all existing <i>and future</i> component
        /// registrations - ordering is not important.
        /// </remarks>
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry,
            IComponentRegistration registration)
        {
            registration.Preparing += RegistrationOnPreparing;
            registration.Activating += RegistrationOnActivating;
            base.AttachToComponentRegistration(componentRegistry, registration);
        }

        /// <summary>
        /// Gets the prefix based on depth
        /// </summary>
        private string GetPrefix()
        {
            return new string('-', depth*2);
        }

        /// <summary>
        /// Writes registration preparation information to the console.
        /// </summary>
        private void RegistrationOnPreparing(object sender, PreparingEventArgs preparingEventArgs)
        {
            Console.WriteLine("{0}Resolving  {1}", GetPrefix(), preparingEventArgs.Component.Activator.LimitType);
            depth++;
        }

        /// <summary>
        /// Writes registration Activation information to the console.
        /// </summary>
        private void RegistrationOnActivating(object sender, ActivatingEventArgs<object> activatingEventArgs)
        {
            depth--;
            Console.WriteLine("{0}Activating {1}", GetPrefix(), activatingEventArgs.Component.Activator.LimitType);
        }
    }
}

#endif