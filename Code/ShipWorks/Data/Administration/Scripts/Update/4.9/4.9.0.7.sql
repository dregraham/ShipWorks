SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[UspsAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping constraints from [dbo].[UspsAccount]'
GO
ALTER TABLE [dbo].[UspsAccount] DROP CONSTRAINT [PK_PostalUspsAccount]
GO
PRINT N'Rebuilding [dbo].[UspsAccount]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_UspsAccount]
(
[UspsAccountID] [bigint] NOT NULL IDENTITY(1052, 1000),
[RowVersion] [timestamp] NOT NULL,
[Description] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
[UspsReseller] [int] NOT NULL,
[ContractType] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[PendingInitialAccount] [int] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UspsAccount] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_UspsAccount]([UspsAccountID], [Description], [Username], [Password], [FirstName], [MiddleName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Email], [Website], [MailingPostalCode], [UspsReseller], [ContractType], [CreatedDate], [PendingInitialAccount]) SELECT [UspsAccountID], [Description], [Username], [Password], [FirstName], [MiddleName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Email], [Website], [MailingPostalCode], [UspsReseller], [ContractType], [CreatedDate], 0 FROM [dbo].[UspsAccount]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UspsAccount] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[UspsAccount]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_UspsAccount]', RESEED, @idVal)
GO
DROP TABLE [dbo].[UspsAccount]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_UspsAccount]', N'UspsAccount'
GO
PRINT N'Creating primary key [PK_PostalUspsAccount] on [dbo].[UspsAccount]'
GO
ALTER TABLE [dbo].[UspsAccount] ADD CONSTRAINT [PK_PostalUspsAccount] PRIMARY KEY CLUSTERED  ([UspsAccountID])
GO
ALTER TABLE [dbo].[UspsAccount] ENABLE CHANGE_TRACKING
GO