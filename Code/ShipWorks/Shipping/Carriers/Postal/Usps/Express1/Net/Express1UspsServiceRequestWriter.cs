using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net
{
    /// <summary>
    /// Decorating XmlWriter to allow ShipWorks to communicate transparently with Stamps and Express1
    /// </summary>
    public class Express1UspsServiceRequestWriter : XmlWriter
    {
        // the writer being decorated
        XmlWriter wrappedWriter;

        // A list of namespaces from which we want to translate
        private static readonly Dictionary<string, string> express1NamespaceMap =
            new Dictionary<string, string> {
                { "http://stamps.com/xml/namespace/2013/05/swsim/swsimv29", "http://www.express1.com/2011/08" }
            };

        // A list of local names we want to translate
        private static readonly Dictionary<string, string> express1LocalNameMap =
            new Dictionary<string, string> {
                { "Authenticator", "Item" }
            };

        // A list of extra attributes to add
        private static readonly Dictionary<string, XAttribute> express1ExtraAttributes =
            new Dictionary<string, XAttribute> {
                { "Item", new XAttribute(XName.Get("type", "http://www.w3.org/2001/XMLSchema-instance"), "xsd:string") }
            };

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsServiceRequestWriter(XmlWriter originalWriter)
        {
            this.wrappedWriter = originalWriter;
        }

        /// <summary>
        /// Translates a Stamps namespace to Express1.
        /// </summary>
        private static string GetExpress1Namespace(string stampsNamespace)
        {
            if(stampsNamespace == null)
                return null;

            string express1Namespace;
            if(express1NamespaceMap.TryGetValue(stampsNamespace, out express1Namespace))
                return express1Namespace;

            return stampsNamespace;
        }

        /// <summary>
        /// Translates a Stamps namespace to Express1.
        /// </summary>
        private static string GetExpress1LocalName(string stampsLocalName)
        {
            if(stampsLocalName == null)
                return null;

            string express1LocalName;
            if(express1LocalNameMap.TryGetValue(stampsLocalName, out express1LocalName))
                return express1LocalName;

            return stampsLocalName;
        }

        /// <summary>
        /// Translates a Stamps namespace to Express1.
        /// </summary>
        private static XAttribute GetExpress1ExtraAttribute(string express1LocalName)
        {
            if(express1LocalName == null)
                return null;

            XAttribute attribute;
            express1ExtraAttributes.TryGetValue(express1LocalName, out attribute);
            return attribute;
        }

        /// <summary>
        /// Writes an attribute under a namespace, but translate from Stamps to Express1.
        /// </summary>
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            wrappedWriter.WriteStartAttribute(prefix, GetExpress1LocalName(localName), GetExpress1Namespace(ns));
        }

        /// <summary>
        /// Writes a qualified name under a namespace, but translates from Stamps to Express1.
        /// </summary>
        public override void WriteQualifiedName(string localName, string ns)
        {
            wrappedWriter.WriteQualifiedName(GetExpress1LocalName(localName), GetExpress1Namespace(ns));
        }

        /// <summary>
        /// Writes an element under a namespace, but translate from Stamps to Express1.
        /// </summary>
        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            localName = GetExpress1LocalName(localName);

            wrappedWriter.WriteStartElement(prefix, localName, GetExpress1Namespace(ns));

            var extraAttribute = GetExpress1ExtraAttribute(localName);
            if(extraAttribute != null)
            {
                wrappedWriter.WriteAttributeString(extraAttribute.Name.LocalName, extraAttribute.Name.NamespaceName, extraAttribute.Value);
            }
        }

        /// <summary>
        /// Looks up the namespace prefix for a translated Stamps to Express1 namespace.
        /// </summary>
        public override string LookupPrefix(string ns)
        {
            return wrappedWriter.LookupPrefix(GetExpress1Namespace(ns));
        }

        #region pass through

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

        public override void WriteEndElement()
        {
            wrappedWriter.WriteEndElement();
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

        public override void WriteRaw(string data)
        {
            wrappedWriter.WriteRaw(data);
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
            get
            {
                return wrappedWriter.WriteState;
            }
        }

        public override void WriteString(string text)
        {
            wrappedWriter.WriteString(text);
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
    }
}
