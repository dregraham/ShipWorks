using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers
{
    /// <summary>
    /// Represents an OdbcField Value Resolver
    /// </summary>
    public interface IOdbcFieldValueResolver
    {
        /// <summary>
        /// Get a value for the specific field from the given entity. 
        /// If resolver cannot find the correct value, null is returned.
        /// </summary>
        object GetValue(IShipWorksOdbcMappableField field, IEntity2 entity);
    }
}