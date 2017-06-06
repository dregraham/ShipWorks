// From: https://github.com/mthamil/Autofac.Extras.Ordering
// License: Apache 2.0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;

namespace Interapptive.Shared.ComponentRegistration.Ordering
{
    /// <summary>
    /// Provides methods that simplify resolution of ordered services.
    /// </summary>
    public static class OrderedResolutionExtensions
    {
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> which is already assumed to be ordered
        /// as an <see cref="IOrderedEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TService">The type of elements</typeparam>
        /// <param name="alreadyOrdered">An already ordered sequence of elements</param>
        /// <returns>The sequence as an <see cref="IOrderedEnumerable{T}"/></returns>
        public static IOrderedEnumerable<TService> AsOrdered<TService>(this IEnumerable<TService> alreadyOrdered)
        {
            return new AlreadyOrderedEnumerable<TService>(alreadyOrdered);
        }

        /// <summary>
        /// Retrieves ordered services from the context.
        /// </summary>
        /// <typeparam name="TService">The type of service to which the results will be cast.</typeparam>
        /// <param name="context">The context from which to resolve the services.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The component instances that provide the service.</returns>
        public static IOrderedEnumerable<TService> ResolveOrdered<TService>(this IComponentContext context, params Parameter[] parameters)
        {
            return ResolveOrdered<TService>(context, parameters.AsEnumerable());
        }

        /// <summary>
        /// Retrieves ordered services from the context.
        /// </summary>
        /// <typeparam name="TService">The type of service to which the results will be cast.</typeparam>
        /// <param name="context">The context from which to resolve the services.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The component instances that provide the service.</returns>
        public static IOrderedEnumerable<TService> ResolveOrdered<TService>(this IComponentContext context, IEnumerable<Parameter> parameters)
        {
            Type registeredType = typeof(IEnumerable<>).MakeGenericType(
                                 typeof(Meta<>).MakeGenericType(typeof(TService)));
            Meta<TService>[] resolved = (Meta<TService>[]) context.Resolve(registeredType, parameters);
            return resolved.Where(HasOrderingMetadata<TService>)
                           .OrderBy(GetOrderFromMetadata<TService>)
                           .Select(t => t.Value)
                           .ToArray<TService>()
                           .AsOrdered();
        }

        /// <summary>
        /// Check if the instance has ordering metadata
        /// </summary>
        private static bool HasOrderingMetadata<TService>(Meta<TService> instance) =>
            instance.Metadata.ContainsKey(OrderedRegistrationSource.OrderingMetadataKey + typeof(TService).Name);

        /// <summary>
        /// Get the component order from metadata
        /// </summary>
        private static object GetOrderFromMetadata<TService>(Meta<TService> instance)
        {
            object orderingFunction = instance.Metadata[OrderedRegistrationSource.OrderingMetadataKey + typeof(TService).Name];
            return ((Delegate) orderingFunction).DynamicInvoke(UnwrapValue(instance.Value));
        }

        /// <summary>
        /// Unwrap the order value
        /// </summary>
        private static object UnwrapValue(object value)
        {
            Type type = value.GetType();
            if (!IsMetadata(type))
            {
                return value;
            }

            return UnwrapValue(type.GetProperty(nameof(Meta<object>.Value))
                .GetValue(value, null)); // Unwrap a layer of metadata.
        }

        /// <summary>
        /// Check whether the type is metadata
        /// </summary>
        private static bool IsMetadata(Type type)
        {
            return type.IsInstanceOfGenericType(typeof(Meta<>)) ||
                   type.IsInstanceOfGenericType(typeof(Meta<,>));
        }

        /// <summary>
        /// A simple wrapper that presents an <see cref="IEnumerable{T}"/> as an assumed-to-already-be-ordered collection.
        /// </summary>
        private sealed class AlreadyOrderedEnumerable<T> : IOrderedEnumerable<T>
        {
            private readonly IEnumerable<T> wrapped;

            /// <summary>
            /// Constructor
            /// </summary>
            public AlreadyOrderedEnumerable(IEnumerable<T> wrapped)
            {
                this.wrapped = wrapped;
            }

            /// <summary>
            /// This method would be invoked if OrderBy was called again on the collection (unlikely for this usage).
            /// </summary>
            public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool @descending)
            {
                return @descending ?
                    wrapped.OrderByDescending(keySelector, comparer) :
                    wrapped.OrderBy(keySelector, comparer);
            }

            /// <summary>
            /// Get an enumerator
            /// </summary>
            public IEnumerator<T> GetEnumerator() => wrapped.GetEnumerator();

            /// <summary>
            /// Get an enumerator
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}