using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Filters.Content;
using System.Xml;
using System.IO;
using ShipWorks.Filters.Content.Conditions.Special;
using System.Xml.XPath;
using Interapptive.Shared;
using System.Diagnostics;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores;
using System.Linq;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// The object model of a filter definition, which is the set of conditions that make up a filter.
    /// </summary>
    public class FilterDefinition
    {
        ConditionGroupContainerRoot root;

        // The target of the definition
        FilterTarget target;

        #region class ConditionGroupContainerRoot

        // A special container we use as the root
        class ConditionGroupContainerRoot : ConditionGroupContainer
        {
            ConditionEntityTarget entityTarget;

            /// <summary>
            /// Constructor
            /// </summary>
            public ConditionGroupContainerRoot(ConditionEntityTarget entityTarget)
            {
                this.entityTarget = entityTarget;
            }

            /// <summary>
            /// Get the entity target in scope
            /// </summary>
            public override ConditionEntityTarget GetScopedEntityTarget()
            {
                return entityTarget;
            }
        }

        #endregion

        /// <summary>
        /// Creates a new, empty filter definition
        /// </summary>
        public FilterDefinition(FilterTarget target)
        {
            this.target = target;
            root = new ConditionGroupContainerRoot(GetEntityTarget(target));
            root.SecondGroup = new ConditionGroupContainer(new ConditionGroup());
        }

        /// <summary>
        /// Loads a filter condition from the specified XML.
        /// </summary>
        public FilterDefinition(string xml)
        {
            LoadXml(xml);
        }

        /// <summary>
        /// Load the filter definition object from the given XML
        /// </summary>
        private void LoadXml(string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            XPathNavigator xpath = xmlDocument.CreateNavigator();
            xpath.MoveToFirstChild();

            // Read the filter target
            target = (FilterTarget) XPathUtility.Evaluate(xpath, "@Target", 0);

            // Create the root condition and load it
            root = new ConditionGroupContainerRoot(GetEntityTarget(target));
            root.SecondGroup = new ConditionGroupContainer();
            root.SecondGroup.DeserializeXml(xpath);
        }

        /// <summary>
        /// Get this filter definition in XML form.
        /// </summary>
        public string GetXml()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.Unicode))
                {
                    xmlWriter.Formatting = Formatting.Indented;

                    xmlWriter.WriteStartDocument(true);
                    xmlWriter.WriteStartElement("FilterDefinition");
                    xmlWriter.WriteAttributeString("Target", ((int) target).ToString());

                    RootContainer.SerializeXml(xmlWriter);

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();

                    xmlWriter.Flush();

                    // Move the stream to the beginning
                    stream.Seek(0, SeekOrigin.Begin);

                    // Read it back
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Get the EntityTarget from the given FilterTarget
        /// </summary>
        private ConditionEntityTarget GetEntityTarget(FilterTarget target)
        {
            switch (target)
            {
                case FilterTarget.Customers: return ConditionEntityTarget.Customer;
                case FilterTarget.Orders: return ConditionEntityTarget.Order;
                case FilterTarget.Shipments: return ConditionEntityTarget.Shipment;
                case FilterTarget.Items: return ConditionEntityTarget.OrderItem;
            }

            throw new InvalidOperationException("Unhandled FilterTarget type " + target);
        }

        /// <summary>
        /// The FilterTarget the definition applies to
        /// </summary>
        public FilterTarget FilterTarget
        {
            get
            {
                return target;
            }
        }

        /// <summary>
        /// The root container of the filter definition
        /// </summary>
        public ConditionGroupContainer RootContainer
        {
            get
            {
                return root.SecondGroup;
            }
            set
            {
                root.SecondGroup = value;
            }
        }

        /// <summary>
        /// Indiciates if no conditions are configured and the definition should match zero content.
        /// </summary>
        public bool IsEmpty()
        {
            return IsEmpty(RootContainer);
        }

        /// <summary>
        /// Indiciates if the given container has no real conditions.
        /// </summary>
        private bool IsEmpty(ConditionGroupContainer groupContainer)
        {
            if (groupContainer == null)
            {
                return true;
            }

            return IsEmpty(groupContainer.FirstGroup) && IsEmpty(groupContainer.SecondGroup);
        }

        /// <summary>
        /// Indiciates if the given group has no real conditions.
        /// </summary>
        private bool IsEmpty(ConditionGroup group)
        {
            if (group == null)
            {
                return true;
            }

            if (group.Conditions.Count == 0)
            {
                return true;
            }

            // May have container conditions though, that are themselves empty
            foreach (Condition condition in group.Conditions)
            {
                ContainerCondition container = condition as ContainerCondition;
                if (container != null)
                {
                    if (!IsEmpty(container.Container))
                    {
                        return false;
                    }
                }
                else
                {
                    // The NotSupportedV2Condition counts as empty
                    if (condition.GetType() != typeof(NotSupportedV2Condition))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Indicates if the definition is relevant given the list of storetypes.  If for example the definition contained an eBay only condition,
        /// but there was no ebay store type, then it's not relevant.
        /// </summary>
        public bool IsRelevantToStoreTypes(List<StoreTypeCode> storeTypes)
        {
            return IsRelevantToStoreTypes(RootContainer, storeTypes);
        }

        /// <summary>
        /// Returns true if the given container is relevant given the specified list of store types
        /// </summary>
        private bool IsRelevantToStoreTypes(ConditionGroupContainer groupContainer, List<StoreTypeCode> storeTypes)
        {
            if (groupContainer == null)
            {
                return true;
            }

            return IsRelevantToStoreTypes(groupContainer.FirstGroup, storeTypes) && IsRelevantToStoreTypes(groupContainer.SecondGroup, storeTypes);
        }

        /// <summary>
        /// Returns false if the given group has any conditions that are store specific but not in the storeTypes list
        /// </summary>
        private bool IsRelevantToStoreTypes(ConditionGroup group, List<StoreTypeCode> storeTypes)
        {
            foreach (Condition condition in group.Conditions)
            {
                ContainerCondition container = condition as ContainerCondition;
                if (container != null)
                {
                    if (!IsRelevantToStoreTypes(container.Container, storeTypes))
                    {
                        return false;
                    }
                }

                ConditionElementDescriptor descriptor = ConditionElementFactory.GetDescriptor(condition.GetType());
                if (descriptor.StoreTypes.Count > 0 && !descriptor.StoreTypes.Intersect(storeTypes).Any())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if the filter contains a date condition that uses a relative operator
        /// </summary>
        public bool HasRelativeDateCondition()
        {
            return HasRelativeDateCondition(RootContainer);
        }

        /// <summary>
        /// Returns true if the group container contains a date condition that uses a relative operator
        /// </summary>
        private bool HasRelativeDateCondition(ConditionGroupContainer groupContainer)
        {
            if (groupContainer == null)
            {
                return false;
            }

            return HasRelativeDateCondition(groupContainer.FirstGroup) || HasRelativeDateCondition(groupContainer.SecondGroup);
        }

        /// <summary>
        /// Returns true if the group contains a date condition that uses a relative operator
        /// </summary>
        private bool HasRelativeDateCondition(ConditionGroup group)
        {
            foreach (Condition condition in group.Conditions)
            {
                DateCondition dateCondition = condition as DateCondition;
                if (dateCondition != null)
                {
                    if (dateCondition.IsRelative())
                    {
                        return true;
                    }
                }

                ContainerCondition container = condition as ContainerCondition;
                if (container != null)
                {
                    if (HasRelativeDateCondition(container.Container))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
