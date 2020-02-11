using System;
using System.Web.Http;
using System.Web.Http.Routing;
using Interapptive.Shared.ComponentRegistration;
using Microsoft.Owin.Hosting;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using Owin;
using Autofac.Integration.WebApi;
using Autofac;
using System.Reflection;
using System.Net;
using log4net;

namespace ShipWorks.Api
{
    /// <summary> 
    /// An local web server leveraging Owin infrastructure that can be
    /// self-hosted within ShipWorks.
    /// </summary>
    [Component(SingleInstance = true)]
    public class ApiService : IApiService
    {
        private IDisposable server;
        private bool isDisposing;
        private readonly ILog log;
        private readonly ILifetimeScope scope;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ApiService(ILifetimeScope scope, Func<Type, ILog> loggerFactory)
        {
            log = loggerFactory(typeof(ApiService));
            this.scope = scope;
        }

        /// <summary>
        /// Start the Shipworks Api
        /// </summary>
        public void Start()
        {
            if (server == null && !IsRunning())
            {
                try
                {
                    server = WebApp.Start("http://+:8081/", Configuration);
                }
                catch (Exception ex)
                {
                    log.Debug("Exception while starting ShipWorks Api", ex);
                }
            }
        }

        /// <summary>
        /// Check to see if there is an ApiSrvice running
        /// </summary>
        /// <returns></returns>
        private bool IsRunning()
        {
            WebRequest request = WebRequest.Create($"http://{Environment.MachineName}/shipworks/api/v1/healthcheck");
            request.Timeout = 2000;

            try
            {
                HttpStatusCode statusCode = ((HttpWebResponse) request.GetResponse()).StatusCode;

                log.Debug($"Api healthcheck response status {statusCode}");

                return statusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                log.Debug("Exception while performing ShipWorks api healthcheck", ex);
                return false;
            }
        }

        /// <summary>
        /// Configures the service. This is called by convention, hense 0 references
        /// </summary>
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration configuration = new HttpConfiguration();

            appBuilder.UseWebApi(configuration);

            ConfigureApiVersioning(configuration);
            RegisterApiControllers(configuration, appBuilder);
        }

        /// <summary>
        /// Registers API controllers with Autofac
        /// </summary>
        public void RegisterApiControllers(HttpConfiguration configuration, IAppBuilder appBuilder)
        {
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(scope);
            appBuilder.UseAutofacMiddleware(scope);
            
            appBuilder.UseAutofacWebApi(configuration);
        }

        /// <summary>
        /// Attach API versioning to the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        private void ConfigureApiVersioning(HttpConfiguration configuration)
        {
            DefaultInlineConstraintResolver constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof(ApiVersionRouteConstraint)
                }
            };

            configuration.MapHttpAttributeRoutes(constraintResolver);
            configuration.AddApiVersioning(options => options.DefaultApiVersion = new ApiVersion(1, 0));
            configuration.AddApiVersioning();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (!isDisposing)
            {
                isDisposing = true;
                server?.Dispose();
                scope?.Dispose();

                server = null;
            }
        }
    }
}
