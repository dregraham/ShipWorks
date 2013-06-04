using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace ShipWorks.Templates.Processing.TemplateXml
{
    /// <summary>
    /// A Text node in the virtual xml tree
    /// </summary>
    public class TemplateXmlText : TemplateXmlNode
    {
        Func<object> valueCallback;
        string value;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateXmlText(TemplateTranslationContext context, Func<object> valueCallback)
            : base(context)
        {
            this.valueCallback = valueCallback;
        }

        /// <summary>
        /// The type of node.  Always Text
        /// </summary>
        public override XPathNodeType NodeType
        {
            get { return XPathNodeType.Text; }
        }

        /// <summary>
        /// The value of the text
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
