


	

using System.Xml;
using System.Web.Services.Protocols;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.Licensing.Activation
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class Activation
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public Activation(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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

namespace ShipWorks.Shipping.Carriers.Postal.Usps.WebServices
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class SwsimV49
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public SwsimV49(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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

namespace ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class SwsimV36
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public SwsimV36(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public EwsLabelService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ELSServicesService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public EwsLabelService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterMgrAcctService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public InventoryService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public AdminService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegistrationService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public RateService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public TrackService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public PackageMovementInformationService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddressValidationService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public CloseService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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

namespace ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress
{
    /// <summary>
    /// Partial class for the webservices generated class, to help with logging
    /// </summary>
    partial class GlobalShipAddressService
    {
        WebServiceRawSoap rawSoap = new WebServiceRawSoap();
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public GlobalShipAddressService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaWebService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalAPISoapBinding(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public merchantinterfacedime(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public eBayAPIInterfaceService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public OMService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceDatabaseIO(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetSolEcomService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public cartAPIAdvanced(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public cartAPI(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public XMLSOAP(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
        IApiLogEntry logEntry;
		bool onlyLogOnMagicKeys = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenAccountService(IApiLogEntry logEntry)
            : this()
        {
            this.logEntry = logEntry;
        }

		/// <summary>
        /// Only log error result.
        /// </summary>
		public bool OnlyLogOnMagicKeys 
		{
			get { return onlyLogOnMagicKeys; }
			set { onlyLogOnMagicKeys = value; }
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
		public IApiLogEntry ApiLogEntry
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
            if (logEntry != null && rawSoap.RequestXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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
            if (logEntry != null && rawSoap.ResponseXml != null && (InterapptiveOnly.MagicKeysDown || !OnlyLogOnMagicKeys))
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

