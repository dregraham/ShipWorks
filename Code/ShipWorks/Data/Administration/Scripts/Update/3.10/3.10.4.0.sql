SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[FilterLayout]'
GO
ALTER TABLE [dbo].[FilterLayout] DROP CONSTRAINT [FK_FilterLayout_FilterNode]
GO
PRINT N'Dropping foreign keys from [dbo].[FilterNode]'
GO
ALTER TABLE [dbo].[FilterNode] DROP CONSTRAINT [FK_FilterNode_Parent]
ALTER TABLE [dbo].[FilterNode] DROP CONSTRAINT [FK_FilterNode_FilterSequence]
ALTER TABLE [dbo].[FilterNode] DROP CONSTRAINT [FK_FilterNode_FilterNodeContent]
GO
PRINT N'Dropping foreign keys from [dbo].[FilterNodeColumnSettings]'
GO
ALTER TABLE [dbo].[FilterNodeColumnSettings] DROP CONSTRAINT [FK_FilterNodeColumnSettings_FilterNode]
GO
PRINT N'Dropping constraints from [dbo].[FilterNode]'
GO
ALTER TABLE [dbo].[FilterNode] DROP CONSTRAINT [PK_FilterNode]
GO
PRINT N'Dropping index [IX_FilterNode_ParentFilterNodeID] from [dbo].[FilterNode]'
GO
DROP INDEX [IX_FilterNode_ParentFilterNodeID] ON [dbo].[FilterNode]
GO
PRINT N'Rebuilding [dbo].[FilterNode]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FilterNode]
(
[FilterNodeID] [bigint] NOT NULL IDENTITY(1007, 1000),
[RowVersion] [timestamp] NOT NULL,
[ParentFilterNodeID] [bigint] NULL,
[FilterSequenceID] [bigint] NOT NULL,
[FilterNodeContentID] [bigint] NOT NULL,
[Created] [datetime] NOT NULL,
[Purpose] [int] NOT NULL,
[State] [tinyint] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FilterNode] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_FilterNode]([FilterNodeID], [ParentFilterNodeID], [FilterSequenceID], [FilterNodeContentID], [Created], 
	[Purpose], [State]) 
	SELECT [FilterNodeID], [ParentFilterNodeID], [FilterSequenceID], [FilterNodeContentID], [Created], 
	[Purpose], 1
	FROM [dbo].[FilterNode]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FilterNode] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[FilterNode]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_FilterNode]', RESEED, @idVal)
GO
DROP TABLE [dbo].[FilterNode]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FilterNode]', N'FilterNode'
GO
PRINT N'Creating primary key [PK_FilterNode] on [dbo].[FilterNode]'
GO
ALTER TABLE [dbo].[FilterNode] ADD CONSTRAINT [PK_FilterNode] PRIMARY KEY CLUSTERED  ([FilterNodeID])
GO
PRINT N'Creating index [IX_FilterNode_ParentFilterNodeID] on [dbo].[FilterNode]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNode_ParentFilterNodeID] ON [dbo].[FilterNode] ([ParentFilterNodeID])
GO
PRINT N'Creating index [IX_FilterNode_State] on [dbo].[FilterNode]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNode_State] ON [dbo].[FilterNode] ([State])
GO
PRINT N'Adding foreign keys to [dbo].[FilterLayout]'
GO
ALTER TABLE [dbo].[FilterLayout] ADD CONSTRAINT [FK_FilterLayout_FilterNode] FOREIGN KEY ([FilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterNode]'
GO
ALTER TABLE [dbo].[FilterNode] ADD CONSTRAINT [FK_FilterNode_Parent] FOREIGN KEY ([ParentFilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID])
ALTER TABLE [dbo].[FilterNode] ADD CONSTRAINT [FK_FilterNode_FilterSequence] FOREIGN KEY ([FilterSequenceID]) REFERENCES [dbo].[FilterSequence] ([FilterSequenceID])
ALTER TABLE [dbo].[FilterNode] ADD CONSTRAINT [FK_FilterNode_FilterNodeContent] FOREIGN KEY ([FilterNodeContentID]) REFERENCES [dbo].[FilterNodeContent] ([FilterNodeContentID])
GO
PRINT N'Adding foreign keys to [dbo].[FilterNodeColumnSettings]'
GO
ALTER TABLE [dbo].[FilterNodeColumnSettings] ADD CONSTRAINT [FK_FilterNodeColumnSettings_FilterNode] FOREIGN KEY ([FilterNodeID]) REFERENCES [dbo].[FilterNode] ([FilterNodeID]) ON DELETE CASCADE
GO
