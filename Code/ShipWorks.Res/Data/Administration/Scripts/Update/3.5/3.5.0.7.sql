PRINT N'Creating [dbo].[ActionQueueSelection]'
GO
CREATE TABLE [dbo].[ActionQueueSelection]
(
[ActionQueueSelectionID] [bigint] NOT NULL IDENTITY(1097, 1000),
[ActionQueueID] [bigint] NOT NULL,
[ObjectID] [bigint] NOT NULL
) ON [PRIMARY]
GO
PRINT N'Creating primary key [PK_ActionQueueSelection] on [dbo].[ActionQueueSelection]'
GO
ALTER TABLE [dbo].[ActionQueueSelection] ADD CONSTRAINT [PK_ActionQueueSelection] PRIMARY KEY CLUSTERED  ([ActionQueueSelectionID]) ON [PRIMARY]
GO
PRINT N'Creating index [IX_ActionQueueSelection_ActionQueueID] on [dbo].[ActionQueueSelection]'
GO
CREATE NONCLUSTERED INDEX [IX_ActionQueueSelection_ActionQueueID] ON [dbo].[ActionQueueSelection] ([ActionQueueID]) ON [PRIMARY]
GO
PRINT N'Adding foreign keys to [dbo].[ActionQueueSelection]'
GO
ALTER TABLE [dbo].[ActionQueueSelection] ADD CONSTRAINT [FK_ActionQueueSelection_ActionQueue] FOREIGN KEY ([ActionQueueID]) REFERENCES [dbo].[ActionQueue] ([ActionQueueID]) ON DELETE CASCADE
GO