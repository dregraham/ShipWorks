﻿using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers;
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
        /// Index to help identify the entry
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Loads the given ODBC Record into the External Field
        /// </summary>
        void LoadExternalField(OdbcColumn record);

        /// <summary>
        /// Loads the given entity into the ShipWorksField
        /// </summary>
        void LoadShipWorksField(IEntity2 entity, IOdbcFieldValueResolver valueResolver);

        /// <summary>
        /// Copies the Value from the external field to the ShipWorks field
        /// </summary>
        void CopyExternalValueToShipWorksField();

        /// <summary>
        /// Loads the given ODBC Column into the External Field
        /// </summary>
        void LoadExternalField(OdbcRecord record);
    }
}