using System;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Service Name resolver
    /// </summary>
    public interface IServiceName
    {
        /// <summary>
        /// Resolves the service name
        /// </summary>
        string Resolve();

        /// <summary>
        /// Gets the instance id of the service based on registry value. Throws if it cannot find the registry value.
        /// </summary>
        /// <returns></returns>
        Guid GetInstanceID();
    }
}
