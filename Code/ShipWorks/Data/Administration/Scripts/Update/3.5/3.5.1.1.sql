SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[StampsAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping constraints from [dbo].[StampsAccount]'
GO
ALTER TABLE [dbo].[StampsAccount] DROP CONSTRAINT [PK_PostalStampsAccount]
GO
PRINT N'Rebuilding [dbo].[StampsAccount]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_StampsAccount]
(
[StampsAccountID] [bigint] NOT NULL IDENTITY(1052, 1000),
[RowVersion] [timestamp] NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Website] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsExpress1] [bit] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_StampsAccount] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_StampsAccount]([StampsAccountID], [Username], [Password], [FirstName], [MiddleName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Email], [Website], [MailingPostalCode], [IsExpress1]) SELECT [StampsAccountID], [Username], [Password], [FirstName], [MiddleName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Email], [Website], [MailingPostalCode], 0 FROM [dbo].[StampsAccount]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_StampsAccount] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[StampsAccount]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_StampsAccount]', RESEED, @idVal)
GO
DROP TABLE [dbo].[StampsAccount]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_StampsAccount]', N'StampsAccount'
GO
PRINT N'Creating primary key [PK_PostalStampsAccount] on [dbo].[StampsAccount]'
GO
ALTER TABLE [dbo].[StampsAccount] ADD CONSTRAINT [PK_PostalStampsAccount] PRIMARY KEY CLUSTERED  ([StampsAccountID])
GO
ALTER TABLE [dbo].[StampsAccount] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[StampsAccount]'
GO