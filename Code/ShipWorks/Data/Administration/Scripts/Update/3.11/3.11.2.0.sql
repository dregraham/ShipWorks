SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[StampsAccount] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping extended properties'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'IntegratorTransactionID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsAccountID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsTransactionID'
GO
PRINT N'Dropping foreign keys from [dbo].[StampsProfile]'
GO
ALTER TABLE [dbo].[StampsProfile] DROP CONSTRAINT [FK_StampsProfile_PostalProfile]
GO
PRINT N'Dropping foreign keys from [dbo].[StampsScanForm]'
GO
ALTER TABLE [dbo].[StampsScanForm] DROP CONSTRAINT [FK_StampsScanForm_ScanFormBatch]
GO
PRINT N'Dropping foreign keys from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [FK_StampsShipment_ScanFormBatch]
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [FK_StampsShipment_PostalShipment]
GO
PRINT N'Dropping constraints from [dbo].[StampsAccount]'
GO
ALTER TABLE [dbo].[StampsAccount] DROP CONSTRAINT [PK_PostalStampsAccount]
GO
PRINT N'Dropping constraints from [dbo].[StampsProfile]'
GO
ALTER TABLE [dbo].[StampsProfile] DROP CONSTRAINT [PK_StampsProfile]
GO
PRINT N'Dropping constraints from [dbo].[StampsScanForm]'
GO
ALTER TABLE [dbo].[StampsScanForm] DROP CONSTRAINT [PK_StampsScanForm]
GO
PRINT N'Dropping constraints from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [PK_StampsShipment]
GO
EXEC sp_rename N'[dbo].[StampsProfile]',N'UspsProfile',N'OBJECT'
EXEC sp_rename N'[dbo].[StampsScanForm]',N'UspsScanForm',N'OBJECT'
EXEC sp_rename N'[dbo].[StampsShipment]',N'UspsShipment',N'OBJECT'
EXEC sp_rename N'[dbo].[StampsAccount]',N'UspsAccount',N'OBJECT'
GO
PRINT N'Altering [dbo].[UspsProfile]'
GO
EXEC sp_rename N'[dbo].[UspsProfile].[StampsAccountID]', N'UspsAccountID', 'COLUMN'
GO
PRINT N'Creating primary key [PK_UspsProfile] on [dbo].[UspsProfile]'
GO
ALTER TABLE [dbo].[UspsProfile] ADD CONSTRAINT [PK_UspsProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Rebuilding [dbo].[UspsScanForm]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_UspsScanForm]
(
[UspsScanFormID] [bigint] NOT NULL IDENTITY(1072, 1000),
[UspsAccountID] [bigint] NOT NULL,
[ScanFormTransactionID] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ScanFormUrl] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ScanFormBatchID] [bigint] NOT NULL,
[Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UspsScanForm] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_UspsScanForm]([UspsScanFormID], [UspsAccountID], [ScanFormTransactionID], [ScanFormUrl], [CreatedDate], [ScanFormBatchID], [Description]) SELECT [StampsScanFormID], [StampsAccountID], [ScanFormTransactionID], [ScanFormUrl], [CreatedDate], [ScanFormBatchID], [Description] FROM [dbo].[UspsScanForm]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UspsScanForm] OFF
GO
DROP TABLE [dbo].[UspsScanForm]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_UspsScanForm]', N'UspsScanForm'
GO
PRINT N'Creating primary key [PK_UspsScanForm] on [dbo].[UspsScanForm]'
GO
ALTER TABLE [dbo].[UspsScanForm] ADD CONSTRAINT [PK_UspsScanForm] PRIMARY KEY CLUSTERED  ([UspsScanFormID])
GO
PRINT N'Altering [dbo].[UspsShipment]'
GO
EXEC sp_rename N'[dbo].[UspsShipment].[StampsAccountID]', N'UspsAccountID', 'COLUMN'
GO
EXEC sp_rename N'[dbo].[UspsShipment].[StampsTransactionID]', N'UspsTransactionID', 'COLUMN'
GO
EXEC sp_rename N'[dbo].[UspsShipment].[OriginalStampsAccountID]', N'OriginalUspsAccountID', 'COLUMN'
GO
PRINT N'Creating primary key [PK_UspsShipment] on [dbo].[UspsShipment]'
GO
ALTER TABLE [dbo].[UspsShipment] ADD CONSTRAINT [PK_UspsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP
COLUMN [EndiciaUspsAutomaticExpedited],
COLUMN [EndiciaUspsAutomaticExpeditedAccount],
COLUMN [StampsUspsAutomaticExpedited],
COLUMN [StampsUspsAutomaticExpeditedAccount]
GO
EXEC sp_rename N'[dbo].[ShippingSettings].[StampsAutomaticExpress1]', N'UspsAutomaticExpress1', 'COLUMN'
GO
EXEC sp_rename N'[dbo].[ShippingSettings].[StampsAutomaticExpress1Account]', N'UspsAutomaticExpress1Account', 'COLUMN'
GO
EXEC sp_rename N'[dbo].[ShippingSettings].[Express1StampsSingleSource]', N'Express1UspsSingleSource', 'COLUMN'
GO
PRINT N'Rebuilding [dbo].[UspsAccount]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_UspsAccount]
(
[UspsAccountID] [bigint] NOT NULL IDENTITY(1052, 1000),
[RowVersion] [timestamp] NOT NULL,
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
[CreatedDate] [datetime] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UspsAccount] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_UspsAccount]([UspsAccountID], [Username], [Password], [FirstName], [MiddleName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Email], [Website], [MailingPostalCode], [UspsReseller], [ContractType], [CreatedDate]) SELECT [StampsAccountID], [Username], [Password], [FirstName], [MiddleName], [LastName], [Company], [Street1], [Street2], [Street3], [City], [StateProvCode], [PostalCode], [CountryCode], [Phone], [Email], [Website], [MailingPostalCode], [StampsReseller], [ContractType], [CreatedDate] FROM [dbo].[UspsAccount]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UspsAccount] OFF
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
PRINT N'Altering [dbo].[UspsAccount]'
GO
PRINT N'Adding foreign keys to [dbo].[UspsProfile]'
GO
ALTER TABLE [dbo].[UspsProfile] ADD CONSTRAINT [FK_UspsProfile_PostalProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[PostalProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UspsScanForm]'
GO
ALTER TABLE [dbo].[UspsScanForm] ADD CONSTRAINT [FK_UspsScanForm_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[UspsShipment]'
GO
ALTER TABLE [dbo].[UspsShipment] ADD CONSTRAINT [FK_UspsShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
ALTER TABLE [dbo].[UspsShipment] ADD CONSTRAINT [FK_UspsShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UspsShipment', 'COLUMN', N'IntegratorTransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'UspsShipment', 'COLUMN', N'UspsAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UspsShipment', 'COLUMN', N'UspsTransactionID'
GO
