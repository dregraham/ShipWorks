using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// An XPathNavigator that can be made namespace aware
    /// </summary>
    public class XPathNamespaceNavigator : XPathNavigator
    {
        // The namespace manager
        XmlNamespaceManager nsManager = null;

        // The underlying aggregate implementation of XPathNavigator
        XPathNavigator baseNavigator = null;

        /// <summary>
        /// Constructor for wrapping a given XPathNavigator
        /// </summary>
        public XPathNamespaceNavigator(XPathNavigator baseNavigator)
        {
            this.baseNavigator = baseNavigator;
            nsManager = new XmlNamespaceManager(new NameTable());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public XPathNamespaceNavigator(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            baseNavigator = xmlDocument.CreateNavigator();
            nsManager = new XmlNamespaceManager(new NameTable());
        }

        public XPathNamespaceNavigator(XPathNavigator xpath, XmlNamespaceManager namespacesToInclude)
            : this(xpath)
        {
            nsManager = namespacesToInclude;
        }

        /// <summary>
        /// The namespace manager that is used in all XPath queries
        /// </summary>
        public XmlNamespaceManager Namespaces
        {
            get
            {
                return nsManager;
            }
        }

        /// <summary>
        /// Selects a node set using the specified XPath expression.
        /// </summary>
        public override XPathNodeIterator Select(string xpath)
        {
            XPathExpression expr = Compile(xpath);
            expr.SetContext(nsManager);

            return baseNavigator.Select(expr);
        }

        /// <summary>
        /// Selects a single node using the specifiex XPath expression
        /// </summary>
        public override XPathNavigator SelectSingleNode(string xpath)
        {
            XPathExpression expr = Compile(xpath);
            expr.SetContext(nsManager);

            return base.SelectSingleNode(expr);
        }

        /// <summary>
        /// Evaluates an XPath expression
        /// </summary>
        public override object Evaluate(string xpath)
        {
            XPathExpression expr = Compile(xpath);
            expr.SetContext(nsManager);

            return baseNavigator.Evaluate(expr);
        }

        #region Abstract Aggregate Forwards

        public override XPathNavigator Clone()
        {
            XPathNamespaceNavigator navigator = new XPathNamespaceNavigator(baseNavigator.Clone());

            // copy the namespaces to the new navigator
            foreach (string prefix in nsManager)
            {
                // skip copying reserved namespaces over
                if (prefix != "xml" && prefix != "xmlns")
                {
                    navigator.Namespaces.AddNamespace(prefix, nsManager.LookupNamespace(prefix));
                }
            }

            return navigator;
        }

        public override string GetAttribute(string localName, string namespaceURI)
        {
            return baseNavigator.GetAttribute(localName, namespaceURI);
        }

        public override string GetNamespace(string name)
        {
            return baseNavigator.GetNamespace(name);
        }

        public override bool IsSamePosition(XPathNavigator other)
        {
            return baseNavigator.IsSamePosition(other);
        }

        public override bool MoveTo(XPathNavigator other)
        {
            return baseNavigator.MoveTo(other);
        }

        public override bool MoveToAttribute(string localName, string namespaceURI)
        {
            return baseNavigator.MoveToAttribute(localName, namespaceURI);
        }

        public override bool MoveToFirst()
        {
            return baseNavigator.MoveToFirst();
        }

        public override bool MoveToFirstAttribute()
        {
            return baseNavigator.MoveToFirstAttribute();
        }

        public override bool MoveToFirstChild()
        {
            return baseNavigator.MoveToFirstChild();
        }

        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
        {
            return baseNavigator.MoveToFirstNamespace(namespaceScope);
        }

        public override bool MoveToId(string id)
        {
            return baseNavigator.MoveToId(id);
        }

        public override bool MoveToNamespace(string name)
        {
            return baseNavigator.MoveToNamespace(name);
        }

        public override bool MoveToNext()
        {
            return baseNavigator.MoveToNext();
        }

        public override bool MoveToNextAttribute()
        {
            return baseNavigator.MoveToNextAttribute();
        }

        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
        {
            return baseNavigator.MoveToNextNamespace(namespaceScope);
        }

        public override bool MoveToParent()
        {
            return baseNavigator.MoveToParent();
        }

        public override bool MoveToPrevious()
        {
            return baseNavigator.MoveToPrevious();
        }

        public override void MoveToRoot()
        {
            baseNavigator.MoveToRoot();
        }

        public override string BaseURI
        {
            get
            {
                return baseNavigator.BaseURI;
            }
        }

        public override bool HasAttributes
        {
            get
            {
                return baseNavigator.HasAttributes;
            }
        }

        public override bool HasChildren
        {
            get
            {
                return baseNavigator.HasChildren;
            }
        }

        public override bool IsEmptyElement
        {
            get
            {
                return baseNavigator.IsEmptyElement;
            }
        }

        public override string LocalName
        {
            get
            {
                return baseNavigator.LocalName;
            }
        }

        public override string Name
        {
            get
            {
                return baseNavigator.Name;
            }
        }

        public override string NamespaceURI
        {
            get
            {
                return baseNavigator.NamespaceURI;
            }
        }

        public override XmlNameTable NameTable
        {
            get
            {
                return baseNavigator.NameTable;
            }
        }

        public override XPathNodeType NodeType
        {
            get
            {
                return baseNavigator.NodeType;
            }
        }

        public override string Prefix
        {
            get
            {
                return baseNavigator.Prefix;
            }
        }

        public override string Value
        {
            get
            {
                return baseNavigator.Value;
            }
        }

        public override string XmlLang
        {
            get
            {
                return baseNavigator.XmlLang;
            }
        }

        #endregion
    }
}
