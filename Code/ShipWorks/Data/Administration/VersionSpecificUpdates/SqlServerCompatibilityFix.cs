using ShipWorks.Data.Connection;
using System;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// Fix an issue found with Sql Server 2014 RTM which causes the retrieval of StoreEntity collections to take 10 minutes
    /// </summary>
    class SqlServerCompatibilityFix : IVersionSpecificUpdate
    {
        /// <summary>
        /// Always run this update
        /// </summary>
        public bool AlwaysRun => true;

        /// <summary>
        /// Always do this check
        /// </summary>
        public Version AppliesTo => new Version(int.MaxValue, int.MaxValue);

        /// <summary>
        /// Update the Shipworks database and set its compatibility level
        /// </summary>
        /// <remarks>
        /// Set the compatibility level to SQL2012 only if they are on SQL2014 RTM due to a performance bug caused by the size of our StoreEntity hierarchy
        /// Set the compatibility level to SQL2014 if they are on anything other than RTM because the bug does not exist outside of 2014 RTM
        /// </remarks>
        public void Update()
        {
            // Grab the Sql version, this query below is only compatible with SQL Server 2014 and newer
            string sqlVersion = string.Empty;
            ExistingConnectionScope.ExecuteWithCommand(cmd =>
            {
                cmd.CommandText = @"SELECT SERVERPROPERTY('productversion') as version;";
                sqlVersion = cmd.ExecuteScalar().ToString();
            });

            // Only run the following query if we are on sql 2014 because THE FOLLOWING QUERY IS NOT SYNTACTICALLY CORRECT with any version lower than 2014
            // if the following query is run on a version lower than 2014 it will error out even if the conditions to execute the alter statement are not met
            // this is because SQL Server validates the query prior to executing it and COMPATIBILITY_LEVEL = 120 is not a valid compatibility level for any
            // version lower than 2014
            if (sqlVersion.StartsWith("12.", StringComparison.InvariantCulture))
            {
                ExistingConnectionScope.ExecuteWithCommand(cmd =>
                {
                    cmd.CommandText = @"
                    DECLARE @version NVARCHAR(20) = CONVERT(VARCHAR(20),SERVERPROPERTY('productversion'));
                    DECLARE @productlevel NVARCHAR(20) = CONVERT(VARCHAR(20),SERVERPROPERTY('ProductLevel'));
                    DECLARE @dbName NVARCHAR(100) = DB_NAME();

                    DECLARE @Sql NVARCHAR(500) = 'IF '''+ @version + ''' LIKE ''12%'' AND ''' + @productlevel + ''' = ''RTM''
						ALTER DATABASE [' + @dbName + ']
							SET COMPATIBILITY_LEVEL = 110

					IF '''+ @version + ''' LIKE ''12%'' AND ''' + @productlevel + ''' != ''RTM''
						ALTER DATABASE [' + @dbName + ']
								SET COMPATIBILITY_LEVEL = 120'
                    EXECUTE sp_executesql @Sql;
                ";
                    cmd.ExecuteNonQuery();
                });
            }
        }
    }
}
