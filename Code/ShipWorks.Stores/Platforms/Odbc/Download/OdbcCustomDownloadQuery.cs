﻿using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Download query to be used when user has entered a custom query
    /// </summary>
    public class OdbcCustomDownloadQuery : IOdbcQuery
    {
        private readonly OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomDownloadQuery"/> class.
        /// </summary>
        public OdbcCustomDownloadQuery(OdbcStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Generates the Sql to download orders.
        /// </summary>
        public string GenerateSql() => store.ImportColumnSource;

        /// <summary>
        /// Populate the command test property of the command
        /// </summary>
        public void ConfigureCommand(IShipWorksOdbcCommand command)
        {
            command.ChangeCommandText(GenerateSql());
        }
    }
}