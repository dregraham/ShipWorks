using System;
using System.IO;
using System.Web.Http;
using Interapptive.Shared.ComponentRegistration;
using Swashbuckle.Application;

namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// An local web server leveraging Owin infrastructure that can be
    /// self-hosted within ShipWorks.
    /// </summary>
    [Component(SingleInstance = true)]
    public class SwaggerConfiguration : ISwaggerConfiguration
    {
        /// <summary>
        /// Attach generation of Swagger API docs to the configuration
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void Configure(HttpConfiguration configuration)
        {
            // See https://github.com/domaindrivendev/Swashbuckle for more info
            configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "ShipWorks REST API V1")
                        .Description("A REST API for programmatically interacting with the ShipWorks application. The API can be used for getting data in and out of ShipWorks via third party applications/services.")
                        .Contact(contactBuilder => contactBuilder
                            .Name("ShipWorks")
                            .Email("support@shipworks.com")
                            .Url("http://support.shipworks.com"));

                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string commentsFileName = "ShipWorks.Api.xml";
                    string commentsFile = Path.Combine(baseDirectory, commentsFileName);

                    c.IncludeXmlComments(commentsFile);
                })
                .EnableSwaggerUi(c =>
                {
                    c.DocExpansion(DocExpansion.List);
                    c.EnableDiscoveryUrlSelector();
                });
        }
    }
}
