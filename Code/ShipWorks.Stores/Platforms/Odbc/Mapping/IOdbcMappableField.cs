using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Interface for a mappable field, which will be part of a field map entry
    /// </summary>
    public interface IOdbcMappableField
    {
        /// <summary>
        /// Gets the qualified name for the field - table.column
        /// </summary>
        string QualifiedName { get; }

        /// <summary>
        /// The fields value
        /// </summary>
        object Value { get; }

        /// <summary>
        /// The fields display name
        /// </summary>
        [Obfuscation(Exclude = true)]
        string DisplayName { get; }
	}
}
