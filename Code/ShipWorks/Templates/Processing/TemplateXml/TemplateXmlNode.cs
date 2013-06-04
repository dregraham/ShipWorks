using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Linq;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System.Diagnostics;

namespace ShipWorks.Templates.Processing.TemplateXml
{
    /// <summary>
    /// Represents a single node in our template XPath traversal
    /// </summary>
    public abstract class TemplateXmlNode
    {
        static TemplateXmlNodeList noChildren = new TemplateXmlNodeList();
        static TemplateXmlNodeList noAttributes = new TemplateXmlNodeList();

        // The parent node
        TemplateXmlNode parentNode;
        bool isRootNode = false;

        // The nametable used for atomic naming
        NameTable nameTable;

        // The translation context, providing tempalte input and progress
        TemplateTranslationContext context;

        // The name of the node
        string name = string.Empty;

        // Cached for performance
        int indexInParent = -1;
        int[] indexPathFromRoot;

        /// <summary>
        /// Standard constructor
        /// </summary>
        protected TemplateXmlNode(TemplateTranslationContext context)
            : this(context, string.Empty)
        {

        }

        /// <summary>
        /// Standard constructor
        /// </summary>
        protected TemplateXmlNode(TemplateTranslationContext context, string name)
        {
            this.context = context;
            this.nameTable = context.NameTable;

            this.parentNode = null;
            this.isRootNode = this.GetType() == typeof(TemplateXmlRoot);

            this.name = (name.Length > 0) ? nameTable.Add(name) : name;
        }

        /// <summary>
        /// The parent of this node
        /// </summary>
        public TemplateXmlNode ParentNode
        {
            get 
            { 
                return parentNode; 
            }
            set
            {
                if (isRootNode)
                {
                    throw new InvalidOperationException("Cannot set ParentNode on Root");
                }

                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (this.parentNode != null)
                {
                    throw new InvalidOperationException("Cannot change ParentNode once it has been set.");
                }

                this.parentNode = value;
            }
        }

        /// <summary>
        /// Get the index of the node within its parent collection
        /// </summary>
        public int IndexInParent
        {
            get
            {
                if (indexPathFromRoot == null)
                {
                    DetermineIndex();
                }

                return indexInParent;
            }
        }

        /// <summary>
        /// Get the full index path from the root to this node.  This helps determine node ordering.
        /// </summary>
        public int[] IndexPathFromRoot
        {
            get 
            {
                if (indexPathFromRoot == null)
                {
                    DetermineIndex();
                }

                return indexPathFromRoot; 
            }
        }

        /// <summary>
        /// Determine the index information of this node
        /// </summary>
        private void DetermineIndex()
        {
            if (parentNode == null)
            {
                if (!isRootNode)
                {
                    throw new InvalidOperationException("The ParentNode has not yet been established.");
                }

                indexInParent = -1;
                indexPathFromRoot = new int[0];
            }
            else
            {
                // Determine the index of this element relevant to the parent
                if (NodeType == XPathNodeType.Attribute)
                {
                    indexInParent = parentNode.Attributes.IndexOf(this);
                }
                else
                {
                    indexInParent = parentNode.ChildNodes.IndexOf(this);
                }

                // Copy the existing path from the parent
                indexPathFromRoot = new int[parentNode.IndexPathFromRoot.Length + 1];
                Array.Copy(parentNode.IndexPathFromRoot, indexPathFromRoot, parentNode.IndexPathFromRoot.Length);

                // For node-order purposes, elements of a parent come after attributes of a parent.  We make attribute indexes in the negatives, so our elements
                // can always start at zero, and we don't have to count the attributes (and force their creation) in order to get the element starting point.
                if (NodeType == XPathNodeType.Attribute)
                {
                    int shift = 100;
                    Debug.Assert(indexInParent < shift);

                    // Ideally we would shift left by the actual number of attributes, so that attribute indexs (0, 1, 2) shifted to be (-2, -1, 0).  But, 
                    // that would require knowing how many there were - which we require forcing their creation.  So... instead we just shift by 100, which assumes
                    // there would never be more than 100 attributes on a single element.
                    indexPathFromRoot[indexPathFromRoot.Length - 1] = indexInParent - shift;
                }
                else
                {
                    indexPathFromRoot[indexPathFromRoot.Length - 1] = indexInParent;
                }
            }
        }

        /// <summary>
        /// The NameTable to be used
        /// </summary>
        public NameTable NameTable
        {
            get { return nameTable; }
        }

        /// <summary>
        /// The translation context
        /// </summary>
        public TemplateTranslationContext Context
        {
            get { return context; }
        }

        /// <summary>
        /// Gets the XPathNodeType of this node.
        /// </summary>
        public abstract XPathNodeType NodeType { get; }

        /// <summary>
        /// The Value of the node.  Should only apply for Text and Attribute nodes.
        /// </summary>
        public abstract string Value { get; }

        /// <summary>
        /// The LocalName (no namespace prefix) of the current node
        /// </summary>
        public string LocalName
        {
            get 
            {
                return name; 
            }
        }

        /// <summary>
        /// Gets the list of child nodes
        /// </summary>
        public virtual TemplateXmlNodeList ChildNodes
        {
            get { return noChildren; }
        }

        /// <summary>
        /// Gets the list of attributes of the node
        /// </summary>
        public virtual TemplateXmlNodeList Attributes
        {
            get { return noAttributes; }
        }

        /// <summary>
        /// Convert the given value to its standard XML representation
        /// </summary>
        protected static string ConvertToXmlValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            switch (value.GetType().Name)
            {
                case "String": return (string) value;
                case "Boolean": return XmlConvert.ToString((bool) value);
                case "Int32": return XmlConvert.ToString((int) value);
                case "Int64": return XmlConvert.ToString((long) value);
                case "Double": return XmlConvert.ToString((double) value);
                case "Decimal": return XmlConvert.ToString((decimal) value);
                case "DateTime": return XmlConvert.ToString((DateTime) value, XmlDateTimeSerializationMode.Utc);
            }

            if (value is DateTime?)
            {
                DateTime? dateTime = (DateTime?) value;
                if (dateTime.HasValue)
                {
                    return XmlConvert.ToString((DateTime) value, XmlDateTimeSerializationMode.Utc);
                }
                else
                {
                    return null;
                }
            }

            throw new InvalidOperationException("ConvertToXmlValue does not support conversion to type: " + value.GetType());
        }
    }
}
