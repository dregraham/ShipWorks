using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers
{
    /// <summary>
    /// Finds the field in the entity and returns its value. If the field doesn't coorespond to the entity or the
    /// field isn't found, null is returned.
    /// </summary>
    public class OdbcDefaultFieldValueResolver : IOdbcFieldValueResolver
    {
        /// <summary>
        /// Get a value for the specified field from the given entity.
        /// </summary>
        public object GetValue(IShipWorksOdbcMappableField field, IEntity2 entity)
        {
            IEntityField2 entityField = null;
            if (entity.LLBLGenProEntityName == field.ContainingObjectName)
            {
                entityField = entity.Fields[field.Name];
            }

            return entityField?.CurrentValue;
        }
    }
}