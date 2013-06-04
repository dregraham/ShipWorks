using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Base class for grouping contexts, which represent the ways in which a multi-select gird can be grouped by
    /// </summary>
    public class GroupingContext
    {
        List<long> keys;
        EntityField2 groupByField;
        Func<EntityBase2, string> headerTextCallback;

        /// <summary>
        /// Constructor
        /// </summary>
        public GroupingContext(List<long> keys, EntityField2 groupByField, Func<EntityBase2, string> headerTextCallback)
        {
            this.keys = keys;
            this.groupByField = groupByField;
            this.headerTextCallback = headerTextCallback;
        }

        /// <summary>
        /// The ordered list of keys to group by
        /// </summary>
        public List<long> GroupingKeys
        {
            get { return keys; }
        }

        /// <summary>
        /// Get the field to be grouped by
        /// </summary>
        public EntityField2 GroupByField
        {
            get { return (EntityField2) groupByField.Clone(); }
        }

        /// <summary>
        /// Get the parent ID that the given child is grouped by
        /// </summary>
        public long GetEntityGroupID(EntityBase2 childEntity)
        {
            return (long) childEntity.Fields[groupByField.FieldIndex].CurrentValue;
        }

        /// <summary>
        /// Get the header to display for the givnen grouping entity
        /// </summary>
        public string GetGroupHeader(EntityBase2 groupEntity)
        {
            return headerTextCallback(groupEntity);
        }

        /// <summary>
        /// Convenience property to factor out common order header text formatting into common static method
        /// </summary>
        public static Func<EntityBase2, string> OrderHeaderText
        {
            get
            {
                return entity => string.Format("Order {0}", ((OrderEntity) entity).OrderNumberComplete);
            }
        }
    }
}
