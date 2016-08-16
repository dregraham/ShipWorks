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
ALTER TABLE [dbo].[ActionQueue] DROP CONSTRAINT [DF_ActionQueue_QueuedDate]
GO
PRINT N'Altering [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ALTER COLUMN [BuyerID] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ALTER COLUMN [SalesSourceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
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
INSERT INTO [dbo].[tmp_rg_xx_ActionQueue]([ActionQueueID], [ActionID], [ActionName], [TriggerDate], [TriggerComputerID], [RunComputerID], [ObjectID], [Status], [NextStep]) SELECT [ActionQueueID], [ActionID], [ActionName], [TriggerDate], [TriggerComputerID], [RunComputerID], [ObjectID], [Status], [NextStep] FROM [dbo].[ActionQueue]
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
