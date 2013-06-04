using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Templates.Processing.TemplateXml
{
    /// <summary>
    /// Class that simplifies implementing an element node
    /// </summary>
    public class TemplateXmlElement : TemplateXmlNode
    {
        TemplateXmlNodeList children;
        TemplateXmlNodeList attributes;

        /// <summary>
        /// Create a new element node
        /// </summary>
        public TemplateXmlElement(TemplateTranslationContext context, string name, ElementOutline outline)
            : base(context, name)
        {
            children = new TemplateXmlNodeList(this, outline, XPathNodeType.Element);
            attributes = new TemplateXmlNodeList(this, outline, XPathNodeType.Attribute);
        }

        /// <summary>
        /// The type of node.  Always element.
        /// </summary>
        public override XPathNodeType NodeType
        {
            get { return XPathNodeType.Element; }
        }

        /// <summary>
        /// All of the child nodes of the element
        /// </summary>
        public override TemplateXmlNodeList ChildNodes
        {
            get
            {
                return children;
            }
        }

        /// <summary>
        /// All of hte attributes of this element
        /// </summary>
        public override TemplateXmlNodeList Attributes
        {
            get
            {
                return attributes;
            }
        }

        /// <summary>
        /// The Value of the element (InnerText as defined by XPathNavigator)
        /// </summary>
        public override string Value
        {
            get 
            {
                StringBuilder sb = new StringBuilder();

                foreach (TemplateXmlNode child in ChildNodes)
                {
                    sb.Append(child.Value);
                }

                return sb.ToString();
            }
        }
    }
}
