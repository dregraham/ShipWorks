﻿using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Provides a mechanism for interacting with an external ODBC field
    /// </summary>
    public interface IExternalOdbcMappableField : IOdbcMappableField
    {
        /// <summary>
        /// The External Column
        /// </summary>
        [Obfuscation(Exclude = true)]
        OdbcColumn Column { get; set; }

        /// <summary>
        /// The External Table
        /// </summary>
        [Obfuscation(Exclude = true)]
        IOdbcTable Table { get; set; }

        /// <summary>
        /// Loads the given record
        /// </summary>
        void LoadValue(OdbcRecord record);

        /// <summary>
        /// Resets the value
        /// </summary>
        void ResetValue();
    }
}