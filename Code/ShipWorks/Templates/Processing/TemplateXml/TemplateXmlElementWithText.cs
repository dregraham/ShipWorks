using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Templates.Processing.TemplateXml
{
    /// <summary>
    /// A Text node in the virtual xml tree
    /// </summary>
    public class TemplateXmlElementWithText : TemplateXmlElement
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateXmlElementWithText(TemplateTranslationContext context, string name, Func<object> valueCallback)
            : base(context, name, CreateOutline(context, valueCallback))
        {

        }

        /// <summary>
        /// Create the outline of the single child Text element
        /// </summary>
        private static ElementOutline CreateOutline(TemplateTranslationContext context, Func<object> valueCallback)
        {
            ElementOutline outline = new ElementOutline(context);
            outline.AddTextContent(valueCallback);

            return outline;
        }
    }
}
