using System.Reflection;
using SD.LLBLGen.Pro.ORMSupportClasses;

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
        /// Index to help identify the entry
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Loads the given ODBC Record into the External Field
        /// </summary>
        void LoadExternalField(OdbcRecord record);

        /// <summary>
        /// Loads the given entity into the ShipWorksField
        /// </summary>
        void LoadShipWorksField(IEntity2 entity);

        /// <summary>
        /// Copies the Value from the external field to the ShipWorks field
        /// </summary>
        void CopyExternalValueToShipWorksField();
    }
}