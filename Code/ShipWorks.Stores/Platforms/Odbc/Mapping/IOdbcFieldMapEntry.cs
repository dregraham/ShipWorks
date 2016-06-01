using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Contains a mapping between a ExternalOdbcMappableField and ShipWorksOdbcMappableField
    /// </summary>
    public interface IOdbcFieldMapEntry
    {
        /// <summary>
        /// Mapped External Field
        /// </summary>
        [Obfuscation(Exclude = true)]
        ExternalOdbcMappableField ExternalField { get; }

        /// <summary>
        /// Mapped Shipworks field
        /// </summary>
        [Obfuscation(Exclude = true)]
        ShipWorksOdbcMappableField ShipWorksField { get; }

        /// <summary>
        /// Loads the given ODBC Record into the External Field
        /// </summary>
        void LoadExternalField(OdbcRecord record);

        /// <summary>
        /// Copies the Value from the external field to the ShipWorks field
        /// </summary>
        void CopyValueToShipWorksField();
    }
}