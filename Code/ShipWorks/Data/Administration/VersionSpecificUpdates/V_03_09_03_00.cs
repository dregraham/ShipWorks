using System;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// ShipWorks update that should be applied for a specific version
    /// </summary>
    /// <remarks>
    /// If we were upgrading from 3.9.3.0 or before we adjust the FILEGROW settings.  Can't be in a transaction, so has to be here.
    /// </remarks>
    public class V_03_09_03_00 : IVersionSpecificUpdate
    {
        /// <summary>
        /// To which version does this update apply
        /// </summary>
        public Version AppliesTo => new Version(3, 9, 3, 0);

        /// <summary>
        /// Execute the update
        /// </summary>
        public void Update()
        {
            ExistingConnectionScope.ExecuteWithCommand(cmd =>
            {
                cmd.CommandText = @"
                    DECLARE @dbName nvarchar(100)
                    DECLARE @isAutoShrink int

                    SET @dbName = DB_NAME()

                    SELECT @isAutoShrink = CONVERT(int,DATABASEPROPERTYEX([Name] , 'IsAutoShrink'))
                    FROM master.dbo.sysdatabases
                    where name = @dbName

                    IF(@isAutoShrink = 1)
                        EXECUTE ('ALTER DATABASE ' + @dbName + ' SET AUTO_SHRINK OFF')";

                cmd.ExecuteNonQuery();
            });

            // Update size and growth of shipworks database
            ExistingConnectionScope.ExecuteWithCommand(cmd =>
            {
                cmd.CommandText = @"
                    DECLARE @logSize int
                    DECLARE @dataSize int
                    DECLARE @dataFileGrowth int
                    DECLARE @logFileGrowth int
                    DECLARE @dataName nvarchar(100)
                    DECLARE @logName nvarchar(100)
                    DECLARE @dbName nvarchar(100)

                    SET @dbName = DB_NAME()

                    SELECT @dataSize = SUM(CASE WHEN type_desc = 'ROWS' THEN size END),
                            @dataName = MAX(CASE WHEN type_desc = 'ROWS' THEN name END),
                            @dataFileGrowth = SUM(CASE WHEN type_desc = 'ROWS' AND is_percent_growth=1 THEN growth ELSE 0 END),
                            @logSize = SUM(CASE WHEN type_desc = 'LOG' THEN size END),
                            @logName = MAX(CASE WHEN type_desc = 'LOG' THEN name END),
                            @logFileGrowth = SUM(CASE WHEN type_desc = 'LOG' AND is_percent_growth=1 THEN growth ELSE 0 END)
                    FROM sys.master_files
                    where DB_NAME(database_id) = @dbName

                    IF (@logSize < 25600)
                        EXECUTE ('ALTER DATABASE ' + @dbName + ' MODIFY FILE ( NAME = N''' + @logName + ''', SIZE = 200MB)' )

                    IF (@dataSize < 25600)
                        EXECUTE ('ALTER DATABASE ' + @dbName + ' MODIFY FILE ( NAME = N''' + @dataName + ''', SIZE = 200MB)' )

                    IF (@dataFileGrowth < 25600 OR @dataFileGrowth >= 64000)
                        EXECUTE ('ALTER DATABASE ' + @dbName + ' MODIFY FILE ( NAME = N''' + @dataName + ''', FILEGROWTH = 200MB)' )

                    IF (@logFileGrowth < 25600 OR @logFileGrowth >= 64000)
                        EXECUTE ('ALTER DATABASE ' + @dbName + ' MODIFY FILE ( NAME = N''' + @logName + ''', FILEGROWTH = 200MB)' )
                ";

                cmd.ExecuteNonQuery();
            });

            // Update size and growth of tempdb
            ExistingConnectionScope.ExecuteWithCommand(cmd =>
            {
                cmd.CommandText = @"
                    DECLARE @logSize int
                    DECLARE @dataSize int
                    DECLARE @dataFileGrowth int
                    DECLARE @logFileGrowth int
                    DECLARE @dataName nvarchar(100)
                    DECLARE @logName nvarchar(100)

                    SELECT @dataSize = SUM(CASE WHEN type_desc = 'ROWS' THEN size END),
                            @dataName = MAX(CASE WHEN type_desc = 'ROWS' THEN name END),
                            @dataFileGrowth = SUM(CASE WHEN type_desc = 'ROWS' AND is_percent_growth=1 THEN growth ELSE 0 END),
                            @logSize = SUM(CASE WHEN type_desc = 'LOG' THEN size END),
                            @logName = MAX(CASE WHEN type_desc = 'LOG' THEN name END),
                            @logFileGrowth = SUM(CASE WHEN type_desc = 'LOG' AND is_percent_growth=1 THEN growth ELSE 0 END)
                    FROM sys.master_files
                    where DB_NAME(database_id) = 'tempdb'

                    IF (@logSize < 25600)
                        EXECUTE ('ALTER DATABASE tempdb MODIFY FILE ( NAME = N''' + @logName + ''', SIZE = 200MB)' )

                    IF (@dataSize < 25600)
                        EXECUTE ('ALTER DATABASE tempdb MODIFY FILE ( NAME = N''' + @dataName + ''', SIZE = 200MB)' )

                    IF (@dataFileGrowth < 25600)
                        EXECUTE ('ALTER DATABASE tempdb MODIFY FILE ( NAME = N''' + @dataName + ''', FILEGROWTH = 200MB)' )

                    IF (@logFileGrowth < 25600)
                        EXECUTE ('ALTER DATABASE tempdb MODIFY FILE ( NAME = N''' + @logName + ''', FILEGROWTH = 200MB)' )
                ";

                cmd.ExecuteNonQuery();
            });
        }
    }
}
