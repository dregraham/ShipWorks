using System.Collections.Generic;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Interface for factory for creating ActionProcessors
    /// </summary>
    public interface IActionProcessorFactory
    {
        /// <summary>
        /// Creates a list a Standard ActionProcessors
        /// </summary>
        IEnumerable<ActionProcessor> CreateStandard();

        /// <summary>
        /// Create an Error ActionProcessor
        /// </summary>
        ActionProcessor CreateError(List<long> queueList);
    }
}