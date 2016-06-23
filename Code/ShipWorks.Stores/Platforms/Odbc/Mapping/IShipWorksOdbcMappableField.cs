using System.Reflection;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// The ShipWorks half of an OdbcFieldMapEntry
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.Odbc.Mapping.IOdbcMappableField" />
    public interface IShipWorksOdbcMappableField: IOdbcMappableField
    {
        /// <summary>
        /// The name of the object that contains this field
        /// </summary>
        string ContainingObjectName { get; }

        /// <summary>
        /// Is the field required to be mapped.
        /// </summary>
        [Obfuscation(Exclude = true)]
        bool IsRequired { get; }

        /// <summary>
        /// The name of the field
        /// </summary>
        [Obfuscation(Exclude = true)]
        string Name { get; }

        /// <summary>
        /// Set the Value to the given value
        /// </summary>
        void LoadValue(object value);

        /// <summary>
        /// Gets the field.
        /// </summary>
        IEntityField2 Field { get; }
    }
}