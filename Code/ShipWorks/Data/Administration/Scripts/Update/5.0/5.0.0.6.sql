-- Note
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_OrderNote_ObjectID] from [dbo].[Note]'
GO
DROP INDEX [IX_OrderNote_ObjectID] ON [dbo].[Note]
GO
PRINT N'Creating index [IX_OrderNote_ObjectID] on [dbo].[Note]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrderNote_ObjectID] ON [dbo].[Note] ([ObjectID] ASC, [NoteID] ASC) INCLUDE ([Edited])
GO