using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using log4net;
using Interapptive.Shared.Data;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database
{
    /// <summary>
    /// A ShipWorks V2 Archive Set
    /// </summary>
    public class ArchiveSet
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(ArchiveSet));

        public string ArchiveSetName { get; set; }
        public string DBName { get; set; }
        public bool CanConnect { get; set; }

        /// <summary>
        /// Testst he connectivity to the Archive Database.  No exceptions thrown here,
        /// only CanConnect gets set.
        /// </summary>
        public void TestConnectivity(SqlConnection con)
        {
            CanConnect = false;

            // connect to this database
            string originalDatabase = con.Database;
            try
            {
                log.InfoFormat("Connecting to Archive Database '{0}'", DBName);
                con.ChangeDatabase(DBName);

                // do a quick call to GetSchemaVersion since it always exists
                using (SqlCommand command = SqlCommandProvider.Create(con))
                {
                    command.CommandText = "GetSchemaVersion";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlCommandProvider.ExecuteScalar(command);

                    // success!
                    log.InfoFormat("Successfully connected to Archive Database '{0}'", DBName);
                    CanConnect = true;
                }
            }
            catch (ArgumentException ex)
            {
                // this happens if the database name is invalid
                log.ErrorFormat("Invalid Arhive Database name '{0}':{1}", DBName, ex.Message);
            }
            catch (SqlException ex)
            {
                log.ErrorFormat("Unable to connect to the Archive Database '{0}':{1}", DBName, ex.Message);
            }
            finally
            {
                con.ChangeDatabase(originalDatabase);
            }
        }
    }
}
