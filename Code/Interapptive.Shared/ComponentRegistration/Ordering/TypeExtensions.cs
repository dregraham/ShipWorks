// From: https://github.com/mthamil/Autofac.Extras.Ordering
// License: Apache 2.0

using System;

namespace Interapptive.Shared.ComponentRegistration.Ordering
{
    /// <summary>
    /// Contains extension methods for <see cref="Type"/>.
    /// </summary>
    static class TypeExtensions
    {
        /// <summary>
        /// Determines whether a type is a closed instance of a generic type definition.
        /// </summary>
        public static bool IsInstanceOfGenericType(this Type type, Type genericTypeDefinition)
        {
            return type.IsGenericType &&
                !type.ContainsGenericParameters &&
                type.GetGenericTypeDefinition() == genericTypeDefinition;
        }
    }
}
