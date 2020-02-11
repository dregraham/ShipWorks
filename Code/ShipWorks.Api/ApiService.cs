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
using ShipWorks.ApplicationCore;
using System.Net;

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
        private ILifetimeScope scope;
        private readonly IShipWorksSession shipWorksSession;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiService(ILifetimeScope scope, IShipWorksSession shipWorksSession)
        {
            this.scope = scope;
            this.shipWorksSession = shipWorksSession;
        }

        /// <summary>
        /// Start the Shipworks Api
        /// </summary>
        public void Start()
        {
            if (server == null && !IsRunning())
            {
                server = WebApp.Start("http://+:8081/", Configuration);
            }
        }

        private bool IsRunning()
        {
            WebRequest request = WebRequest.Create($"http://{Environment.MachineName}/shipworks/api/v1/healthcheck");
            request.Timeout = 2000;

            try
            {
                var response = request.GetResponse();
                var statusCode = ((HttpWebResponse) response).StatusCode;

                return statusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Configures the service. This is called by convention, hense 0 references
        /// </summary>
        [Obfuscation(Exclude = true)]
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

            //configuration.Routes.MapHttpRoute(
            //    "VersionedUrl",
            //    "api/v{apiVersion}/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional },
            //    constraints: new { apiVersion = new ApiVersionRouteConstraint() });
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
