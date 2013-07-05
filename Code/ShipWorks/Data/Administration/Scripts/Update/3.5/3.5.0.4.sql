﻿/*
Run this script on:

        INTSQL01\Development.ShipWorksSchema_Scheduling    -  This database will be modified

to synchronize it with:

        (local)\development.ShipWorksLocal

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.3.8 from Red Gate Software Ltd at 7/5/2013 2:02:50 PM

*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping foreign keys from [dbo].[ActionFilterTrigger]'
GO
ALTER TABLE [dbo].[ActionFilterTrigger] DROP CONSTRAINT [FK_ActionFilterTrigger_Action]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[ActionFilterTrigger]'
GO
ALTER TABLE [dbo].[ActionFilterTrigger] DROP CONSTRAINT [PK_ActionFilterTrigger]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[ActionFilterTrigger]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ActionFilterTrigger]
(
[ActionID] [bigint] NOT NULL,
[FilterNodeID] [bigint] NOT NULL,
[Direction] [int] NOT NULL,
[ComputerLimitedType] [int] NOT NULL,
[ComputerLimitedList] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_ActionFilterTrigger]([ActionID], [FilterNodeID], [Direction], [ComputerLimitedType], [ComputerLimitedList]) SELECT [ActionID], [FilterNodeID], [Direction], CAST (ComputerLimited AS int), '' FROM [dbo].[ActionFilterTrigger]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[ActionFilterTrigger]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ActionFilterTrigger]', N'ActionFilterTrigger'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActionFilterTrigger] on [dbo].[ActionFilterTrigger]'
GO
ALTER TABLE [dbo].[ActionFilterTrigger] ADD CONSTRAINT [PK_ActionFilterTrigger] PRIMARY KEY CLUSTERED  ([ActionID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActionFilterTrigger]'
GO
ALTER TABLE [dbo].[ActionFilterTrigger] ADD CONSTRAINT [FK_ActionFilterTrigger_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO
