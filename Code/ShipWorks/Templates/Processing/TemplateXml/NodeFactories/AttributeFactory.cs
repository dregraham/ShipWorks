using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Processing.TemplateXml.NodeFactories
{
    /// <summary>
    /// Factory for creating a single attribute
    /// </summary>
    public class AttributeFactory : NodeFactory
    {
        Func<object> valueCallback;

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeFactory(Func<object> valueCallback)
        {
            this.valueCallback = valueCallback;
        }

        /// <summary>
        /// Create the attribute
        /// </summary>
        public override List<TemplateXmlNode> Create(TemplateTranslationContext context, string name)
        {
            return new List<TemplateXmlNode> 
                { 
                    new TemplateXmlAttribute(
                            context, 
                            name, 
                            valueCallback) };
        }
    }
}
