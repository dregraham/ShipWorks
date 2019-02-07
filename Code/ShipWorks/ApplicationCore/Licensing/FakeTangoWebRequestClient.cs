using System.IO;
using System.Xml;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Client for making Tango web requests
    /// </summary>
    public class FakeTangoWebRequestClient : ITangoWebRequestClient
    {
        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        public GenericResult<string> ProcessRequest(IHttpVariableRequestSubmitter postRequest, string logEntryName, bool collectTelemetry) =>
            GetXmlStringFromFile($"{logEntryName}.txt");

        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        public GenericResult<XmlDocument> ProcessXmlRequest(IHttpVariableRequestSubmitter postRequest, string logEntryName, bool collectTelemetry)
        {
            string rawXml = GetXmlStringFromFile($"{logEntryName}.xml");

            XmlDocument document = new XmlDocument();
            document.LoadXml(rawXml);
            return document;
        }

        /// <summary>
        /// Get an xml string from the given file
        /// </summary>
        private static string GetXmlStringFromFile(string fileName)
        {
            string filePath = InterapptiveOnly.Registry.GetValue(FakeTangoWebClient.CustomizedTangoFilesKeyName, @"C:\Temp");
            string fullFileName = Path.Combine(filePath, fileName);

            try
            {
                using (StreamReader licenseFile = new StreamReader(fullFileName))
                {
                    return licenseFile.ReadToEnd();
                }
            }
            catch (IOException)
            {
                // Fall back to the hard-coded values if there is a problem reading from the
                // license.xml file
                return null;
            }
        }
    }
}
