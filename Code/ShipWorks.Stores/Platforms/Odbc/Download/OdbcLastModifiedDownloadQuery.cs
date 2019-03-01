using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Download Query where the results are limited to rows greater than the online last modified date
    /// </summary>
    public class OdbcLastModifiedDownloadQuery : IOdbcQuery
    {
        private readonly OdbcColumnSourceType importColumnSourceType;
        private readonly IOdbcQuery downloadQuery;
        private readonly DateTime onlineLastModifiedStartingPoint;
        private readonly string lastModifiedColumnName;
        private readonly string quotedLastModifiedColumnName;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcLastModifiedDownloadQuery(OdbcColumnSourceType importColumnSourceType,
            IOdbcQuery downloadQuery,
            DateTime onlineLastModifiedStartingPoint,
            string lastModifiedColumnName,
            string quotedLastModifiedColumnName)
        {
            this.importColumnSourceType = importColumnSourceType;
            this.downloadQuery = downloadQuery;
            this.onlineLastModifiedStartingPoint = onlineLastModifiedStartingPoint;
            this.lastModifiedColumnName = lastModifiedColumnName;
            this.quotedLastModifiedColumnName = quotedLastModifiedColumnName;
        }

        /// <summary>
        /// Makes the downloadQuery a sub query of limits the results to
        /// OnlineLastModified greater than the onlineLastModifiedStartingPoint
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ShipWorksOdbcException">The OnlineLastModified column must be mapped to download by OnlineLastModified.</exception>
        public string GenerateSql()
        {
            // If the online last modified column is not mapped we cannot generate the query
            if (string.IsNullOrWhiteSpace(lastModifiedColumnName))
            {
                throw new ShipWorksOdbcException("The OnlineLastModified column must be mapped to download by OnlineLastModified.");
            }

            if (importColumnSourceType == OdbcColumnSourceType.CustomParameterizedQuery)
            {
                return downloadQuery.GenerateSql();
            }

            // Generate the query
            return $@"SELECT sub.* FROM({downloadQuery.GenerateSql()}) sub WHERE {quotedLastModifiedColumnName} > ? ORDER BY {quotedLastModifiedColumnName} ASC";
        }

        /// <summary>
        /// Sets the command text property of the command
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        public void ConfigureCommand(IShipWorksOdbcCommand command)
        {
            command.ChangeCommandText(GenerateSql());

            OdbcParameter param = new OdbcParameter(quotedLastModifiedColumnName, OdbcType.DateTime, 23, ParameterDirection.Input, false, 1, 3, quotedLastModifiedColumnName, DataRowVersion.Current, onlineLastModifiedStartingPoint);
            command.AddParameter(param);
        }
    }
}
