using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using log4net;
using Interapptive.Shared.Data;

namespace ShipWorks.Data.Administration.UpdateFrom2x.LegacyCode
{
    /// <summary>
    /// Utility class for working with ShipWorks2x archives
    /// </summary>
    public static class ShipWorks2xArchiveUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorks2xArchiveUtility));

        /// <summary>
        /// Get the list of archive database names from the database on the given connection.  If its not a ShipWorks database, 
        /// or has no archives, an empty list is returned.
        /// </summary>
        public static List<string> GetArchiveDatabaseNames(SqlConnection con)
        {
            List<string> names = new List<string>();

            if (Convert.ToInt32(SqlCommandProvider.ExecuteScalar(con, "SELECT COALESCE(OBJECT_ID('ArchiveSets', 'TABLE'), -1)")) > 0)
            {
                log.InfoFormat("Found ArchiveSets table...");

                // autocreate:temp was used by sw2 to mark a placeholder FK archive _within_ another archive db
                SqlCommand cmdArchives = SqlCommandProvider.Create(con);
                cmdArchives.CommandText = "SELECT DbName FROM ArchiveSets WHERE ArchiveSetName != 'autocreate:temp'";

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmdArchives))
                {
                    while (reader.Read())
                    {
                        string dbName = (string) reader["DbName"];
                        names.Add(dbName);

                        log.InfoFormat("Found archive database name {0}", dbName);
                    }
                }
            }

            return names;
        }

    }
}
