﻿using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Stores.Orders.Combine
{
    /// <summary>
    /// EqualityComparer for PersonAdapters used by the OrderCombine function
    /// </summary>
    [Component]
    public class CombineOrderAddressComparer : ICombineOrderAddressComparer
    {
        /// <summary>
        /// Are the two person adapters equal
        /// </summary>
        public bool Equals(PersonAdapter x, PersonAdapter y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.FirstName == y.FirstName &&
                x.LastName == y.LastName &&
                x.Street1 == y.Street1 &&
                x.Street2 == y.Street2 &&
                x.Street3 == y.Street3 &&
                x.City == y.City &&
                x.StateProvCode == y.StateProvCode &&
                x.PostalCode == y.PostalCode &&
                x.CountryCode == y.CountryCode;
        }

        /// <summary>
        /// Get the hash code
        /// </summary>
        public int GetHashCode(PersonAdapter obj) => obj.GetHashCode();
    }
}