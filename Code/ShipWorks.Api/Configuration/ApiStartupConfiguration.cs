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
    [Component(SingleInstance = true)]
    public class ApiStartupConfiguration : IApiStartupConfiguration
    {
        private readonly ILifetimeScope scope;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiStartupConfiguration(ILifetimeScope scope)
        {
            this.scope = scope;
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
    }
}
