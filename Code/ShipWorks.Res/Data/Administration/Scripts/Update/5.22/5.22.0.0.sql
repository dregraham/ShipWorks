SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[AsendiaAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping constraints from [dbo].[AsendiaAccount]'
GO
ALTER TABLE [dbo].[AsendiaAccount] DROP CONSTRAINT [PK_AsendiaAccount]
GO
PRINT N'Rebuilding [dbo].[AsendiaAccount]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_AsendiaAccount]
(
[AsendiaAccountID] [bigint] NOT NULL IDENTITY(1103, 1000),
[RowVersion] [timestamp] NOT NULL,
[AccountNumber] [bigint] NOT NULL,
[ShipEngineCarrierId] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (43) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[RG_Recovery_1_AsendiaAccount] ON
GO
INSERT INTO [dbo].[RG_Recovery_1_AsendiaAccount]([AsendiaAccountID], [AccountNumber], [ShipEngineCarrierId], [Description], [FirstName], [MiddleName], [LastName], [Company], [Street1], [City], [StateProvCode], [PostalCode], [CountryCode], [Email], [Phone]) SELECT [AsendiaAccountID], [AccountNumber], [ShipEngineCarrierId], [Description], [FirstName], [MiddleName], [LastName], [Company], [Street1], [City], [StateProvCode], [PostalCode], [CountryCode], [Email], [Phone] FROM [dbo].[AsendiaAccount]
GO
SET IDENTITY_INSERT [dbo].[RG_Recovery_1_AsendiaAccount] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[AsendiaAccount]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[RG_Recovery_1_AsendiaAccount]', RESEED, @idVal)
GO
DROP TABLE [dbo].[AsendiaAccount]
GO
EXEC sp_rename N'[dbo].[RG_Recovery_1_AsendiaAccount]', N'AsendiaAccount', N'OBJECT'
GO
PRINT N'Creating primary key [PK_AsendiaAccount] on [dbo].[AsendiaAccount]'
GO
ALTER TABLE [dbo].[AsendiaAccount] ADD CONSTRAINT [PK_AsendiaAccount] PRIMARY KEY CLUSTERED  ([AsendiaAccountID])
GO
ALTER TABLE [dbo].[AsendiaAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[AsendiaAccount]'
GO