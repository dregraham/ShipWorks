using Interapptive.Shared.UI;
using log4net;
using System;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Saves and Opens Odbc Upload Settings from disk.
    /// </summary>
    public class OdbcUploadSettingsFile : OdbcSettingsFile
    {
        /// <summary>
        /// The action to perform on this file (Upload)
        /// </summary>
        public override string Action => "Upload";

        /// <summary>
        /// The file extension.
        /// </summary>
        public override string Extension => ".swoum";

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploadSettingsFile"/> class.
        /// </summary>
        public OdbcUploadSettingsFile(IOdbcFieldMap fieldMap, IMessageHelper messageHelper, Func<Type, ILog> logFactory)
            : base(fieldMap, messageHelper, logFactory(typeof(OdbcUploadSettingsFile)))
        {
        }
    }
}