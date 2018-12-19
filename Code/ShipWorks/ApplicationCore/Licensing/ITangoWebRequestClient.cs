using System.Xml;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Client for making Tango web requests
    /// </summary>
    public interface ITangoWebRequestClient
    {
        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        GenericResult<string> ProcessRequest(IHttpVariableRequestSubmitter postRequest, string logEntryName, bool collectTelemetry);

        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        GenericResult<XmlDocument> ProcessXmlRequest(IHttpVariableRequestSubmitter postRequest, string logEntryName, bool collectTelemetry);
    }
}