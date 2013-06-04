﻿SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[StampsAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping foreign keys from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP
CONSTRAINT [FK_StampsShipment_PostalShipment]
GO
PRINT N'Dropping constraints from [dbo].[StampsAccount]'
GO
ALTER TABLE [dbo].[StampsAccount] DROP CONSTRAINT [PK_PostalStampsAccount]
GO
PRINT N'Dropping constraints from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [PK_StampsShipment]
GO

PRINT N'Rebuilding [dbo].[StampsShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_StampsShipment]
(
[ShipmentID] [bigint] NOT NULL,
[StampsAccountID] [bigint] NOT NULL,
[HidePostage] [bit] NOT NULL,
[RequireFullAddressValidation] [bit] NOT NULL,
[IntegratorTransactionID] [uniqueidentifier] NOT NULL,
[StampsTransactionID] [uniqueidentifier] NOT NULL,
[Memo] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ScanFormID] [bigint] NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_StampsShipment]([ShipmentID], [StampsAccountID], [HidePostage], [RequireFullAddressValidation], [IntegratorTransactionID], [StampsTransactionID], Memo, ScanFormID) SELECT [ShipmentID], [StampsAccountID], [HidePostage], [RequireFullAddressValidation], [IntegratorTransactionID], [StampsTransactionID], '', NULL FROM [dbo].[StampsShipment]
GO
DROP TABLE [dbo].[StampsShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_StampsShipment]', N'StampsShipment'
GO
PRINT N'Creating primary key [PK_StampsShipment] on [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD CONSTRAINT [PK_StampsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Creating [dbo].[StampsScanForm]'
GO
CREATE TABLE [dbo].[StampsScanForm]
(
[StampsScanFormID] [bigint] NOT NULL IDENTITY(1072, 1000),
[StampsAccountID] [bigint] NOT NULL,
[ScanFormTransactionID] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ScanFormUrl] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipmentCount] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL
)
GO
PRINT N'Creating primary key [PK_StampsScanForm] on [dbo].[StampsScanForm]'
GO
ALTER TABLE [dbo].[StampsScanForm] ADD CONSTRAINT [PK_StampsScanForm] PRIMARY KEY CLUSTERED  ([StampsScanFormID])
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
[MailingPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_StampsAccount] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_StampsAccount]([StampsAccountID], [Username], [Password], FirstName, MiddleName, LastName, Company, Street1, Street2, Street3, City, StateProvCode, PostalCode, CountryCode, Phone, Email, Website, MailingPostalCode) SELECT [StampsAccountID], [Username], [Password], '', '', '', '', '', '', '', '', '', '', '', '', '', '', '' FROM [dbo].[StampsAccount]
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
PRINT N'Adding foreign keys to [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD
CONSTRAINT [FK_StampsShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'IntegratorTransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsTransactionID'
GO

PRINT N'Altering [dbo].[StampsProfile]'
GO
ALTER TABLE [dbo].[StampsProfile] ADD
[Memo] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

UPDATE [dbo].StampsProfile
	SET Memo = ''
	WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType IN (3) AND ShipmentTypePrimary = 1)
GO