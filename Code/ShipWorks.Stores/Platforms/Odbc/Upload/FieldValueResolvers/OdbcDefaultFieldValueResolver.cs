using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers
{
    /// <summary>
    /// Field value resolver for standard fields
    /// </summary>
    public class OdbcDefaultFieldValueResolver : IOdbcFieldValueResolver
    {
        /// <summary>
        /// Get a value for the specific field from the given entity
        /// </summary>
        public object GetValue(IShipWorksOdbcMappableField field, IEntity2 entity)
        {
            IEntityField2 entityField = entity.Fields[field.Name];

            return entityField?.CurrentValue;
        }
    }
}