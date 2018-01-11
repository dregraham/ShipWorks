using System.Collections.Generic;
using System.Xml;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.OpenShip
{
    /// <summary>
    /// Reads the open ship response from FedEx and returns a FedEx ship response.
    /// </summary>
    public class FedExOpenShipXmlReader : XmlReader
    {
        // the decorated XmlReader
        private readonly XmlReader wrappedReader;

        /// <summary>
        /// Nodes that match the name of the key will be renamed to the value name.
        /// </summary>
        private static readonly Dictionary<string, string> elementNameReplacements = new Dictionary<string, string>
        {
            { "CreatePendingShipmentReply", "ProcessShipmentReply" },
            { "DeletePendingShipmentReply", "ShipmentReply" }
        };

        /// <summary>
        /// Elements not to return
        /// </summary>
        private static readonly Dictionary<string, string> elementsToOmit = new Dictionary<string, string>()
        {
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
                string wrappedReaderName = wrappedReader.LocalName;

                // replace the element name if it is in the dictionary.
                if (elementNameReplacements.ContainsKey(wrappedReaderName))
                {
                    return elementNameReplacements[wrappedReaderName];
                }

                // if in elementsToOmit, then skip the element in the wrapped reader and recursively all this property to get the next name.
                if (elementsToOmit.ContainsKey(wrappedReaderName))
                {
                    wrappedReader.Skip();

                    // recursive call as this node was skipped.
                    return LocalName;
                }

                return wrappedReaderName;
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
        /// Translates Ship namespace to OpenShip namespace
        /// </summary>
        private static string GetShipNamespace(string nameSpace)
        {
            if (nameSpace == null)
            {
                return null;
            }

            const string openShipNamespace = "http://fedex.com/ws/openship/v" + FedExSettings.OpenShipVersionNumber;
            const string shipNamespace = "http://fedex.com/ws/ship/v" + FedExSettings.ShipVersionNumber;

            return nameSpace == openShipNamespace ? shipNamespace : nameSpace;
        }

        #region pass through

        /// <summary>
        /// When overridden in a derived class, gets the text value of the current node.
        /// </summary>
        /// <returns>The value returned depends on the <see cref="P:System.Xml.XmlReader.NodeType" /> of the node. The following table lists node types that have a value to return. All other node types return String.Empty.Node type Value AttributeThe value of the attribute. CDATAThe content of the CDATA section. CommentThe content of the comment. DocumentTypeThe internal subset. ProcessingInstructionThe entire content, excluding the target. SignificantWhitespaceThe white space between markup in a mixed content model. TextThe content of the text node. WhitespaceThe white space between markup. XmlDeclarationThe content of the declaration. </returns>
        public override string Value
        {
            get { return wrappedReader.Value; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the number of attributes on the current node.
        /// </summary>
        /// <returns>The number of attributes on the current node.</returns>
        public override int AttributeCount
        {
            get { return wrappedReader.AttributeCount; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the base URI of the current node.
        /// </summary>
        /// <returns>The base URI of the current node.</returns>
        public override string BaseURI
        {
            get { return wrappedReader.BaseURI; }
        }

        /// <summary>
        /// When overridden in a derived class, changes the <see cref="P:System.Xml.XmlReader.ReadState" /> to Closed.
        /// </summary>
        public override void Close()
        {
            wrappedReader.Close();
        }

        /// <summary>
        /// When overridden in a derived class, gets the depth of the current node in the XML document.
        /// </summary>
        /// <returns>The depth of the current node in the XML document.</returns>
        public override int Depth
        {
            get { return wrappedReader.Depth; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the reader is positioned at the end of the stream.
        /// </summary>
        /// <returns>true if the reader is positioned at the end of the stream; otherwise, false.</returns>
        public override bool EOF
        {
            get { return wrappedReader.EOF; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the value of the attribute with the specified index.
        /// </summary>
        /// <param name="i">The index of the attribute. The index is zero-based. (The first attribute has index 0.)</param>
        /// <returns>
        /// The value of the specified attribute. This method does not move the reader.
        /// </returns>
        public override string GetAttribute(int i)
        {
            return wrappedReader.GetAttribute(i);
        }

        /// <summary>
        /// When overridden in a derived class, gets the value of the attribute with the specified <see cref="P:System.Xml.XmlReader.Name" />.
        /// </summary>
        /// <param name="name">The qualified name of the attribute.</param>
        /// <returns>
        /// The value of the specified attribute. If the attribute is not found or the value is String.Empty, null is returned.
        /// </returns>
        public override string GetAttribute(string name)
        {
            return wrappedReader.GetAttribute(name);
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current node is an empty element (for example, &lt;MyElement/&gt;).
        /// </summary>
        /// <returns>true if the current node is an element (<see cref="P:System.Xml.XmlReader.NodeType" /> equals XmlNodeType.Element) that ends with /&gt;; otherwise, false.</returns>
        public override bool IsEmptyElement
        {
            get { return wrappedReader.IsEmptyElement; }
        }

        /// <summary>
        /// When overridden in a derived class, resolves a namespace prefix in the current element's scope.
        /// </summary>
        /// <param name="prefix">The prefix whose namespace URI you want to resolve. To match the default namespace, pass an empty string.</param>
        /// <returns>
        /// The namespace URI to which the prefix maps or null if no matching prefix is found.
        /// </returns>
        public override string LookupNamespace(string prefix)
        {
            return wrappedReader.LookupNamespace(prefix);
        }

        /// <summary>
        /// When overridden in a derived class, moves to the attribute with the specified <see cref="P:System.Xml.XmlReader.Name" />.
        /// </summary>
        /// <param name="name">The qualified name of the attribute.</param>
        /// <returns>
        /// true if the attribute is found; otherwise, false. If false, the reader's position does not change.
        /// </returns>
        public override bool MoveToAttribute(string name)
        {
            return wrappedReader.MoveToAttribute(name);
        }

        /// <summary>
        /// When overridden in a derived class, moves to the element that contains the current attribute node.
        /// </summary>
        /// <returns>
        /// true if the reader is positioned on an attribute (the reader moves to the element that owns the attribute); false if the reader is not positioned on an attribute (the position of the reader does not change).
        /// </returns>
        public override bool MoveToElement()
        {
            return wrappedReader.MoveToElement();
        }

        /// <summary>
        /// When overridden in a derived class, moves to the first attribute.
        /// </summary>
        /// <returns>
        /// true if an attribute exists (the reader moves to the first attribute); otherwise, false (the position of the reader does not change).
        /// </returns>
        public override bool MoveToFirstAttribute()
        {
            return wrappedReader.MoveToFirstAttribute();
        }

        /// <summary>
        /// When overridden in a derived class, moves to the next attribute.
        /// </summary>
        /// <returns>
        /// true if there is a next attribute; false if there are no more attributes.
        /// </returns>
        public override bool MoveToNextAttribute()
        {
            return wrappedReader.MoveToNextAttribute();
        }

        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Xml.XmlNameTable" /> associated with this implementation.
        /// </summary>
        /// <returns>The XmlNameTable enabling you to get the atomized version of a string within the node.</returns>
        public override XmlNameTable NameTable
        {
            get { return wrappedReader.NameTable; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the current node.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Xml.XmlNodeType" /> values representing the type of the current node.</returns>
        public override XmlNodeType NodeType
        {
            get { return wrappedReader.NodeType; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the namespace prefix associated with the current node.
        /// </summary>
        /// <returns>The namespace prefix associated with the current node.</returns>
        public override string Prefix
        {
            get { return wrappedReader.Prefix; }
        }

        /// <summary>
        /// When overridden in a derived class, reads the next node from the stream.
        /// </summary>
        /// <returns>
        /// true if the next node was read successfully; false if there are no more nodes to read.
        /// </returns>
        public override bool Read()
        {
            return wrappedReader.Read();
        }

        /// <summary>
        /// When overridden in a derived class, parses the attribute value into one or more Text, EntityReference, or EndEntity nodes.
        /// </summary>
        /// <returns>
        /// true if there are nodes to return.false if the reader is not positioned on an attribute node when the initial call is made or if all the attribute values have been read.An empty attribute, such as, misc="", returns true with a single node with a value of String.Empty.
        /// </returns>
        public override bool ReadAttributeValue()
        {
            return wrappedReader.ReadAttributeValue();
        }

        /// <summary>
        /// When overridden in a derived class, gets the state of the reader.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Xml.ReadState" /> values.</returns>
        public override ReadState ReadState
        {
            get { return wrappedReader.ReadState; }
        }

        /// <summary>
        /// When overridden in a derived class, resolves the entity reference for EntityReference nodes.
        /// </summary>
        public override void ResolveEntity()
        {
            wrappedReader.ResolveEntity();
        }

        #endregion
    }
}
