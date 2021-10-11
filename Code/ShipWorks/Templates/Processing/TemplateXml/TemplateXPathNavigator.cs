using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml;
using System.Diagnostics;
using ShipWorks.UI;

namespace ShipWorks.Templates.Processing.TemplateXml
{
    /// <summary>
    /// Custom XPath navigation for ShipWorksXML
    /// </summary>
    public class TemplateXPathNavigator : XPathNavigator
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateXPathNavigator));

        // Data source
        TemplateEntity template;
        TemplateTranslationContext context;

        // The NameTable
        NameTable nameTable;

        // The node we are currently positioned on
        TemplateXmlNode currentNode;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateXPathNavigator(TemplateTranslationContext context)
        {
            this.context = context;
            this.nameTable = context.NameTable;
            this.template = context.Template;

            // We always sart at the root
            currentNode = new TemplateXmlRoot(context);
        }

        /// <summary>
        /// Private construction, for Clone
        /// </summary>
        private TemplateXPathNavigator(TemplateXPathNavigator copy)
        {
            // Primay data
            this.template = copy.template;
            this.context = copy.context;

            // Secondary
            this.nameTable = copy.nameTable;

            // Position
            this.currentNode = copy.currentNode;
        }

        /// <summary>
        /// The Template, if any, that defined this transformation.
        /// </summary>
        public TemplateEntity Template
        {
            get { return template; }
        }

        /// <summary>
        /// The Entity data used as input into the transformation
        /// </summary>
        public TemplateInput Input
        {
            get { return context.Input; }
        }

        /// <summary>
        /// The context used to translate.
        /// </summary>
        public TemplateTranslationContext Context
        {
            get { return context; }
        }

        /// <summary>
        /// Creates a new TemplateXPathNavigator positioned at the same node as this TemplateXPathNavigator
        /// </summary>
        public override XPathNavigator Clone()
        {
            return new TemplateXPathNavigator(this);
        }

        /// <summary>
        /// The NameTable
        /// </summary>
        public override XmlNameTable NameTable
        {
            get
            {
                return nameTable;
            }
        }

        /// <summary>
        /// Gets the XPathNodeType of the current node
        /// </summary>
        public override XPathNodeType NodeType
        {
            get
            {
                return currentNode.NodeType;
            }
        }

        /// <summary>
        /// Gets the Name of the current node without any namespace prefix
        /// </summary>
        public override string LocalName
        {
            get
            {
                // This will get called a lot, a good place to check
                context.CheckForCancel();

                return currentNode.LocalName;
            }
        }

        /// <summary>
        /// Gets the qualified name of the current node.
        /// </summary>
        public override string Name
        {
            get
            {
                // We don't use prefixes, so this is the same as LocalName
                return LocalName;
            }
        }

        /// <summary>
        /// The value of the current node.  This really only applies to attributes and "Text" nodes.
        /// </summary>
        public override string Value
        {
            get
            {
                return currentNode.Value;
            }
        }

        /// <summary>
        /// Compare this position to that of the given position
        /// </summary>
        public override XmlNodeOrder ComparePosition(XPathNavigator nav)
        {
            XmlNodeOrder ours = InternalComparePosition(nav);

            #if DEBUG
                // XmlNodeOrder theirs = base.ComparePosition(nav);
                // Debug.Assert(ours == theirs);
            #endif

            return ours;
        }

        /// <summary>
        /// Our ComparePosition implementation that is designed to be faster than the base approach.
        /// </summary>
        private XmlNodeOrder InternalComparePosition(XPathNavigator nav)
        {
            TemplateXPathNavigator templateNav = nav as TemplateXPathNavigator;
            if (templateNav == null)
            {
                return XmlNodeOrder.Unknown;
            }

            if (templateNav.nameTable != this.nameTable)
            {
                return XmlNodeOrder.Unknown;
            }

            // Determine up front if they are the same
            if (this.currentNode == templateNav.currentNode)
            {
                return XmlNodeOrder.Same;
            }

            int[] thisPathFromRoot = currentNode.IndexPathFromRoot;
            int[] otherPathFromRoot = templateNav.currentNode.IndexPathFromRoot;

            int depth = 0;
            while (true)
            {
                // So far they are the same... but now the other one keeps going deeper, and we've run out, so we are before it
                if (thisPathFromRoot.Length == depth && otherPathFromRoot.Length > depth)
                {
                    return XmlNodeOrder.Before;
                }

                // So far they are the same... but now this one keeps going deeper, and the other one run out, so we are after it
                if (otherPathFromRoot.Length == depth && thisPathFromRoot.Length > depth)
                {
                    return XmlNodeOrder.After;
                }

                // So far they are the same... and neither keeps going.  So they are the same
                if (otherPathFromRoot.Length == depth && thisPathFromRoot.Length == depth)
                {
                    return XmlNodeOrder.Same;
                }

                // If they are the same at this level we have to keep going deeper
                if (thisPathFromRoot[depth] == otherPathFromRoot[depth])
                {
                    depth++;
                }
                else
                {
                    // Otherwise if they are different at this level, we have our answer
                    return thisPathFromRoot[depth] < otherPathFromRoot[depth] ? XmlNodeOrder.Before : XmlNodeOrder.After;
                }
            }
        }

        /// <summary>
        /// Move up to the parent node from the current node
        /// </summary>
        public override bool MoveToParent()
        {
            if (NodeType == XPathNodeType.Root)
            {
                return false;
            }

            currentNode = currentNode.ParentNode;

            return true;
        }

        /// <summary>
        /// Moves the XPathNavigator to the element with the local name specified, to the boundary specified, in document order.
        /// </summary>
        public override bool MoveToFollowing(string localName, string namespaceURI, XPathNavigator end)
        {
            XPathNavigator originalPosition = this.Clone();

            // Determine proper ending point
            if (end != null)
            {
                // If the XPathNavigator boundary passed as a parameter is positioned over an attribute or namespace node, it is equivalent to the 
                // XPathNavigator boundary parameter having been positioned on the first child node of its parent element. This ensures that the 
                // parent element of the attribute or namespace node that the XPathNavigator boundary parameter is positioned on can be matched by this method.
                if (end.NodeType == XPathNodeType.Attribute)
                {
                    // Move up to the parent node
                    end = end.Clone();
                    end.MoveToParent();

                    // Move to the first child of the parent
                    if (!end.MoveToFirstChild())
                    {
                        // But if we couldn't just move to the next sibling
                        while (end != null && !end.MoveToNext())
                        {
                            // But if there was no next sibling just move up to the parent
                            if (!end.MoveToParent())
                            {
                                // The 'end' then is actually the end of the document, which is nothing
                                end = null;
                            }
                        }
                    }
                }
            }

            // If it's an attribute, first ting we need to do is move up to the element.  MoveToFollowing does not search to attributes
            if (NodeType == XPathNodeType.Attribute)
            {
                MoveToParent();
            }

            do
            {
                // Try to find a direct child with the name we are looking for
                if (!MoveToFirstChild(localName, true))
                {
                    // If we couldn't move to the first child, move to the next sibling
                    while (!MoveToNextElement(localName, true))
                    {
                        // If there are no more siblings, move on up to the parent
                        if (!MoveToParent())
                        {
                            // We've reached the top! No more to do
                            MoveTo(originalPosition);
                            return false;
                        }
                    }
                }

                // Make sure we haven't reached the end
                if (end != null && IsSamePosition(end))
                {
                    MoveTo(originalPosition);
                    return false;
                }

            } while (NodeType != XPathNodeType.Element || localName != LocalName);

            return true;
        }

        /// <summary>
        /// Move directly to the first child element with the specified name
        /// </summary>
        public override bool MoveToChild(string localName, string namespaceURI)
        {
            return MoveToFirstChild(localName, false);
        }

        /// <summary>
        /// Moves the navigator to the first child node of the current node
        /// </summary>
        public override bool MoveToFirstChild()
        {
            return MoveToFirstChild(null, false);
        }

        /// <summary>
        /// Move to the first child with the given name. If name is null, then just move to the very first child.
        /// </summary>
        private bool MoveToFirstChild(string name, bool orIfDescendantHasName)
        {
            return MoveToNode(currentNode.ChildNodes.GetNext(name, null, orIfDescendantHasName));
        }

        /// <summary>
        /// Move to the next sibling element with the specified name
        /// </summary>
        public override bool MoveToNext(string localName, string namespaceURI)
        {
            return MoveToNextElement(localName, false);
        }

        /// <summary>
        /// Moves to the next sibling node of the current node
        /// </summary>
        public override bool MoveToNext()
        {
            return MoveToNextElement(null, false);
        }

        /// <summary>
        /// Move to the next sibling node with the specified name.  If name is null, just move to the next sibiling node whatever it is.
        /// </summary>
        private bool MoveToNextElement(string name, bool orIfDescendantHasName)
        {
            // Specification says this does nothing when positioned on an attribute
            if (NodeType == XPathNodeType.Attribute || NodeType == XPathNodeType.Root)
            {
                return false;
            }

            return MoveToNode(currentNode.ParentNode.ChildNodes.GetNext(name, currentNode, orIfDescendantHasName));
        }

        /// <summary>
        /// Move to the previous sibling of the current node
        /// </summary>
        public override bool MoveToPrevious()
        {
            // Specification says this does nothing when positioned on an attribute
            if (NodeType == XPathNodeType.Attribute || NodeType == XPathNodeType.Root)
            {
                return false;
            }

            return MoveToNode(currentNode.ParentNode.ChildNodes.GetPrevious(null, currentNode));
        }

        /// <summary>
        /// Moves to the first attribute of the selected node
        /// </summary>
        public override bool MoveToFirstAttribute()
        {
            return MoveToNode(currentNode.Attributes.GetNext(null, null, false));
        }

        /// <summary>
        /// Move to the first attribute with the given name.
        /// </summary>
        public override bool MoveToAttribute(string localName, string namespaceURI)
        {
            return MoveToNode(currentNode.Attributes.GetNext(localName, null, false));
        }

        /// <summary>
        /// Moves the navigator to the next attribute
        /// </summary>
        public override bool MoveToNextAttribute()
        {
            if (NodeType != XPathNodeType.Attribute)
            {
                return false;
            }

            return MoveToNode(currentNode.ParentNode.Attributes.GetNext(null, currentNode, false));
        }

        /// <summary>
        /// Move to the given node if its not null, otherwise stay where we are
        /// </summary>
        private bool MoveToNode(TemplateXmlNode node)
        {
            if (node != null)
            {
                currentNode = node;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Move to the exact position as the specified navigator
        /// </summary>
        public override bool MoveTo(XPathNavigator other)
        {
            TemplateXPathNavigator from = other as TemplateXPathNavigator;
            if (from == null)
            {
                return false;
            }

            // Has to have the exact same inputs
            if (from.context != this.context)
            {
                return false;
            }

            this.currentNode = from.currentNode;

            return true;
        }

        /// <summary>
        /// Move to the spectic ID.
        /// </summary>
        public override bool MoveToId(string id)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets a value indicating whether the current node is an empty element without an end element tag
        /// </summary>
        public override bool IsEmptyElement
        {
            get
            {
                return (NodeType == XPathNodeType.Element) && (currentNode.ChildNodes.GetNext(null, null, false) == null);
            }
        }

        /// <summary>
        /// Returns true if the navigator is using the same input data and is positioned at the same location as the other.
        /// </summary>
        public override bool IsSamePosition(XPathNavigator other)
        {
            TemplateXPathNavigator from = other as TemplateXPathNavigator;
            if (from == null)
            {
                return false;
            }

            return this.currentNode == from.currentNode;
        }

        /// <summary>
        /// The location from which the node was loaded.
        /// </summary>
        public override string BaseURI
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the namespace prefix associated with the current node
        /// </summary>
        public override string Prefix
        {
            get
            {
                // We don't do namespaces or prefixes
                return string.Empty;
            }
        }

        /// <summary>
        /// The URI of the nammespace of hte current node
        /// </summary>
        public override string NamespaceURI
        {
            get
            {
                // We don't do namespaces
                return string.Empty;
            }
        }

        /// <summary>
        /// Move to the first namespace matching the filter
        /// </summary>
        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
        {
            // We dont do namespaces
            return false;
        }

        /// <summary>
        /// Move to the next namespaces, when already positioned on a namespace
        /// </summary>
        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
        {
            // We dont do namespaces
            return false;
        }
    }
}
