using System;
using System.Web.Http;
using System.Web.Http.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Interapptive.Shared.ComponentRegistration;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using Owin;

namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// Configures a webservice
    /// </summary>
    [Component]
    public class ApiStartupConfiguration : IApiStartupConfiguration
    {
        private readonly ILifetimeScope scope;
        private readonly HttpConfiguration configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiStartupConfiguration(ILifetimeScope scope)
        {
            this.scope = scope.BeginLifetimeScope();
            configuration = scope.Resolve<HttpConfiguration>();
        }

        /// <summary>
        /// Configures the service. This is called by convention, hence 0 references
        /// </summary>
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseWebApi(configuration);

            ConfigureApiVersioning(configuration);
            RegisterApiControllers(configuration, appBuilder);
        }

        /// <summary>
        /// Registers API controllers with Autofac
        /// </summary>
        private void RegisterApiControllers(HttpConfiguration configuration, IAppBuilder appBuilder)
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
        /// Dispose the scope
        /// </summary>
        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
