using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc.Loaders
{
    /// <summary>
    /// Loads attributes into an item
    /// </summary>
    public class OdbcItemAttributeLoader : IOdbcItemAttributeLoader
    {
        /// <summary>
        /// Load the item attributes from the given map into the given item entity
        /// </summary>
        public void Load(IOdbcFieldMap map, OrderItemEntity item, int index)
        {
            MethodConditions.EnsureArgumentIsNotNull(item);

            IEnumerable<IOdbcFieldMapEntry> itemEntries =
                map.FindEntriesBy(OrderItemAttributeFields.Name, false).Where(e => e.Index == index);

            foreach (IOdbcFieldMapEntry entry in itemEntries)
            {
                OrderItemAttributeEntity attribute = new OrderItemAttributeEntity(item);
                attribute.Name = entry.ExternalField.Column.Name;
                attribute.Description = entry.ShipWorksField.Value.ToString();
            }
        }
    }
}