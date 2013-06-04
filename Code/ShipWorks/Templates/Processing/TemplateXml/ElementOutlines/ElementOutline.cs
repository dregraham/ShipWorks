using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ShipWorks.Templates.Processing.TemplateXml.NodeFactories;
using System.Collections;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Base for all ElementOutlines
    /// </summary>
    public class ElementOutline
    {
        TemplateTranslationContext context;

        class OutlineEntry
        {
            public string Name { get; set; }
            public NodeFactory Factory { get; set; }

            public override string ToString()
            {
                return string.Format("{0}: {1}", Name, Factory.GetType().Name);
            }
        }

        class SingleObjectEnumerator
        {
            Func<object> objectCallback;

            public SingleObjectEnumerator(Func<object> objectCallback)
            {
                this.objectCallback = objectCallback;
            }

            public IEnumerable GetData()
            {
                yield return objectCallback();
            }
        }

        // The list of entries
        List<OutlineEntry> childEntries = new List<OutlineEntry>();
        List<OutlineEntry> attributeEntries = new List<OutlineEntry>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ElementOutline(TemplateTranslationContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// The active TemplateTranslationContext
        /// </summary>
        public TemplateTranslationContext Context
        {
            get { return context; }
        }

        /// <summary>
        /// Creates a clone of the outline that is bound to the given data.  Must be implemented by derived classes that are dynamically generated based on a dynamic data source.
        /// </summary>
        public virtual ElementOutline CreateDataBoundClone(object data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates the next set of child nodes with the given name, or the next set of nodes with any name if name is null.  If null or empty list is returned,
        /// there are no more to generate by the given name.  After generation, the outline entry for the children is removed.
        /// </summary>
        public List<TemplateXmlNode> GenerateNextChildren(string name, bool orIfDescendantHasName)
        {
            return GenerateNext(childEntries, name, orIfDescendantHasName);
        }

        /// <summary>
        /// Generates the next set of child attributes with the given name, or any attributes with any name if name is null.  If null or empty list is returned,
        /// there are no more to generate by the given name.  After generation, the outline entry for the attributes is removed.
        /// </summary>
        public List<TemplateXmlNode> GenerateNextAttributes(string name)
        {
            return GenerateNext(attributeEntries, name, false);
        }

        /// <summary>
        /// Generates the next set of nodes with the given name, or any with any name if name is null.  If null or empty list is returned,
        /// there are no more to generate by the given name.  After generation, the outline entry for the attributes is removed.
        /// </summary>
        private List<TemplateXmlNode> GenerateNext(List<OutlineEntry> entries, string name, bool orIfDescendantHasName)
        {
            if (entries.Count == 0)
            {
                return null;
            }

            OutlineEntry entry = null;

            do
            {
                // Find the first matching outline entry
                entry = entries.FirstOrDefault(e => 
                    name == null 
                    || 
                    (
                        e.Name == name 
                        ||
                        (
                            orIfDescendantHasName && e.Factory.HasDescendantWithName(name)
                        )
                    ));

                // If we found it, we have to generate the nodes are remove this factory as a candidate
                if (entry != null)
                {
                    // No matter what we are now done with this entry
                    entries.Remove(entry);

                    // Create the nodes using the factory of this entry
                    List<TemplateXmlNode> nodes = entry.Factory.Create(context, entry.Name);
                    if (nodes != null && nodes.Count > 0)
                    {
                        return nodes;
                    }
                }
            } while (entry != null && entries.Count > 0);

            return null;
        }

        /// <summary>
        /// Indicates if this outline - or any descendant outline recursively - contains an element with the specified name
        /// </summary>
        public bool HasElementOrDescendantWithName(string name)
        {
            foreach (OutlineEntry entry in childEntries)
            {
                if (entry.Name == name)
                {
                    return true;
                }

                if (entry.Factory.HasDescendantWithName(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add the given factory with the given name to the child outline
        /// </summary>
        private void AddChildEntry(string name, NodeFactory factory)
        {
            childEntries.Add(new OutlineEntry { Name = name, Factory = factory });
        }

        /// <summary>
        /// Add the given factory with the given name to the child outline
        /// </summary>
        private void AddAttributeEntry(string name, NodeFactory factory)
        {
            attributeEntries.Add(new OutlineEntry { Name = name, Factory = factory });
        }

        /// <summary>
        /// Add an attribute with the given name, with the specified value as the text content
        /// </summary>
        public void AddAttribute(string name, object value)
        {
            AddAttribute(name, () => value);
        }

        /// <summary>
        /// Add an attribute that defers its value resolution until the outline is bound
        /// </summary>
        public void AddAttribute(string name, Func<object> valueCallback)
        {
            AddAttribute(name, valueCallback, null);
        }

        /// <summary>
        /// Add an attribute that defers its value resolution until the outline is bound
        /// </summary>
        public void AddAttribute(string name, Func<object> valueCallback, NodeCreationCondition condition)
        {
            AddAttributeEntry(name, new ConditionalFactory(new AttributeFactory(valueCallback), condition));
        }

        /// <summary>
        /// Add the attribute that marks an element as a legacy 2x element
        /// </summary>
        public void AddAttributeLegacy2x()
        {
            AddAttribute("legacy", true);
        }

        /// <summary>
        /// Add a new ElementOutline with the given name, returning the newly created ElementOutline
        /// </summary>
        public ElementOutline AddElement(string name)
        {
            return AddElement(name, (NodeCreationCondition) null);
        }

        /// <summary>
        /// Add a new ElementOutline with the given name, returning the newly created ElementOutline
        /// </summary>
        public ElementOutline AddElement(string name, NodeCreationCondition condition)
        {
            ElementOutline outline = new ElementOutline(context);
            AddElement(name, outline, condition);

            return outline;
        }

        /// <summary>
        /// Add an element with the given name that will manifest itself as exactly one element with the specified outline
        /// </summary>
        public void AddElement(string name, ElementOutline outline)
        {
            AddElement(name, outline, (NodeCreationCondition) null);
        }

        /// <summary>
        /// Add an element with the given name that will manifest itself as exactly one element with the specified outline
        /// </summary>
        public void AddElement(string name, ElementOutline outline, NodeCreationCondition condition)
        {
            AddChildEntry(name, new ConditionalFactory(new ElementFactory(outline, null), condition));
        }

        /// <summary>
        /// Add an element that will produce exactly one entry in the final tree using the given callback for the outline bound datasource
        /// </summary>
        public void AddElement(string name, ElementOutline outline, Func<object> dataSource)
        {
            AddElement(name, outline, new SingleObjectEnumerator(dataSource).GetData());
        }

        /// <summary>
        /// Add an outline entry for elements with the given name, with one entry per item pulled from the data source.
        /// </summary>
        public void AddElement(string name, ElementOutline outline, IEnumerable dataSource)
        {
            AddElement(name, outline, () => dataSource);
        }

        /// <summary>
        /// Add an outline entry for elements with the given name, with one entry per item pulled form the deferred loaded data source
        /// </summary>
        public void AddElement(string name, ElementOutline outline, Func<IEnumerable> dataSource)
        {
            AddElement(name, outline, dataSource, null);
        }

        /// <summary>
        /// Add an outline entry for elements with the given name, with one entry per item pulled form the deferred loaded data source
        /// </summary>
        public void AddElement(string name, ElementOutline outline, Func<IEnumerable> dataSource, NodeCreationCondition condition)
        {
            AddChildEntry(name, new ConditionalFactory(new ElementFactory(outline, dataSource), condition));
        }

        /// <summary>
        /// Add an element that is of a simple value type 
        /// </summary>
        public void AddElement(string name, object value)
        {
            AddElement(name, () => value, (NodeCreationCondition) null);
        }

        /// <summary>
        /// Add an element that is of a simple value type that is deferred
        /// </summary>
        public void AddElement(string name, Func<object> valueCallback)
        {
            AddElement(name, valueCallback, (NodeCreationCondition) null);
        }

        /// <summary>
        /// Add an element that is of a simple value type that is deferred
        /// </summary>
        public void AddElement(string name, Func<object> valueCallback, NodeCreationCondition condition)
        {
            AddChildEntry(name, new ConditionalFactory(new ElementWithTextFactory(valueCallback), condition));
        }

        /// <summary>
        /// Add all the child elements from the other outline to this outline, applying the given (optional) condition as they go.  The elements
        /// are remove from the source on completion - this is NOT a copy.
        /// </summary>
        public void AddElements(ElementOutline source, NodeCreationCondition condition)
        {
            if (source.attributeEntries.Count > 0)
            {
                throw new InvalidOperationException("The source container contained attributes, which are not allowed.");
            }

            // Add each of the source child entries in order
            foreach (OutlineEntry entry in source.childEntries)
            {
                if (entry.Factory is TextContentFactory)
                {
                    throw new InvalidOperationException("The source container contains text nodes, which are not allowed.");
                }

                AddChildEntry(entry.Name, new ConditionalFactory(entry.Factory, condition));
            }
        }

        /// <summary>
        /// Add an element that is of a simple value type that is deferred
        /// </summary>
        public void AddElementLegacy2x(string name, Func<object> valueCallback)
        {
            ElementOutline outline = new ElementOutline(context);
            outline.AddAttributeLegacy2x();
            outline.AddTextContent(valueCallback);

            AddElement(name, outline);
        }

        /// <summary>
        /// Add a factory that adds raw text as a 'Text' node - not an element.
        /// </summary>
        public void AddTextContent(Func<object> valueCallback)
        {
            AddChildEntry(string.Empty, new TextContentFactory(valueCallback));
        }

        /// <summary>
        /// Utility function to make it easier and more readable to create a NodeCreationCondition
        /// </summary>
        public static NodeCreationCondition If(Func<bool> condition)
        {
            return new NodeCreationCondition(condition);
        }
    }
}
