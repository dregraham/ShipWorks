using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.OpenShip
{
    /// <summary>
    /// Overrides ship requests and turns it into an OpenShip request.
    /// </summary>
    public class FedExOpenShipXmlWriter : XmlWriter
    {
        private readonly XmlWriter wrappedWriter;
        private readonly Stack<string> xmlPath = new Stack<string>();

        private readonly Dictionary<string, string> nodeReplacements;
        private readonly Dictionary<string, string> valueReplacements;
        private readonly Dictionary<string, string> nameSpaceReplacements;
        private readonly Dictionary<List<string>, string> appendAfter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExOpenShipXmlWriter"/> class.
        /// </summary>
        public FedExOpenShipXmlWriter(XmlWriter originalWriter)
        {
            nodeReplacements = new Dictionary<string, string>();
            nodeReplacements.Add("ProcessShipmentRequest", "CreatePendingShipmentRequest");

            valueReplacements = new Dictionary<string, string>();
            valueReplacements.Add("Major", "7");

            nameSpaceReplacements = new Dictionary<string, string>();
            nameSpaceReplacements.Add("http://fedex.com/ws/ship/v15", "http://fedex.com/ws/openship/v7");

            // The key to the dictionary is a list of tag names. If, going backwards through the tags, the tags match the xml path, the value will be added.
            appendAfter = new Dictionary<List<string>, string>();
            appendAfter.Add(new List<string>() { "Version" }, "<Actions>TRANSFER</Actions>");
            appendAfter.Add(new List<string>() { "EmailAddress", "Recipients", "EmailLabelDetail", "PendingShipmentDetail" }, "<Role>SHIPMENT_COMPLETOR</Role>");

            wrappedWriter = originalWriter;
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified start tag and associates it with the given namespace and prefix.
        /// 
        /// Replaces namespace if original namespace found in nameSpaceReplacements 
        /// </summary>
        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            if (nodeReplacements.ContainsKey(localName))
            {
                localName = nodeReplacements[localName];
            }

            xmlPath.Push(localName);

            if (!string.IsNullOrEmpty(ns) && nameSpaceReplacements.ContainsKey(ns))
            {
                ns = nameSpaceReplacements[ns];
            }

            wrappedWriter.WriteStartElement(prefix, localName, ns);
        }

        /// <summary>
        /// When overridden in a derived class, closes one element and pops the corresponding namespace scope.
        /// 
        /// Appends text from appendAfter
        /// </summary>
        public override void WriteEndElement()
        {
            wrappedWriter.WriteEndElement();

            string[] xmlPathArray = xmlPath.ToArray();

            appendAfter
                .ToList()
                .Where(specifiedTags => !specifiedTags.Key.Where((tagToFind, tagIndex) => xmlPathArray[tagIndex] != tagToFind).Any())
                .Select(toAppend => toAppend.Value)
                .ToList()
                .ForEach(value => wrappedWriter.WriteRaw(value));

            xmlPath.Pop();
        }

        /// <summary>
        /// Writes raw markup manually from a string.
        /// 
        /// If the current node name is foud in valueReplacements, the value from the valueReplacements is used instead of the parameter.
        /// </summary>
        public override void WriteRaw(string data)
        {
            string currentNode = xmlPath.Peek();

            if (valueReplacements.ContainsKey(currentNode))
            {
                data = valueReplacements[currentNode];
            }

            wrappedWriter.WriteRaw(data);
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