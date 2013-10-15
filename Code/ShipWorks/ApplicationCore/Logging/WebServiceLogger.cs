


	

using System.Xml;
using System.Web.Services.Protocols;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class SwsimV29
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal SwsimV29(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class EwsLabelService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal EwsLabelService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.AccountService
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class ELSServicesService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal ELSServicesService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class CustomerService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal CustomerService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.LabelService
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class EwsLabelService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal EwsLabelService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class RegisterMgrAcctService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal RegisterMgrAcctService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Shipping
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class ShippingService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal ShippingService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class OrderService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal OrderService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Inventory
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class InventoryService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal InventoryService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Admin
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class AdminService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal AdminService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class RegistrationService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal RegistrationService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class RateService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal RateService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class ShipService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal ShipService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.Track
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class TrackService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal TrackService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class PackageMovementInformationService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal PackageMovementInformationService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.AddressValidation
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class AddressValidationService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal AddressValidationService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.Close
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class CloseService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal CloseService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.Infopia.WebServices
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class InfopiaWebService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal InfopiaWebService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.PayPal.WebServices
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class PayPalAPISoapBinding
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal PayPalAPISoapBinding(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.Amazon.WebServices.SellerCentral
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class merchantinterfacedime
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal merchantinterfacedime(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.Ebay.WebServices
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class eBayAPIInterfaceService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal eBayAPIInterfaceService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.WebServices.Oms
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class OMService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal OMService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.AmeriCommerce.WebServices
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class AmeriCommerceDatabaseIO
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal AmeriCommerceDatabaseIO(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.NetworkSolutions.WebServices
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class NetSolEcomService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal NetSolEcomService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.Magento.WebServices
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class MagentoService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal MagentoService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.EquaShip.WebServices
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class EquashipAPI
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal EquashipAPI(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.ThreeDCart.WebServices.CartAdvanced
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class cartAPIAdvanced
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal cartAPIAdvanced(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Stores.Platforms.ThreeDCart.WebServices.Cart
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class cartAPI
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal cartAPI(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Ship
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class ShipService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal ShipService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.PackageMovement
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class PackageMovementInformationService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal PackageMovementInformationService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class RegistrationService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal RegistrationService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.GlobalShipAddress
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class GlobalShipAddressService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal GlobalShipAddressService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.iParcel.WebServices
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class XMLSOAP
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal XMLSOAP(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class CloseService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal CloseService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class RateService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal RateService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Track
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class TrackService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal TrackService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class OpenAccountService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal OpenAccountService(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

namespace ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.Stamps
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class SDCV24Service
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        ApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        internal SDCV24Service(ApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

        /// <summary>
        /// Provides access to the raw soap XML sent and recieved
        /// </summary>
        public WebServiceRawSoap RawSoap
        {
            get { return rawSoap; }
        }
        
        /// <summary>
        /// The log entry being used to log the request and response
        /// </summary>
		public ApiLogEntry ApiLogEntry
        {
			get { return logEntry; }
		}

        /// <summary>
        /// Get the writer for the message
        /// </summary>
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadOutgoingMessage(message);

            return base.GetWriterForMessage(message, bufferSize);
        }

        /// <summary>
        /// Get the response to the message that has been generated
        /// </summary>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            // At this point the message has been completely serialized and ready to be logged
            if (logEntry != null && rawSoap.RequestXml != null)
            {
                logEntry.LogRequest(rawSoap.RequestXml);
            }

			// get the reponse
            System.Net.WebResponse baseResponse = base.GetWebResponse(request);

			// check for Soap the same way the framework does
			if (!IsSoap(baseResponse.ContentType))
			{
				RaiseInvalidSoapException(baseResponse);
			}

			return baseResponse;
        }

        /// <summary>
        /// Get the XmlReader used to read the response message
        /// </summary>
        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            rawSoap.ReadIncomingMessage(message);

            // Response is now ready to be logged
            if (logEntry != null && rawSoap.ResponseXml != null)
            {
                logEntry.LogResponse(rawSoap.ResponseXml);
            }

            return base.GetReaderForMessage(message, bufferSize);
        }

        /// <summary>
        /// Checks the contentType to see if it is one that would indicate a SOAP response.
		/// This was pulled out of a .NET Framework internal class. 
        /// </summary>
		private bool IsSoap(string contentType)
        {
            if (!contentType.StartsWith("text/xml", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return (contentType.StartsWith("application/soap+xml", System.StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

		/// <summary>
        /// Extract the response and raise an exception
        /// </summary>
        private void RaiseInvalidSoapException(System.Net.WebResponse response)
        {
            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    string responseContent = reader.ReadToEnd();

                    // http-specific properties
                    System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
                    string statusDescription = "";

                    System.Net.HttpWebResponse httpWebResponse = response as System.Net.HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        statusCode = httpWebResponse.StatusCode;
                        statusDescription = httpWebResponse.StatusDescription;
                    }

                    throw new Interapptive.Shared.Net.InvalidSoapException(statusCode, statusDescription, responseContent);
                }
            }
        }
    }
}		

