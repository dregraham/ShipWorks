using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.Services3.Security;
using Interapptive.Shared.Net;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Messaging;
using ShipWorks.ApplicationCore.Logging;
using log4net;
using System.Xml;

namespace ShipWorks.Stores.Platforms.Amazon
{
    static class AmazonWse
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonWse));

        /// <summary>
        /// WSE client output filter for adding the message signature directives
        /// </summary>
        class AmazonSecurityFilter : SendSecurityFilter
        {
            // the certificate being used to sign the request
            ClientCertificate clientCertificate;

            /// <summary>
            /// Construct the custom security filter
            /// </summary>
            public AmazonSecurityFilter(ClientCertificate certificate, AmazonPolicyAssertion parentAssertion)
                : base(parentAssertion.ClientActor, true)
            {
                clientCertificate = certificate;
            }

            /// <summary>
            /// Attach the necessary elements and tokens to sign the message for Amazon
            /// </summary>
            public override void SecureMessage(SoapEnvelope envelope, Security security)
            {
                Microsoft.Web.Services3.Security.Tokens.X509SecurityToken securityToken = new Microsoft.Web.Services3.Security.Tokens.X509SecurityToken(clientCertificate.X509Certificate);

                try
                {
                    MessageSignature signature = new MessageSignature(securityToken);

                    // assign the token and new security elements to the message
                    security.Elements.Add(signature);
                    security.Tokens.Add(securityToken);
                }
                catch (ArgumentException ex)
                {
                    // Means its expired or postdated token
                    if (ex.Message.StartsWith("WSE511"))
                    {
                        log.Warn("Rethrowing WSE511 as SoapException", ex);

                        // Client.AuthFailure is what Amazon itself uses to indicate that a token is expired, so we set the code manually so the amazon error handlers display that message
                        throw new System.Web.Services.Protocols.SoapException(ex.Message, new XmlQualifiedName("Client.AuthFailure"));
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// WSE filter for logging requests and responses from the target webservice
        /// </summary>
        class LoggingFilter : SoapFilter
        {
            // The type of log that will be written
            ApiLogCategory loggingType = ApiLogCategory.Request;

            // The log entry the filter should log to
            ApiLogEntry logEntry;

            /// <summary>
            /// Constructor to specify the type of log to be written
            /// </summary>
            public LoggingFilter(ApiLogEntry logEntry, ApiLogCategory loggingType)
            {
                this.loggingType = loggingType;
                this.logEntry = logEntry;
            }

            /// <summary>
            /// The ShipWorks ApiLogEntry associated with the filter
            /// </summary>
            public override T GetBehavior<T>()
            {
                if (typeof(T) == typeof(ApiLogEntry))
                {
                    return (T) (object) logEntry;
                }

                return base.GetBehavior<T>();
            }

            /// <summary>
            /// Process the Soap message in the pipeline
            /// </summary>
            public override SoapFilterResult ProcessMessage(SoapEnvelope envelope)
            {
                if (loggingType == ApiLogCategory.Request)
                {
                    logEntry.LogRequest(envelope);
                }
                else if (loggingType == ApiLogCategory.Response)
                {
                    // log the response
                    logEntry.LogResponse(envelope);
                }

                return SoapFilterResult.Continue;
            }
        }

        /// <summary>
        /// Customk assertion that inserts our Logging information
        /// </summary>
        class LoggingAssertion : PolicyAssertion
        {
            ApiLogEntry logEntry;

            /// <summary>
            /// Constructor
            /// </summary>
            public LoggingAssertion(ApiLogEntry logEntry)
            {
                this.logEntry = logEntry;
            }

            /// <summary>
            /// Create the incoming filter for the soap response
            /// </summary>
            public override SoapFilter CreateClientInputFilter(FilterCreationContext context)
            {
                return new LoggingFilter(logEntry, ApiLogCategory.Response);
            }

            /// <summary>
            /// Create the outgoing filter for the soap requests
            /// </summary>
            public override SoapFilter CreateClientOutputFilter(FilterCreationContext context)
            {
                return new LoggingFilter(logEntry, ApiLogCategory.Request);
            }

            /// <summary>
            /// Unused for client filters
            /// </summary>
            public override SoapFilter CreateServiceInputFilter(FilterCreationContext context)
            {
                return null;
            }

            /// <summary>
            /// Unused for client filters
            /// </summary>
            public override SoapFilter CreateServiceOutputFilter(FilterCreationContext context)
            {
                return null;
            }
        }

        /// <summary>
        /// WSE security policy assertion for adding our own outgoing filter
        /// </summary>
        class AmazonPolicyAssertion : SecurityPolicyAssertion
        {
            // the client certificate that will be attached to the message
            ClientCertificate clientCertificate;

            /// <summary>
            /// Constructor
            /// </summary>
            public AmazonPolicyAssertion(ClientCertificate certificate)
            {
                clientCertificate = certificate;
            }

            /// <summary>
            /// Creates the custom output filter needed for Amazon
            /// </summary>
            public override SoapFilter CreateClientOutputFilter(FilterCreationContext context)
            {
                // create output filter here
                return new AmazonSecurityFilter(clientCertificate, this);
            }

            #region Unused Filter creation methods

            /// <summary>
            /// Creates an input filter, not needed here
            /// </summary>
            public override SoapFilter CreateClientInputFilter(FilterCreationContext context)
            {
                return null;
            }

            /// <summary>
            /// Creates a service-side input filter
            /// </summary>
            public override SoapFilter CreateServiceInputFilter(FilterCreationContext context)
            {
                return null;
            }

            /// <summary>
            /// Creates a service-side output filter
            /// </summary>
            public override SoapFilter CreateServiceOutputFilter(FilterCreationContext context)
            {
                return null;
            }

            #endregion
        }

        /// <summary>
        /// Applies the necessary security policy to allow Amazon communication
        /// </summary>
        public static void ConfigureWse(SoapClient client, ClientCertificate certificate, ApiLogEntry logEntry)
        {
            // attach the certificate and logging policy 
            Policy policy = new Policy(new AmazonPolicyAssertion(certificate), new LoggingAssertion(logEntry));
            client.SetPolicy(policy);
        }
    }
}
