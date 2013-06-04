using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System.Xml.XPath;

namespace ShipWorks.Templates.Processing.TemplateXml
{
    /// <summary>
    /// A list that can be treated as a list but can use a lazy-loaded IEnumerable backing store for its item collection.
    /// </summary>
    public class TemplateXmlNodeList : IEnumerable<TemplateXmlNode>
    {
        TemplateXmlNode owner;

        // The outline of what children we can expect to have, and source to get them
        ElementOutline childOutline;

        // The cache of elements that we've already pulled from the source and what type they are
        List<TemplateXmlNode> cache;
        XPathNodeType listNodeType;

        /// <summary>
        /// Constructor for an empty node list
        /// </summary>
        public TemplateXmlNodeList()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateXmlNodeList(TemplateXmlNode owner, ElementOutline childOutline, XPathNodeType listNodeType)
        {
            this.owner = owner;
            this.cache = new List<TemplateXmlNode>();
            this.childOutline = childOutline;
            this.listNodeType = listNodeType;
        }

        /// <summary>
        /// Get the next node in the collection with the given name (if not null).  name and startAfter can both be null.  If startAfter is not null, it must be a child
        /// of the parent that owns this list.  If 'orIfDescendantHasName', then we move to the first node that either has the given name, or whose outline shows that any
        /// descendant could potentially have the name.
        /// </summary>
        public TemplateXmlNode GetNext(string name, TemplateXmlNode startAfter, bool orIfDescendantHasName)
        {
            // Empty list check
            if (cache == null)
            {
                return null;
            }

            int startAfterIndex = (startAfter != null) ? startAfter.IndexInParent : -1;

            // Seach in our cache first
            for (int i = startAfterIndex + 1; i < cache.Count; i++)
            {
                TemplateXmlNode node = cache[i];

                if (name == null || node.LocalName == name)
                {
                    return node;
                }

                if (orIfDescendantHasName)
                {
                    // If this node will potentially have a child, or a descendant with the given name, then it works
                    if (node.ChildNodes.HasElementOrDescendantWithName(name))
                    {
                        return node;
                    }
                }
            }

            if (childOutline == null || owner.Context.ProcessingComplete)
            {
                return null;
            }

            // See if we can generate any new nodes by that name
            List<TemplateXmlNode> generated = (listNodeType == XPathNodeType.Attribute) ? childOutline.GenerateNextAttributes(name) : childOutline.GenerateNextChildren(name, orIfDescendantHasName);

            // If there arent any, then we get out
            if (generated == null || generated.Count == 0)
            {
                // If there are no more at all, then kill the outline se we stop checking
                if (name == null)
                {
                    childOutline = null;
                }

                return null;
            }

            // Apply the parent
            generated.ForEach(n => n.ParentNode = owner);

            // Add them to our cache
            cache.AddRange(generated);

            // Return the first one (which would be the next)
            return generated[0];
        }

        /// <summary>
        /// Check if we have any elements in our cache with the given name, or could potentially generate any elements with the given name based on our Outline entries
        /// that have not yet been materialized.
        /// </summary>
        private bool HasElementOrDescendantWithName(string name)
        {
            if (cache != null)
            {
                // Check all the ones in cache first
                foreach (TemplateXmlNode node in cache)
                {
                    if (node.LocalName == name || node.ChildNodes.HasElementOrDescendantWithName(name))
                    {
                        return true;
                    }
                }
            }

            // Next check the outline
            return childOutline != null && childOutline.HasElementOrDescendantWithName(name);
        }

        /// <summary>
        /// Get the previous node in the collection with the given name (if not null).  name and startBefore can both be null.  If startBefore is not null, it must be a child
        /// of the parent that owns this list.
        /// </summary>
        public TemplateXmlNode GetPrevious(string name, TemplateXmlNode startBefore)
        {
            // Empty list check
            if (cache == null)
            {
                return null;
            }

            int startBeforeIndex = (startBefore != null) ? startBefore.IndexInParent: cache.Count;

            for (int i = startBeforeIndex - 1; i >= 0; i--)
            {
                if (name == null || cache[i].LocalName == name)
                {
                    return cache[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the index of the specifed node.  This method only looks in the cache and does not lazily load any new elements from the source.
        /// </summary>
        public int IndexOf(TemplateXmlNode node)
        {
            return cache.IndexOf(node);
        }

        /// <summary>
        /// Enumerator implementation
        /// </summary>
        public IEnumerator<TemplateXmlNode> GetEnumerator()
        {
            TemplateXmlNode node = GetNext(null, null, false);

            while (node != null)
            {
                yield return node;

                node = GetNext(null, node, false);
            }
        }

        /// <summary>
        /// Hidden non-generic version
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
