using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Extension methods for Guids
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Return a string representation of a Warehouse/Hub Guid
        /// </summary>
        public static string ToWarehouseString(this Guid? value)
        {
            return value.HasValue ? value.ToString().ToUpperInvariant() : null;
        }

        /// <summary>
        /// Return a string representation of a Warehouse/Hub Guid
        /// </summary>
        public static string ToWarehouseString(this Guid value)
        {
            return value.ToString().ToUpperInvariant();
        }

        /// <summary>
        /// Return true if value and string representation of a Warehouse/Hub Guid are equal.
        /// </summary>
        public static bool MatchesWarehouseString(this Guid? value, string warehouseGuid)
        {
            return value.ToWarehouseString().Equals(warehouseGuid);
        }

        /// <summary>
        /// Return true if value and string representation of a Warehouse/Hub Guid are equal.
        /// </summary>
        public static bool MatchesWarehouseString(this Guid? value, Guid? warehouseGuid)
        {
            return value.ToWarehouseString().Equals(warehouseGuid.ToWarehouseString());
        }

        /// <summary>
        /// Return true if value and string representation of a Warehouse/Hub Guid are equal.
        /// </summary>
        public static bool MatchesWarehouseString(this Guid value, string warehouseGuid)
        {
            return value.ToWarehouseString().Equals(warehouseGuid);
        }
    }
}
