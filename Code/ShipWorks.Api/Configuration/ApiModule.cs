using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Module = Autofac.Module;

namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// Autofac module for registering controllers with Autofac
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class ApiModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>Note that the ContainerBuilder parameter is unique to this module.</remarks>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterWebApiModelBinderProvider();
            
            builder.RegisterType<HttpConfiguration>().AsSelf();
        }
    }
}
