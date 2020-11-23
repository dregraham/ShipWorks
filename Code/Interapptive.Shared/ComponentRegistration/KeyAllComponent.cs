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
    /// Register a component keyed for specific service
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class KeyAllComponentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyAllComponentAttribute"/> class.
        /// </summary>
        public KeyAllComponentAttribute(Type service, Type enumType, object[] exclusions = null)
        {
            Service = service;
            EnumType = enumType;
            Exlusions = exclusions;
        }

        /// <summary>
        /// Service for which this component should be registered
        /// </summary>
        public Type Service { get; set; }

        /// <summary>
        /// Enum that provides values that will be used for the service registration
        /// </summary>
        public Type EnumType { get; set; }

        /// <summary>
        /// A list of values to exlude from registration
        /// </summary>
        public object[] Exlusions { get; set; }

        /// <summary>
        /// Gets a value indicating whether [externally owned].
        /// </summary>
        public bool ExternallyOwned { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [single instance].
        /// </summary>
        public bool SingleInstance { get; set; } = false;

        /// <summary>
        /// Register all components that use this attribute
        /// </summary>
        public static void Register(ContainerBuilder builder,
            IDictionary<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registrationCache,
            params Assembly[] assemblies)
        {
            var keyedComponents = assemblies.SelectMany(x => x.GetTypes())
                .Select(x => new
                {
                    Component = x,
                    Attributes = GetAttributes(x),
                })
                .Where(x => x.Attributes.Any());

            foreach (var item in keyedComponents)
            {
                foreach (KeyAllComponentAttribute attribute in item.Attributes)
                {
                    foreach (var key in Enum.GetValues(attribute.EnumType))
                    {
                        if (attribute.Exlusions != null && !attribute.Exlusions.Contains(key))
                        {
                            var registration = ComponentAttribute.GetRegistrationBuilder(item.Component, builder, registrationCache)
                      .Keyed(key, attribute.Service)
                      .OrderByMetadata(attribute.Service);

                            if (attribute.ExternallyOwned)
                            {
                                registration.ExternallyOwned();
                            }

                            if (attribute.SingleInstance)
                            {
                                registration.SingleInstance();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get a component attribute from the type
        /// </summary>
        private static IEnumerable<KeyAllComponentAttribute> GetAttributes(Type type) =>
            type.GetCustomAttributes(false).OfType<KeyAllComponentAttribute>();
    }
}
