using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Processing.TemplateXml.NodeFactories
{
    /// <summary>
    /// Factory that generates a single Element with Text content
    /// </summary>
    public class ElementWithTextFactory : NodeFactory
    {
        Func<object> valueCallback;

        /// <summary>
        /// Constructor
        /// </summary>
        public ElementWithTextFactory(Func<object> valueCallback)
        {
            this.valueCallback = valueCallback;
        }

        /// <summary>
        /// Generate the node
        /// </summary>
        public override List<TemplateXmlNode> Create(TemplateTranslationContext context, string name)
        {
            return new List<TemplateXmlNode> 
                { 
                    new TemplateXmlElementWithText(
                            context, 
                            name, 
                            valueCallback) };
        }
    }
}
