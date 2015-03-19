using System;
using System.Collections.Generic;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Generic equality comparer that uses a property accessor to test for equality
    /// </summary>
    public class GenericPropertyEqualityComparer<T, TProp> : EqualityComparer<T> where T : class
    {
        private readonly Func<T, TProp> propertyAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericPropertyEqualityComparer(Func<T, TProp> propertyAccessor)
        {
            this.propertyAccessor = propertyAccessor;
        }

        /// <summary>
        /// Test whether the two objects are equal
        /// </summary>
        public override bool Equals(T x, T y)
        {
            return x != null && y != null &&
                   Equals(propertyAccessor(x), propertyAccessor(y));
        }

        /// <summary>
        /// Get the hash code for the property accessor
        /// </summary>
        public override int GetHashCode(T obj)
        {
            return propertyAccessor(obj).GetHashCode();
        }
    }
}