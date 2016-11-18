// From: https://github.com/mthamil/Autofac.Extras.Ordering
// License: Apache 2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Builder;
using Autofac.Core;

namespace ShipWorks.ApplicationCore.ComponentRegistration.Ordering
{
    /// <summary>
    /// Provides support for <see cref="IOrderedEnumerable{TElement}"/>.
    /// </summary>
    public class OrderedRegistrationSource : IRegistrationSource
    {
        internal const string OrderingMetadataKey = "AutofacOrderingMetadataKey";

        /// <summary>
        /// Retrieve registrations for an unregistered service, to be used
        /// by the container.
        /// </summary>
        /// <param name="service">The service that was requested.</param>
        /// <param name="registrationAccessor">A function that will return existing registrations for a service.</param>
        /// <returns>Registrations providing the service.</returns>
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            IServiceWithType typedService = service as IServiceWithType;
            if (typedService != null)
            {
                Type serviceType = typedService.ServiceType;
                if (serviceType.IsInstanceOfGenericType(typeof(IOrderedEnumerable<>)))
                {
                    Type dependencyType = serviceType.GetGenericArguments().Single();

                    IComponentRegistration registration = (IComponentRegistration) CreateRegistrationMethod
                        .MakeGenericMethod(dependencyType)
                        .Invoke(null, new object[0]);

                    return new[] { registration };
                }
            }

            return Enumerable.Empty<IComponentRegistration>();
        }

        /// <summary>
        /// Gets whether the registrations provided by this source are 1:1 adapters on top
        /// of other components (ie. like Meta, Func, or Owned.)
        /// </summary>
        /// <remarks>Always returns false.</remarks>
        public bool IsAdapterForIndividualComponents => false;

        /// <summary>
        /// A description of the registration source.
        /// </summary>
        public override string ToString() => "Guaranteed Resolution Order Support (IOrderedEnumerable<T>)";

        /// <summary>
        /// Create an ordered registration
        /// </summary>
        private static IComponentRegistration CreateOrderedRegistration<TService>()
        {
            return RegistrationBuilder
                .ForDelegate((c, ps) => c.ResolveOrdered<TService>(ps))
                .ExternallyOwned()
                .CreateRegistration();
        }

        /// <summary>
        /// Method used to get ordered registration
        /// </summary>
        private static readonly MethodInfo CreateRegistrationMethod =
            typeof(OrderedRegistrationSource).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                                             .Single(m => m.Name == nameof(CreateOrderedRegistration));
    }
}
