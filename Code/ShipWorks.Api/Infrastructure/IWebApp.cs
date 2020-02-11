using System;
using Owin;

namespace ShipWorks.Api.Infrastructure
{
    /// <summary>
    /// Method used to load, assemble and start a webapp
    /// </summary>
    public interface IWebApp
    {
        /// <summary>
        //  Start a web app using default settings and the given url and entry point. e.g.
        //  Discover the ServerFactory and run at the given url.
        /// </summary>
        /// <returns>An IDisposible instance that can be called to shut down the web app.</returns>
        IDisposable Start(string url, Action<IAppBuilder> startup);
    }
}
