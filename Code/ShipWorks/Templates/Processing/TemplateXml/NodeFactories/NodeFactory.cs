using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Processing.TemplateXml.NodeFactories
{
    /// <summary>
    /// Base for all node factories that are designed to generate the action template elements
    /// </summary>
    public abstract class NodeFactory
    {
        /// <summary>
        /// Generate all the nodes for the factory.  Can return null if there are no nodes that should be generated.
        /// </summary>
        public abstract List<TemplateXmlNode> Create(TemplateTranslationContext context, string name);

        /// <summary>
        /// Indicates if the node type the factory creates could have a descendant element with the given name
        /// </summary>
        public virtual bool HasDescendantWithName(string name)
        {
            return false;
        }
    }
}
