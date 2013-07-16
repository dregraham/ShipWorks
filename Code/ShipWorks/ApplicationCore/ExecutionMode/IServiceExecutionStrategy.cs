

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// Executes a ShipWorks service.
    /// </summary>
    public interface IServiceExecutionStrategy
    {
        /// <summary>
        /// Runs the service.
        /// </summary>
        void Run();

        /// <summary>
        /// Signals a running instance, if any, that it should shut down.
        /// </summary>
        void Stop();
    }
}
