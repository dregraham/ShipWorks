using System.Collections.Generic;
using System.Xml;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.OpenShip
{
    public class FedExOpenShipXmlReader : XmlReader
    {
        // the decorated XmlReader
        private XmlReader wrappedReader;

        // A list of namespaces from which we want to translate
        private static readonly Dictionary<string, string> namespaceMap =
            new Dictionary<string, string> {
                { "http://fedex.com/ws/openship/v7", "http://fedex.com/ws/ship/v15" }
            };

        // Will be used to replace tag names with the new tag name. If null, tag name is ignored.
        private static readonly Dictionary<string, string> nodeReplacements =
            new Dictionary<string, string>
            {
                { "CreatePendingShipmentReply", "ProcessShipmentReply" },
                { "AsynchronousProcessingResults", null },
                { "Index", null }
            };

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExOpenShipXmlReader(XmlReader originalReader)
        {
            wrappedReader = originalReader;
        }

        /// <summary>
        /// Gets the local name of the current node.
        /// </summary>
        public override string LocalName
        {
            get
            {
                string shipReplacement;
                if (!nodeReplacements.TryGetValue(wrappedReader.LocalName, out shipReplacement))
                {
                    shipReplacement = wrappedReader.LocalName;
                }

                if (shipReplacement==null)
                {
                    wrappedReader.Skip();
                    
                    // recursive call as this node was skipped.
                    return LocalName;
                }

                return shipReplacement;
            }
        }

        /// <summary>
        /// Gets the namespace URI (as defined in the W3C Namespace specification) of the node on which the reader is positioned.
        /// </summary>
        public override string NamespaceURI
        {
            get
            {
                return GetShipNamespace(wrappedReader.NamespaceURI);
            }
        }

        /// <summary>
        /// Moves to the attribute with the specified <see cref="P:System.Xml.XmlReader.LocalName" /> and <see cref="P:System.Xml.XmlReader.NamespaceURI" />.
        /// </summary>
        public override bool MoveToAttribute(string name, string ns)
        {
            return wrappedReader.MoveToAttribute(name, GetShipNamespace(ns));
        }

        /// <summary>
        /// Gets the value of the attribute with the specified <see cref="P:System.Xml.XmlReader.LocalName" /> and <see cref="P:System.Xml.XmlReader.NamespaceURI" />.
        /// </summary>
        public override string GetAttribute(string name, string namespaceURI)
        {
            return wrappedReader.GetAttribute(name, GetShipNamespace(namespaceURI));
        }

        /// <summary>
        /// Translates an Express1 namespace to Stamps.
        /// </summary>
        private static string GetShipNamespace(string nameSpace)
        {
            if (nameSpace == null)
                return null;

            string shipNameSpace;
            if (namespaceMap.TryGetValue(nameSpace, out shipNameSpace))
            {
                return shipNameSpace;
            }

            return nameSpace;
        }

        #region pass through

        public override string Value
        {
            get { return wrappedReader.Value; }
        }

        public override int AttributeCount
        {
            get { return wrappedReader.AttributeCount; }
        }

        public override string BaseURI
        {
            get { return wrappedReader.BaseURI; }
        }

        public override void Close()
        {
            wrappedReader.Close();
        }

        public override int Depth
        {
            get { return wrappedReader.Depth; }
        }

        public override bool EOF
        {
            get { return wrappedReader.EOF; }
        }

        public override string GetAttribute(int i)
        {
            return wrappedReader.GetAttribute(i);
        }

        public override string GetAttribute(string name)
        {
            return wrappedReader.GetAttribute(name);
        }

        public override bool IsEmptyElement
        {
            get { return wrappedReader.IsEmptyElement; }
        }

        public override string LookupNamespace(string prefix)
        {
            return wrappedReader.LookupNamespace(prefix);
        }

        public override bool MoveToAttribute(string name)
        {
            return wrappedReader.MoveToAttribute(name);
        }

        public override bool MoveToElement()
        {
            return wrappedReader.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            return wrappedReader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return wrappedReader.MoveToNextAttribute();
        }

        public override XmlNameTable NameTable
        {
            get { return wrappedReader.NameTable; }
        }

        public override XmlNodeType NodeType
        {
            get { return wrappedReader.NodeType; }
        }

        public override string Prefix
        {
            get { return wrappedReader.Prefix; }
        }

        public override bool Read()
        {
            return wrappedReader.Read();
        }

        public override bool ReadAttributeValue()
        {
            return wrappedReader.ReadAttributeValue();
        }

        public override ReadState ReadState
        {
            get { return wrappedReader.ReadState; }
        }

        public override void ResolveEntity()
        {
            wrappedReader.ResolveEntity();
        }

        #endregion
    }
}