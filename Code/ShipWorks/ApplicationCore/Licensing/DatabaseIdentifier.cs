using System;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class DatabaseIdentifier : IDatabaseIdentifier
    {

        private Guid? identifier;

        /// <summary>
        /// Returns DatabaseGuid from database.
        /// </summary>
        public Guid Get()
        {
            try
            {


                if (identifier == null)
                {
                    using (SqlConnection con = SqlSession.Current.OpenConnection())
                    {
                        identifier = SqlCommandProvider.ExecuteScalar<Guid>(con, "exec GetDatabaseGuid");
                    }
                }

                return identifier.Value;
            }
            catch (Exception ex)
            {
                throw new DatabaseIdentifierException(ex);
            }
        }
    }
}