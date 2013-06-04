using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// An IEqualityComparer for the ASIN property of an AmazonOrderItemEntity.
    /// </summary>
    public class AmazonOrderItemASINComparer : IEqualityComparer<AmazonOrderItemEntity>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T"/> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(AmazonOrderItemEntity x, AmazonOrderItemEntity y)
        {
            return string.Compare(x.ASIN, y.ASIN, true) == 0;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        ///   </exception>
        public int GetHashCode(AmazonOrderItemEntity obj)
        {
            return obj.ASIN.GetHashCode();
        }
    }
}
