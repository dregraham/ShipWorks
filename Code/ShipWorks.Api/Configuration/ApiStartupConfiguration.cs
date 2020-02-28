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
        private ILifetimeScope scope;
        private HttpConfiguration httpConfiguration;
        private readonly ISwaggerConfiguration swaggerConfiguration;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiStartupConfiguration(ILifetimeScope scope, HttpConfiguration httpConfiguration, ISwaggerConfiguration swaggerConfiguration)
        {
            this.scope = scope.BeginLifetimeScope();
            this.httpConfiguration = httpConfiguration;
            this.swaggerConfiguration = swaggerConfiguration;
        }

        /// <summary>
        /// Configures the service. This is called by convention, hence 0 references
        /// </summary>
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseWebApi(httpConfiguration);

            ConfigureApiVersioning();
            RegisterApiControllers(appBuilder);

            swaggerConfiguration.Configure(httpConfiguration);
        }

        /// <summary>
        /// Registers API controllers with Autofac
        /// </summary>
        private void RegisterApiControllers(IAppBuilder appBuilder)
        {
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(scope);

            appBuilder.UseAutofacMiddleware(scope);
            appBuilder.UseAutofacWebApi(httpConfiguration);
        }

        /// <summary>
        /// Attach API versioning to the configuration.
        /// </summary>
        private void ConfigureApiVersioning()
        {
            DefaultInlineConstraintResolver constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof(ApiVersionRouteConstraint)
                }
            };

            httpConfiguration.MapHttpAttributeRoutes(constraintResolver);
            httpConfiguration.AddApiVersioning(options => options.DefaultApiVersion = new ApiVersion(1, 0));
            httpConfiguration.AddApiVersioning();
        }

        /// <summary>
        /// Dispose the scope
        /// </summary>
        public void Dispose()
        {
            scope?.Dispose();
            scope = null;

            httpConfiguration?.Dispose();
            httpConfiguration = null;
        }
    }
}
