using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace ShipWorks.Templates.Processing.TemplateXml
{
    /// <summary>
    /// Represents an attribute of an element
    /// </summary>
    public class TemplateXmlAttribute : TemplateXmlNode
    {
        Func<object> valueCallback;
        string value = null;

        /// <summary>
        /// Standard constructor
        /// </summary>
        public TemplateXmlAttribute(TemplateTranslationContext context, string name, Func<object> valueCallback)
            : base(context, name)
        {
            this.valueCallback = valueCallback;
        }

        /// <summary>
        /// The node type, always Attribute
        /// </summary>
        public override XPathNodeType NodeType
        {
            get { return XPathNodeType.Attribute; }
        }

        /// <summary>
        /// The value of the attribute
        /// </summary>
        public override string Value
        {
            get
            {
                if (value == null && valueCallback != null && !Context.ProcessingComplete)
                {
                    value = ConvertToXmlValue(valueCallback());
                    valueCallback = null;
                }

                return value ?? string.Empty;
            }
        }
    }
}
