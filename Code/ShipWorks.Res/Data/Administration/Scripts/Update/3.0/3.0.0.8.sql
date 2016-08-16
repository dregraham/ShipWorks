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
ALTER TABLE [dbo].[ActionQueue] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping foreign keys from [dbo].[ActionQueueStep]'
GO
ALTER TABLE [dbo].[ActionQueueStep] DROP
CONSTRAINT [FK_ActionQueueStep_ActionQueue]
GO
PRINT N'Dropping foreign keys from [dbo].[ActionQueue]'
GO
ALTER TABLE [dbo].[ActionQueue] DROP
CONSTRAINT [FK_ActionQueue_Action],
CONSTRAINT [FK_ActionQueue_Computer]
GO
PRINT N'Dropping constraints from [dbo].[ActionQueue]'
GO
ALTER TABLE [dbo].[ActionQueue] DROP CONSTRAINT [PK_ActionQueue]
GO
PRINT N'Dropping constraints from [dbo].[ActionQueue]'
GO
ALTER TABLE [dbo].[ActionQueue] DROP CONSTRAINT [DF_ActionQueue_ActionVersion]
GO
PRINT N'Dropping constraints from [dbo].[ActionQueue]'
GO
ALTER TABLE [dbo].[ActionQueue] DROP CONSTRAINT [DF_ActionQueue_QueuedDate]
GO
PRINT N'Dropping constraints from [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [PK_ShippingSettings]
GO
PRINT N'Dropping index [IX_ActionQueue_Search] from [dbo].[ActionQueue]'
GO
DROP INDEX [IX_ActionQueue_Search] ON [dbo].[ActionQueue]
GO
PRINT N'Dropping index [IX_ActionQueue_ContextLock] from [dbo].[ActionQueue]'
GO
DROP INDEX [IX_ActionQueue_ContextLock] ON [dbo].[ActionQueue]
GO
PRINT N'Rebuilding [dbo].[ActionQueue]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ActionQueue]
(
[ActionQueueID] [bigint] NOT NULL IDENTITY(1041, 1000),
[RowVersion] [timestamp] NOT NULL,
[ActionID] [bigint] NOT NULL,
[ActionName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ActionVersion] [binary] (8) NOT NULL CONSTRAINT [DF_ActionQueue_ActionVersion] DEFAULT ((0)),
[QueueVersion] [binary] (8) NOT NULL CONSTRAINT [DF_ActionQueue_QueueVersion] DEFAULT (@@dbts),
[TriggerDate] [datetime] NOT NULL CONSTRAINT [DF_ActionQueue_QueuedDate] DEFAULT (getutcdate()),
[TriggerComputerID] [bigint] NOT NULL,
[RunComputerID] [bigint] NULL,
[ObjectID] [bigint] NULL,
[Status] [int] NOT NULL,
[NextStep] [int] NOT NULL,
[ContextLock] [nvarchar] (36) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ActionQueue] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_ActionQueue]([ActionQueueID], [ActionID], [ActionName], [ActionVersion], [TriggerDate], [TriggerComputerID], [RunComputerID], [ObjectID], [Status], [NextStep], [ContextLock]) SELECT [ActionQueueID], [ActionID], [ActionName], [ActionVersion], [TriggerDate], [TriggerComputerID], [RunComputerID], [ObjectID], [Status], [NextStep], [ContextLock] FROM [dbo].[ActionQueue]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ActionQueue] OFF
GO
DECLARE @idVal INT
SELECT @idVal = IDENT_CURRENT(N'ActionQueue')
DBCC CHECKIDENT(tmp_rg_xx_ActionQueue, RESEED, @idVal)
GO
DROP TABLE [dbo].[ActionQueue]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ActionQueue]', N'ActionQueue'
GO
PRINT N'Creating primary key [PK_ActionQueue] on [dbo].[ActionQueue]'
GO
ALTER TABLE [dbo].[ActionQueue] ADD CONSTRAINT [PK_ActionQueue] PRIMARY KEY CLUSTERED  ([ActionQueueID])
GO
PRINT N'Creating index [IX_ActionQueue_Search] on [dbo].[ActionQueue]'
GO
CREATE NONCLUSTERED INDEX [IX_ActionQueue_Search] ON [dbo].[ActionQueue] ([ActionQueueID], [RunComputerID], [Status])
GO
PRINT N'Creating index [IX_ActionQueue_ContextLock] on [dbo].[ActionQueue]'
GO
CREATE NONCLUSTERED INDEX [IX_ActionQueue_ContextLock] ON [dbo].[ActionQueue] ([ContextLock])
GO
ALTER TABLE [dbo].[ActionQueue] ENABLE CHANGE_TRACKING
GO
PRINT N'Rebuilding [dbo].[ShippingSettings]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ShippingSettings]
(
[ShippingSettingsID] [bit] NOT NULL,
[Activated] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Configured] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Excluded] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DefaultType] [int] NOT NULL,
[BlankPhoneOption] [int] NOT NULL,
[BlankPhoneNumber] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsuranceAgreement] [bit] NOT NULL,
[FedExUsername] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExPassword] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FedExMaskAccount] [bit] NOT NULL,
[FedExLimitValue] [bit] NOT NULL,
[FedExThermal] [bit] NOT NULL,
[FedExThermalType] [int] NOT NULL,
[FedExThermalDocTab] [bit] NOT NULL,
[FedExThermalDocTabType] [int] NOT NULL,
[UpsAccessKey] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpsThermal] [bit] NOT NULL,
[UpsThermalType] [int] NOT NULL,
[EndiciaThermal] [bit] NOT NULL,
[EndiciaThermalType] [int] NOT NULL,
[EndiciaCustomsCertify] [bit] NOT NULL,
[EndiciaCustomsSigner] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WorldShipLaunch] [bit] NOT NULL,
[StampsThermal] [bit] NOT NULL,
[StampsThermalType] [int] NOT NULL
)
GO
-- Manually edited to insert proper default values
INSERT INTO [dbo].[tmp_rg_xx_ShippingSettings]([ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsuranceAgreement], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExLimitValue], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [UpsAccessKey], [UpsThermal], [UpsThermalType], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [WorldShipLaunch], [StampsThermal], [StampsThermalType]) 
                                        SELECT [ShippingSettingsID], [Activated], [Configured], [Excluded], [DefaultType], [BlankPhoneOption], [BlankPhoneNumber], [InsuranceAgreement], [FedExUsername], [FedExPassword], [FedExMaskAccount], [FedExLimitValue], [FedExThermal], [FedExThermalType], [FedExThermalDocTab], [FedExThermalDocTabType], [UpsAccessKey], [UpsThermal], [UpsThermalType], [EndiciaThermal], [EndiciaThermalType], [EndiciaCustomsCertify], [EndiciaCustomsSigner], [WorldShipLaunch],               0,                   0 FROM [dbo].[ShippingSettings]
GO
DROP TABLE [dbo].[ShippingSettings]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingSettings]', N'ShippingSettings'
GO
PRINT N'Creating primary key [PK_ShippingSettings] on [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD CONSTRAINT [PK_ShippingSettings] PRIMARY KEY CLUSTERED  ([ShippingSettingsID])
GO
PRINT N'Adding foreign keys to [dbo].[ActionQueueStep]'
GO
ALTER TABLE [dbo].[ActionQueueStep] ADD
CONSTRAINT [FK_ActionQueueStep_ActionQueue] FOREIGN KEY ([ActionQueueID]) REFERENCES [dbo].[ActionQueue] ([ActionQueueID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[ActionQueue]'
GO
ALTER TABLE [dbo].[ActionQueue] ADD
CONSTRAINT [FK_ActionQueue_Action] FOREIGN KEY ([ActionID]) REFERENCES [dbo].[Action] ([ActionID]) ON DELETE CASCADE,
CONSTRAINT [FK_ActionQueue_Computer] FOREIGN KEY ([TriggerComputerID]) REFERENCES [dbo].[Computer] ([ComputerID])
GO
