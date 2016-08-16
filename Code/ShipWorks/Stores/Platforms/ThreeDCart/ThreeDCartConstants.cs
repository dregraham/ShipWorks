using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Constants for accessing ThreeDCart API
    /// </summary>
    public static class ThreeDCartConstants
    {
        // 100 is the max that ThreeDCart allows for all batch queries, but using 10 to get orders into the db faster.
        public const int MaxResultsToReturn = 10;

        public const int OrderStatusNotCompleted = 7;
    }
}
