using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.IO;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Utility class for extracting the raw soap XML from web service requests and responses
    /// </summary>
    public class WebServiceRawSoap
    {
        string requestXml;
        string responseXml;

        static MethodInfo methodSetStream = null;

        /// <summary>
        /// Outgoing stream to intercept the outgoing request SOAP
        /// </summary>
        class OutgoingStream : MemoryStream
        {
            WebServiceRawSoap owner;

            StreamReader reader;
            StreamWriter writer;

            /// <summary>
            /// Constructor
            /// </summary>
            public OutgoingStream(WebServiceRawSoap owner, Stream realStream, Encoding encoding)
            {
                this.owner = owner;

                reader = new StreamReader(this, encoding);
                writer = new StreamWriter(realStream, encoding);
            }

            /// <summary>
            /// Override close to know when writing to the stream is done
            /// </summary>
            public override void Flush()
            {
                // Flush and go back to the beginning
                base.Flush();
                Seek(0, SeekOrigin.Begin);

                // Read everything that was written
                string fragment = reader.ReadToEnd();
                
                // Add the content to the request XML read so far
                owner.requestXml += fragment;

                // Go back to the beginning, and write it all to the real stream
                writer.Write(fragment);
                writer.Flush();
                
                // Clear it out for the next batch
                SetLength(0);
            }
        }

        /// <summary>
        /// Static constructror
        /// </summary>
        static WebServiceRawSoap()
        {
            methodSetStream = typeof(SoapClientMessage).GetMethod("SetStream", BindingFlags.Instance | BindingFlags.NonPublic);
            if (methodSetStream == null)
            {
                throw new InvalidOperationException("Could not get SetStream method for SoapClientMessage.");
            }
        }

        /// <summary>
        /// Get the soap xml sent as the request
        /// </summary>
        public string RequestXml
        {
            get { return requestXml; }
        }

        /// <summary>
        /// Get the soap xml recieved as a response
        /// </summary>
        public string ResponseXml
        {
            get { return responseXml; }
        }

        /// <summary>
        /// Read the XML from the outgoing message before it gets sent off
        /// </summary>
        public void ReadOutgoingMessage(SoapClientMessage message)
        {
            ReplaceMessageStream(message, new OutgoingStream(this, message.Stream, GetEncoding(message)));
        }

        /// <summary>
        /// Read the XML from the incoming message before it gets serialized
        /// </summary>
        public void ReadIncomingMessage(SoapClientMessage message)
        {
            Encoding encoding = GetEncoding(message);

            // Read the raw response
            using (StreamReader reader = new StreamReader(message.Stream, encoding))
            {
                responseXml = reader.ReadToEnd();
            }

            // Some SOAP endpoints use XML1.1 (or just don't care) and generate responses with whitepace characters not valid in XML1.0 (which .NET uses to deserialize responses).  We clean those
            // out here ensuring a clean response.
            string cleanXml = XmlUtility.StripInvalidXmlCharacters(responseXml);

            // We've wasted the original stream, we need to replace it with one that the SOAP class can read
            MemoryStream replaceStream = new MemoryStream(cleanXml.Length);
            StreamWriter writer = new StreamWriter(replaceStream, encoding);
            writer.Write(cleanXml);
            writer.Flush();

            // Move the stream back the beginning
            replaceStream.Seek(0, SeekOrigin.Begin);

            // Set this as the message stream
            ReplaceMessageStream(message, replaceStream);
        }

        /// <summary>
        /// Update the given message to use the specified stream as its source
        /// </summary>
        private void ReplaceMessageStream(SoapClientMessage message, Stream replaceStream)
        {
            methodSetStream.Invoke(message, new object[] { replaceStream });
        }

        /// <summary>
        /// Get the encoding to use for the specified message.  This is basically stolen from the SoapHttpClientProtocol class via Reflector
        /// </summary>
        private Encoding GetEncoding(SoapClientMessage message)
        {
            Encoding encoding = null;

            // See if we can get the encoding form the charset
            string charset = GetContentTypeParameter(message.ContentType, "charset");
            if (!string.IsNullOrEmpty(charset))
            {
                encoding = Encoding.GetEncoding(charset);
            }

            if (encoding != null)
            {
                // the default UTF8 instance writes the byte order mark, which we don't want 
                // (for infopia at least, need to test others)
                if (encoding == Encoding.UTF8)
                {
                    encoding = new UTF8Encoding(false); 
                }

                return encoding;
            }

            if (message.SoapVersion == SoapProtocolVersion.Soap12 &&
                string.Compare(GetMediaType(message.ContentType), "application", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return null;
            }
            else
            {
                return new ASCIIEncoding();
            }
        }

        /// <summary>
        /// This method stolen via reflector from System.Web.Services.Protocols.ContentType.GetParameter
        /// </summary>
        private static string GetContentTypeParameter(string contentType, string paramName)
        {
            // Magento is including multiple content types separated by commas, which is why the comma is included here
            string[] strArray = contentType.Split(new char[] { ';', ',' });
            for (int i = 1; i < strArray.Length; i++)
            {
                string strA = strArray[i].TrimStart(null);
                if (string.Compare(strA, 0, paramName, 0, paramName.Length, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    int index = strA.IndexOf('=', paramName.Length);
                    if (index >= 0)
                    {
                        return strA.Substring(index + 1).Trim(new char[] { ' ', '\'', '"', '\t' });
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// This method stolen via reflector from System.Web.Services.Protocols.ContentType.GetMediaType
        /// </summary>
        private static string GetMediaType(string contentType)
        {
            string str = GetContentTypeBase(contentType);
            int index = str.IndexOf('/');
            if (index >= 0)
            {
                return str.Substring(0, index);
            }
            return str;
        }

        /// <summary>
        /// This method stolen via reflector from System.Web.Services.Protocols.ContentType.GetBase
        /// </summary>
        internal static string GetContentTypeBase(string contentType)
        {
            int index = contentType.IndexOf(';');
            if (index >= 0)
            {
                return contentType.Substring(0, index);
            }
            return contentType;
        }
    }
}
