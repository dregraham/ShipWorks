// From: https://github.com/mthamil/Autofac.Extras.Ordering
// License: Apache 2.0

using System;
using System.Linq;
using Autofac.Builder;

namespace Interapptive.Shared.ComponentRegistration.Ordering
{
    /// <summary>
    /// An Autofac extension that provides control over the order in which multiple dependencies are resolved.
    /// By default, order is not guaranteed when resolving <see cref="System.Collections.Generic.IEnumerable{T}"/>.
    /// Essentially, a new relationship type is supported. Declaring a dependency of type
    /// <see cref="IOrderedEnumerable{TElement}"/> with this extension allows for a deterministic order.
    /// </summary>
    public static class OrderedRegistrationExtensions
    {
        /// <summary>
        /// Configures an explicit order that a service should be resolved in.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to set parameter on.</param>
        /// <param name="order">The order for which a service will be resolved</param>
        /// <returns>A registration builder allowing further configuration of the component.</returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OrderBy<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, string key, IComparable order)
        {
            return OrderBy<TLimit, TActivatorData, TRegistrationStyle>(registration, key, _ => order);
        }

        /// <summary>
        /// Configures a function that will determine a service's resolution order dynamically.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to set parameter on.</param>
        /// <param name="keySelector">Selects an ordering based on a component's properties</param>
        /// <returns>A registration builder allowing further configuration of the component.</returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OrderBy<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, string key, Func<TLimit, IComparable> keySelector)
        {
            return registration.WithMetadata(OrderedRegistrationSource.OrderingMetadataKey + key, keySelector);
        }

        /// <summary>
        /// Configures a function that will determine a service's resolution order dynamically.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to set parameter on.</param>
        /// <returns>A registration builder allowing further configuration of the component.</returns>
        public static IRegistrationBuilder<TLimit, ReflectionActivatorData, TRegistrationStyle> OrderByMetadata<TLimit, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, ReflectionActivatorData, TRegistrationStyle> registration, Type service)
        {
            OrderAttribute orderAttribute = Enumerable.OfType<OrderAttribute>(registration.ActivatorData.ImplementationType
                    .GetCustomAttributes(typeof(OrderAttribute), false))
                .Where(x => x.Service == service)
                .FirstOrDefault();

            return orderAttribute != null ?
                registration.OrderBy(orderAttribute.Service.Name, orderAttribute.Order) :
                registration;
        }
    }
}
