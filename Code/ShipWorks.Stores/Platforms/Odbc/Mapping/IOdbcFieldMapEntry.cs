using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Contains a mapping between a ExternalOdbcMappableField and ShipWorksOdbcMappableField
    /// </summary>
    [Obfuscation(Exclude = true)]
    public interface IOdbcFieldMapEntry
    {
        /// <summary>
        /// Mapped External Field
        /// </summary>
        IExternalOdbcMappableField ExternalField { get; }

        /// <summary>
        /// Mapped Shipworks field
        /// </summary>
        IShipWorksOdbcMappableField ShipWorksField { get; }

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