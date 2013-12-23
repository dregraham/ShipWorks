GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "NewDB"
:setvar DefaultFilePrefix "NewDB"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.DEVELOPMENT\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.DEVELOPMENT\MSSQL\DATA\"


IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'$(DatabaseName)') 
    RAISERROR('The database $(DatabaseName) already exists on the server.', 16, 1)
GO

CREATE DATABASE [$(DatabaseName)]  
    ON (
        NAME = N'ShipWorks_Data', 
        FILENAME = N'$(DefaultDataPath)$(DefaultFilePrefix).mdf' , 
        SIZE = 10, 
        FILEGROWTH = 100MB) 
    LOG ON (
        NAME = N'ShipWorks_Log', 
        FILENAME = N'$(DefaultDataPath)$(DefaultFilePrefix)_log.ldf' , 
        SIZE = 10, 
        FILEGROWTH = 100MB)
    COLLATE SQL_Latin1_General_CP1_CI_AS
GO

ALTER DATABASE [$(DatabaseName)] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [$(DatabaseName)].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO

ALTER DATABASE [$(DatabaseName)] set auto_close on
GO

ALTER DATABASE [$(DatabaseName)] SET RECOVERY simple --trunc. log
GO

ALTER DATABASE [$(DatabaseName)] SET AUTO_SHRINK ON 
GO

ALTER DATABASE [$(DatabaseName)] set Page_verify Torn_Page_Detection
GO

ALTER DATABASE [$(DatabaseName)] set  Read_Write
GO

ALTER DATABASE [$(DatabaseName)] SET MULTI_USER --dbo use
GO

ALTER DATABASE [$(DatabaseName)] SET ANSI_NULL_DEFAULT OFF
GO

ALTER DATABASE [$(DatabaseName)] SET RECURSIVE_TRIGGERS OFF
GO

ALTER DATABASE [$(DatabaseName)] SET ANSI_NULLS OFF
GO

ALTER DATABASE [$(DatabaseName)] SET CONCAT_NULL_YIELDS_NULL OFF
GO

ALTER DATABASE [$(DatabaseName)] SET CURSOR_CLOSE_ON_COMMIT OFF
GO

ALTER DATABASE [$(DatabaseName)] SET CURSOR_DEFAULT GLOBAL
GO

ALTER DATABASE [$(DatabaseName)] SET QUOTED_IDENTIFIER OFF
GO

ALTER DATABASE [$(DatabaseName)] SET ANSI_WARNINGS OFF
GO

ALTER DATABASE [$(DatabaseName)] SET AUTO_CREATE_STATISTICS ON
GO

ALTER DATABASE [$(DatabaseName)] SET AUTO_UPDATE_STATISTICS ON
GO

ALTER DATABASE [$(DatabaseName)]
  SET CHANGE_TRACKING = ON
  (CHANGE_RETENTION = 1 DAYS, AUTO_CLEANUP = ON)
GO
