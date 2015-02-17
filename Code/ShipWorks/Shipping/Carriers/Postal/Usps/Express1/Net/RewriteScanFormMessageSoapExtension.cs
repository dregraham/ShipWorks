using System;
using System.IO;
using System.Linq;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.Collections;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net
{
    /// <summary>
    /// Rewrites the SCAN form message since Express1 returns an array instead of a string
    /// </summary>
    public class RewriteScanFormMessageSoapExtension : SoapExtension
    {
        private Stream oldStream;
        private Stream newStream;

        /// <summary>
        /// Save the Stream representing the SOAP request or SOAP response into a local memory buffer.
        /// </summary>
        /// <param name="stream">Stream that should be saved</param>
        /// <returns>New stream that should be used by any other readers</returns>
        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        /// <summary>
        /// Gets initialization data from the attribute
        /// </summary>
        /// <param name="methodInfo">Details about the method call</param>
        /// <param name="attribute">Attribute for which this method is being called</param>
        /// <returns>Initialization data</returns>
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return attribute;
        }

        /// <summary>
        /// Gets initialization data from the configuration file
        /// </summary>
        /// <param name="serviceType">Type of web service for which to initialize</param>
        /// <returns></returns>
        public override object GetInitializer(Type serviceType)
        {
            return typeof(RewriteScanFormMessageSoapExtension);
        }

        /// <summary>
        /// Initialize the extension
        /// </summary>
        /// <param name="initializer">Data that should be used for initialization</param>
        public override void Initialize(object initializer)
        {
        }

        /// <summary>
        /// Process the SOAP message
        /// </summary>
        /// <param name="message">Message that should be processed</param>
        public override void ProcessMessage(SoapMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    return;
                case SoapMessageStage.AfterDeserialize:
                    return;
                case SoapMessageStage.AfterSerialize:
                    // Copy the request without any rewriting
                    newStream.Position = 0;
                    Copy(newStream, oldStream);
                    return;
                case SoapMessageStage.BeforeDeserialize:
                    if (message.Url.ToLower().Contains("express1"))
                    {
                        // Rewrite the response
                        ReplaceArrayElements();
                    }
                    else
                    {
                        // We don't want to rewrite if this isn't a call to Express1
                        Copy(oldStream, newStream);
                        newStream.Position = 0;
                    }
                    
                    return;
            }

            throw new InvalidOperationException("invalid stage");
        }

        /// <summary>
        /// Copy one stream into another
        /// </summary>
        /// <param name="from">Source stream</param>
        /// <param name="to">Destination stream</param>
        private static void Copy(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }

        /// <summary>
        /// Replace the two array elements with a space delimited list
        /// </summary>
        public void ReplaceArrayElements()
        {
            // We cannot dispose of xmlReader here because it will close oldStream, which we don't want
            var xmlReader = XmlReader.Create(oldStream);
            XDocument doc = XDocument.Load(xmlReader);

            // Replace the elements that are different between Express1 and Stamps
            ReplaceArrayWithString(doc, "ScanFormIds", "ScanFormId");
            ReplaceArrayWithString(doc, "Urls", "Url");

            // We cannot dispose of xmlWrite here because it will close newStream, which we don't want
            var xmlWriter = XmlWriter.Create(newStream);
            doc.WriteTo(xmlWriter);
            xmlWriter.Flush();
            newStream.Position = 0;
        }

        /// <summary>
        /// Replace the specified element array with a space delimited list of values
        /// </summary>
        /// <param name="doc">XDocument from which to retrieve nodes</param>
        /// <param name="originalElementName">Name of the element that should be replaced</param>
        /// <param name="newElementName">Name of the new space delimited element to add</param>
        private static void ReplaceArrayWithString(XContainer doc, string originalElementName, string newElementName)
        {
            var nodesToReplace = doc.DescendantNodes()
                                 .OfType<XElement>()
                                 .Where(x => x.Name.LocalName == originalElementName)
                                 .ToList();

            foreach (XElement node in nodesToReplace)
            {
                XName elementName = XName.Get(newElementName, node.Name.Namespace.NamespaceName);
                string values = node.Nodes().OfType<XElement>()
                    .Where(x => string.Equals(x.Name.LocalName, "string", StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Value).Combine(" ");

                node.ReplaceWith(new XElement(elementName, values));
            }
        }
    }
}