using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Loads attributes into an item
    /// </summary>
    public class OdbcItemAttributeLoader : IOdbcItemAttributeLoader
    {
        /// <summary>
        /// Load the item attributes from the given map into the given item entity
        /// </summary>
        public void Load(IOdbcFieldMap map, OrderItemEntity item)
        {
            if (item != null)
            {
                IEnumerable<IOdbcFieldMapEntry> itemEntries = map.FindEntriesBy(OrderItemAttributeFields.Name, false);

                foreach (IOdbcFieldMapEntry entry in itemEntries)
                {
                    AddItemAttributeToItem(item, entry);
                }
            }
        }

        /// <summary>
        /// Adds an item attribute to an item.
        /// </summary>
        private void AddItemAttributeToItem(OrderItemEntity item, IOdbcFieldMapEntry entry)
        {
            OrderItemAttributeEntity attribute = new OrderItemAttributeEntity(item);
            attribute.Name = entry.ExternalField.Column.Name;
            attribute.Description = entry.ShipWorksField.Value.ToString();
        }
    }
}