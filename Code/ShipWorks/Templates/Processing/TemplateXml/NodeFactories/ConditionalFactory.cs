using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Processing.TemplateXml.NodeFactories
{
    /// <summary>
    /// Derived from by all factories that allow a NodeCreationCondition to control whether they actually create nodes
    /// </summary>
    public class ConditionalFactory : NodeFactory
    {
        NodeCreationCondition condition;
        NodeFactory factory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionalFactory(NodeFactory factory, NodeCreationCondition condition)
        {
            this.factory = factory;
            this.condition = condition;
        }

        /// <summary>
        /// Create elements using the inner factory if the condition passes
        /// </summary>
        public override List<TemplateXmlNode> Create(TemplateTranslationContext context, string name)
        {
            if (condition != null && !condition.Check())
            {
                return null;
            }

            return factory.Create(context, name);
        }

        /// <summary>
        /// Indicates if the node type the factory creates could have a descendant element with the given name
        /// </summary>
        public override bool HasDescendantWithName(string name)
        {
            return factory.HasDescendantWithName(name);
        }
    }
}
