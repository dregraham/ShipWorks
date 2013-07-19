using ShipWorks.ApplicationCore.Services;
using System;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// Hosts a ShipWorks service.
    /// </summary>
    public interface IServiceHost
    {
        /// <summary>
        /// Gets the service being hosted.
        /// </summary>
        ShipWorksServiceBase Service { get; }

        /// <summary>
        /// Runs the service.
        /// </summary>
        void Run();

        /// <summary>
        /// Signals a running instance, if any, that it should shut down.
        /// </summary>
        void Stop();

        /// <summary>
        /// Invoked when the process is terminating due to an unhandled exception.
        /// </summary>
        /// <param name="exception">The unhandled exception.</param>
        void OnUnhandledException(Exception exception);
    }
}
