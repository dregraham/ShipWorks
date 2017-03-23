using System.Collections.Generic;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Interface for factory for creating ActionProcessors
    /// </summary>
    public interface IActionProcessorFactory
    {
        /// <summary>
        /// Create a Standard ActionProcessor
        /// </summary>
        ActionProcessor CreateStandard();

        /// <summary>
        /// Create an Error ActionProcessor
        /// </summary>
        ActionProcessor CreateError(List<long> queueList);
    }
}