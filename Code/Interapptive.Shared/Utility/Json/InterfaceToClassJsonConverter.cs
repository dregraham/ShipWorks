using System;
using Newtonsoft.Json.Converters;

namespace Interapptive.Shared.Utility.Json
{
    /// <summary>
    /// Converts the interface into the concrete class given
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <typeparam name="TClass">The type of the class.</typeparam>
    /// <seealso cref="Newtonsoft.Json.Converters.CustomCreationConverter{TInterface}" />
    public class InterfaceToClassJsonConverter<TInterface, TClass> : CustomCreationConverter<TInterface>
        where TClass : TInterface, new()
    {
        /// <summary>
        /// Creates an object which will then be populated by the serializer.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// The created object.
        /// </returns>
        public override TInterface Create(Type objectType)
        {
            return new TClass();
        }
    }
}