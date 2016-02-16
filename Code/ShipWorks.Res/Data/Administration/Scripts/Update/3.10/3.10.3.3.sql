SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_ActionQueueSelection_ActionQueueID] from [dbo].[ActionQueueSelection]'
GO
DROP INDEX [IX_ActionQueueSelection_ActionQueueID] ON [dbo].[ActionQueueSelection]
GO
PRINT N'Altering [dbo].[Dirty]'
GO
ALTER TABLE [dbo].[Dirty] ADD
[Count] [bigint] NULL
GO
PRINT N'Creating index [IX_ActionQueueStep_ActionQueue] on [dbo].[ActionQueueStep]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ActionQueueStep_ActionQueue] ON [dbo].[ActionQueueStep] ([ActionQueueID], [StepIndex])
GO
PRINT N'Creating index [IX_ActionQueueSelection_ActionQueueID] on [dbo].[ActionQueueSelection]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ActionQueueSelection_ActionQueueID] ON [dbo].[ActionQueueSelection] ([ActionQueueID], [ObjectID])
GO