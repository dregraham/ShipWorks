using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Templates.Processing.TemplateXml
{
    /// <summary>
    /// Represents the single root of an XML document
    /// </summary>
    public class TemplateXmlRoot : TemplateXmlElement
    {
        /// <summary>
        /// Standard constructor
        /// </summary>
        public TemplateXmlRoot(TemplateTranslationContext context)
            : base(context, string.Empty, CreateOutline(context))
        {


        }

        /// <summary>
        /// Create the instance of the root outline
        /// </summary>
        private static ElementOutline CreateOutline(TemplateTranslationContext context)
        {
            ElementOutline rootOutline = new ElementOutline(context);
            rootOutline.AddElement("ShipWorks", new DocumentOutline(context));

            return rootOutline;
        }

        /// <summary>
        /// The current node type, which is always root
        /// </summary>
        public override XPathNodeType NodeType
        {
            get 
            {
                return XPathNodeType.Root;
            }
        }
    }
}
