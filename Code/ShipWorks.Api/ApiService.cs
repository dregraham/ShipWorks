using System;
using System.IO;
using System.Web.Http;
using System.Web.Http.Routing;
using Interapptive.Shared.ComponentRegistration;
using Microsoft.Owin.Hosting;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using Owin;
using ShipWorks.ApplicationCore;
using Autofac.Integration.WebApi;

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

        /// <summary>
        /// Start the Shipworks Api
        /// </summary>
        public void Start()
        {
            if (server == null)
            {
                server = WebApp.Start<ApiService>("http://+:8080/");
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
            // Definately want to figure out another way to do this
            var scope = IoC.UnsafeGlobalLifetimeScope;
            
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

                server = null;
            }
        }
    }
}
