
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'{DBNAME}')
    RAISERROR('The database {DBNAME} already exists on the server.', 16, 1)
GO

CREATE DATABASE {DBNAME}
    ON (
        NAME = N'ShipWorks_Data',
        FILENAME = N'{FILEPATH}{FILENAME}.mdf' ,
        SIZE = 200,
        FILEGROWTH = 200MB)
    LOG ON (
        NAME = N'ShipWorks_Log',
        FILENAME = N'{FILEPATH}{FILENAME}_log.ldf' ,
        SIZE = 200,
        FILEGROWTH = 200MB)
    COLLATE SQL_Latin1_General_CP1_CI_AS
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [{DBNAME}].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO

ALTER DATABASE {DBNAME} set auto_close on
GO

ALTER DATABASE {DBNAME} SET RECOVERY simple --trunc. log
GO

ALTER DATABASE {DBNAME} set Page_verify CHECKSUM
GO

ALTER DATABASE {DBNAME} set  Read_Write
GO

ALTER DATABASE {DBNAME} SET MULTI_USER --dbo use
GO

ALTER DATABASE {DBNAME} SET ANSI_NULL_DEFAULT OFF
GO

ALTER DATABASE {DBNAME} SET RECURSIVE_TRIGGERS OFF
GO

ALTER DATABASE {DBNAME} SET ANSI_NULLS OFF
GO

ALTER DATABASE {DBNAME} SET CONCAT_NULL_YIELDS_NULL OFF
GO

ALTER DATABASE {DBNAME} SET CURSOR_CLOSE_ON_COMMIT OFF
GO

ALTER DATABASE {DBNAME} SET CURSOR_DEFAULT GLOBAL
GO

ALTER DATABASE {DBNAME} SET QUOTED_IDENTIFIER OFF
GO

ALTER DATABASE {DBNAME} SET ANSI_WARNINGS OFF
GO

ALTER DATABASE {DBNAME} SET AUTO_CREATE_STATISTICS ON
GO

ALTER DATABASE {DBNAME} SET AUTO_UPDATE_STATISTICS ON
GO

ALTER DATABASE {DBNAME}
  SET CHANGE_TRACKING = ON
  (CHANGE_RETENTION = 1 DAYS, AUTO_CLEANUP = ON)
GO

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

GO

DECLARE @version NVARCHAR(20) = CONVERT(VARCHAR(20),SERVERPROPERTY('productversion'));
DECLARE @productlevel NVARCHAR(20) = CONVERT(VARCHAR(20),SERVERPROPERTY('ProductLevel'));

DECLARE @Sql NVARCHAR(500) =  'IF '''+ @version + ''' LIKE ''12%'' AND ''' + @productlevel + ''' = ''RTM''
									ALTER DATABASE [' + {DBNAME} + ']
										SET COMPATIBILITY_LEVEL = 110' +

							   'IF '''+ @version + ''' LIKE ''12%'' AND ''' + @productlevel + ''' != ''RTM''
									ALTER DATABASE [' + {DBNAME} + ']
										SET COMPATIBILITY_LEVEL = 120'
EXECUTE sp_executesql @Sql;
