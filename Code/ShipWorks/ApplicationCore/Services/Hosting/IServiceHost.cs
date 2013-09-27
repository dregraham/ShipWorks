using System;
using ShipWorks.ApplicationCore.Crashes;


namespace ShipWorks.ApplicationCore.Services.Hosting
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
        /// Intended to handle a service crash to perform any cleanup/recovery actions.
        /// </summary>
        /// <param name="serviceCrash">The number of times we've crashed and tried to recover so far.</param>
        void HandleServiceCrash(int recoveryCount);
    }
}
