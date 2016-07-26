using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Represents an OdbcField Value Resolver
    /// </summary>
    public interface IOdbcFieldValueResolver
    {
        /// <summary>
        /// Get a value for the specific field from the given entity
        /// </summary>
        object GetValue(IShipWorksOdbcMappableField field, IEntity2 entity);
    }
}