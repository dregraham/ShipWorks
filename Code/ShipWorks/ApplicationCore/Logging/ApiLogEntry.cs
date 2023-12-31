using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rebex.Mail;
using RestSharp;
using ShipWorks.ApplicationCore.Crashes;
using Formatting = Newtonsoft.Json.Formatting;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Class for logging request/response from the various API's that ShipWorks
    /// works with.
    /// </summary>
    public class ApiLogEntry : IApiLogEntry
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ApiLogEntry));

        // The API that is doing the logging
        ApiLogSource source;

        // Controls how encryption is handled
        ApiLogEncryption encryption = ApiLogEncryption.Default;

        // The name of the log entry. Used as a base name for result file.
        readonly string name;

        // The entry number of this object
        int lognumber = -1;

        // Tracks how many log entries have been created for the session.
        static int logcount = 0;

        // For private log source encryption
        static readonly byte[] cryptoKey = new byte[] { 138, 93, 185, 133, 67, 44, 244, 240, 16, 54, 134, 138, 120, 144, 76, 213, 141, 89, 88, 153, 107, 76, 73, 73, 148, 17, 198, 118, 61, 30, 100, 146 };
        static readonly byte[] cryptoIV = new byte[] { 232, 28, 7, 50, 134, 234, 13, 195, 111, 182, 111, 224, 92, 44, 70, 237 };

        private static readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// Static constructor
        /// </summary>
        static ApiLogEntry()
        {
            // If debug, indent json.  Otherwise don't indent
            serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Debugger.IsAttached ? Formatting.Indented : Formatting.None
            };
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApiLogEntry(ApiLogSource source, string name)
        {
            this.source = source;
            this.name = name;
        }

        /// <summary>
        /// Contols how the log output will be encrypted
        /// </summary>
        public ApiLogEncryption Encryption
        {
            get { return encryption; }
            set { encryption = value; }
        }

        /// <summary>
        /// Log the ShipWorks request content.
        /// </summary>
        public void LogRequest(string xml)
        {
            WriteLog(xml, ApiLogCategory.Request);
        }

        /// <summary>
        /// Log the request XmlDocument content
        /// </summary>
        public void LogRequest(XmlDocument xmlDocument)
        {
            LogRequest(xmlDocument.OuterXml);
        }

        /// <summary>
        /// Log the text content request
        /// </summary>
        public void LogRequest(string text, string fileExtension)
        {
            WriteLog(text, fileExtension, ApiLogCategory.Request, null);
        }

        /// <summary>
        /// Log the contents of the given HttpRequestSubmitter
        /// </summary>
        public void LogRequest(IHttpRequestSubmitter request)
        {
            HttpJsonVariableRequestSubmitter json = request as HttpJsonVariableRequestSubmitter;
            if (json != null)
            {
                LogHttpVariableRequest(json, EncryptJsonPassword(json.RequestBody));
                return;
            }

            // Look for specific derived instance
            HttpVariableRequestSubmitter variable = request as HttpVariableRequestSubmitter;
            if (variable != null)
            {
                LogHttpVariableRequest(variable, string.Empty);
                return;
            }

            // We also know how to log any Post-based request
            if (request.Verb != HttpVerb.Get)
            {
                LogHttpPostRequest(request);
                return;
            }

            throw new InvalidOperationException("Unhandled HttpRequestSubmitter derivative.");
        }

        /// <summary>
        /// Log an IRestRequest
        /// </summary>
        public void LogRequest(IRestRequest request, IRestClient client, string extension)
        {
            var requestToLog = new
            {
                resource = request.Resource,
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                method = request.Method.ToString(),
                uri = client.BuildUri(request),
            };

            var toLog = request.JsonSerializer != null ? 
                request.JsonSerializer.Serialize(requestToLog) : 
                JsonConvert.SerializeObject(requestToLog, serializerSettings);

            WriteLog(toLog, extension, ApiLogCategory.Request, null);
        }

        /// <summary>
        /// Log an IRestResponse
        /// </summary>
        public void LogResponse(IRestResponse response, string extension)
        {
            string toLog = string.Empty;

            try
            {                
                var responseToLogAsJson = new
                {
                    statusCode = response.StatusCode,
                    content = string.IsNullOrWhiteSpace(response.Content) ? default(JObject) : JObject.Parse(response.Content),
                    headers = response.Headers,
                    errorMessage = response.ErrorMessage,
                };

                toLog = JsonConvert.SerializeObject(responseToLogAsJson, serializerSettings);
            }
            catch
            {
                var responseToLog = new
                {
                    statusCode = response.StatusCode,
                    content = response.Content,
                    headers = response.Headers,
                    errorMessage = response.ErrorMessage,
                };

                toLog = JsonConvert.SerializeObject(responseToLog, serializerSettings);
            }

            WriteLog(toLog, extension, ApiLogCategory.Response, null);
        }

        /// <summary>
        /// Given a JsonRequest, look for a password element and encrypt the value
        /// </summary>
        private string EncryptJsonPassword(string jsonRequestBody)
        {
            if (!string.IsNullOrEmpty(jsonRequestBody))
            {
                try
                {
                    JObject json = JObject.Parse(jsonRequestBody);
                    if (json["password"] != null)
                    {
                        json["password"] = SecureText.Encrypt((string) json["password"], "password");
                    }
                    return json.ToString();
                }
                catch (Exception ex)
                {
                    log.Error($"Error parsing jsonRequestBody for ApiLog: {jsonRequestBody}", ex);
                }
            }

            return jsonRequestBody;
        }

        /// <summary>
        /// Log the specified Binary Post request
        /// </summary>
        private void LogHttpPostRequest(IHttpRequestSubmitter request)
        {
            if (request.Verb == HttpVerb.Get)
            {
                throw new InvalidOperationException("Should not be trying to log a GET request as POST.");
            }

            using (StringWriter writer = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(writer);

                xmlWriter.WriteStartElement(request.Verb.ToString());
                xmlWriter.WriteElementString("Url", request.Uri.ToString());
                xmlWriter.WriteElementString("ContentType", request.ContentType);
                xmlWriter.WriteElementString("Data", "See supplemental request file.");
                xmlWriter.WriteEndElement();

                // output the log
                WriteLog(writer.ToString(), ApiLogCategory.Request);

                // Default to bin
                string extension = "bin";

                // If it contains xml anywhere
                if (request.ContentType.IndexOf("xml", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    extension = "xml";
                }
                else if (request.ContentType.IndexOf("json", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    extension = "txt";
                }

                WriteLog(request.GetPostContent(), ApiLogCategory.RequestSupplement, "RawData", extension);
            }
        }

        /// <summary>
        /// Log the specified post request
        /// </summary>
        private void LogHttpVariableRequest(HttpVariableRequestSubmitter request, string body)
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(writer);

                string method = request.Verb.ToString();
                xmlWriter.WriteStartElement(method);

                xmlWriter.WriteElementString("Url", request.Uri.ToString());

                foreach (HttpVariable variable in request.Variables)
                {
                    string value = variable.Value;
                    if (string.Compare(variable.Name, "password", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        value = SecureText.Encrypt(value, "password");
                    }

                    xmlWriter.WriteStartElement("Parameter");
                    xmlWriter.WriteElementString("Name", variable.Name);
                    xmlWriter.WriteElementString("Value", value);
                    xmlWriter.WriteEndElement();
                }

                if (!string.IsNullOrEmpty(body))
                {
                    xmlWriter.WriteElementString("Body", body);
                }

                xmlWriter.WriteEndElement();

                WriteLog(writer.ToString(), ApiLogCategory.Request);
            }
        }

        /// <summary>
        /// Log the request as the given mail message
        /// </summary>
        public void LogRequest(MailMessage mailMessage)
        {
            WriteLog(mailMessage, ApiLogCategory.Request);
        }

        /// <summary>
        /// Log the API's response to ShipWorks
        /// </summary>
        public void LogResponse(string xml)
        {
            WriteLog(xml, ApiLogCategory.Response);
        }

        /// <summary>
        /// Logs the response.
        /// </summary>
        public void LogResponse(byte[] data, string extension)
        {
            WriteLog(data, ApiLogCategory.Response, null, extension);
        }

        /// <summary>
        /// Log the API's response to ShipWorks
        /// </summary>
        public void LogResponse(string text, string fileExtension)
        {
            WriteLog(text, fileExtension, ApiLogCategory.Response, null);
        }

        /// <summary>
        /// Log the response XmlDocument
        /// </summary>
        public void LogResponse(XmlDocument xmlDocument)
        {
            LogResponse(xmlDocument.OuterXml);
        }

        /// <summary>
        /// Log the response exception
        /// </summary>
        public void LogResponse(Exception ex)
        {
            StringBuilder result = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(result, new XmlWriterSettings { CheckCharacters = false }))
            {
                var xRoot = new XElement("Exception", new XCData(CrashSubmitter.GetExceptionDetail(ex)));

                xRoot.WriteTo(writer);
            }

            WriteLog(result.ToString(), ApiLogCategory.Response);
        }

        /// <summary>
        /// Logs the response.
        /// </summary>
        internal void LogResponse(XPathDocument xPathDocument)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(xPathDocument.CreateNavigator().ReadSubtree());

            LogResponse(doc);
        }

        /// <summary>
        /// Log supplemental request data
        /// </summary>
        public void LogRequestSupplement(string xml, string supplementName)
        {
            WriteLog(xml, "xml", ApiLogCategory.RequestSupplement, supplementName);
        }

        /// <summary>
        /// Log supplemental request data
        /// </summary>
        public void LogRequestSupplement(FileInfo fileInfo, string supplementName)
        {
            WriteLog(fileInfo, ApiLogCategory.RequestSupplement, supplementName);
        }

        /// <summary>
        /// Log supplement binary request data
        /// </summary>
        public void LogRequestSupplement(byte[] supplementData, string supplementName, string extension)
        {
            WriteLog(supplementData, ApiLogCategory.RequestSupplement, supplementName, extension);
        }

        /// <summary>
        /// Log supplemental request data
        /// </summary>
        public void LogResponseSupplement(string xml, string supplementName)
        {
            WriteLog(xml, "xml", ApiLogCategory.ResponseSupplement, supplementName);
        }

        /// <summary>
        /// Write the XML string to the disk.
        /// </summary>
        private void WriteLog(string xml, ApiLogCategory category)
        {
            WriteLog(xml, "xml", category, null);
        }

        /// <summary>
        /// Write the XML string to the disk.
        /// </summary>
        private void WriteLog(string textToLog, string fileExtension, ApiLogCategory category, string supplementName)
        {
            if (!LogSession.IsApiLogSourceEnabled(source))
            {
                return;
            }

            string logfile = GetNextLogFile(category, supplementName, fileExtension);

            using (Stream stream = CreateOutputStream(logfile))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(textToLog);
                }
            }
        }

        /// <summary>
        /// Write the mail message to the disk.
        /// </summary>
        private void WriteLog(MailMessage mailMessage, ApiLogCategory category)
        {
            if (!LogSession.IsApiLogSourceEnabled(source))
            {
                return;
            }

            string logfile = GetNextLogFile(category, null, "eml");

            using (Stream stream = CreateOutputStream(logfile))
            {
                mailMessage.Save(stream);
            }
        }

        /// <summary>
        /// Write byte array to the disk
        /// </summary>
        private void WriteLog(byte[] data, ApiLogCategory category, string supplementName, string extension)
        {
            if (!LogSession.IsApiLogSourceEnabled(source))
            {
                return;
            }

            string logFile = GetNextLogFile(category, supplementName, extension);

            using (Stream stream = CreateOutputStream(logFile))
            {
                stream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// Write the mail message to the disk.
        /// </summary>
        private void WriteLog(FileInfo fileInfo, ApiLogCategory category, string supplementName)
        {
            if (!LogSession.IsApiLogSourceEnabled(source))
            {
                return;
            }

            string logfile = GetNextLogFile(category, supplementName, fileInfo.Extension);

            using (FileStream sourceStream = fileInfo.OpenRead())
            {
                using (Stream targetStream = CreateOutputStream(logfile))
                {
                    sourceStream.CopyTo(targetStream);
                }
            }
        }

        /// <summary>
        /// Returns the name of the file that should be logged to.
        /// </summary>
        private string GetNextLogFile(ApiLogCategory category, string supplementName, string extension)
        {
            Debug.Assert(!extension.StartsWith(".", StringComparison.Ordinal), "extension should not begin with '.'");

            EnsureLogNumber();

            string sourceName = source.ToString();

            string logpath = Path.Combine(LogSession.LogFolder, sourceName);

            string categoryName = (category == ApiLogCategory.Response || category == ApiLogCategory.ResponseSupplement) ?
                "Response" : "Request";

            string supplementText = "";

            // See if we need supplement text
            if (category == ApiLogCategory.RequestSupplement || category == ApiLogCategory.ResponseSupplement)
            {
                if (string.IsNullOrEmpty(supplementName))
                {
                    throw new InvalidOperationException("Must specify a supplement name for a supplemental log.");
                }

                supplementText = string.Format(" - {0}", supplementName);
            }

            // Build the logname, without the number
            string logname = string.Format("{0} - ({1}){2}.{3}",
                name,
                categoryName,
                supplementText,
                extension);

            string filename = string.Format("{0:0000} - {1}",
                lognumber,
                logname);

            string logfile = Path.Combine(logpath, filename);

            log.InfoFormat("Logfile {0}: {1}", sourceName, filename);
            log.DebugFormat("Logfile {0}: {1}", sourceName, "\"file://" + logfile + "\"");

            Directory.CreateDirectory(logpath);

            return logfile;
        }

        /// <summary>
        /// Create the stream to be used for writing to the given target file.
        /// </summary>
        private Stream CreateOutputStream(string logfile)
        {
            // Create the core stream for file writing
            FileStream fileStream = new FileStream(logfile, FileMode.Create, FileAccess.Write, FileShare.Read);

            if (LogSession.IsApiLogSourceEncrypted(source) || encryption == ApiLogEncryption.Encrypted)
            {
                Rijndael cryptoAlgo = Rijndael.Create();

                CryptoStream cryptoStream = new CryptoStream(
                    fileStream,
                    cryptoAlgo.CreateEncryptor(cryptoKey, cryptoIV),
                    CryptoStreamMode.Write);

                return cryptoStream;
            }
            else
            {
                return fileStream;
            }
        }

        /// <summary>
        /// Ensure the log number for this log entry has been created
        /// </summary>
        private void EnsureLogNumber()
        {
            if (lognumber < 0)
            {
                lognumber = Interlocked.Increment(ref logcount);
            }
        }
    }
}
