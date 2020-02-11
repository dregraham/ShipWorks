using System;
using Interapptive.Shared.ComponentRegistration;
using Microsoft.Owin.Hosting;
using Owin;

namespace ShipWorks.Api.Infrastructure
{
    /// <summary>
    /// Used to load, assemble and start a webapp
    /// </summary>
    [Component]
    public class WebAppWrapper : IWebApp
    {
        /// <summary>
        //  Start a web app using default settings and the given url and entry point. e.g.
        //  Discover the ServerFactory and run at the given url.
        /// </summary>
        /// <returns>An IDisposible instance that can be called to shut down the web app.</returns>
        public IDisposable Start(string url, Action<IAppBuilder> startup) => 
            WebApp.Start(url, startup);
    }
}
