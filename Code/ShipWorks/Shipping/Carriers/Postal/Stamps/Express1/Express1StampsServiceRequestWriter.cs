using System.Collections.Generic;
using System.Xml;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Decorating XmlWriter to allow ShipWorks to communicate transparently with Stamps and Express1
    /// </summary>
    public class Express1StampsServiceRequestWriter : XmlWriter
    {
        // the writer being decorated
        XmlWriter wrappedWriter;

        // A list of namespaces from which we want to translate
        private static readonly Dictionary<string, string> express1NamespaceMap =
            new Dictionary<string, string> {
                { "http://stamps.com/xml/namespace/2013/05/swsim/swsimv29", "http://www.express1.com/2011/08" }
            };

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1StampsServiceRequestWriter(XmlWriter originalWriter)
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
        /// Writes an attribute under a namespace, but translate from Stamps to Express1.
        /// </summary>
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            wrappedWriter.WriteStartAttribute(prefix, localName, GetExpress1Namespace(ns));
        }

        /// <summary>
        /// Writes a qualified name under a namespace, but translates from Stamps to Express1.
        /// </summary>
        public override void WriteQualifiedName(string localName, string ns)
        {
            wrappedWriter.WriteQualifiedName(localName, GetExpress1Namespace(ns));
        }

        /// <summary>
        /// Writes an element under a namespace, but translate from Stamps to Express1.
        /// </summary>
        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            wrappedWriter.WriteStartElement(prefix, localName, GetExpress1Namespace(ns));
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
