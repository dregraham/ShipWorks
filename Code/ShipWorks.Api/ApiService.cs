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
using ShipWorks.ApplicationCore.ExecutionMode;


namespace ShipWorks.Api
{
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

        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration configuration = new HttpConfiguration();

            appBuilder.UseWebApi(configuration);

            ConfigureApiVersioning(configuration);
           // IoC.RegisterApiControllers(configuration, appBuilder);

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
