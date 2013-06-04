using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace ShipWorks.Templates.Processing.TemplateXml.NodeFactories
{
    /// <summary>
    /// Factory that generates a single unnamed 'Text' node - not an element
    /// </summary>
    public class TextContentFactory : NodeFactory
    {
        Func<object> valueCallback;

        /// <summary>
        /// Constructor
        /// </summary>
        public TextContentFactory(Func<object> valueCallback)
        {
            this.valueCallback = valueCallback;
        }

        /// <summary>
        /// Generate the node
        /// </summary>
        public override List<TemplateXmlNode> Create(TemplateTranslationContext context, string name)
        {
            return new List<TemplateXmlNode> { new TemplateXmlText(context, valueCallback) };
        }
    }
}
