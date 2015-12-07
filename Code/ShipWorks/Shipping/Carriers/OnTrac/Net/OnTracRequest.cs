using System;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared;

namespace ShipWorks.Shipping.Carriers.OnTrac.Net
{
    /// <summary>
    /// Base Class for OnTracRequests
    /// </summary>
    public abstract class OnTracRequest
    {
        static readonly ILog log = LogManager.GetLogger(typeof(OnTracRequest));

        readonly IApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        protected OnTracRequest(OnTracAccountEntity onTracAccount, string actionDescriptionToLog)
            : this(
                onTracAccount.AccountNumber,
                SecureText.Decrypt(onTracAccount.Password, onTracAccount.AccountNumber.ToString()),
                actionDescriptionToLog)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected OnTracRequest(long onTracAccountNumber, string onTracPassword, string actionDescriptionToLog)
            : this(onTracAccountNumber, onTracPassword,new LogEntryFactory(), ApiLogSource.OnTrac, actionDescriptionToLog, LogActionType.Other)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        protected OnTracRequest(long onTracAccountNumber, string onTracPassword, ILogEntryFactory logEntryFactory, ApiLogSource logSource, string actionDescriptionToLog, LogActionType logActionType)
        {
            //THIS CONSTRUCTOR MUST BE CALLED IN THE CONSTRUCTION CHAIN

            logEntry = logEntryFactory.GetLogEntry(logSource, actionDescriptionToLog, logActionType);

            AccountNumber = onTracAccountNumber;
            OnTracPassword = onTracPassword;

            BaseUrlUsedToCallOnTrac = UseTestServer
                          ? "https://www.shipontrac.net/OnTracTestWebServices/OnTracServices.svc/v2/"
                          : "https://www.shipontrac.net/OnTracWebServices/OnTracServices.svc/v2/";
        }

        /// <summary>
        /// Ontrac Account Number 
        /// </summary>
        protected long AccountNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// OnTrac Password
        /// </summary>
        protected string OnTracPassword
        {
            get;
            private set;
        }

        /// <summary>
        /// OnTrac base URL
        /// </summary>
        protected string BaseUrlUsedToCallOnTrac
        {
            get;
            private set;
        }
        /// <summary>
        /// Determine live/test server to use
        /// </summary>
        public static bool UseTestServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("OnTracTestServer", false);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("OnTracTestServer", value);
            }
        }

        /// <summary>
        /// Makes the actual call to OnTrac
        /// </summary>
        /// <typeparam name="T">Return Type to serailze the response to</typeparam>
        /// <param name="request">The request to serialize and send to OnTrac</param>
        protected T ExecuteLoggedRequest<T>(HttpRequestSubmitter request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", "null variable in ExecuteLoggedRequest");
            }
            try
            {
                logEntry.LogRequest(request);

                using (IHttpResponseReader onTracResponse = request.GetResponse())
                {
                    string onTracResponseText = onTracResponse.ReadResult();

                    logEntry.LogResponse(onTracResponseText);

                    CheckForErrors(onTracResponseText);

                    return SerializationUtility.DeserializeFromXml<T>(onTracResponseText);
                }
            }
            catch (XmlException ex)
            {
                log.Error("Cannot deserialize OnTrac Response", ex);
                throw new OnTracException("OnTrac returned an invalid response.");
            }
            catch (Exception ex)
            {
                log.Error("Error in ExecuteLoggedRequest", ex);

                throw WebHelper.TranslateWebException(ex, typeof(OnTracException));
            }
        }

        /// <summary>
        /// Check the given response text for "Error" elements
        /// </summary>
        private void CheckForErrors(string response)
        {
            XDocument xDocument = XDocument.Parse(response);

            // Find the first error, if any
            XElement xError = xDocument.Descendants("Error").FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Value));
            if (xError != null)
            {
                throw new OnTracApiErrorException(xError.Value);
            }
        }
    }
}