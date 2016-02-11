SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[UpsAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping constraints from [dbo].[UpsAccount]'
GO
ALTER TABLE [dbo].[UpsAccount] DROP CONSTRAINT [PK_UpsAccount]
GO
PRINT N'Altering [dbo].[ChannelAdvisorOrder]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrder] ALTER COLUMN [CustomOrderIdentifier] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Rebuilding [dbo].[UpsAccount]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_UpsAccount]
(
[UpsAccountID] [bigint] NOT NULL IDENTITY(1056, 1000),
[RowVersion] [timestamp] NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AccountNumber] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UserID] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RateType] [int] NOT NULL,
[InvoiceAuth] [bit] NOT NULL,
[FirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MiddleName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Company] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Street3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UpsAccount] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_UpsAccount]([UpsAccountID], [Description], [AccountNumber], [UserID], [Password], [RateType], [InvoiceAuth], [FirstName], [MiddleName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Email], [Website]) SELECT [UpsAccountID], [Description], [AccountNumber], [UserID], [Password], [RateType], 0, [FirstName], [MiddleName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Email], [Website] FROM [dbo].[UpsAccount]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UpsAccount] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[UpsAccount]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_UpsAccount]', RESEED, @idVal)
GO
DROP TABLE [dbo].[UpsAccount]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_UpsAccount]', N'UpsAccount'
GO
PRINT N'Creating primary key [PK_UpsAccount] on [dbo].[UpsAccount]'
GO
ALTER TABLE [dbo].[UpsAccount] ADD CONSTRAINT [PK_UpsAccount] PRIMARY KEY CLUSTERED  ([UpsAccountID])
GO
ALTER TABLE [dbo].[UpsAccount] ENABLE CHANGE_TRACKING
GO
