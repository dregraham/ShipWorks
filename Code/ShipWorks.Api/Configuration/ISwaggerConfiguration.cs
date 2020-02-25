using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Application;

namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// Configures Swagger API documentation
    /// </summary>
    public interface ISwaggerConfiguration
    {
        /// <summary>
        /// Attach generation of Swagger API docs to the configuration
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void Configure(HttpConfiguration configuration);
    }
}
