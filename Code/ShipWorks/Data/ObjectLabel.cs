using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    /// <summary>
    /// Represents a label and information of an object
    /// </summary>
    public class ObjectLabel
    {
        ObjectLabelEntity label;

        /// <summary>
        /// Prefix formatting. Uses a list instead of a dictionary b\c this is also the sort order in the EntityGridColumn
        /// </summary>
        static List<KeyValuePair<EntityType, string>> prefixFormats = new List<KeyValuePair<EntityType, string>>();

        /// <summary>
        /// Static constructor
        /// </summary>
        static ObjectLabel()
        {
            prefixFormats.Add(new KeyValuePair<EntityType, string>(EntityType.StoreEntity, "Store {0}"));
            prefixFormats.Add(new KeyValuePair<EntityType, string>(EntityType.TemplateEntity, "Template {0}"));
            prefixFormats.Add(new KeyValuePair<EntityType, string>(EntityType.CustomerEntity, "{0}"));
            prefixFormats.Add(new KeyValuePair<EntityType, string>(EntityType.OrderEntity, "Order {0}"));
            prefixFormats.Add(new KeyValuePair<EntityType, string>(EntityType.OrderItemEntity, "Item '{0}'"));
            prefixFormats.Add(new KeyValuePair<EntityType, string>(EntityType.OrderItemAttributeEntity, "Attribute '{0}'"));
            prefixFormats.Add(new KeyValuePair<EntityType, string>(EntityType.OrderChargeEntity, "Charge '{0}'"));
            prefixFormats.Add(new KeyValuePair<EntityType, string>(EntityType.ShipmentEntity, "Shipment ({0})"));
            prefixFormats.Add(new KeyValuePair<EntityType, string>(EntityType.NoteEntity, "Note ({0})"));
        }

        /// <summary>
        /// A label for an existing object
        /// </summary>
        public ObjectLabel(ObjectLabelEntity label)
        {
            this.label = label;
        }

        /// <summary>
        /// The prefixes for each entity type as they will be displayed in the grid
        /// </summary>
        public static List<EntityType> SortOrderByEntity
        {
            get { return prefixFormats.Select(p => p.Key).ToList(); }
        }

        /// <summary>
        /// Indicates if the label represents an object that is deleted
        /// </summary>
        public bool IsDeleted
        {
            get { return label.IsDeleted; }
        }

        /// <summary>
        /// Indicates if the label represents an item that is new
        /// </summary>
        public bool IsNew
        {
            get { return label.EntityID < 0; }
        }

        /// <summary>
        /// The parent, if any, of the label
        /// </summary>
        public long? ParentID
        {
            get { return label.ParentID; }
        }

        /// <summary>
        /// Get the text of the label, only as its stored in the database.
        /// </summary>
        public string ShortText
        {
            get { return label.Label; }
        }

        /// <summary>
        /// The long text of the label, include type prefix, owner information, and if its deleted
        /// </summary>
        public string LongText
        {
            get { return GetCustomText(true, true, true); }
        }

        /// <summary>
        /// Get a custom text of the label, optionally including owner label information as well as text sayings its deleted
        /// </summary>
        public string GetCustomText(bool includeTypePrefix, bool includeParent, bool includeDeleted)
        {
            string text = GetTextWithPrefix(label, includeTypePrefix);

            if (includeParent)
            {
                string parentText = GetParentText(label.EntityID, label.ParentID, includeTypePrefix);

                if (!string.IsNullOrEmpty(parentText))
                {
                    text += ", " + parentText;
                }
            }

            if (includeDeleted && IsDeleted)
            {
                text += " (Deleted)";
            }

            return text;
        }

        /// <summary>
        /// Get the text of the parents of the current label
        /// </summary>
        private string GetParentText(long entityID, long? parentID, bool includeTypePrefix)
        {
            if (parentID == null)
            {
                return string.Empty;
            }

            // We don't show the owner customer for an order
            if (EntityUtility.GetEntityType(entityID) == EntityType.OrderEntity)
            {
                return string.Empty;
            }

            ObjectLabel parent = ObjectLabelManager.GetLabel(parentID.Value);

            // Get the plain text of this owner
            string parentText = GetTextWithPrefix(parent.label, includeTypePrefix);

            // See if the parent itself has an parent.
            string grandParentText = GetParentText(parentID.Value, parent.label.ParentID, includeTypePrefix);

            // If it does, it goes first
            if (!string.IsNullOrEmpty(grandParentText))
            {
                return string.Format("{0}: {1}", grandParentText, parentText);
            }
            else
            {
                return parentText;
            }
        }

        /// <summary>
        /// Get the text of the label, optionally including the prefix
        /// </summary>
        private static string GetTextWithPrefix(ObjectLabelEntity label, bool includeTypePrefix)
        {
            // If its new, or expclitly asked not to, don't include prefix
            if (label.EntityID < 0 || !includeTypePrefix)
            {
                return label.Label;
            }

            EntityType entityType = EntityUtility.GetEntityType(label.EntityID);

            bool hasFormat = prefixFormats.Any(p => p.Key == entityType);

            if (!hasFormat)
            {
                throw new InvalidOperationException(string.Format("No ObjectLabel support added for {0} in code yet.", label.EntityID));
            }

            return string.Format(prefixFormats.Single(p => p.Key == entityType).Value, label.Label);
        }
    }
}
