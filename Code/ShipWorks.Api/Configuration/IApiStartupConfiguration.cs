using System;
using Owin;

namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// Configures the API Service
    /// </summary>
    public interface IApiStartupConfiguration: IDisposable
    {
        /// <summary>
        /// Configures the API Service
        /// </summary>
        void Configuration(IAppBuilder appBuilder);
    }
}