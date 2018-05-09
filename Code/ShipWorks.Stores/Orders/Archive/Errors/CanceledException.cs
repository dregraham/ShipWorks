using System;

namespace ShipWorks.Stores.Orders.Archive.Errors
{
    /// <summary>
    /// Represents a canceled operation
    /// </summary>
    /// <remarks>
    /// This is an exception so that it can be used as Task failures
    /// </remarks>
    public class CanceledException : Exception
    {

    }
}