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
            {"Major", FedExSettings.OpenShipVersionNumber}
        };

        /// <summary>
        /// For nodes whose name matches the key, when the node is closed, the text is returned.
        /// </summary>
        private readonly List<AppendAfter> appendAfterElement = new List<AppendAfter>()
        {
            new AppendAfter() { XmlPath = new List<string>() { "Version", "CreatePendingShipmentRequest" }, StringToAppend = "<Actions>TRANSFER</Actions>" },
            new AppendAfter() { XmlPath = new List<string>() { "EmailAddress", "Recipients", "EmailLabelDetail", "PendingShipmentDetail" }, StringToAppend = "<Role>SHIPMENT_COMPLETOR</Role>" }
        };


        /// <summary>
        /// Nodes that match the name in the list will not be written.
        /// </summary>
        private readonly List<string> elementsToOmit = new List<string>()
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

        /// <summary>
        /// If the current path matches appendAfter.XmlPath, return true. else false.
        /// 
        /// The path in appendAfter.XmlPath may be a partial path.
        /// </summary>
        private bool AppendAfterMatches(AppendAfter appendAfter)
        {
            string[] actualXmlPath = xmlPath.ToArray();

            int appendAfterXmlPathCount = appendAfter.XmlPath.Count;
            
            // Make sure the expected path is at least as long as the actual path.
            bool matches = appendAfterXmlPathCount <= actualXmlPath.Count();

            if (matches)
            {
                for (int index = 0; index < appendAfterXmlPathCount; index++)
                {
                    if (appendAfter.XmlPath[index] != actualXmlPath[index])
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
        /// Gets a value indicating whether [should write]. This is determined if the current node is in the elementsToOmit collection.
        /// </summary>
        private bool ShouldWrite
        {
            get
            {
                return !elementsToOmit.Contains(xmlPath.Peek());
            }
        }

        #region pass through

        /// <summary>
        /// When overridden in a derived class, writes the start of an attribute with the specified prefix, local name, and namespace URI.
        /// </summary>
        /// <param name="prefix">The namespace prefix of the attribute.</param>
        /// <param name="localName">The local name of the attribute.</param>
        /// <param name="ns">The namespace URI for the attribute.</param>
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            wrappedWriter.WriteStartAttribute(prefix, localName, ns);
        }

        /// <summary>
        /// When overridden in a derived class, returns the closest prefix defined in the current namespace scope for the namespace URI.
        /// </summary>
        /// <param name="ns">The namespace URI whose prefix you want to find.</param>
        /// <returns>
        /// The matching prefix or null if no matching namespace URI is found in the current scope.
        /// </returns>
        public override string LookupPrefix(string ns)
        {
            return wrappedWriter.LookupPrefix(ns);
        }

        /// <summary>
        /// When overridden in a derived class, closes this stream and the underlying stream.
        /// </summary>
        public override void Close()
        {
            wrappedWriter.Close();
        }

        /// <summary>
        /// When overridden in a derived class, flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
        /// </summary>
        public override void Flush()
        {
            wrappedWriter.Flush();
        }

        /// <summary>
        /// When overridden in a derived class, encodes the specified binary bytes as Base64 and writes out the resulting text.
        /// </summary>
        /// <param name="buffer">Byte array to encode.</param>
        /// <param name="index">The position in the buffer indicating the start of the bytes to write.</param>
        /// <param name="count">The number of bytes to write.</param>
        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            wrappedWriter.WriteBase64(buffer, index, count);
        }

        /// <summary>
        /// When overridden in a derived class, writes out a &lt;![CDATA[...]]&gt; block containing the specified text.
        /// </summary>
        /// <param name="text">The text to place inside the CDATA block.</param>
        public override void WriteCData(string text)
        {
            wrappedWriter.WriteCData(text);
        }

        /// <summary>
        /// When overridden in a derived class, forces the generation of a character entity for the specified Unicode character value.
        /// </summary>
        /// <param name="ch">The Unicode character for which to generate a character entity.</param>
        public override void WriteCharEntity(char ch)
        {
            wrappedWriter.WriteCharEntity(ch);
        }

        /// <summary>
        /// When overridden in a derived class, writes text one buffer at a time.
        /// </summary>
        /// <param name="buffer">Character array containing the text to write.</param>
        /// <param name="index">The position in the buffer indicating the start of the text to write.</param>
        /// <param name="count">The number of characters to write.</param>
        public override void WriteChars(char[] buffer, int index, int count)
        {
            wrappedWriter.WriteChars(buffer, index, count);
        }

        /// <summary>
        /// When overridden in a derived class, writes out a comment &lt;!--...--&gt; containing the specified text.
        /// </summary>
        /// <param name="text">Text to place inside the comment.</param>
        public override void WriteComment(string text)
        {
            wrappedWriter.WriteComment(text);
        }

        /// <summary>
        /// When overridden in a derived class, writes the DOCTYPE declaration with the specified name and optional attributes.
        /// </summary>
        /// <param name="name">The name of the DOCTYPE. This must be non-empty.</param>
        /// <param name="pubid">If non-null it also writes PUBLIC "pubid" "sysid" where <paramref name="pubid" /> and <paramref name="sysid" /> are replaced with the value of the given arguments.</param>
        /// <param name="sysid">If <paramref name="pubid" /> is null and <paramref name="sysid" /> is non-null it writes SYSTEM "sysid" where <paramref name="sysid" /> is replaced with the value of this argument.</param>
        /// <param name="subset">If non-null it writes [subset] where subset is replaced with the value of this argument.</param>
        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            wrappedWriter.WriteDocType(name, pubid, sysid, subset);
        }

        /// <summary>
        /// When overridden in a derived class, closes the previous <see cref="M:System.Xml.XmlWriter.WriteStartAttribute(System.String,System.String)" /> call.
        /// </summary>
        public override void WriteEndAttribute()
        {
            wrappedWriter.WriteEndAttribute();
        }

        /// <summary>
        /// When overridden in a derived class, closes any open elements or attributes and puts the writer back in the Start state.
        /// </summary>
        public override void WriteEndDocument()
        {
            wrappedWriter.WriteEndDocument();
        }

        /// <summary>
        /// When overridden in a derived class, writes out an entity reference as &amp;name;.
        /// </summary>
        /// <param name="name">The name of the entity reference.</param>
        public override void WriteEntityRef(string name)
        {
            wrappedWriter.WriteEntityRef(name);
        }

        /// <summary>
        /// When overridden in a derived class, closes one element and pops the corresponding namespace scope.
        /// </summary>
        public override void WriteFullEndElement()
        {
            wrappedWriter.WriteFullEndElement();
        }

        /// <summary>
        /// When overridden in a derived class, writes out a processing instruction with a space between the name and text as follows: &lt;?name text?&gt;.
        /// </summary>
        /// <param name="name">The name of the processing instruction.</param>
        /// <param name="text">The text to include in the processing instruction.</param>
        public override void WriteProcessingInstruction(string name, string text)
        {
            wrappedWriter.WriteProcessingInstruction(name, text);
        }

        /// <summary>
        /// When overridden in a derived class, writes raw markup manually from a character buffer.
        /// </summary>
        /// <param name="buffer">Character array containing the text to write.</param>
        /// <param name="index">The position within the buffer indicating the start of the text to write.</param>
        /// <param name="count">The number of characters to write.</param>
        public override void WriteRaw(char[] buffer, int index, int count)
        {
            wrappedWriter.WriteRaw(buffer, index, count);
        }

        /// <summary>
        /// When overridden in a derived class, writes the XML declaration with the version "1.0" and the standalone attribute.
        /// </summary>
        /// <param name="standalone">If true, it writes "standalone=yes"; if false, it writes "standalone=no".</param>
        public override void WriteStartDocument(bool standalone)
        {
            wrappedWriter.WriteStartDocument(standalone);
        }

        /// <summary>
        /// When overridden in a derived class, writes the XML declaration with the version "1.0".
        /// </summary>
        public override void WriteStartDocument()
        {
            wrappedWriter.WriteStartDocument();
        }

        /// <summary>
        /// When overridden in a derived class, gets the state of the writer.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Xml.WriteState" /> values.</returns>
        public override WriteState WriteState
        {
            get { return wrappedWriter.WriteState; }
        }

        /// <summary>
        /// When overridden in a derived class, generates and writes the surrogate character entity for the surrogate character pair.
        /// </summary>
        /// <param name="lowChar">The low surrogate. This must be a value between 0xDC00 and 0xDFFF.</param>
        /// <param name="highChar">The high surrogate. This must be a value between 0xD800 and 0xDBFF.</param>
        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            wrappedWriter.WriteSurrogateCharEntity(lowChar, highChar);
        }

        /// <summary>
        /// When overridden in a derived class, writes out the given white space.
        /// </summary>
        /// <param name="ws">The string of white space characters.</param>
        public override void WriteWhitespace(string ws)
        {
            wrappedWriter.WriteWhitespace(ws);
        }

        #endregion

        /// <summary>
        /// Defines items to append a string after. XmlPath may be a partial path.
        /// </summary>
        class AppendAfter
        {
            /// <summary>
            /// Gets or sets the XML path to match in the document.
            /// </summary>
            public List<string> XmlPath
            {
                get; set;
            }

            /// <summary>
            /// String to append after element that matches the XmlPath
            /// </summary>
            public string StringToAppend
            {
                get; set; 
            }
        }
    }
}