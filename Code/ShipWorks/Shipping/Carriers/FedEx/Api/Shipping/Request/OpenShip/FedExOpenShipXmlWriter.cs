using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Stores.Platforms.Amazon.WebServices.SellerCentral;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.OpenShip
{
    /// <summary>
    /// Overrides ship requests and turns it into an OpenShip request.
    /// </summary>
    public class FedExOpenShipXmlWriter : XmlWriter
    {
        private readonly XmlWriter wrappedWriter;
        private readonly Stack<string> xmlPath = new Stack<string>();

        /// <summary>
        /// Nodes that match the name of the key will be renamed to the value name.
        /// </summary>
        private readonly Dictionary<string, string> elementNameReplacements = new Dictionary<string, string>()
        {
            { "ProcessShipmentRequest", "CreatePendingShipmentRequest" },
            { "DeleteShipmentRequest", "DeletePendingShipmentRequest" },
            { "TrackingId", "TrackingIds" }
        };

        /// <summary>
        /// For nodes that match the name of the key, replace the XML value with the value.
        /// </summary>
        private readonly Dictionary<string, string> valueReplacements = new Dictionary<string, string>()
        {
            {"Major", FedExSettings.OpenShipVersionNumber.ToString()}
        };

        /// <summary>
        /// The namespace in the key will be replaced with the value in the XML.
        /// </summary>
        private readonly Dictionary<string, string> namespaceReplacements = new Dictionary<string, string>()
        {
            {
                string.Format("http://fedex.com/ws/ship/v{0}", FedExSettings.ShipVersionNumber),
                string.Format("http://fedex.com/ws/openship/v{0}", FedExSettings.OpenShipVersionNumber)
            }
        };


        /// <summary>
        /// For nodes whose name matches the key, when the node is closed, the text is returned.
        /// </summary>
        private readonly List<AppendAfter> appendAfterElement = new List<AppendAfter>()
        {
            new AppendAfter() { XmlPath = new List<string>() { "CreatePendingShipmentRequest", "Version" }, StringToAppend = "<Actions>TRANSFER</Actions>" },
            new AppendAfter() { XmlPath = new List<string>() { "PendingShipmentDetail", "EmailAddress", "Recipients", "EmailLabelDetail" }, StringToAppend = "<Role>SHIPMENT_COMPLETOR</Role>" }
        };


        /// <summary>
        /// Nodes that match the name in the list will not be written.
        /// </summary>
        private readonly List<string> elementsNameToOmit = new List<string>()
        {
            "DeletionControl"
        };
        
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExOpenShipXmlWriter(XmlWriter originalWriter)
        {
            wrappedWriter = originalWriter;
        }

        /// <summary>
        /// Writes the specified start tag and associates it with the given namespace and prefix.
        /// 
        /// Replaces namespace if original namespace found in namespaceReplacements 
        /// </summary>
        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            if (elementNameReplacements.ContainsKey(localName))
            {
                localName = elementNameReplacements[localName];
            }

            xmlPath.Push(localName);

            if (!string.IsNullOrEmpty(ns) && namespaceReplacements.ContainsKey(ns))
            {
                ns = namespaceReplacements[ns];
            }

            if (ShouldWrite)
            {
                wrappedWriter.WriteStartElement(prefix, localName, ns);                
            }
        }

        /// <summary>
        /// When overridden in a derived class, closes one element and pops the corresponding namespace scope.
        /// 
        /// Appends text from appendAfterElement
        /// </summary>
        public override void WriteEndElement()
        {
            if (ShouldWrite)
            {
                wrappedWriter.WriteEndElement(); 
            }

            foreach (AppendAfter appendAfter in appendAfterElement.Where(AppendAfterMatches))
            {
                wrappedWriter.WriteRaw(appendAfter.StringToAppend);
            }

            xmlPath.Pop();
        }

        private bool AppendAfterMatches(AppendAfter appendAfter)
        {
            string[] actualXmlPath = xmlPath.ToArray();

            int appendAfterXmlPathCount = appendAfter.XmlPath.Count;
            bool matches = appendAfterXmlPathCount <= actualXmlPath.Count();

            if (matches)
            {
                for (int index = 1; index <= appendAfterXmlPathCount; index++)
                {
                    if (appendAfter.XmlPath[appendAfterXmlPathCount - index] != actualXmlPath[index])
                    {
                        matches = false;
                        break;
                    }
                }
            }
            return matches;
        }

        /// <summary>
        /// Writes raw markup manually from a string.
        /// 
        /// If the current node name is foud in valueReplacements, the value from the valueReplacements is used instead of the parameter.
        /// </summary>
        public override void WriteRaw(string data)
        {
            if (ShouldWrite)
            {
                string currentNode = xmlPath.Peek();

                if (valueReplacements.ContainsKey(currentNode))
                {
                    data = valueReplacements[currentNode];
                }

                wrappedWriter.WriteRaw(data);
            }
        }

        /// <summary>
        /// Writes the given text content when it should.
        /// </summary>
        public override void WriteString(string text)
        {
            if (ShouldWrite)
            {
                wrappedWriter.WriteString(text);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [should write]. This is determined if the current node is in the elementsNameToOmit collection.
        /// </summary>
        private bool ShouldWrite
        {
            get
            {
                return !elementsNameToOmit.Contains(xmlPath.Peek());
            }
        }

        #region pass through

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            wrappedWriter.WriteStartAttribute(prefix, localName, ns);
        }

        public override string LookupPrefix(string ns)
        {
            return wrappedWriter.LookupPrefix(ns);
        }

        public override void Close()
        {
            wrappedWriter.Close();
        }

        public override void Flush()
        {
            wrappedWriter.Flush();
        }

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            wrappedWriter.WriteBase64(buffer, index, count);
        }

        public override void WriteCData(string text)
        {
            wrappedWriter.WriteCData(text);
        }

        public override void WriteCharEntity(char ch)
        {
            wrappedWriter.WriteCharEntity(ch);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            wrappedWriter.WriteChars(buffer, index, count);
        }

        public override void WriteComment(string text)
        {
            wrappedWriter.WriteComment(text);
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            wrappedWriter.WriteDocType(name, pubid, sysid, subset);
        }

        public override void WriteEndAttribute()
        {
            wrappedWriter.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            wrappedWriter.WriteEndDocument();
        }

        public override void WriteEntityRef(string name)
        {
            wrappedWriter.WriteEntityRef(name);
        }

        public override void WriteFullEndElement()
        {
            wrappedWriter.WriteFullEndElement();
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            wrappedWriter.WriteProcessingInstruction(name, text);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            wrappedWriter.WriteRaw(buffer, index, count);
        }

        public override void WriteStartDocument(bool standalone)
        {
            wrappedWriter.WriteStartDocument(standalone);
        }

        public override void WriteStartDocument()
        {
            wrappedWriter.WriteStartDocument();
        }

        public override WriteState WriteState
        {
            get { return wrappedWriter.WriteState; }
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            wrappedWriter.WriteSurrogateCharEntity(lowChar, highChar);
        }

        public override void WriteWhitespace(string ws)
        {
            wrappedWriter.WriteWhitespace(ws);
        }

        #endregion

        class AppendAfter
        {
            public List<string> XmlPath { get; set; }
            public string StringToAppend { get; set; }
        }
    }
}