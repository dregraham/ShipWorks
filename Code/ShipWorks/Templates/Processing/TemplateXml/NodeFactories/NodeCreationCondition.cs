using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Processing.TemplateXml.NodeFactories
{
    /// <summary>
    /// Represents a condition that must hold true for a node(s) to be created
    /// </summary>
    public class NodeCreationCondition
    {
        Func<bool> condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public NodeCreationCondition(Func<bool> condition)
        {
            this.condition = condition;
        }

        /// <summary>
        /// Check to see if the condition is true
        /// </summary>
        public bool Check()
        {
            return condition == null || condition();
        }
    }
}
