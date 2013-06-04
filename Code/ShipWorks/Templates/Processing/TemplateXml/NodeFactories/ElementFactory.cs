using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System.Collections;

namespace ShipWorks.Templates.Processing.TemplateXml.NodeFactories
{
    /// <summary>
    /// Factory for generating new elements based on an ElementOutline
    /// </summary>
    public class ElementFactory : NodeFactory
    {
        ElementOutline outline;
        Func<IEnumerable> dataSource;

        /// <summary>
        /// Constructor which takes the outline that defines the element
        /// </summary>
        public ElementFactory(ElementOutline outline, Func<IEnumerable> dataSource)
        {
            this.outline = outline;
            this.dataSource = dataSource;
        }

        /// <summary>
        /// The outline that this factory creates an element based on
        /// </summary>
        public ElementOutline ElementOutline
        {
            get { return outline; }
        }

        /// <summary>
        /// Create a new instance of an element
        /// </summary>
        public override List<TemplateXmlNode> Create(TemplateTranslationContext context, string name)
        {
            List<TemplateXmlNode> nodes = new List<TemplateXmlNode>();

            // If DataSource is null we just create one element that is not DataBound based on the ElementOutline as-is
            if (dataSource == null)
            {
                nodes.Add(new TemplateXmlElement(context, name, outline));
            }
            else
            {
                var collection = dataSource();

                if (collection != null)
                {
                    // Otherwise we create a node per-item in the dataSource, bound to each item in the datasource
                    foreach (object data in collection)
                    {
                        var outlineClone = outline.CreateDataBoundClone(data);

                        // If the outline didn't like the data \ didn't see the need to make a clone, then don't materialize it to the tree.
                        if (outlineClone != null)
                        {
                            nodes.Add(new TemplateXmlElement(context, name, outlineClone));
                        }
                    }
                }
            }

            return nodes;
        }

        /// <summary>
        /// Indicates if the node type the factory creates could have a descendant element with the given name
        /// </summary>
        public override bool HasDescendantWithName(string name)
        {
            return outline.HasElementOrDescendantWithName(name);
        }
    }
}
