using System;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Attribute applied to ApiLogSource enum values to indicate they are not logged for general users.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    sealed class ApiPrivateLogSourceAttribute : Attribute
    {
    }
}
