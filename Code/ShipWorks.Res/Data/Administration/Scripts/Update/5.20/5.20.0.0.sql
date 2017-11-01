SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[DhlExpressAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping constraints from [dbo].[DhlExpressAccount]'
GO
ALTER TABLE [dbo].[DhlExpressAccount] DROP CONSTRAINT [PK_DhlExpressAccount]
GO
EXEC sp_rename N'[dbo].[DhlExpressAccount]', N'ShipEngineAccount', N'OBJECT'
GO
PRINT N'Rebuilding [dbo].[ShipEngineAccount]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_ShipEngineAccount]
(
[ShipEngineAccountID] [bigint] NOT NULL IDENTITY(1102, 1000),
[RowVersion] [timestamp] NOT NULL,
[ShipmentTypeCode] [int] NOT NULL,
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
SET IDENTITY_INSERT [dbo].[RG_Recovery_1_ShipEngineAccount] ON
GO
INSERT INTO [dbo].[RG_Recovery_1_ShipEngineAccount]
	([ShipEngineAccountID], [ShipmentTypeCode], [AccountNumber], [ShipEngineCarrierId], [Description], [FirstName], [MiddleName], [LastName], [Company], [Street1], [City], [StateProvCode], [PostalCode], [CountryCode], [Email], [Phone]) 
SELECT [DhlExpressAccountID], 17, [AccountNumber], [ShipEngineCarrierId], [Description], [FirstName], [MiddleName], [LastName], [Company], [Street1], [City], [StateProvCode], [PostalCode], [CountryCode], [Email], [Phone] FROM [dbo].[ShipEngineAccount]
GO
SET IDENTITY_INSERT [dbo].[RG_Recovery_1_ShipEngineAccount] OFF
GO
DROP TABLE [dbo].[ShipEngineAccount]
GO
EXEC sp_rename N'[dbo].[RG_Recovery_1_ShipEngineAccount]', N'ShipEngineAccount', N'OBJECT'
GO
PRINT N'Creating primary key [PK_ShipEngineAccount] on [dbo].[ShipEngineAccount]'
GO
ALTER TABLE [dbo].[ShipEngineAccount] ADD CONSTRAINT [PK_ShipEngineAccount] PRIMARY KEY CLUSTERED  ([ShipEngineAccountID])
GO
ALTER TABLE [dbo].[ShipEngineAccount] ENABLE CHANGE_TRACKING
GO
GO
EXECUTE sp_rename N'dbo.DhlExpressProfile.DhlExpressAccountID', N'ShipEngineAccountID', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.DhlExpressShipment.DhlExpressAccountID', N'ShipEngineAccountID', 'COLUMN' 
GO